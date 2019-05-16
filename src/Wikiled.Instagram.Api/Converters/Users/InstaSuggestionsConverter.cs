using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaSuggestionsConverter : IObjectConverter<InstaSuggestions, InstaSuggestionUserContainerResponse>
    {
        public InstaSuggestionUserContainerResponse SourceObject { get; set; }

        public InstaSuggestions Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var suggest = new InstaSuggestions
            {
                MoreAvailable = SourceObject.MoreAvailable, NextMaxId = SourceObject.MaxId ?? string.Empty
            };
            try
            {
                if (SourceObject.SuggestedUsers != null &&
                    SourceObject.SuggestedUsers?.Suggestions != null &&
                    SourceObject.SuggestedUsers?.Suggestions?.Count > 0)
                {
                    suggest.SuggestedUsers = InstaConvertersFabric.Instance
                        .GetSuggestionItemListConverter(SourceObject.SuggestedUsers.Suggestions)
                        .Convert();
                }

                if (SourceObject.NewSuggestedUsers != null &&
                    SourceObject.NewSuggestedUsers?.Suggestions != null &&
                    SourceObject.NewSuggestedUsers?.Suggestions?.Count > 0)
                {
                    suggest.NewSuggestedUsers = InstaConvertersFabric.Instance
                        .GetSuggestionItemListConverter(SourceObject.NewSuggestedUsers.Suggestions)
                        .Convert();
                }
            }
            catch
            {
            }

            return suggest;
        }
    }
}