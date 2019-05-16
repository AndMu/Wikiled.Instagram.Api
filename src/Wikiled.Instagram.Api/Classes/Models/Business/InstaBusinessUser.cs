namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaBusinessUser
    {
        public string AddressStreet { get; set; }

        public string AllowedCommenterType { get; set; }

        public string Biography { get; set; }

        public InstaBiographyEntities BiographyWithEntities { get; set; }

        public InstaBusinessContactType BusinessContactMethod { get; set; }

        public bool CanBoostPost { get; set; }

        public bool CanClaimPage { get; set; }

        public bool CanConvertToBusiness { get; set; }

        public bool CanCrosspostWithoutFbToken { get; set; }

        public bool CanLinkEntitiesInBio { get; set; }

        public bool CanSeeOrganicInsights { get; set; }

        public string Category { get; set; }

        public long CityId { get; set; }

        public string CityName { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string DirectMessaging { get; set; }

        public string ExternalUrl { get; set; }

        public string FbPageCallToActionId { get; set; }

        public long FbPageCallToActionIxAppId { get; set; }

        public InstaBusinessUserFbBundle FbPageCallToActionIxLabelBundle { get; set; }

        public string FbPageCallToActionIxPartner { get; set; }

        public string FbPageCallToActionIxUrl { get; set; }

        public string FullName { get; set; }

        public bool HasAnonymousProfilePicture { get; set; }

        public bool HasPlacedOrders { get; set; }

        public string InstagramLocationId { get; set; }

        public bool IsBusiness { get; set; }

        public bool IsCallToActionEnabled { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsVerified { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public long PageId { get; set; }

        public string PageName { get; set; }

        public long Pk { get; set; }

        public string ProfilePicId { get; set; }

        public int ProfileVisitsCount { get; set; }

        public int ProfileVisitsNumDays { get; set; }

        public string ProfilPicUrl { get; set; }

        public string PublicEmail { get; set; }

        public string PublicPhoneCountryCode { get; set; }

        public string PublicPhoneNumber { get; set; }

        public string ReelAutoArchive { get; set; }

        public bool ShowBusinessConversionIcon { get; set; }

        public bool ShowConversionEditEntry { get; set; }

        public bool ShowInsightsTerms { get; set; }

        public string Username { get; set; }

        public string ZipCode { get; set; }
    }
}
