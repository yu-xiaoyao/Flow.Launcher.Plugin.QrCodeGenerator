using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Flow.Launcher.Plugin.QrCodeGenerator
{
    /// <summary>
    /// Flow plugin entrance
    /// </summary>
    public class QrCodeGenerator : IPlugin, IContextMenu
    {
        public static readonly string IconPath = "Images\\QrCodeGenerator-icon.png";

        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            var content = query.Search.TrimEnd();

            if (string.IsNullOrWhiteSpace(content))
            {
                // TIP
                return new List<Result>
                {
                    new()
                    {
                        Title = "Generate Text QR Code",
                        SubTitle = "Enter Text to Generate QR Code",
                        IcoPath = IconPath
                    },
                    new()
                    {
                        Title = "Resolve File QR Code",
                        SubTitle = "Resolve QR Code File with @FilePath",
                        IcoPath = IconPath
                    }
                };
            }

            var list = new List<Result>();

            if (content.StartsWith("@"))
            {
                var codeFile = query.FirstSearch[1..];
                if (File.Exists(codeFile))
                {
                    var resolve = QrCodeUtil.ResolveQrCodeFile(codeFile);
                    if (resolve != null)
                    {
                        list.Add(new Result
                        {
                            IcoPath = IconPath,
                            Title = $"Resolve QR Code File = {codeFile}",
                            SubTitle = resolve,
                            Action = (c) =>
                            {
                                _context.API.CopyToClipboard(resolve);
                                return true;
                            }
                        });
                    }
                }
            }


            var item1 = new Result
            {
                IcoPath = IconPath,
                Title = "Generate QR Code",
                // SubTitle = "Show QR Code by Enter OR Preview shortcut Key",
                SubTitle = content,
                PreviewPanel = new Lazy<UserControl>(() => new ShowQRCodePanel(_context, content)),
                ContextData = content,
                Action = (c) => ShowImage(content)
            };

            list.Add(item1);
            return list;
        }


        public List<Result> LoadContextMenus(Result selectedResult)
        {
            // _context.API.ShowMsg($"show data  = {selectedResult.ContextData.ToString()}");
            var filePath = QrCodeUtil.CreateQrCode<string>(selectedResult.ContextData as string);

            return new List<Result>
            {
                new()
                {
                    Action = _ =>
                    {
                        try
                        {
                            if (File.Exists(filePath))
                            {
                                CopyFileToClipboard(filePath);
                                _context.API.ShowMsg("copy success");
                                // use this will cause error
                                // _context.API.CopyToClipboard(filePath);
                            }
                        }
                        catch (Exception e)
                        {
                            var message = $"{filePath} -- {e.Message}";
                            _context.API.ShowMsg(message);
                        }

                        return false;
                    },
                    IcoPath = IconPath,
                    Title = "Copy QRCode File",
                    SubTitle = filePath
                }
            };
        }

        private bool ShowImage(string search)
        {
            new QRCodeForm(_context, search).Show();
            return true;
        }

        private static void CopyFileToClipboard(string filePath)
        {
            var stC = new StringCollection { filePath };
            var t = new Thread(() => { Clipboard.SetFileDropList(stC); });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
    }
}