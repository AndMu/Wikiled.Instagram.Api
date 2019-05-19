using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class HashtagMediaConverter : IObjectConverter<SectionMedia, SectionMediaListResponse>
    {
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
                        foreach (var item in section.LayoutContent.Medias)
                        {
                            try
                            {
                                media.Medias.Add(
                                    InstaConvertersFabric.Instance.GetSingleMediaConverter(item.Media).Convert());
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
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