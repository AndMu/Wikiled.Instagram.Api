using System;

namespace Wikiled.Instagram.Api.Converters.Other
{
    internal class InstaTranslateContainerConverter : IObjectConverter<InstaTranslateList, InstaTranslateContainerResponse>
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
                    list.Add(ConvertersFabric.Instance.GetSingleTranslateConverter(item).Convert());
                }
            }

            return list;
        }
    }
}
