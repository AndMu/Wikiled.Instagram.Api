using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Logic;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace Examples.Samples
{
    internal class InstaCommentMedia : IDemoSample
    {
        private readonly IInstaApi api;

        public InstaCommentMedia(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            var commentResult = await api.CommentProcessor.CommentMediaAsync("", "Hi there!").ConfigureAwait(false);
            Console.WriteLine(commentResult.Succeeded
                                  ? $"Comment created: {commentResult.Value.Pk}, text: {commentResult.Value.Text}"
                                  : $"Unable to create comment: {commentResult.Info.Message}");
        }
    }
}