using System;
using System.Threading.Tasks;
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
    internal class InstaCollectionSample : IDemoSample
    {
        private readonly IInstaApi api;

        public InstaCollectionSample(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            // get all collections of current user
            var collections =
                await api.CollectionProcessor.GetCollectionsAsync(PaginationParameters.MaxPagesToLoad(1));
            Console.WriteLine($"Loaded {collections.Value.Items.Count} collections for current user");
            foreach (var instaCollection in collections.Value.Items)
            {
                Console.WriteLine(
                    $"Collection: name={instaCollection.CollectionName}, id={instaCollection.CollectionId}");
            }
        }
    }
}