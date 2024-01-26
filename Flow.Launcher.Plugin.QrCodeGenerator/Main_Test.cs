using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flow.Launcher.Plugin.QrCodeGenerator;

public class Main_Test
{
    public static void Main()
    {
        var bit = QrCodeUtil.CreateQrCode<BitmapImage>("test");
        Console.WriteLine($"test result =  {bit}");
    }
}