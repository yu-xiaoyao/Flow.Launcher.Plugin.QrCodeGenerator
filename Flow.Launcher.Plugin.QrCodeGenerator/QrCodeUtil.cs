using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;


namespace Flow.Launcher.Plugin.QrCodeGenerator
{
    public class QrCodeUtil
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="qLevel"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateQrCode<T>(string content,
            int width = 1024, int height = 1024, uint qLevel = 0)
        {
            // Lib SkiaSharp
            /*var eccLevel = ECCLevel.H;
            var values = Enum.GetValues(typeof(ECCLevel));
            if (qLevel < values.Length)
            {
                eccLevel = (ECCLevel)values.GetValue(qLevel)!;
            }

            using var generator = new QRCodeGenerator();

            // Generate QrCode
            var qr = generator.CreateQrCode(content, eccLevel);

            // Render to canvas
            var info = new SKImageInfo(width, height);
            using var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            canvas.Render(qr, info.Width, info.Height);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            var type = typeof(T);
            if (type == typeof(string))
            {
                var path = Path.GetTempFileName() + ".png";
                using var stream = File.OpenWrite(path);
                data.SaveTo(stream);
                return (T)(object)path;
            }

            var imageBytes = data.ToArray();
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();

            return (T)(object)bitmapImage;
            */

            // zxing System.Drawing
            QrCodeEncodingOptions options = new()
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
                ErrorCorrection = ErrorCorrectionLevel.forBits(Convert.ToInt32(qLevel))
            };

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };

            var bitmap = writer.Write(content);
            var type = typeof(T);
            if (type == typeof(string))
            {
                var path = Path.GetTempFileName() + ".png";
                using var stream = File.OpenWrite(path);
                bitmap.Save(stream, ImageFormat.Png);
                return (T)(object)path;
            }

            // MemoryStream can not with using ...
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();

            return (T)(object)bitmapImage;


            // ImageSharp
            /*QrCodeEncodingOptions options = new()
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
                ErrorCorrection = ErrorCorrectionLevel.forBits(Convert.ToInt32(qLevel))
            };

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };
            var pixelData = writer.Write(content);
            Console.WriteLine($"pixelData = {pixelData.Pixels.Length}");
            var type = typeof(T);
            if (type == typeof(string))
            {
                var path = @"C:\Users\farben\Desktop\qr-test.png";
                // var path = Path.GetTempFileName() + ".png";
                using var stream = File.OpenWrite(path);
                stream.Write(pixelData.Pixels);
                return (T)(object)path;
            }

            using var ms = new MemoryStream();
            ms.Write(pixelData.Pixels);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();

            return (T)(object)bitmapImage;*/
        }
    }
}