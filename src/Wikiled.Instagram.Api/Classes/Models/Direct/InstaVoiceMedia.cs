using System.Collections.Generic;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaVoiceMedia
    {
        public InstaVoice Media { get; set; }

        public string ReplayExpiringAtUs { get; set; }

        public int? SeenCount { get; set; }

        public List<long> SeenUserIds { get; set; } = new List<long>();

        public InstaViewMode ViewMode { get; set; }
    }
}