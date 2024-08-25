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
    public class QrCodeGenerator : IPlugin, IContextMenu, IPluginI18n
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
                        Title = _context.API.GetTranslation("qr_code_generator_input_qr_text"),
                        SubTitle = _context.API.GetTranslation("qr_code_generator_preview_qrcode"),
                        IcoPath = IconPath,
                        AutoCompleteText = $"{query.ActionKeyword} ",
                        Action = _ =>
                        {
                            _context.API.ChangeQuery($"{query.ActionKeyword} ");
                            return false;
                        }
                    },
                    new()
                    {
                        Title = _context.API.GetTranslation("qr_code_generator_input_qr_file_path"),
                        SubTitle = _context.API.GetTranslation("qr_code_generator_parse_qrcode_file"),
                        IcoPath = IconPath,
                        AutoCompleteText = $"{query.ActionKeyword} @",
                        Action = _ =>
                        {
                            _context.API.ChangeQuery($"{query.ActionKeyword} @");
                            return false;
                        }
                    }
                };
            }

            var list = new List<Result>();
            var qrCodeItem = new Result
            {
                Title = _context.API.GetTranslation("qr_code_generator_preview_qrcode"),
                SubTitle = content,
                IcoPath = IconPath,
                PreviewPanel = new Lazy<UserControl>(() => new ShowQRCodePanel(_context, content)),
                ContextData = content,
                Action = (c) => ShowImage(content)
            };
            list.Add(qrCodeItem);

            if (!content.StartsWith("@"))
                return list;
            var codeFile = query.FirstSearch[1..];
            if (!File.Exists(codeFile))
                return list;

            var resolve = QrCodeUtil.ResolveQrCodeFile(codeFile);
            if (resolve != null)
            {
                list.Add(new Result
                {
                    Title = string.Format(_context.API.GetTranslation("qr_code_generator_qrcode_file_content"),
                        codeFile),
                    SubTitle = resolve,
                    IcoPath = IconPath,
                    Action = (c) =>
                    {
                        _context.API.CopyToClipboard(resolve);
                        return true;
                    }
                });
            }

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
                                // _context.API.ShowMsg("copy success");
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
                    Title = _context.API.GetTranslation("qr_code_generator_copy_file"),
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

        public string GetTranslatedPluginTitle()
        {
            return _context.API.GetTranslation("qr_code_generator_plugin_title");
        }

        public string GetTranslatedPluginDescription()
        {
            return _context.API.GetTranslation("qr_code_generator_plugin_description");
        }
    }
}