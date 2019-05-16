using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaTranslateContainerResponse : InstaDefault
    {
        [JsonProperty("comment_translations")] public List<InstaTranslateResponse> Translations { get; set; }
    }
}
