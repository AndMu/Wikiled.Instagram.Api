using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public class InstaSmartTags : ISmartTags
    {
        private readonly IInstaApi instagram;

        private readonly ILogger<InstaSmartTags> log;

        private readonly IMediaSmartTags smartTags;

        public InstaSmartTags(ILogger<InstaSmartTags> log, IInstaApi instagram, IMediaSmartTags smartTags)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.instagram = instagram ?? throw new ArgumentNullException(nameof(instagram));
            this.smartTags = smartTags ?? throw new ArgumentNullException(nameof(smartTags));
        }

        public async Task<HashTagData[]> Get(HashTagData tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            log.LogInformation("Extracting popular tags...");
            IResult<SectionMedia> topMedia = await instagram.HashtagProcessor.GetTopHashtagMediaListAsync(tag.Text, PaginationParameters.MaxPagesToLoad(1)).ConfigureAwait(false);
            if (!topMedia.Succeeded)
            {
                log.LogWarning("Top media query failed");
                return new HashTagData[] { };
            }

            return await smartTags.Get(topMedia.Value).ConfigureAwait(false);
        }
    }
}
