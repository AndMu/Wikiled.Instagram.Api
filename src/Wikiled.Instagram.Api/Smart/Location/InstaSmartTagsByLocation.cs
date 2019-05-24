using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Extensions;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Smart.Data;
using Wikiled.Instagram.Api.Smart.Tags;

namespace Wikiled.Instagram.Api.Smart.Location
{
    public class InstaSmartTagsByLocation : ISmartTagsByLocation
    {
        private readonly IInstaApi instagram;

        private readonly ILogger<InstaSmartTagsByLocation> log;

        private readonly IMediaSmartTags smartTags;

        public InstaSmartTagsByLocation(ILogger<InstaSmartTagsByLocation> log, IInstaApi instagram, IMediaSmartTags smartTags)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.instagram = instagram ?? throw new ArgumentNullException(nameof(instagram));
            this.smartTags = smartTags ?? throw new ArgumentNullException(nameof(smartTags));
        }

        public async Task<HashTagData[]> Get(Classes.Models.Location.Location location)
        {
            if (location == null)
            {
                log.LogInformation("Location is not set.");
                return new HashTagData[] { };
            }

            log.LogInformation("Extracting popular tags...");
            try
            {
                var topMedia = await instagram.Resilience.WebPolicy
                                              .ExecuteAsync(
                                                  () => ResultExtension.UnWrap(() => instagram.LocationProcessor.GetTopLocationFeedsAsync(location.Pk, PaginationParameters.MaxPagesToLoad(1)), log))
                                              .ConfigureAwait(false);
                //return await smartTags.Get(topMedia).ConfigureAwait(false);
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                log.LogError(e, "Failed");
            }

            log.LogWarning("Top media query failed");
            return new HashTagData[] { };
        }
    }
}
