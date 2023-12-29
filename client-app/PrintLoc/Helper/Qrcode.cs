using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;

namespace PrintLoc.Helper
{
    public class Qrcode
    {
        public static BitmapImage GenerateQRCode(string content, int width, int height)
        {
            var barcodeWriter = new BarcodeWriter();
            var options = new QrCodeEncodingOptions
            {
                Width = width,
                Height = height
            };
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = options;

            var qrCode = barcodeWriter.Write(content);

            // Convert the QR code to a BitmapImage
            BitmapImage bitmapImage = new BitmapImage();
            using (var memory = new System.IO.MemoryStream())
            {
                qrCode.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
