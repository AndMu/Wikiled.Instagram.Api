using System;
using System.IO;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Logic;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace Examples.Samples
{
    internal class InstaAccount : IDemoSample
    {
        private readonly IInstaApi api;

        public InstaAccount(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            // get currently logged in user
            var currentUser = await api.GetCurrentUserAsync();
            Console.WriteLine(
                $"Logged in: username - {currentUser.Value.UserName}, full name - {currentUser.Value.FullName}");

            Console.WriteLine("See Samples/Account.cs to see how it's works");
            Console.WriteLine("Accounts functions: ");
            Console.WriteLine(@"EditProfileAsync
GetRequestForEditProfileAsync
SetNameAndPhoneNumberAsync
RemoveProfilePictureAsync
ChangeProfilePictureAsync
GetStorySettingsAsync
EnableSaveStoryToGalleryAsync
DisableSaveStoryToGalleryAsync
EnableSaveStoryToArchiveAsync
DisableSaveStoryToArchiveAsync
AllowStorySharingAsync
AllowStoryMessageRepliesAsync
CheckUsernameAsync
GetSecuritySettingsInfoAsync
DisableTwoFactorAuthenticationAsync
SendTwoFactorEnableSmsAsync
TwoFactorEnableAsync
SendConfirmEmailAsync
SendSmsCodeAsync
VerifySmsCodeAsync");
        }

        public async void EditProfile()
        {
            var name = "Ramtin Jokar"; // leave null if you don't want to change it
            GenderType? gender = GenderType.Male; // leave null if you don't want to change it
            var email = "Ramtinak@live.com"; // leave null if you don't want to change it
            var url = ""; // leave empty if you have no site/blog | leave null if you don't want to change it
            var phone = "+989171234567"; // leave null if you don't want to change it
            var biography = "C# Programmer\n\nIRaN/FARS/KaZeRouN"; // leave null if you don't want to change it
            var newUsername = ""; // leave empty if you don't want to change your username

            var result =
                await api.AccountProcessor.EditProfileAsync(name,
                                                                 biography,
                                                                 url,
                                                                 email,
                                                                 phone,
                                                                 gender,
                                                                 newUsername);

            if (result.Succeeded)
            {
                Console.WriteLine("Profile changed");
                Console.WriteLine("Username: " + result.Value.Username);
                Console.WriteLine("FullName: " + result.Value.FullName);
                Console.WriteLine("Biography: " + result.Value.Biography);
                Console.WriteLine("Email: " + result.Value.Email);
                Console.WriteLine("PhoneNumber: " + result.Value.PhoneNumber);
                Console.WriteLine("Url: " + result.Value.ExternalUrl);
                Console.WriteLine("Gender: " + result.Value.Gender);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Error while editing profile: " + result.Info.Message);
            }
        }

        public async void ChangeProfilePicture()
        {
            var picturePath = @"c:\someawesomepicture.jpg";
            // note: only JPG and JPEG format will accept it in instagram!
            var pictureBytes = File.ReadAllBytes(picturePath);

            var result = await api.AccountProcessor.ChangeProfilePictureAsync(pictureBytes);
            if (result.Succeeded)
            {
                Console.WriteLine("New profile picture: " + result.Value.ProfilePicUrl);
            }
            else
            {
                Console.WriteLine("Error while changing profile picture: " + result.Info.Message);
            }
        }

        public async void RemoveProfilePicture()
        {
            var result = await api.AccountProcessor.RemoveProfilePictureAsync();
            if (result.Succeeded)
            {
                Console.WriteLine("Profile picture removed.");
            }
            else
            {
                Console.WriteLine("Error while removing profile picture: " + result.Info.Message);
            }
        }

        public async void SetNameAndPhoneNumber()
        {
            var name = "Ramtin Jokar";
            var phone = "+989171234567";
            var result = await api.AccountProcessor.SetNameAndPhoneNumberAsync(name, phone);
            if (result.Succeeded)
            {
                Console.WriteLine("Name and phone number changed");
            }
            else
            {
                Console.WriteLine("Error while changing name and phone number: " + result.Info.Message);
            }
        }

        public async void StorySettings()
        {
            var storySettings = await api.AccountProcessor.GetStorySettingsAsync();
            if (storySettings.Succeeded)
            {
                Console.WriteLine("Story settings");
                Console.WriteLine("Save story to gallery(camera roll): " + storySettings.Value.SaveToCameraRoll);
                Console.WriteLine("Save story to archive: " + storySettings.Value.ReelAutoArchive);
                Console.WriteLine("Allow message replies: " + storySettings.Value.MessagePrefsType);
                Console.WriteLine("Allow sharing story: " + storySettings.Value.AllowStoryReshare);

                // enable/disable save story to gallery(camera roll)
                await api.AccountProcessor.EnableSaveStoryToGalleryAsync();
                await api.AccountProcessor.DisableSaveStoryToGalleryAsync();

                // enable/disable save story to archive
                await api.AccountProcessor.EnableSaveStoryToArchiveAsync();
                await api.AccountProcessor.DisableSaveStoryToArchiveAsync();

                // allow/disallow sharing stories
                await api.AccountProcessor.AllowStorySharingAsync();
                // await _instaApi.AccountProcessor.AllowStorySharingAsync(false);

                // allow story message replies
                await api.AccountProcessor.AllowStoryMessageRepliesAsync(InstaMessageRepliesType.Everyone);
                // await _instaApi.AccountProcessor.AllowStoryMessageRepliesAsync(InstaMessageRepliesType.Following);
                // await _instaApi.AccountProcessor.AllowStoryMessageRepliesAsync(InstaMessageRepliesType.Off);
            }
        }

        public async void CheckUsernameAvailable()
        {
            var username = "rmt4006";

            var result = await api.AccountProcessor.CheckUsernameAsync(username);
            if (result.Succeeded)
            {
                if (result.Value.Available)
                {
                    Console.WriteLine($"'{username}' available.");
                }
                else
                {
                    Console.WriteLine($"'{username}' taken.");
                }
            }
            else
            {
                Console.WriteLine("Error while checking username available: " + result.Info.Message);
            }
        }

        public async void SecuritySettingsAndTwoFactor()
        {
            var result = await api.AccountProcessor.GetSecuritySettingsInfoAsync();
            if (result.Succeeded)
            {
                Console.WriteLine("Security settings information");
                Console.WriteLine("PhoneNumber: " + result.Value.PhoneNumber);
                Console.WriteLine("NationalNumber: " + result.Value.NationalNumber);
                Console.WriteLine("CountryCode: " + result.Value.CountryCode);
                Console.WriteLine("IsTwoFactorEnabled: " + result.Value.IsTwoFactorEnabled);
                Console.WriteLine("IsPhoneConfirmed: " + result.Value.IsPhoneConfirmed);
                Console.WriteLine("BackupCodes: " + string.Join("\t", result.Value.BackupCodes));

                // disable two factor authentication
                await api.AccountProcessor.DisableTwoFactorAuthenticationAsync();


                var phoneNumber = result.Value.PhoneNumber; // "+989171234567"
                // send enable two factor sms authentication 
                await api.AccountProcessor.SendTwoFactorEnableSmsAsync(phoneNumber);

                // enable(verify) two factor authentication
                var verificationCode = "40061373";
                await api.AccountProcessor.TwoFactorEnableAsync(phoneNumber, verificationCode);


                // send sms code to verify account with sms
                await api.AccountProcessor.SendSmsCodeAsync(phoneNumber);

                // verify sms code for verify account with sms
                await api.AccountProcessor.VerifySmsCodeAsync(phoneNumber, "13734006");


                // send confirm email for verify account with email
                await api.AccountProcessor.SendConfirmEmailAsync();
            }
        }
    }
}