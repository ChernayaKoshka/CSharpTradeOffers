using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using CSharpTradeOffers;
using CSharpTradeOffers.Community;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using NReco.ImageGenerator;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace MiniProfileDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SteamUserHandler _userHandler = new SteamUserHandler(null);

        public MainWindow()
        {
            InitializeComponent();
        }

        public string GetInput(string question, string title)
        {
            string toReturn = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dialog = new InputDialogBox(question, title);
                toReturn = dialog.Result();
            });
            return toReturn;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            SteamId id = ParseId();
            if (id == null) return;

            string profileHtml = _userHandler.FetchMiniProfile(id);
            HtmlBox.Text = profileHtml;

            Bitmap profileBitmap = RepairBitmap(ConvertHtmlToImage(profileHtml));
            BitmapSource source = ToBitmapSource(profileBitmap);
            ImageBox.Source = source;

            if (DownloadAsHtmlCheckBox.IsChecked == true)
            {
                File.WriteAllText(id.SteamIdUlong + ".html", profileHtml);
            }
            if (DownloadAsImageCheckBox.IsChecked == true)
            {
                profileBitmap.Save(id.SteamIdUlong + ".png");
            }
        }

        private Bitmap ConvertHtmlToImage(string html)
        {
            byte[] pngBytes = new HtmlToImageConverter().GenerateImage(html, ImageFormat.Png);
            Bitmap bmp;
            using (var ms = new MemoryStream(pngBytes))
                bmp = new Bitmap(ms);
            return bmp;
        }

        private Bitmap RepairBitmap(Bitmap bmp)
        {
            Rectangle cropRectangle = new Rectangle(0, 0, 302, bmp.Height);
            return bmp.Clone(cropRectangle, bmp.PixelFormat);
        }

        private SteamId ParseId()
        {
            try
            {
                SteamId id = null;
                if (IdBox.Text.StartsWith("STEAM_"))
                {
                    id = new SteamId(IdBox.Text);
                }
                else
                {
                    ulong tryParseUlong;
                    if (ulong.TryParse(IdBox.Text, out tryParseUlong))
                    {
                        id = new SteamId(tryParseUlong);
                    }
                    else
                    {
                        string apiKey = GetInput(
                            "If you are trying to resolve a CustomURL, please enter in your API key. Please note this is not necessary for SteamID/SteamID64.",
                            "API key required!");
                        _userHandler = new SteamUserHandler(apiKey);
                        if (ulong.TryParse(_userHandler.ResolveVanityUrl(IdBox.Text).SteamId, out tryParseUlong))
                        {
                            id = new SteamId(tryParseUlong);
                        }
                    }
                }
                return id;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid SteamId, please try using a valid SteamID, SteamID64, or CustomURL.");
                return null;
            }
        }

        #region FromSO

        //Code within this region is credited to user "Alastair Pitts" in the thread http://stackoverflow.com/questions/94456/load-a-wpf-bitmapimage-from-a-system-drawing-bitmap

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="source">The source bitmap.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitSrc;
        }

        /// <summary>
        /// FxCop requires all Marshalled functions to be in a class called NativeMethods.
        /// </summary>
        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }

        #endregion
    }
}
