using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaSuggestionItemConverter : IObjectConverter<InstaSuggestionItem, InstaSuggestionItemResponse>
    {
        public InstaSuggestionItemResponse SourceObject { get; set; }

        public InstaSuggestionItem Convert()
        {
            var suggestion = new InstaSuggestionItem
            {
                Caption = SourceObject.Caption ?? string.Empty,
                IsNewSuggestion = SourceObject.IsNewSuggestion,
                SocialContext = SourceObject.SocialContext ?? string.Empty,
                User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert(),
                Algorithm = SourceObject.Algorithm ?? string.Empty,
                Icon = SourceObject.Icon ?? string.Empty,
                Value = SourceObject.Value ?? 0,
                Uuid = SourceObject.Uuid
            };
            try
            {
                if (SourceObject.LargeUrls != null && SourceObject.LargeUrls?.Length > 0)
                {
                    foreach (var url in SourceObject.LargeUrls)
                    {
                        suggestion.LargeUrls.Add(url);
                    }
                }

                if (SourceObject.MediaIds != null && SourceObject.MediaIds?.Length > 0)
                {
                    foreach (var url in SourceObject.MediaIds)
                    {
                        suggestion.MediaIds.Add(url);
                    }
                }

                if (SourceObject.ThumbnailUrls != null && SourceObject.ThumbnailUrls?.Length > 0)
                {
                    foreach (var url in SourceObject.ThumbnailUrls)
                    {
                        suggestion.ThumbnailUrls.Add(url);
                    }
                }

                if (SourceObject.MediaInfos != null && SourceObject.MediaInfos?.Count > 0)
                {
                    foreach (var item in SourceObject.MediaInfos)
                    {
                        try
                        {
                            var converted = InstaConvertersFabric.Instance.GetSingleMediaConverter(item).Convert();
                            suggestion.MediaInfos.Add(converted);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }

            return suggestion;
        }
    }
}