using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Shopping;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping;

namespace Wikiled.Instagram.Api.Converters.Shopping
{
    internal class InstaProductInfoConverter : IObjectConverter<InstaProductInfo, InstaProductInfoResponse>
    {
        public InstaProductInfoResponse SourceObject { get; set; }

        public InstaProductInfo Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var productInfo = new InstaProductInfo
            {
                Product = InstaConvertersFabric.Instance.GetProductConverter(SourceObject.Product).Convert(),
                User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert()
            };
            if (SourceObject.OtherProductItems != null && SourceObject.OtherProductItems.Any())
            {
                foreach (var product in SourceObject.OtherProductItems)
                {
                    productInfo.OtherProducts.Add(InstaConvertersFabric.Instance.GetProductConverter(product).Convert());
                }
            }

            if (SourceObject.MoreFromBusiness != null)
            {
                productInfo.MoreFromBusiness = InstaConvertersFabric.Instance
                    .GetProductMediaListConverter(SourceObject.MoreFromBusiness)
                    .Convert();
            }

            return productInfo;
        }
    }
}