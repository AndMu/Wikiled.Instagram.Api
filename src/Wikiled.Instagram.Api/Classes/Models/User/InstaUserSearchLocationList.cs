using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserSearchLocationList
    {
        [JsonProperty("hashtag")]
        public HashtagResponse Hashtag { get; set; }

        [JsonProperty("place")]
        public PlaceResponse Place { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonIgnore]
        public InstaSearchType Type
        {
            get
            {
                if (User != null)
                {
                    return InstaSearchType.User;
                }

                if (Hashtag != null)
                {
                    return InstaSearchType.Hashtag;
                }

                if (Place != null)
                {
                    return InstaSearchType.Place;
                }

                return InstaSearchType.Unknown;
            }
        }

        [JsonProperty("user")]
        public InstaUserShortFriendshipResponse User { get; set; }
    }
}