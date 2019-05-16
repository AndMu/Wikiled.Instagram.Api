using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaPlaceListConverter : IObjectConverter<InstaPlaceList, InstaPlaceListResponse>
    {
        public InstaPlaceListResponse SourceObject { get; set; }

        public InstaPlaceList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var list = new InstaPlaceList
            {
                HasMore = SourceObject.HasMore ?? false,
                RankToken = SourceObject.RankToken,
                Status = SourceObject.Status
            };
            if (SourceObject.Items != null && SourceObject.Items.Any())
            {
                foreach (var place in SourceObject.Items)
                {
                    try
                    {
                        list.Items.Add(InstaConvertersFabric.Instance.GetPlaceConverter(place).Convert());
                    }
                    catch
                    {
                    }
                }

                list.ExcludeList = SourceObject.ExcludeList;
            }

            return list;
        }
    }
}