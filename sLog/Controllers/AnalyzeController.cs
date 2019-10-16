using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sLog.Demo.DataSource.NetScanner;
using sLog.Models;

namespace sLog.Controllers
{
    public class AnalyzeController : Controller
    {
        private readonly sLogContext _context;

        public AnalyzeController(sLogContext context)
        {
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            DbSet<Log> logs = _context.Log;
            IQueryable<LogOfPingResult> pingLogs = LogOfPingResult.Filter(logs);
            return View(await pingLogs
                .ToListAsync()                
                );
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            string targetIp = id;
            if (targetIp == null)
            {
                return NotFound();
            }

            DbSet<Log> logs = _context.Log;
            IQueryable<LogOf<PingResult>> pingLogs = LogOf<PingResult>
                .Filter(logs)
                .Where(pingLog => pingLog.Item.Target == targetIp)
                ;

            List<LogOf<PingResult>> allPingLogs = await pingLogs.ToListAsync();

            //TODO use chart instead of this pseudo average

            var averagedPingResult = new PingResult();
            averagedPingResult.RoundtripTime = TimeSpan.FromTicks((long)allPingLogs.Average(ping => ping.Item.RoundtripTime.Ticks));
            averagedPingResult.Status = 0;
            averagedPingResult.Target = targetIp;


            var averagedPingLog = new LogOfPingResult(allPingLogs.Last().TimeStamp, averagedPingResult);
            return View(averagedPingLog);
        }  
    }
}
