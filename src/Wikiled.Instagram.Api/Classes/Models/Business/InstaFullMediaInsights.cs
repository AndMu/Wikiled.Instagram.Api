using System;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaFullMediaInsights
    {
        public int CommentCount { get; set; }

        public DateTime CreationTime { get; set; }

        public string DisplayUrl { get; set; }

        public string Id { get; set; }

        public InstaFullMediaInsightsMetrics InlineInsightsNode { get; set; }

        public int LikeCount { get; set; }

        public InstaMediaType MediaType { get; set; }

        public int SaveCount { get; set; }
    }
}