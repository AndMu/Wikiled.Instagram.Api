using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Logic.Builder;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace FacebookLoginExample
{
    public partial class InstaForm1 : Form
    {
        private const string AppName = "Facebook login example";

        private const string StateFile = "state.bin";

        private const int InternetCookieHttponly = 0x2000;
        // Facebook examples
        // Facebook login is not a built-in implements in InstagramApiSharp but you can
        // use it easily to login with Facebook.
        // Please read all comments one by one to know how can you add it to your projects.

        // Note 1: if you in Iran, you cannot test this example without VPN.
        // only VPN works for winform/wpf apps. TunnelPlus or SSL Tunnel won't work.

        private IInstaApi api;

        public InstaForm1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FacebookWebBrowser.Dock = DockStyle.Fill;
        }

        private async void FacebookLoginButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(1500);
            // visible fb web browser
            FacebookWebBrowser.Visible = true;
            // suppress script errors
            FacebookWebBrowser.ScriptErrorsSuppressed = true;
            try
            {
                // remove handler
                FacebookWebBrowser.DocumentCompleted -= FacebookWebBrowserDocumentCompleted;
            }
            catch
            {
            }

            // add handler
            FacebookWebBrowser.DocumentCompleted += FacebookWebBrowserDocumentCompleted;

            // Every time we want to login with another facebook account, we need to clear
            // all cached and cookies for facebook addresses.
            // WebBrowser control uses Internet Explorer so we need to clean up.
            InstaWebBrowserHelper.ClearForSpecificUrl(InstaFbHelper.FacebookAddressWithWwwAddress.ToString());
            InstaWebBrowserHelper.ClearForSpecificUrl(InstaFbHelper.FacebookAddress.ToString());
            InstaWebBrowserHelper.ClearForSpecificUrl(InstaFbHelper.FacebookMobileAddress.ToString());

            // wait 3.5 second
            Thread.Sleep(3500);

            var facebookLoginUri = InstaFbHelper.GetFacebookLoginUri();
            var userAgent = InstaFbHelper.GetFacebookUserAgent();

            FacebookWebBrowser.Navigate(facebookLoginUri,
                                        null,
                                        null,
                                        string.Format("\r\nUser-Agent: {0}\r\n", userAgent));

            do
            {
                Application.DoEvents();
                Thread.Sleep(1);
            } while (FacebookWebBrowser.ReadyState != WebBrowserReadyState.Complete);
        }

        private async void FacebookWebBrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            try
            {
                var html = FacebookWebBrowser.DocumentText;
                if (InstaFbHelper.IsLoggedIn(html))
                {
                    var cookies = GetUriCookies(args.Url);
                    var fbToken = InstaFbHelper.GetAccessToken(html);

                    api = BuildApi();
                    Text = $"{AppName} Connecting";
                    var loginResult = await api.LoginWithFacebookAsync(fbToken, cookies);

                    if (loginResult.Succeeded)
                    {
                        Text = $"{AppName} Connected";
                        GetFeedButton.Visible = true;
                        SaveSession();
                    }
                    else
                    {
                        switch (loginResult.Value)
                        {
                            case InstaLoginResult.BadPassword:
                                MessageBox.Show("Wrong Password");
                                break;
                            case InstaLoginResult.ChallengeRequired:
                            case InstaLoginResult.TwoFactorRequired:
                                MessageBox.Show("You must combine Challenge Example to your project");
                                break;
                            default:
                                MessageBox.Show($"ERR: {loginResult.Value}\r\n{loginResult.Info.Message}");
                                break;
                        }

                        Text = $"{AppName} ERROR";
                    }
                }
            }
            catch
            {
            }
        }

        private async void GetFeedButtonClick(object sender, EventArgs e)
        {
            // Note2: A RichTextBox control added to show you some of feeds.

            if (api == null)
            {
                MessageBox.Show("Login first.");
                return;
            }

            if (!api.IsUserAuthenticated)
            {
                MessageBox.Show("Login first.");
                return;
            }

            var x = await api.FeedProcessor.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));

            if (x.Succeeded)
            {
                var sb = new StringBuilder();
                var sb2 = new StringBuilder();
                sb2.AppendLine("Like 5 Media>");
                foreach (var item in x.Value.Medias.Take(5))
                {
                    // like media...
                    var liked = await api.MediaProcessor.LikeMediaAsync(item.Identifier);
                    sb2.AppendLine($"{item.Identifier} liked? {liked.Succeeded}");
                }

                sb.AppendLine(("Explore Feeds Result: " + x.Succeeded).Output());
                foreach (var media in x.Value.Medias)
                {
                    sb.AppendLine(InstaDebugUtils.PrintMedia("Feed media", media));
                }

                RtBox.Text = sb2 + Environment.NewLine + Environment.NewLine + Environment.NewLine;

                RtBox.Text += sb.ToString();
                RtBox.Visible = true;
            }
        }

        private IInstaApi BuildApi()
        {
            return InstaApiBuilder.CreateBuilder()
                .SetUser(UserSessionData.ForUsername("FAKEUSERNAME").WithPassword("FAKEPASS"))
                .UseLogger(new DebugLogger(LogLevel.All))
                .SetRequestDelay(RequestDelay.FromSeconds(0, 1))
                // Session handler, set a file path to save/load your state/session data
                .SetSessionHandler(new FileSessionHandler { FilePath = StateFile })
                .Build();
        }

        private void LoadSession()
        {
            api?.SessionHandler?.Load();

            //// Old load session
            //try
            //{
            //    if (File.Exists(StateFile))
            //    {
            //        Debug.WriteLine("Loading state from file");
            //        using (var fs = File.OpenRead(StateFile))
            //        {
            //            InstaApi.LoadStateDataFromStream(fs);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }

        private void SaveSession()
        {
            if (api == null)
            {
                return;
            }

            if (!api.IsUserAuthenticated)
            {
                return;
            }

            api.SessionHandler.Save();

            //// Old save session 
            //var state = InstaApi.GetStateDataAsStream();
            //using (var fileStream = File.Create(StateFile))
            //{
            //    state.Seek(0, SeekOrigin.Begin);
            //    state.CopyTo(fileStream);
            //}
        }

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(string url,
                                                      string cookieName,
                                                      StringBuilder cookieData,
                                                      ref int size,
                                                      int dwFlags,
                                                      IntPtr lpReserved);

        public static string GetUriCookies(Uri uri)
        {
            var cookies = "";
            var datasize = 8192 * 16;
            var cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(),
                                     null,
                                     cookieData,
                                     ref datasize,
                                     InternetCookieHttponly,
                                     IntPtr.Zero))
            {
                if (datasize < 0)
                {
                    return cookies;
                }

                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null,
                    cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                {
                    return cookies;
                }
            }

            if (cookieData.Length > 0)
            {
                cookies = cookieData.ToString();
            }

            return cookies;
        }
    }

    public static class InstaDebugUtils
    {
        public static string PrintMedia(string header, InstaMedia media)
        {
            var content = $"{header}: {media.Caption?.Text.Truncate(30)}, {media.Code}";
            content.Output();
            return content;
        }

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}