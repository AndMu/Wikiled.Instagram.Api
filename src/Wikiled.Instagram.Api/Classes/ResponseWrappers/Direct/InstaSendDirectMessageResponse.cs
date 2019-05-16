using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaSendDirectMessageResponse : BaseStatusResponse
    {
        public List<InstaDirectInboxThreadResponse> Threads { get; set; } = new List<InstaDirectInboxThreadResponse>();
    }
}
