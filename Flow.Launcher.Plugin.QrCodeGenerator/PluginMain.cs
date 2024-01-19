using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.QrCodeGenerator
{
    public class QrCodeGenerator : IPlugin
    {
        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            List<Result> list = new List<Result>();

            var item1 = new Result()
            {
                Title = "Enter Text to Generate QR Code",
                SubTitle = "Show QR Code by Enter OR Preview shortcut Key",
                IcoPath = "icon.png",
                PreviewPanel = new Lazy<UserControl>(() => new ShowQRCodePanel(query.Search)),
                Action = (c) => ShowImage(query.Search)
            };

            list.Add(item1);


            return list;
        }

        public bool ShowImage(string search)
        {
            new QRCodeForm(search).Show();
            return true;
        }
    }
}