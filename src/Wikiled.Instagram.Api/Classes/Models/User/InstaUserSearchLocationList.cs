using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserSearchLocationList
    {
        [JsonProperty("hashtag")] public InstaHashtagResponse Hashtag { get; set; }

        [JsonProperty("place")] public InstaPlaceResponse Place { get; set; }

        [JsonProperty("position")] public int Position { get; set; }

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

        [JsonProperty("user")] public InstaUserShortFriendshipResponse User { get; set; }
    }
}
