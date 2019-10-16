using Newtonsoft.Json;
using sLog.Demo.DataSource.NetScanner;
using sLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace sLog.Analyzers
{
    /// <summary>An analyzer for ping logs.</summary>
    public class PingLogAnalyzer
    {
        //TODO Later Use Log<PingResult>

        public DateTime TimeStamp { get; set; }
        public string Target { get; set; }
        public IPStatus Status { get; set; }
        public TimeSpan RoundtripTime { get; set; }

        public static IQueryable<PingLogAnalyzer> Filter(IQueryable<Log> filterable)
        {
            return filterable
                .Where(log => log.ContentType.Equals("sLog.Demo.DataSource.NetScanner.PingResult"))
                .Select(log => new PingLogAnalyzer(log));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingLogAnalyzer"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public PingLogAnalyzer(Log log)
        {
            TimeStamp = log.Timestamp;
            PingResult pingResult = JsonConvert.DeserializeObject<PingResult>(log.Data);
            Target = pingResult.Target;
            Status = pingResult.Status;
            RoundtripTime = pingResult.RoundtripTime;
            
        }

        public PingLogAnalyzer()
        {
        }
    }


}
