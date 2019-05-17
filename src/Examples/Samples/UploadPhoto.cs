using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Logic;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace Examples.Samples
{
    internal class InstaUploadPhoto : IDemoSample
    {
        private readonly IInstaApi api;

        public InstaUploadPhoto(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            var mediaImage = new InstaImageUpload
            {
                // leave zero, if you don't know how height and width is it.
                Height = 1080, Width = 1080, Uri = @"c:\someawesomepicture.jpg"
            };
            // Add user tag (tag people)
            mediaImage.UserTags.Add(new UserTagUpload { Username = "rmt4006", X = 0.5, Y = 0.5 });
            var result = await api.MediaProcessor.UploadPhotoAsync(mediaImage, "someawesomepicture");
            Console.WriteLine(result.Succeeded
                                  ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                                  : $"Unable to upload photo: {result.Info.Message}");
        }

        public async Task DoShowWithProgress()
        {
            var mediaImage = new InstaImageUpload
            {
                // leave zero, if you don't know how height and width is it.
                Height = 1080, Width = 1080, Uri = @"c:\someawesomepicture.jpg"
            };
            // Add user tag (tag people)
            mediaImage.UserTags.Add(new UserTagUpload { Username = "rmt4006", X = 0.5, Y = 0.5 });
            // Upload photo with progress
            var result =
                await api.MediaProcessor.UploadPhotoAsync(UploadProgress, mediaImage, "someawesomepicture");
            Console.WriteLine(result.Succeeded
                                  ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                                  : $"Unable to upload photo: {result.Info.Message}");
        }

        private void UploadProgress(InstaUploaderProgress progress)
        {
            if (progress == null)
            {
                return;
            }

            Console.WriteLine($"{progress.Name} {progress.UploadState}");
        }
    }
}