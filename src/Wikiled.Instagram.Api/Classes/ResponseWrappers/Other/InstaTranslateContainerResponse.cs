using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaTranslateContainerResponse : InstaDefault
    {
        [JsonProperty("comment_translations")]
        public List<InstaTranslateResponse> Translations { get; set; }
    }
}