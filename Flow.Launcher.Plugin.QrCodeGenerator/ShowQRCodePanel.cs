using System.Windows.Controls;

namespace Flow.Launcher.Plugin.QrCodeGenerator;

public class ShowQRCodePanel : UserControl
{
    public ShowQRCodePanel(string search)
    {
        var imageView = new Image
        {
            Source = QrCodeUtil.CreateBitmapImage(search, 1024, 1024)
        };
        AddChild(imageView);
    }
}