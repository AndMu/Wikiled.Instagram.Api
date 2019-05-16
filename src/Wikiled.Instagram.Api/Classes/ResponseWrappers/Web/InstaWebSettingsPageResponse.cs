using System;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Web
{
    public class InstaWebSettingsPageResponse
    {
        [JsonProperty("data")] public InstaWebDataListResponse Data { get; set; }

        [JsonProperty("date_joined")] public InstaWebDataResponse DateJoined { get; set; }

        [JsonProperty("is_blocked")] public bool? IsBlocked { get; set; }

        [JsonProperty("page_name")] public string PageName { get; set; }

        [JsonProperty("switched_to_business")] public InstaWebDataResponse SwitchedToBusiness { get; set; }

        internal InstaWebType PageType
        {
            get
            {
                if (string.IsNullOrEmpty(PageName))
                {
                    return InstaWebType.Unknown;
                }

                try
                {
                    var name = PageName.Replace("_", "");

                    return (InstaWebType)Enum.Parse(typeof(InstaWebType), name, true);
                }
                catch
                {
                }

                return InstaWebType.Unknown;
            }
        }
    }
}
