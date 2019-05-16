namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserLookup
    {
        public bool CanEmailReset { get; set; }

        public bool CanSmsReset { get; set; }

        public bool CanWaReset { get; set; }

        public string CorrectedInput { get; set; }

        //public string UserId { get; set; }

        public string Email { get; set; }

        public bool EmailSent { get; set; }

        public bool HasValidPhone { get; set; }

        public InstaLookupType LookupSourceType { get; set; }

        public bool MultipleUsersFound { get; set; }

        public string PhoneNumber { get; set; }

        public bool SmsSent { get; set; }

        /// <summary>
        ///     Note: This always is null except when <see cref="LookupSourceType" /> is Username
        /// </summary>
        public InstaUserShort User { get; set; }
    }
}
