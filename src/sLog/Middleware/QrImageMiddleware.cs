using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing.QrCode;

namespace sLog
{
    /// <summary>
    /// Handles the QR image creation, if requested by the request query.
    /// </summary>
    internal class QrImageMiddleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            string qrCode = context.Request.Query["qrcode"].FirstOrDefault();
            if (qrCode != null)
            {
                //string qrCcode = qrcodeKeys.FirstOrDefault();
                //await context.Response.wri($"Branch used = {qrcode}");

                //TODO make more async style
                       context.Response.ContentType = "image/png";
                RenderQrCode(context.Response.Body, "test", "altTest", 200, 200, 0);

            }
            else
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
        }

        private static void RenderQrCode(Stream output, string qrText, string alt, int width, int height, int margin)
        {
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
            ZXing.Rendering.PixelData pixelData = qrCodeWriter.Write(qrText);
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