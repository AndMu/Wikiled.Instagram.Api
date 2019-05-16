using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Highlight;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight;

namespace Wikiled.Instagram.Api.Converters.Highlights
{
    internal class
        InstaHighlightShortListConverter : IObjectConverter<InstaHighlightShortList, InstaHighlightShortListResponse>
    {
        public InstaHighlightShortListResponse SourceObject { get; set; }

        public InstaHighlightShortList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var highlight = new InstaHighlightShortList
            {
                MaxId = SourceObject.MaxId ?? string.Empty,
                MoreAvailable = SourceObject.MoreAvailable,
                ResultsCount = SourceObject.ResultsCount
            };
            if (SourceObject.Items != null && SourceObject.Items.Any())
            {
                foreach (var item in SourceObject.Items)
                {
                    try
                    {
                        highlight.Items.Add(InstaConvertersFabric.Instance.GetSingleHighlightShortConverter(item).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return highlight;
        }
    }
}