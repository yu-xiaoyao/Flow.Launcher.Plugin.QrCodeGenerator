using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Flow.Launcher.Plugin.QrCodeGenerator;

public partial class QRCodeForm : Window
{
    public QRCodeForm(string content)
    {
        Title = "二维码";
        Width = 640.0;
        Height = 640.0;
        
        ResizeMode = ResizeMode.NoResize;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        
        Topmost = true;
        ShowInTaskbar = false;

        // Opacity = 0.9;
        // AllowsTransparency = true;
        // Background = new SolidColorBrush(Colors.White);

        WindowStyle = WindowStyle.None;

        Deactivated += Window_Deactivated;
        KeyDown += Esc_Exit_KeyDown;

        AddChild(new ShowQRCodePanel(content));
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        Close();
    }

    private void Esc_Exit_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true; // cancels the window close    
        Hide(); // Programmatically hides the window
    }
}