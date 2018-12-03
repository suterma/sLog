using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sLog.Analyzers;
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
            IQueryable<PingLogAnalyzer> pingLogs = PingLogAnalyzer.Filter(logs);
            return View(await pingLogs.ToListAsync());
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
            IQueryable<PingLogAnalyzer> pingLogs = PingLogAnalyzer
                .Filter(logs)
                .Where(pingLog => pingLog.Target == targetIp);

            var allPingLogs = await pingLogs.ToListAsync();

            //TODO use chart instead of this pseudo average
            var averagedPingLog = new PingLogAnalyzer()
            {
                TimeStamp = allPingLogs.FirstOrDefault().TimeStamp,
                Target = targetIp,
                Status = 0,
                RoundtripTime = TimeSpan.FromTicks((long)allPingLogs.Average(ping => ping.RoundtripTime.Ticks))

            };
            return View(averagedPingLog);
        }  
    }
}
