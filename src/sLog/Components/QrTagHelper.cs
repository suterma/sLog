using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing.QrCode;


namespace sLog.Components
{
    /// <summary>A TagHelper for adding QR-codes to HTML.</summary>
    /// <remarks>
    ///   <para>See the Example for a QR-Code to the current page</para>
    /// </remarks>
    /// <example>&lt;qrcode alt="QR Code" content="@($"{Context.Request.Scheme}://{Context.Request.Host}{Context.Request.Path}{Context.Request.QueryString}")" /&gt;
    /// <code></code></example>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper"/>
    [HtmlTargetElement("qrcode")]
    public class QrTagHelper : TagHelper
    {
        /// <summary>
        /// Synchronously executes the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string qrText = context.AllAttributes["content"].Value.ToString();
            string alt = context.AllAttributes["alt"].Value.ToString();

            //Get sizes
            int width;
            if (!int.TryParse(context.AllAttributes["width"]?.Value.ToString(), out width))
            {
                width = 250; //Default
            }
            int height;
            if (!int.TryParse(context.AllAttributes["height"]?.Value.ToString(), out height))
            {
                height = 250; //Default
            }
            int margin = 0;
            RenderQrCode(output, qrText, alt, width, height, margin);
        }

        private static void RenderQrCode(TagHelperOutput output, string qrText, string alt, int width, int height, int margin)
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
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                output.TagName = "img";
                output.Attributes.Clear();
                output.Attributes.Add("width", width);
                output.Attributes.Add("height", height);
                output.Attributes.Add("alt", alt);
                output.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
            }
        }
    }
}