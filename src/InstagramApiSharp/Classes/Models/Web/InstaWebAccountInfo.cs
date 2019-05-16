using System;

namespace Wikiled.Instagram.Api.Classes.Models.Web
{
    public class InstaWebAccountInfo
    {
        public DateTime? JoinedDate { get; set; } = DateTime.MinValue;

        public DateTime? SwitchedToBusinessDate { get; set; } = DateTime.MinValue;
    }
}
