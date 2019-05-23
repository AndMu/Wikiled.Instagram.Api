using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Net.Client;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Tags
{
    public class WebSmartTags : ISmartTags
    {
        private readonly ILogger<WebSmartTags> logger;

        private readonly IResilientApiClient client;

        public WebSmartTags(ILogger<WebSmartTags> logger, IGenericClientFactory clientFactory)
        {
            if (clientFactory == null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            client = clientFactory.ConstructResilient(new Uri("https://d212rkvo8t62el.cloudfront.net/tag/"));
        }

        public async Task<HashTagData[]> Get(HashTagData tag)
        {
            logger.LogDebug("Get Web Tags: {0}", tag);
            var result = await client.GetRequest<SmartResults>($"{tag.Text}", CancellationToken.None).ConfigureAwait(false);
            return result.Results.Select(
                item =>
                {
                    var tagItem = HashTagData.FromText(item.Tag);
                    tagItem.Rank = item.Rank;
                    tagItem.Relevance = item.Relevance;
                    tagItem.MediaCount = item.MediaCount;
                    return tagItem;
                }).ToArray();
        }
    }
}
