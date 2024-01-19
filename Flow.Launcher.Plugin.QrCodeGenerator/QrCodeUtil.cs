using System.IO;
using System.Windows.Media.Imaging;
using SkiaSharp;
using SkiaSharp.QrCode;

namespace Flow.Launcher.Plugin.QrCodeGenerator
{
    public class QrCodeUtil
    {
        public static BitmapImage CreateBitmapImage(string content,
            int width = 512, int height = 512, ECCLevel eccLevel = ECCLevel.L)
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

            var imageBytes = data.ToArray();
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}