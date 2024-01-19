using System;
using System.IO;
using System.Windows.Media.Imaging;
using SkiaSharp;
using SkiaSharp.QrCode;

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
        /// <param name="eccLevel"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateQrCode<T>(string content,
            int width = 1024, int height = 1024, ECCLevel eccLevel = ECCLevel.H)
        {
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
            else
            {
                var imageBytes = data.ToArray();
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(imageBytes);
                bitmapImage.EndInit();

                return (T)(object)bitmapImage;
            }
        }
    }
}