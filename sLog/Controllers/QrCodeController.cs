using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sLog.Controllers
{
    /// <summary>An API Controller, that produces a QR Code Image from route data.</summary>
    /// <remarks>
    ///   <para>Call this API as follows:    
    ///   <list type="bullet">
    ///     <item>
    /// "text" is the text to display in the QR code
    /// </item>
    ///     <item>
    /// "size" is the image size</item>
    ///     <item>"margin" is the margin around the code</item>
    ///   </list>
    ///   <code>https://yourdomain/api/qrcode/text/size/margin</code>
    ///   </para>
    /// </remarks>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    [Route("api/qrcode")]
    [ApiController]
    public class QrCodeApiController : ControllerBase
    {
        // GET: api/qrcode/text/size
        /// <summary>
        /// Gets the QR code according to the provided route data, including the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="size">The size (width x height), in (screen) pixels. Optional, default ist 100.</param>
        /// <param name="margin">The margin (quiet zone), in QR pixels. Optional, default ist 5.</param>
        /// <returns></returns>
        [HttpGet("{text}/{size:int?}/{margin:int?}")]
        public async Task<IActionResult> GetQrCode([FromRoute] string text, [FromRoute] int? size = 100, [FromRoute] int? margin = 5)
        {
            //TODO move to the QuickCore assembly and import using Aplication Parts

            //Render the QR image
            ZXing.Rendering.PixelData pixelData = QuickCore.Renderer.GetPixelData(text, size.Value, size.Value, margin.Value);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference   
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB   
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (MemoryStream ms = new MemoryStream())
            {
                QuickCore.Renderer.DrawBitmap(pixelData, bitmap);

                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.GetBuffer(), "image/png");
            }
        }

        //TODO us a different controller (QrAppendixcontroller?) or different route 

    }
}