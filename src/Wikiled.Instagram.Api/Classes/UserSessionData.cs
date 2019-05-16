using System;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes
{
    [Serializable]
    public class UserSessionData
    {
        public static UserSessionData Empty => new UserSessionData();

        public string CsrfToken { get; set; }

        /// <summary>
        ///     Only for facebook login
        /// </summary>
        public string FacebookAccessToken { get; internal set; } = string.Empty;

        /// <summary>
        ///     Only for facebook login
        /// </summary>
        public string FacebookUserId { get; internal set; } = string.Empty;

        public InstaUserShort LoggedInUser { get; set; }

        public string Password { get; set; }

        public string RankToken { get; set; }

        public string UserName { get; set; }

        public static UserSessionData ForUsername(string username)
        {
            return new UserSessionData { UserName = username };
        }

        public UserSessionData WithPassword(string password)
        {
            Password = password;
            return this;
        }
    }
}