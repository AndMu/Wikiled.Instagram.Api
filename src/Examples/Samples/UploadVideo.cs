using System;
using System.Threading.Tasks;
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
    internal class InstaUploadVideo : IDemoSample
    {
        private readonly IInstaApi api;

        public InstaUploadVideo(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            var video = new InstaVideoUpload
            {
                // leave zero, if you don't know how height and width is it.
                Video = new InstaVideo(@"c:\video1.mp4", 0, 0),
                VideoThumbnail = new InstaImage(@"c:\video thumbnail 1.jpg", 0, 0)
            };
            // Add user tag (tag people)
            video.UserTags.Add(new InstaUserTagVideoUpload { Username = "rmt4006" });
            var result = await api.MediaProcessor.UploadVideoAsync(video, "ramtinak");
            Console.WriteLine(result.Succeeded
                                  ? $"Media created: {result.Value.Pk}, {result.Value.Caption}"
                                  : $"Unable to upload video: {result.Info.Message}");
        }
    }
}