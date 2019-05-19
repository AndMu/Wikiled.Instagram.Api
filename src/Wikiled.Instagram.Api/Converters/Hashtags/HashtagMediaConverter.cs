using System;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class HashtagMediaConverter : IObjectConverter<SectionMedia, SectionMediaListResponse>
    {
        private readonly ILogger<HashtagMediaConverter> logger;

        public HashtagMediaConverter(ILogger<HashtagMediaConverter> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public SectionMediaListResponse SourceObject { get; set; }

        public SectionMedia Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var media = new SectionMedia
            {
                AutoLoadMoreEnabled = SourceObject.AutoLoadMoreEnabled ?? false,
                MoreAvailable = SourceObject.MoreAvailable,
                NextMaxId = SourceObject.NextMaxId,
                NextMediaIds = SourceObject.NextMediaIds,
                NextPage = SourceObject.NextPage ?? 0
            };
            if (SourceObject.Sections != null)
            {
                foreach (var section in SourceObject.Sections)
                {
                    try
                    {
                        if (section.LayoutContent?.Medias == null)
                        {
                            continue;
                        }

                        foreach (var item in section.LayoutContent.Medias)
                        {
                            try
                            {
                                media.Medias.Add(InstaConvertersFabric.Instance.GetSingleMediaConverter(item.Media).Convert());
                            }
                            catch (Exception ex)
                            {
                                logger.LogWarning(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex.Message);
                    }
                }
            }

            if (SourceObject.PersistentSections?.Count > 0)
            {
                try
                {
                    foreach (var section in SourceObject.PersistentSections)
                    {
                        if (section.LayoutContent?.Related?.Count > 0)
                        {
                            foreach (var related in section.LayoutContent.Related)
                            {
                                try
                                {
                                    media.RelatedHashtags.Add(
                                        InstaConvertersFabric.Instance.GetRelatedHashtagConverter(related).Convert());
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return media;
        }
    }
}