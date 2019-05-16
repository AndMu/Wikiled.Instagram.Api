using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Web
{
    public class InstaWebTextData
    {
        public List<string> Items { get; set; } = new List<string>();

        public string MaxId { get; set; }
    }
}