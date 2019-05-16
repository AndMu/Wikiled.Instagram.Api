using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
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
namespace ChallengeRequireExample
{
    public partial class InstaForm1 : Form
    {
        // There are two different type of challenge is exists!
        //  - 1. You receive challenge while you already logged in:
        //       "This is me" or "This is not me" option!
        //       If some suspecious login happend, this will promp up, and you should accept it to get rid of it
        //
        //       Use Task<IResult<InstaLoggedInChallengeDataInfo>> GetLoggedInChallengeDataInfoAsync() to get information like coordinate of
        //       login request and more data info
        //
        //       Use Task<IResult<bool>> AcceptChallengeAsync() to accept that you are the ONE that requests for login!


        //  - 2. You receive challenge while you calling LoginAsync

        // Note: new challenge require functions is very easy to use.
        // there are 5 functions I've added to IInstaApi for challenge require (checkpoint_endpoint)

        // here:
        // 1. Task<IResult<ChallengeRequireVerifyMethod>> GetChallengeRequireVerifyMethodAsync();
        // If your login needs challenge, first you should call this function.
        // Note: if you call this and SubmitPhoneRequired was true, you should sumbit phone number
        // with this function:
        // Task<IResult<ChallengeRequireSMSVerify>> SubmitPhoneNumberForChallengeRequireAsync();


        // 2. Task<IResult<ChallengeRequireSMSVerify>> RequestVerifyCodeToSMSForChallengeRequireAsync();
        // This function will send you verification code via SMS.


        // 3. Task<IResult<ChallengeRequireEmailVerify>> RequestVerifyCodeToEmailForChallengeRequireAsync();
        // This function will send you verification code via Email.


        // 4. Task<IResult<ChallengeRequireVerifyMethod>> ResetChallengeRequireVerifyMethodAsync();
        // Reset challenge require.
        // Example: if your account has phone number and email, and you request for email(or phone number)
        // and now you want to change it to another one, you should first call this function,
        // then you have to call GetChallengeRequireVerifyMethodAsync and after that you can change your method!!!


        // 5. Task<IResult<ChallengeRequireVerifyCode>> VerifyCodeForChallengeRequireAsync(string verifyCode);
        // Verify sms or email verification code for login.

        private const string AppName = "Challenge Required";
        private const string StateFile = "state.bin";
        private static IInstaApi api;
        private readonly Size challengeSize = new Size(432, 604);
        private readonly Size normalSize = new Size(432, 164);

        public InstaForm1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size = normalSize;
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            Size = normalSize;
            var userSession = new UserSessionData { UserName = txtUser.Text, Password = txtPass.Text };

            api = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(LogLevel.All))
                .SetRequestDelay(RequestDelay.FromSeconds(0, 1))
                // Session handler, set a file path to save/load your state/session data
                .SetSessionHandler(new FileSessionHandler { FilePath = StateFile })
                .Build();
            Text = $"{AppName} Connecting";
            //Load session
            LoadSession();
            if (!api.IsUserAuthenticated)
            {
                var logInResult = await api.LoginAsync();
                Debug.WriteLine(logInResult.Value);
                if (logInResult.Succeeded)
                {
                    GetFeedButton.Visible = true;
                    Text = $"{AppName} Connected";
                    // Save session 
                    SaveSession();
                }
                else
                {
                    if (logInResult.Value == InstaLoginResult.ChallengeRequired)
                    {
                        var challenge = await api.GetChallengeRequireVerifyMethodAsync();
                        if (challenge.Succeeded)
                        {
                            if (challenge.Value.SubmitPhoneRequired)
                            {
                                SubmitPhoneChallengeGroup.Visible = true;
                                Size = challengeSize;
                            }
                            else
                            {
                                if (challenge.Value.StepData != null)
                                {
                                    if (!string.IsNullOrEmpty(challenge.Value.StepData.PhoneNumber))
                                    {
                                        RadioVerifyWithPhoneNumber.Checked = false;
                                        RadioVerifyWithPhoneNumber.Visible = true;
                                        RadioVerifyWithPhoneNumber.Text = challenge.Value.StepData.PhoneNumber;
                                    }

                                    if (!string.IsNullOrEmpty(challenge.Value.StepData.Email))
                                    {
                                        RadioVerifyWithEmail.Checked = false;
                                        RadioVerifyWithEmail.Visible = true;
                                        RadioVerifyWithEmail.Text = challenge.Value.StepData.Email;
                                    }

                                    SelectMethodGroupBox.Visible = true;
                                    Size = challengeSize;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(challenge.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (logInResult.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        TwoFactorGroupBox.Visible = true;
                        Size = challengeSize;
                    }
                }
            }
            else
            {
                Text = $"{AppName} Connected";
                GetFeedButton.Visible = true;
            }
        }

        private async void SubmitPhoneChallengeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSubmitPhoneForChallenge.Text) ||
                    string.IsNullOrWhiteSpace(txtSubmitPhoneForChallenge.Text))
                {
                    MessageBox.Show("Please type a valid phone number(with country code).\r\ni.e: +989123456789",
                                    "ERR",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                var phoneNumber = txtSubmitPhoneForChallenge.Text;
                if (!phoneNumber.StartsWith("+"))
                {
                    phoneNumber = $"+{phoneNumber}";
                }

                var submitPhone = await api.SubmitPhoneNumberForChallengeRequireAsync(phoneNumber);
                if (submitPhone.Succeeded)
                {
                    VerifyCodeGroupBox.Visible = true;
                    SubmitPhoneChallengeGroup.Visible = false;
                }
                else
                {
                    MessageBox.Show(submitPhone.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SendCodeButton_Click(object sender, EventArgs e)
        {
            var isEmail = false;
            if (RadioVerifyWithEmail.Checked)
            {
                isEmail = true;
            }
            //if (RadioVerifyWithPhoneNumber.Checked)
            //    isEmail = false;

            try
            {
                // Note: every request to this endpoint is limited to 60 seconds                 
                if (isEmail)
                {
                    // send verification code to email
                    var email = await api.RequestVerifyCodeToEmailForChallengeRequireAsync();
                    if (email.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verify code to this email:\n{email.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show(email.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // send verification code to phone number
                    var phoneNumber = await api.RequestVerifyCodeToSmsForChallengeRequireAsync();
                    if (phoneNumber.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verify code to this phone number(it's end with this):\n{phoneNumber.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show(phoneNumber.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ResendButton_Click(object sender, EventArgs e)
        {
            var isEmail = false;
            if (RadioVerifyWithEmail.Checked)
            {
                isEmail = true;
            }

            try
            {
                // Note: every request to this endpoint is limited to 60 seconds                 
                if (isEmail)
                {
                    // send verification code to email
                    var email = await api.RequestVerifyCodeToEmailForChallengeRequireAsync(true);
                    if (email.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verification code one more time\r\nto this email:\n{email.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show(email.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // send verification code to phone number
                    var phoneNumber =
                        await api.RequestVerifyCodeToSmsForChallengeRequireAsync(true);
                    if (phoneNumber.Succeeded)
                    {
                        LblForSmsEmail.Text =
                            $"We sent verification code one more time\r\nto this phone number(it's end with this):{phoneNumber.Value.StepData.ContactPoint}";
                        VerifyCodeGroupBox.Visible = true;
                        SelectMethodGroupBox.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show(phoneNumber.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void VerifyButton_Click(object sender, EventArgs e)
        {
            txtVerifyCode.Text = txtVerifyCode.Text.Trim();
            txtVerifyCode.Text = txtVerifyCode.Text.Replace(" ", "");
            var regex = new Regex(@"^-*[0-9,\.]+$");
            if (!regex.IsMatch(txtVerifyCode.Text))
            {
                MessageBox.Show("Verification code is numeric!!!", "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtVerifyCode.Text.Length != 6)
            {
                MessageBox.Show("Verification code must be 6 digits!!!",
                                "ERR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Note: calling VerifyCodeForChallengeRequireAsync function, 
                // if user has two factor enabled, will wait 15 seconds and it will try to
                // call LoginAsync.

                var verifyLogin = await api.VerifyCodeForChallengeRequireAsync(txtVerifyCode.Text);
                if (verifyLogin.Succeeded)
                {
                    // you are logged in sucessfully.
                    VerifyCodeGroupBox.Visible = SelectMethodGroupBox.Visible = false;
                    Size = challengeSize;
                    GetFeedButton.Visible = true;
                    // Save session
                    SaveSession();
                    Text = $"{AppName} Connected";
                }
                else
                {
                    VerifyCodeGroupBox.Visible = SelectMethodGroupBox.Visible = false;
                    // two factor is required
                    if (verifyLogin.Value == InstaLoginResult.TwoFactorRequired)
                    {
                        TwoFactorGroupBox.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show(verifyLogin.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EX", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Debug.WriteLine(twoFactorLogin.Value);
            if (twoFactorLogin.Succeeded)
            {
                // connected
                // save session
                SaveSession();
                Size = challengeSize;
                TwoFactorGroupBox.Visible = false;
                GetFeedButton.Visible = true;
                Text = $"{AppName} Connected";
                Size = normalSize;
            }
            else
            {
                MessageBox.Show(twoFactorLogin.Info.Message, "ERR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void GetFeedButton_Click(object sender, EventArgs e)
        {
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

            var topicalExplore =
                await api.FeedProcessor.GetTopicalExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));

            if (topicalExplore.Succeeded == false)
            {
                if (topicalExplore.Info.ResponseType == InstaResponseType.ChallengeRequired)
                {
                    var challengeData = await api.GetLoggedInChallengeDataInfoAsync();
                    // Do something to challenge data, if you want!

                    var acceptChallenge = await api.AcceptChallengeAsync();
                    // If Succeeded was TRUE, you can continue to your work!
                }
            }
            else
            {
                var sb = new StringBuilder();
                var sb2 = new StringBuilder();
                sb2.AppendLine("Like 5 Media>");
                foreach (var item in topicalExplore.Value.Medias.Take(5))
                {
                    // like media...
                    var liked = await api.MediaProcessor.LikeMediaAsync(item.Identifier);
                    sb2.AppendLine($"{item.Identifier} liked? {liked.Succeeded}");
                }

                sb.AppendLine("Explore categories: " + topicalExplore.Value.Clusters.Count);
                var ix = 1;
                foreach (var cluster in topicalExplore.Value.Clusters)
                {
                    sb.AppendLine($"#{ix++} {cluster.Name}");
                }

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Explore tv channels: " + topicalExplore.Value.TvChannels.Count);
                sb.AppendLine();
                sb.AppendLine();

                sb.AppendLine("Explore Feeds Result: " + topicalExplore.Succeeded);
                foreach (var media in topicalExplore.Value.Medias)
                {
                    sb.AppendLine(InstaDebugUtils.PrintMedia("Feed media", media));
                }

                RtBox.Text = sb2 + Environment.NewLine + Environment.NewLine + Environment.NewLine;

                RtBox.Text += sb.ToString();
                RtBox.Visible = true;
                Size = challengeSize;
            }


            //// old explore page
            //var x = await InstaApi.FeedProcessor.GetExploreFeedAsync(PaginationParameters.MaxPagesToLoad(1));
            //if (x.Succeeded == false)
            //{
            //    if (x.Info.ResponseType == ResponseType.ChallengeRequired)
            //    {
            //        var challengeData = await InstaApi.GetLoggedInChallengeDataInfoAsync();
            //        // Do something to challenge data, if you want!

            //        var acceptChallenge = await InstaApi.AcceptChallengeAsync();
            //        // If Succeeded was TRUE, you can continue to your work!
            //    }
            //}
            //else
            //{
            //    StringBuilder sb = new StringBuilder();
            //    StringBuilder sb2 = new StringBuilder();
            //    sb2.AppendLine("Like 5 Media>");
            //    foreach (var item in x.Value.Medias.Take(5))
            //    {
            //        // like media...
            //        var liked = await InstaApi.MediaProcessor.LikeMediaAsync(item.InstaIdentifier);
            //        sb2.AppendLine($"{item.InstaIdentifier} liked? {liked.Succeeded}");
            //    }

            //    sb.AppendLine("Explore Feeds Result: " + x.Succeeded);
            //    foreach (var media in x.Value.Medias)
            //    {
            //        sb.AppendLine(DebugUtils.PrintMedia("Feed media", media));
            //    }
            //    RtBox.Text = sb2.ToString() + Environment.NewLine + Environment.NewLine + Environment.NewLine;

            //    RtBox.Text += sb.ToString();
            //    RtBox.Visible = true;
            //    Size = ChallengeSize;
            //}
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
    }

    public static class InstaDebugUtils
    {
        public static string PrintMedia(string header, InstaMedia media)
        {
            var content = $"{header}: {media.Caption?.Text.Truncate(30)}, {media.Code}";
            Debug.WriteLine(content);
            return content;
        }

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}