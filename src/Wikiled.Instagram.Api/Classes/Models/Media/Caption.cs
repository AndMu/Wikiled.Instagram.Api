using System;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class Caption
    {
        public DateTime CreatedAt { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public string MediaId { get; set; }

        public string Pk { get; set; }

        public string Text { get; set; }

        public UserShortDescription User { get; set; }

        public long UserId { get; set; }
    }
}