﻿using Microsoft.AspNetCore.Http;
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
        public IActionResult Index(ConnectionStringModel connectionStringModel)
        {
            return View(connectionStringModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult StoreAndShowTables([Bind("ConnectionString")] ConnectionStringModel connectionStringModel)
        public IActionResult StoreAndShowTables(ConnectionStringModel connectionStringModel)
        {
            if (ModelState.IsValid)
            {
                //Connection string was provided, so redirect to tables display
                return RedirectToAction("Index", "Tables");
            }

            return View("Index", connectionStringModel);
        }
    }
}

