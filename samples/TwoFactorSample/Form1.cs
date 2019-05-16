using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Logger;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Logic.Builder;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace TwoFactorSample
{
    public partial class InstaForm1 : Form
    {
        private const string AppName = "Two Factor";
        private const string StateFile = "state.bin";

        private static IInstaApi api;

        //307, 280
        private readonly Size normalSize = new Size(307, 150);
        private readonly Size twoFactorSize = new Size(307, 280);

        public InstaForm1()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            Size = normalSize;
            var userSession = new UserSessionData { UserName = txtUsername.Text, Password = txtPassword.Text };

            api = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(LogLevel.All))
                .SetRequestDelay(RequestDelay.FromSeconds(0, 1))
                .Build();
            Text = $"{AppName} Connecting";
            LoadSession();

            if (!api.IsUserAuthenticated)
            {
                var logInResult = await api.LoginAsync();
                Debug.WriteLine(logInResult.Value);
                if (logInResult.Succeeded)
                {
                    Text = $"{AppName} Connected";
                    // Save session 
                    SaveSession();
                }
                else
                {
                    // two factor is required
                    if (logInResult.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        // open a box so user can send two factor code
                        Size = twoFactorSize;
                    }
                }
            }
            else
            {
                Text = $"{AppName} Connected";
            }
        }

        private async void TwoFactorButton_Click(object sender, EventArgs e)
        {
            if (api == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(txtTwoFactorCode.Text))
            {
                MessageBox.Show("Please type your two factor code and then press Auth button.",
                                "",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // send two factor code
            var twoFactorLogin = await api.TwoFactorLoginAsync(txtTwoFactorCode.Text);
            if (twoFactorLogin.Succeeded)
            {
                // connected
                // save session
                SaveSession();
                Text = $"{AppName} Connected";
                Size = normalSize;
            }
            else
            {
                MessageBox.Show(twoFactorLogin.Info.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSession()
        {
            try
            {
                if (File.Exists(StateFile))
                {
                    Debug.WriteLine("Loading state from file");
                    using (var fs = File.OpenRead(StateFile))
                    {
                        api.LoadStateDataFromStream(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void SaveSession()
        {
            if (api == null)
            {
                return;
            }

            var state = api.GetStateDataAsStream();
            using (var fileStream = File.Create(StateFile))
            {
                state.Seek(0, SeekOrigin.Begin);
                state.CopyTo(fileStream);
            }
        }
    }
}