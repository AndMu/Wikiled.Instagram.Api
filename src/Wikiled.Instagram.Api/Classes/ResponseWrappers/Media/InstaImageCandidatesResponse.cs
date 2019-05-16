using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaImageCandidatesResponse
    {
        [JsonProperty("candidates")]
        public List<InstaImageResponse> Candidates { get; set; }
    }
}