namespace Wikiled.Instagram.Api.Helpers
{
    public class UserAuthValidate
    {
        public bool IsUserAuthenticated { get; internal set; }

        public UserSessionData User { get; internal set; }
    }
}
