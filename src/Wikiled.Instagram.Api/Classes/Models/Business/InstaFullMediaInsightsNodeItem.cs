using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaFullMediaInsightsNodeItem
    {
        public List<InstaInsightsDataNode> Items { get; set; } = new List<InstaInsightsDataNode>();

        public int Value { get; set; }
    }
}