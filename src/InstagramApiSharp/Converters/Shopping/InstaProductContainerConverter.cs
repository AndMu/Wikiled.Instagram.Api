using System;

namespace Wikiled.Instagram.Api.Converters.Shopping
{
    internal class InstaProductContainerConverter : IObjectConverter<InstaProductTag, InstaProductContainerResponse>
    {
        public InstaProductContainerResponse SourceObject { get; set; }

        public InstaProductTag Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var productTag = new InstaProductTag
                             {
                                 Product = ConvertersFabric.Instance.GetProductConverter(SourceObject.Product).Convert()
                             };

            if (SourceObject.Position != null)
            {
                productTag.Position = new InstaPosition(SourceObject.Position[0], SourceObject.Position[1]);
            }

            return productTag;
        }
    }
}
