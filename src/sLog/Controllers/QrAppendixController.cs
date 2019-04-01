using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sLog.Controllers
{
    /// <summary>An API Controller, that produces a QR Code Image from route data.</summary>
    /// <remarks>
    ///   <para>Call this API by appending a "/qrcode/size/margin" at the end (as 'appendix') of an existing URL:    
    ///   <list type="bullet">
    ///     <item>
    /// The text is the URL withouth the "/qrcode/size/margin" part
    /// </item>
    ///     <item>
    /// "size" is the image size</item>
    ///     <item>"margin" is the margin around the code</item>
    ///   </list>
    ///   <code>https://yourdomain/yourroute/qrcode/size/margin</code>
    ///   </para>
    /// </remarks>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    /// <devdoc>
    /// <para>To have the end of the URL match (instead of a prefix), the asterisk variant of the regex matcher is used. The regex will match any URL that has a '/qrcode' segement, followed by any number of digit-only segments (the optional QR code parameters). See https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing#get-books-by-publication-date with the wildcard example at the end.</para>
    /// </devdoc>
    [Route("/{*url:regex(^.*qrcode(\\/\\d*)*$)}")]
    [ApiController]
    public class QrAppendixController : ControllerBase
    {
        /// <summary>
        /// Gets the QR code for the given matched URL.
        /// </summary>
        /// <remarks>
        /// The default size = 100, the default margin = 5
        /// </remarks>
        /// <param name="url">The URL's path from the request.</param>
        /// <returns></returns>
        /// <devdoc>The parameters can not be used, since the URL matcher already consumed the complete URL. We must get the QR parameters from the (relative) URL</devdoc>
        [HttpGet()]
        public async Task<IActionResult> GetQrCode([FromRoute] string url)
        {
            //TODO move to the QuickCore assembly and import using Application Parts
            var qrPostfix = url.Substring(url.IndexOf("qrcode"));
            var qrSegments = qrPostfix.Split('/');

            int size;
            if (!int.TryParse(qrSegments.Skip(1).FirstOrDefault(), out size)) {
                size = 100;
            }
            int margin;
            if (!int.TryParse(qrSegments.Skip(2).FirstOrDefault(), out margin)) {
                margin = 5;
            }

            //Get the target URL and use as text
            string targetPath = url.Substring(0, url.IndexOf("qrcode"));
            string text = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{targetPath}{HttpContext.Request.QueryString}";

            //Render the QR image
            ZXing.Rendering.PixelData pixelData = QuickCore.Renderer.GetPixelData(text, size, size, margin);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference   
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB   
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (MemoryStream ms = new MemoryStream()) {
                QuickCore.Renderer.DrawBitmap(pixelData, bitmap);

                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.GetBuffer(), "image/png");
            }
        }
    }
}