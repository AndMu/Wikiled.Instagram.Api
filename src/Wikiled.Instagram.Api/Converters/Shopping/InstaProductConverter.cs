﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Shopping;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping;

namespace Wikiled.Instagram.Api.Converters.Shopping
{
    internal class InstaProductConverter : IObjectConverter<InstaProduct, InstaProductResponse>
    {
        public InstaProductResponse SourceObject { get; set; }

        public InstaProduct Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var product = new InstaProduct
            {
                CheckoutStyle = SourceObject.CheckoutStyle,
                CurrentPrice = SourceObject.CurrentPrice,
                ExternalUrl = SourceObject.ExternalUrl,
                FullPrice = SourceObject.FullPrice,
                HasViewerSaved = SourceObject.HasViewerSaved,
                Merchant = InstaConvertersFabric.Instance.GetMerchantConverter(SourceObject.Merchant).Convert(),
                Name = SourceObject.Name,
                Price = SourceObject.Price,
                ProductId = SourceObject.ProductId,
                ReviewStatus = SourceObject.ReviewStatus,
                CurrentPriceStripped = SourceObject.CurrentPriceStripped,
                FullPriceStripped = SourceObject.FullPriceStripped,
                ProductAppealReviewStatus = SourceObject.ProductAppealReviewStatus
            };
            if (SourceObject.MainImage?.Images?.Candidates?.Count > 0)
            {
                foreach (var image in SourceObject.MainImage.Images.Candidates)
                {
                    try
                    {
                        product.MainImage.Add(
                            new InstaImage(image.Url, int.Parse(image.Width), int.Parse(image.Height)));
                    }
                    catch
                    {
                    }
                }
            }

            if (SourceObject.ThumbnailImage?.Images?.Candidates?.Count > 0)
            {
                foreach (var image in SourceObject.ThumbnailImage.Images.Candidates)
                {
                    try
                    {
                        product.ThumbnailImage.Add(
                            new InstaImage(image.Url, int.Parse(image.Width), int.Parse(image.Height)));
                    }
                    catch
                    {
                    }
                }
            }

            if (SourceObject.ProductImages?.Count > 0)
            {
                foreach (var productImage in SourceObject.ProductImages)
                {
                    if (productImage?.Images?.Candidates?.Count > 0)
                    {
                        foreach (var image in productImage.Images.Candidates)
                        {
                            try
                            {
                                product.ThumbnailImage.Add(
                                    new InstaImage(image.Url, int.Parse(image.Width), int.Parse(image.Height)));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            return product;
        }
    }
}