using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sLog.Filters;

namespace sLog.Controllers.DbBrowser
{
    [EvaluatePerformanceFilter]
    public class ConnectionStringController : Controller
    {
        /// <summary>
        ///     Provides the view to collect the connection string.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Index(string connectionString)
        //{
        //    if (string.IsNullOrEmpty(connectionString))
        //        return View();

        //    //available, store
        //    HttpContext.Session.SetString("DbBrowserConnectionString", connectionString);

        //    IEnumerable<string> tableNames = GetTableNames(connectionString);

        //    return View();
        //}

        [HttpPost]
        public IActionResult Store(string connectionString)
        {
            if (ModelState.IsValid)
            {
                //available, store and redirect to tables
                HttpContext.Session.SetString("DbBrowserConnectionString", connectionString);

                return RedirectToAction("Index", "Tables");
            }

            //not available, throw error
            //TODO connectionString = HttpContext.Session.GetString("DbBrowserConnectionString");
            return View("Index");
        }
    }
}

