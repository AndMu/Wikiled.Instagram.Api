using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Extensions;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Smart.Caption;
using Wikiled.Instagram.Api.Smart.Data;
using Wikiled.Text.Analysis.Similarity;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Instagram.Api.Smart.Tags
{
    public class SimilarMediaTags : ISimilarMediaTags
    {
        private readonly ILogger<SimilarMediaTags> log;

        private readonly IInstaApi instagram;

        private readonly ICaptionHandler captionHandler;

        private readonly ISimilarityDetector similarity;

        public SimilarMediaTags(ILogger<SimilarMediaTags> log,
                                IInstaApi instagram,
                                ICaptionHandler captionHandler,
                                ISimilarityDetector similarity)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.captionHandler = captionHandler ?? throw new ArgumentNullException(nameof(captionHandler));
            this.similarity = similarity ?? throw new ArgumentNullException(nameof(similarity));
            this.instagram = instagram;
        }

        public async Task<HashTagData[]> Get(SmartCaption caption)
        {
            log.LogDebug("Finding similar posts - [{0}]", caption.Original);
            var tags = caption.Tags.ToString();
            if (tags.Length < 3)
            {
                return new HashTagData[]{};
            }

            foreach (var tag in caption.Tags)
            {
                var topMedia = await instagram.Resilience.WebPolicy
                    .ExecuteAsync(
                        () => ResultExtension.UnWrap(() => instagram.HashtagProcessor.GetTopHashtagMediaListAsync(tag.Text, PaginationParameters.MaxPagesToLoad(1)), log))
                    .ConfigureAwait(false);

                foreach (var media in topMedia.Medias)
                {
                    var text = media.Caption?.Text;
                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    var smart = captionHandler.Extract(text);
                    var bag = BagOfWords.Create(smart.Tags.Select(item => item.Tag).ToArray());
                    similarity.Register(bag);
                }
            }

            var result = similarity.FindSimilar(BagOfWords.Create(caption.Tags.Select(item => item.Tag).ToArray()));
            return result.SelectMany(
                             item => item.Document.Words.Select(
                                 x =>
                                 {
                                     var tag = HashTagData.FromTag(x.Text);
                                     tag.Relevance = (int)(100 * item.Similarity);
                                     return tag;
                                 }))
                         .ToArray();
        }
    }
}
