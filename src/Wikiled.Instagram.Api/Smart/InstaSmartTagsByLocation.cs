using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
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

        public async Task<HashTagData[]> Get(Location location)
        {
            if (location == null)
            {
                log.LogInformation("Location is not set.");
                return new HashTagData[] { };
            }

            log.LogInformation("Extracting popular tags...");
            IResult<SectionMedia> topMedia = await instagram.LocationProcessor.GetTopLocationFeedsAsync(location.Pk, PaginationParameters.MaxPagesToLoad(1)).ConfigureAwait(false);
            if (!topMedia.Succeeded)
            {
                log.LogWarning("Top media query failed");
                return new HashTagData[]{};
            }

            return await smartTags.Get(topMedia.Value).ConfigureAwait(false);
        }
    }
}
