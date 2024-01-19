using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Flow.Launcher.Plugin.QrCodeGenerator;

public class ShowQRCodePanel : UserControl
{
    private readonly string _content;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="content"></param>
    public ShowQRCodePanel(string content)
    {
        _content = content;
        
        Loaded += MyLoadedRoutedEventHandler;
    }

    private void MyLoadedRoutedEventHandler(object sender, RoutedEventArgs e)
    {
        var imageView = new Image
        {
            Source = QrCodeUtil.CreateQrCode<BitmapImage>(_content)
        };
        AddChild(imageView);
    }
}