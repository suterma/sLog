using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickCore
{
    /// <summary>Handles the QR image creation, if triggerd by the request query.</summary>
    /// <remarks>
    ///   <para>To request a QR image, simply add query parameters to the URL:
    /// </para>
    ///   <list type="bullet">
    ///     <item>'qrcode' (without value), to trigger the creation
    /// </item>
    ///     <item>'content' as content of the QR code  (an empty text ist used as default)
    /// </item>
    ///     <item>'widht' and 'height' (in pixesl) for the size of the image (250 is used as default)
    /// </item>
    ///   </list>
    /// </remarks>
    public class QrImageMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="QrImageMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public QrImageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the creation asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            string qrCode = context.Request.Query["qrcode"].FirstOrDefault();
            if (qrCode != null)
            {
                //Get the QR code attributes
                string text = context.Request.Query["content"].FirstOrDefault();

                //Get sizes (specific width and height override size)
                int size = 250; //Default
                int width;
                int height;
                if (int.TryParse(context.Request.Query["size"].FirstOrDefault(), out size))
                {
                    width = size;
                    height = size;
                }

                if (!int.TryParse(context.Request.Query["width"].FirstOrDefault(), out width))
                {
                    width = size; //Default
                }
                if (!int.TryParse(context.Request.Query["height"].FirstOrDefault(), out height))
                {
                    height = size; //Default
                }
                int margin = 0;

                //Render the QR image
                context.Response.ContentType = "image/png";
                ZXing.Rendering.PixelData pixelData = Renderer.GetPixelData(text, width, height, margin);

                // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference   
                // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB   
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                using (MemoryStream ms = new MemoryStream())
                {
                    Renderer.DrawBitmap(pixelData, bitmap);

                    // save to stream as PNG   
                    bitmap.Save(context.Response.Body, System.Drawing.Imaging.ImageFormat.Png);
                }

            }
            else
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
        }




    }


}