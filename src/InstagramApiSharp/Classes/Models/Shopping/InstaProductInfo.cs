using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Shopping
{
    public class InstaProductInfo
    {
        public InstaProductMediaList MoreFromBusiness { get; set; }

        public List<InstaProduct> OtherProducts { get; set; } = new List<InstaProduct>();

        public InstaProduct Product { get; set; }

        public InstaUserShort User { get; set; }
    }
}
