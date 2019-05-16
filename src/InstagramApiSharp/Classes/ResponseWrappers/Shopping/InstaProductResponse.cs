﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping
{
    public class InstaProductResponse
    {
        [JsonProperty("checkout_style")] public string CheckoutStyle { get; set; }

        [JsonProperty("current_price")] public string CurrentPrice { get; set; }

        [JsonProperty("current_price_stripped")]
        public string CurrentPriceStripped { get; set; }

        [JsonProperty("external_url")] public string ExternalUrl { get; set; }

        [JsonProperty("full_price")] public string FullPrice { get; set; }

        [JsonProperty("full_price_stripped")] public string FullPriceStripped { get; set; }

        [JsonProperty("has_viewer_saved")] public bool HasViewerSaved { get; set; }

        [JsonProperty("main_image")] public InstaProductImageResponse MainImage { get; set; }

        [JsonProperty("merchant")] public InstaMerchantResponse Merchant { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("price")] public string Price { get; set; }

        [JsonProperty("product_appeal_review_status")]
        public string ProductAppealReviewStatus { get; set; }

        [JsonProperty("product_id")] public long ProductId { get; set; }

        [JsonProperty("product_images")] public List<InstaProductImageResponse> ProductImages { get; set; }

        [JsonProperty("review_status")] public string ReviewStatus { get; set; }

        [JsonProperty("thumbnail_image")] public InstaProductImageResponse ThumbnailImage { get; set; }
    }
}
