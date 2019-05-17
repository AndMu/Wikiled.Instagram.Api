using System;
using System.Linq;
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
    internal class InstaLocationSample : IDemoSample
    {
        private readonly IInstaApi api;

        public InstaLocationSample(IInstaApi instaApi)
        {
            api = instaApi;
        }

        public async Task DoShow()
        {
            // search for related locations near location with latitude = 55.753923, logitude = 37.620940
            // additionaly you can specify search query or just empty string
            var result = await api.LocationProcessor.SearchLocationAsync(55.753923, 37.620940, "square").ConfigureAwait(false);
            Console.WriteLine($"Loaded {result.Value.Count} locations");
            var firstLocation = result.Value?.FirstOrDefault();
            if (firstLocation == null)
            {
                return;
            }

            Console.WriteLine($"Loading feed for location: name={firstLocation.Name}; id={firstLocation.ExternalId}.");

            var locationStories = await api.LocationProcessor.GetLocationStoriesAsync(long.Parse(firstLocation.ExternalId)).ConfigureAwait(false);

            Console.WriteLine(locationStories.Succeeded
                                  ? $"Loaded {locationStories.Value.Items?.Count} stoires for location"
                                  : $"Unable to load location '{firstLocation.Name}' stories");
        }
    }
}