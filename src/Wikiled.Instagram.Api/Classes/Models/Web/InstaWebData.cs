using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Web
{
    public class InstaWebData
    {
        public List<InstaWebDataItem> Items { get; set; } = new List<InstaWebDataItem>();

        public string MaxId { get; set; }
    }
}
