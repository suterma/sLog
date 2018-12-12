using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sLog.Filters;
using sLog.Models;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StoreAndShowTables([Bind("ConnectionString")] ConnectionStringModel connectionStringModel)
        {
            if (ModelState.IsValid)
            {
                //available, store and redirect to tables
                HttpContext.Session.SetString("DbBrowserConnectionString", connectionStringModel.ConnectionString);

                return RedirectToAction("Index", "Tables");
            }

            return View("Index");
        }
    }
}

