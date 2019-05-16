using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class
        InstaSuggestionItemListConverter : IObjectConverter<InstaSuggestionItemList, InstaSuggestionItemListResponse>
    {
        public InstaSuggestionItemListResponse SourceObject { get; set; }

        public InstaSuggestionItemList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var suggest = new InstaSuggestionItemList();

            if (SourceObject != null && SourceObject?.Count > 0)
            {
                foreach (var item in SourceObject)
                {
                    try
                    {
                        var convertedItem = InstaConvertersFabric.Instance.GetSuggestionItemConverter(item).Convert();
                        suggest.Add(convertedItem);
                    }
                    catch
                    {
                    }
                }
            }

            return suggest;
        }
    }
}