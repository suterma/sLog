using Newtonsoft.Json;
using sLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLog.Demo.DataSource.NetScanner
{
    public class LogOfPingResult : LogOf<PingResult>
    {
        public LogOfPingResult(DateTime timeStamp, PingResult pingResult): base(timeStamp, pingResult)
        {
        }

        public LogOfPingResult()
        {
        }

        public static new IQueryable<LogOfPingResult> Filter(IQueryable<Log> filterable)
        {
            return filterable
                .Where(log => log.ContentType.Equals(typeof(PingResult).FullName))
                .Select(log => Create(log));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" PingResult"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public static new LogOfPingResult Create(Log log)

        {
            PingResult pingResult = JsonConvert.DeserializeObject<PingResult>(log.Data);


            PingResult item = new PingResult();
            LogOfPingResult logOfitem = new LogOfPingResult();
            logOfitem.TimeStamp = log.Timestamp;
            logOfitem.Item = pingResult;

            return logOfitem;
        }
    }
}
