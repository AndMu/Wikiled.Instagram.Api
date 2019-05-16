using Wikiled.Instagram.Api.Classes.Models.Business;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Business;

namespace Wikiled.Instagram.Api.Converters.Business
{
    internal class
        InstaStatisticsDataPointConverter : IObjectConverter<InstaStatisticsDataPointItem,
            InstaStatisticsDataPointItemResponse>
    {
        public InstaStatisticsDataPointItemResponse SourceObject { get; set; }

        public InstaStatisticsDataPointItem Convert()
        {
            var dataPoint = new InstaStatisticsDataPointItem
            {
                Label = SourceObject.Label, Value = SourceObject.Value ?? 0
            };
            return dataPoint;
        }
    }
}