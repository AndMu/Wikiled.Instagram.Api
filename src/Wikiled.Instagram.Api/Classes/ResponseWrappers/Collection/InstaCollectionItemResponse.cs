using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Collection
{
    public class InstaCollectionItemResponse : BaseLoadableResponse
    {
        [JsonProperty("collection_id")] public long CollectionId { get; set; }

        [JsonProperty("collection_name")] public string CollectionName { get; set; }

        [JsonProperty("cover_media")] public InstaCoverMediaResponse CoverMedia { get; set; }

        [JsonProperty("has_related_media")] public bool HasRelatedMedia { get; set; }

        [JsonProperty("items")] public InstaMediaListResponse Media { get; set; }
    }
}
