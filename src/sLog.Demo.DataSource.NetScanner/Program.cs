using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace sLog.Demo.DataSource.NetScanner
{
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            IConfigurationRoot config = GetConfiguration();

            //Prepare client
            client.BaseAddress = new Uri("http://localhost:50062/api/");


            //Execute the scans according to the config
            if (config["ScanSubnet"] == "true")
            {
                Console.WriteLine("Scanning LAN Subnet...");
                StartLocalScan();
            }
            else
            {
                Console.WriteLine("Skip scanning LAN Subnet...");
            }

            IPAddress target;
            string scanTarget = config["ScanTarget"];
            if (IPAddress.TryParse(scanTarget, out target))
            {
                Console.WriteLine("Scanning target " + scanTarget);
                StartTargetScan(target);
            }
            else
            {
                Console.WriteLine("Skip scanning targets.");
            }
        }

        /// <summary>
        /// Starts the target scan.
        /// </summary>
        /// <param name="target">The target.</param>
        private static void StartTargetScan(IPAddress target)
        {
            Ping(target);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        /// <devdoc>Taken from: https://blog.bitscry.com/2017/05/30/appsettings-json-in-net-core-console-app/
        /// </devdoc>
        private static IConfigurationRoot GetConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }

        /// <summary>
        /// Starts the local ping scan scan.
        /// </summary>
        private static void StartLocalScan()
        {
            //Taken from http://www.morethantechnical.com/2009/01/26/scanning-your-entire-lan-for-mac-addresses/

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Down)
                    continue;
                if (ni.GetIPProperties().GatewayAddresses.Count == 0)
                    continue;
                foreach (UnicastIPAddressInformation uipi in ni.GetIPProperties().UnicastAddresses)
                {
                    if (uipi.IPv4Mask == null)
                        continue;
                    System.Console.WriteLine("IP: " + uipi.Address + ", Netmask: " + uipi.IPv4Mask);
                    String[] IPParts = uipi.Address.ToString().Split('.');
                    String[] NetmaskParts = uipi.IPv4Mask.ToString().Split('.');
                    String StartIP;
                    StartIP = (int.Parse(IPParts[0]) & (int.Parse(NetmaskParts[0]))) + "." + (int.Parse(IPParts[1]) & (int.Parse(NetmaskParts[1]))) + "." + (int.Parse(IPParts[2]) & (int.Parse(NetmaskParts[2]))) + "." + (int.Parse(IPParts[3]) & (int.Parse(NetmaskParts[3])));
                    String EndIP;
                    String[] StartIPParts = StartIP.Split('.');
                    EndIP = (int.Parse(StartIPParts[0]) + 255 - (int.Parse(NetmaskParts[0]))) + "." + (int.Parse(StartIPParts[1]) + 255 - (int.Parse(NetmaskParts[1]))) + "." + (int.Parse(StartIPParts[2]) + 255 - (int.Parse(NetmaskParts[2]))) + "." + (int.Parse(StartIPParts[3]) + 255 - (int.Parse(NetmaskParts[3])));
                    System.Console.WriteLine("StartIP: " + StartIP);
                    System.Console.WriteLine("EndIP : " + EndIP);
                    String ItemIP, ItemMAC, ItemName;
                    for (int o0 = int.Parse(StartIP.Split('.')[0]); o0 <= int.Parse(EndIP.Split('.')[0]); o0++)
                        for (int o1 = int.Parse(StartIP.Split('.')[1]); o1 <= int.Parse(EndIP.Split('.')[1]); o1++)
                            for (int o2 = int.Parse(StartIP.Split('.')[2]); o2 <= int.Parse(EndIP.Split('.')[2]); o2++)
                                for (int o3 = int.Parse(StartIP.Split('.')[3]); o3 <= int.Parse(EndIP.Split('.')[3]); o3++)
                                {
                                    if ((o3 == 0) || (o3 == 255))
                                        continue;

                                    IPAddress ipAddress = IPAddress.Parse(o0 + "." + o1 + "." + o2 + "." + o3);
                                    Ping(ipAddress);
                                    //String MAC = GetMacFromIP(IPAddress.Parse(o0 + "." + o1 + "." + o2 + "." + o3));
                                    //if (MAC == "00:00:00:00:00:00")
                                    //    continue;
                                    //ItemIP = o0 + "." + o1 + "." + o2 + "." + o3;
                                    //ItemMAC = GetMacFromIP(IPAddress.Parse(o0 + "." + o1 + "." + o2 + "." + o3));
                                    //String[] Item = new String[2];
                                    //Item[0] = ItemMAC;
                                    //Item[1] = ItemIP;                                                                                                                               /** You can add Item[] to any collection */
                                    //Console.WriteLine(Item[0] + " --> " + Item[1]);
                                }
                }
            }
            System.Console.WriteLine("Scan Ended");
            System.Console.ReadKey();
        }

        private static void Ping(IPAddress ipAddress)
        {
            Ping p = new Ping();
            PingReply rep = p.Send(ipAddress);
            if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Console.WriteLine(ipAddress + " is active. RTT: " + rep.RoundtripTime + " ms");
            }
            else
            {
                Console.WriteLine(ipAddress + " is missing.");
            }

            PingResult result = new PingResult()
            {
                Target = ipAddress.ToString(),
                RoundtripTime = TimeSpan.FromMilliseconds(rep.RoundtripTime),
                Status = rep.Status
            };

            PostResponseToLogAsync(result).Wait();
            Console.WriteLine("Log posted.");

        }

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Posts the response to the sLog.
        /// </summary>
        /// <param name="rep">The PING reply.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <devdoc>See https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient</devdoc>
        private static async Task PostResponseToLogAsync(PingResult result)
        {

            string data = JsonConvert.SerializeObject(result);

            Models.Log log = new sLog.Models.Log()
            {
                MimeType = "application/json",
                Data = data, 
                ContentType = result.GetType().FullName.ToString(),
                RegistrationId = 1,
                Timestamp = DateTime.Now
            };

            string jsonInString = JsonConvert.SerializeObject(log);
            StringContent stringContent = new StringContent(jsonInString, Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> stringTask = client.PostAsync("LogApi", stringContent);
            HttpResponseMessage msg = await stringTask;
            Console.WriteLine(msg);
        }
    }
}
