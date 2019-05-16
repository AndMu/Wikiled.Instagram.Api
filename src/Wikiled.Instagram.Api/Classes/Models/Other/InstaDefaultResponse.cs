namespace Wikiled.Instagram.Api.Classes.Models.Other
{
    internal class InstaDefaultResponse : InstaDefault
    {
        public bool IsSucceed => Status.ToLower() == "ok";
    }
}
