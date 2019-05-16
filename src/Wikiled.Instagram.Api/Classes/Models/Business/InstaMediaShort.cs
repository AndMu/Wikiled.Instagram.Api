using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaMediaShort
    {
        public string Id { get; set; }

        public string Image { get; set; }

        public string InsightsState { get; set; }

        public string MediaIdentifier { get; set; }

        public InstaMediaType MediaType { get; set; }

        public long MetricsImpressionsOrganicValue { get; set; }
    }
}