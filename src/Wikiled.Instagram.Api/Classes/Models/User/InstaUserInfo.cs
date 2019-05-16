using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserInfo
    {
        public int AccountType { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string AddressStreet { get; set; }

        public bool AggregatePromoteEngagement { get; set; }

        public string AllowedCommenterType { get; set; }

        public bool AutoExpandChaining { get; set; }

        public string Biography { get; set; }

        public InstaBiographyEntities BiographyWithEntities { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public InstaBusinessContactType BusinessContactMethod { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool CanBeReportedAsFraud { get; set; }

        public bool CanBeTaggedAsSponsor { get; set; }

        public bool CanBoostPost { get; set; }

        public bool CanConvertToBusiness { get; set; }

        public bool CanCreateSponsorTags { get; set; }

        public bool CanLinkEntitiesInBio { get; set; }

        public bool CanSeeOrganicInsights { get; set; }

        public bool CanTagProductsFromMerchants { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public long CityId { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string ContactPhoneNumber { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string DirectMessaging { get; set; }

        public string ExternalLynxUrl { get; set; }

        public string ExternalUrl { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string FbPageCallToActionId { get; set; }

        public long FollowerCount { get; set; }

        public long FollowingCount { get; set; }

        public long FollowingTagCount { get; set; }

        public InstaStoryFriendshipStatus FriendshipStatus { get; set; }

        public string FullName { get; set; }

        public long GeoMediaCount { get; set; }

        public bool HasAnonymousProfilePicture { get; set; }

        public bool HasBiographyTranslation { get; set; }

        public bool HasChaining { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool HasHighlightReels { get; set; }

        public bool HasPlacedOrders { get; set; }

        public bool HasProfileVideoFeed { get; set; }

        public bool HasRecommendAccounts { get; set; }

        public bool HasUnseenBestiesMedia { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public InstaImage HdProfilePicUrlInfo { get; set; }

        // Business accounts

        /// <summary>
        ///     Only for business account
        /// </summary>
        public List<InstaImage> HdProfilePicVersions { get; set; } = new List<InstaImage>();

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool HighlightReshareDisabled { get; set; }

        public bool IncludeDirectBlacklistStatus { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool IsBestie { get; set; }

        public bool IsBusiness { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool IsCallToActionEnabled { get; set; }

        public bool IsEligibleForSchool { get; set; }

        public bool IsEligibleToShowFBCrossSharingNux { get; set; }

        public bool IsFavorite { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool IsFavoriteForHighlights { get; set; }

        public bool IsFavoriteForStories { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool IsInterestAccount { get; set; }

        public bool IsNeedy { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool IsPotentialBusiness { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsProfileActionNeeded { get; set; }

        public bool IsVerified { get; set; }

        public bool IsVideoCreator { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public double Longitude { get; set; }

        public long MediaCount { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public long? PageId { get; set; }

        public object PageIdForNewSumaBizAccount { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string PageName { get; set; }

        public long Pk { get; set; }

        public string ProfileContext { get; set; }

        public List<InstaUserContext> ProfileContextIds { get; set; } = new List<InstaUserContext>();

        public List<long> ProfileContextMutualFollowIds { get; set; }

        public string ProfilePicId { get; set; }

        public string ProfilePicUrl { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string PublicEmail { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string PublicPhoneCountryCode { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string PublicPhoneNumber { get; set; }

        public string ReelAutoArchive { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public int ShoppablePostsCount { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool ShowAccountTransparencyDetails { get; set; }

        public bool ShowBusinessConversionIcon { get; set; }

        public bool ShowConversionEditEntry { get; set; }

        public bool ShowInsightsTerms { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public bool ShowShoppableFeed { get; set; }

        public int TotalArEffects { get; set; }

        public int TotalIGTVVideos { get; set; }

        public string Username { get; set; }

        public bool UsertagReviewEnabled { get; set; }

        public long UsertagsCount { get; set; }

        /// <summary>
        ///     Only for business account
        /// </summary>
        public string ZipCode { get; set; }
    }
}
