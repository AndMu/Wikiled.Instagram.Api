using System;
using System.Linq;
using System.Threading.Tasks;
using Examples.Utils;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Logic;

/////////////////////////////////////////////////////////////////////
////////////////////// IMPORTANT NOTE ///////////////////////////////
// Please check wiki pages for more information:
// https://github.com/ramtinak/InstagramApiSharp/wiki
////////////////////// IMPORTANT NOTE ///////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace Examples.Samples
{
    internal class InstaBasics : IDemoSample
    {
        /// <summary>
        ///     Config values
        /// </summary>
        private static readonly int MaxDescriptionLength = 20;

        private readonly IInstaApi api;

        public InstaBasics(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            // get currently logged in user
            var currentUser = await api.GetCurrentUserAsync();
            Console.WriteLine(
                $"Logged in: username - {currentUser.Value.UserName}, full name - {currentUser.Value.FullName}");

            // get followers of user 'elonmusk'
            var followers = await api.UserProcessor.GetUserFollowersAsync("elonmusk",
                                                                               PaginationParameters.MaxPagesToLoad(5)
                                                                                   .StartFromMaxId(
                                                                                       "AQAC8w90POWyM7zMjHWmO9vsZNL_TuLp6FR506_C_y3fUAjlCclrIDI2RdSGvur5UjLrq4Cq7NJN8QUhHG-vpbT6pCLB5X9crDxBOHUEuNJ4fA"));
            Console.WriteLine($"Count of followers [elonmusk]:{followers.Value.Count}");
            Console.WriteLine($"Next id will be: '{followers.Value.NextMaxId}'");

            // get self folling 
            var following =
                await api.UserProcessor.GetUserFollowingAsync(currentUser.Value.UserName,
                                                                   PaginationParameters.MaxPagesToLoad(5));
            Console.WriteLine($"Count of following [{currentUser.Value.UserName}]:{following.Value.Count}");

            // get self user's media, latest 5 pages
            var currentUserMedia =
                await api.UserProcessor.GetUserMediaAsync(currentUser.Value.UserName,
                                                               PaginationParameters.MaxPagesToLoad(5));
            if (currentUserMedia.Succeeded)
            {
                Console.WriteLine($"Media count [{currentUser.Value.UserName}]: {currentUserMedia.Value.Count}");
                foreach (var media in currentUserMedia.Value)
                {
                    InstaConsoleUtils.PrintMedia("Self media", media, MaxDescriptionLength);
                }
            }

            //get user time line feed, latest 5 pages
            var userFeed =
                await api.FeedProcessor.GetUserTimelineFeedAsync(PaginationParameters.MaxPagesToLoad(5));
            if (userFeed.Succeeded)
            {
                Console.WriteLine(
                    $"Feed items (in {userFeed.Value.MediaItemsCount} pages) [{currentUser.Value.UserName}]: {userFeed.Value.Medias.Count}");
                foreach (var media in userFeed.Value.Medias)
                {
                    InstaConsoleUtils.PrintMedia("Feed media", media, MaxDescriptionLength);
                }

                //like first 10 medias from user timeline feed
                foreach (var media in userFeed.Value.Medias.Take(10))
                {
                    var likeResult = await api.MediaProcessor.LikeMediaAsync(media.Identifier);
                    var resultString = likeResult.Value ? "liked" : "not liked";
                    Console.WriteLine($"Media {media.Code} {resultString}");
                }
            }

            // get tag feed, latest 5 pages
            var tagFeed =
                await api.FeedProcessor.GetTagFeedAsync("quadcopter", PaginationParameters.MaxPagesToLoad(5));
            if (tagFeed.Succeeded)
            {
                Console.WriteLine(
                    $"Tag feed items (in {tagFeed.Value.MediaItemsCount} pages) [{currentUser.Value.UserName}]: {tagFeed.Value.Medias.Count}");
                foreach (var media in tagFeed.Value.Medias)
                {
                    InstaConsoleUtils.PrintMedia("Tag feed", media, MaxDescriptionLength);
                }
            }
        }
    }
}