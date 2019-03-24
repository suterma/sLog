using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sLog.Controllers
{
    /// <summary>
    /// An API Controller, that produces a QR Code Image from route data.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        // GET: api/QrCode/text
        [HttpGet("{text}")]
        public async Task<IActionResult> GetQrCode([FromRoute] string text)
        {
            //TODO return a proper QR code image
            //TODO move to the QuickCore assembly and import using Aplication Parts
            //read https://docs.microsoft.com/en-us/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-2.2
            return Ok("image//TODO");
        }
    }
}