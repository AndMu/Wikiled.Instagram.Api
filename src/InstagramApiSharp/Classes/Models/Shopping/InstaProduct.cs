using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Shopping
{
    public class InstaProduct
    {
        public string CheckoutStyle { get; set; }

        public string CurrentPrice { get; set; }

        public string CurrentPriceStripped { get; set; }

        public string ExternalUrl { get; set; }

        public string FullPrice { get; set; }

        public string FullPriceStripped { get; set; }

        public bool HasViewerSaved { get; set; }

        public List<InstaImage> MainImage { get; set; } = new List<InstaImage>();

        public InstaMerchant Merchant { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string ProductAppealReviewStatus { get; set; }

        public long ProductId { get; set; }

        public List<InstaImage> ProductImages { get; set; } = new List<InstaImage>();

        public string ReviewStatus { get; set; }

        public List<InstaImage> ThumbnailImage { get; set; } = new List<InstaImage>();
    }
}
