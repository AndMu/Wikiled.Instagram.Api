using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping
{
    public class InstaProductInfoResponse : InstaDefault
    {
        [JsonProperty("more_from_business")] public InstaProductMediaListResponse MoreFromBusiness { get; set; }

        [JsonProperty("other_product_items")] public List<InstaProductResponse> OtherProductItems { get; set; }

        [JsonProperty("product_item")] public InstaProductResponse Product { get; set; }

        [JsonProperty("user")] public InstaUserShortResponse User { get; set; }
    }
}
