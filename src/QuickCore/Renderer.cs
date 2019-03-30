using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace QuickCore
{
    public static class Renderer
    {
        /// <summary>
        /// Gets the pixel data for the given QR atributes.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="margin">The margin.</param>
        /// <returns></returns>
        public static ZXing.Rendering.PixelData GetPixelData(string text, int width, int height, int margin = 0)
        {
                ZXing.BarcodeWriterPixelData qrCodeWriter = new ZXing.BarcodeWriterPixelData
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = margin,
                        ErrorCorrection = ErrorCorrectionLevel.L
                    }
                };
                ZXing.Rendering.PixelData pixelData = qrCodeWriter.Write(text);
                return pixelData;            
        }

        /// <summary>
        /// Draws the bitmap using the given pixel data.
        /// </summary>
        /// <param name="pixelData">The pixel data.</param>
        /// <param name="bitmap">The bitmap.</param>
        public static void DrawBitmap(ZXing.Rendering.PixelData pixelData, System.Drawing.Bitmap bitmap)
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
        }
    }
}

