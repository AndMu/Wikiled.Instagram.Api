using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Extensions;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Tags
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

            log.LogInformation("Extracting popular tags for [{0}]...", tag);
            try
            {
                var topMedia = await instagram.Resilience.WebPolicy
                                              .ExecuteAsync(
                                                  () => ResultExtension.UnWrap(() => instagram.HashtagProcessor.GetTopHashtagMediaListAsync(tag.Text, PaginationParameters.MaxPagesToLoad(10)), log))
                                              .ConfigureAwait(false);
                return await smartTags.Get(topMedia).ConfigureAwait(false);
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
