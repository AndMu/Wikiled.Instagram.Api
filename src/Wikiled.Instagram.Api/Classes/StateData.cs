﻿using System.Collections.Generic;
using System.Net;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes
{
    public class StateData
    {
        public CookieContainer Cookies { get; set; }

        public AndroidDevice DeviceInfo { get; set; }

        public InstaApiVersionType ApiVersion { get; set; }

        public bool IsAuthenticated { get; set; }

        public List<Cookie> RawCookies { get; set; }

        public UserSessionData UserSession { get; set; }
    }
}