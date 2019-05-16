using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;

namespace Wikiled.Instagram.Api.Converters.Other
{
    internal class
        InstaTranslateContainerConverter : IObjectConverter<InstaTranslateList, InstaTranslateContainerResponse>
    {
        public InstaTranslateContainerResponse SourceObject { get; set; }

        public InstaTranslateList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var list = new InstaTranslateList();
            if (SourceObject.Translations != null && SourceObject.Translations.Any())
            {
                foreach (var item in SourceObject.Translations)
                {
                    list.Add(InstaConvertersFabric.Instance.GetSingleTranslateConverter(item).Convert());
                }
            }

            return list;
        }
    }
}