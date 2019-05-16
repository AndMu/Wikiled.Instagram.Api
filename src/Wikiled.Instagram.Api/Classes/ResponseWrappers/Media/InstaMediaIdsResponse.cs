using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaMediaIdsResponse : InstaDefault
    {
        [JsonProperty("media_ids")]
        public InstaMediaIdList MediaIds = new InstaMediaIdList();
    }
}
