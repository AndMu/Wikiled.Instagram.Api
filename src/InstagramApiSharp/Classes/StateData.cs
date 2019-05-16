using System;
using System.Collections.Generic;
using System.Net;

namespace Wikiled.Instagram.Api.Classes
{
    [Serializable]
    public class StateData
    {
        public CookieContainer Cookies { get; set; }

        public AndroidDevice DeviceInfo { get; set; }

        public InstaApiVersionType? InstaApiVersion { get; set; }

        public bool IsAuthenticated { get; set; }

        public List<Cookie> RawCookies { get; set; }

        public UserSessionData UserSession { get; set; }
    }
}
