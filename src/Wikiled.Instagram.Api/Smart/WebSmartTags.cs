using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public class WebSmartTags : ISmartTags
    {
        private readonly ILogger<WebSmartTags> logger;

        public WebSmartTags(ILogger<WebSmartTags> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HashTagData[]> Get(HashTagData tag)
        {
            logger.LogDebug("Get");
            var client = new HttpClient();
            var query = client.GetAsync($"https://d212rkvo8t62el.cloudfront.net/tag/{tag.Text}");
            var responce = await query.ConfigureAwait(false);
            var text = await responce.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = SerializationHelper.DeserializeFromString<SmartResults>(text);
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
