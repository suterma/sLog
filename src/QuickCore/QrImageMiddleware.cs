using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing.QrCode;

namespace sLog
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
                //TODO get the QR code attributes

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

                //string qrCcode = qrcodeKeys.FirstOrDefault();
                //await context.Response.wri($"Branch used = {qrcode}");

                //TODO make more async style
                context.Response.ContentType = "image/png";
                RenderQrCode(context.Response.Body, text, width, height, margin);

            }
            else
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
        }

        private static void RenderQrCode(Stream output, string text, int width, int height, int margin)
        {
            //TODO use 1 function, combine taghelper and middleware
            ZXing.BarcodeWriterPixelData qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin,
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.L
                }
            };
            ZXing.Rendering.PixelData pixelData = qrCodeWriter.Write(text);
            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference   
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB   
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (MemoryStream ms = new MemoryStream())
            {
                System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image   
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG   
                //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                bitmap.Save(output, System.Drawing.Imaging.ImageFormat.Png);
                //output.TagName = "img";
                //output.Attributes.Clear();
                //output.Attributes.Add("width", width);
                //output.Attributes.Add("height", height);
                //output.Attributes.Add("alt", alt);
                //output.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
            }
        }
    }

   
}