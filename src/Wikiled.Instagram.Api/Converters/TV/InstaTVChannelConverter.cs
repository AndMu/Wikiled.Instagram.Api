using System;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTvUserConverter : IObjectConverter<InstaTvUser, InstaTvUserResponse>
    {
        public InstaTvUserResponse SourceObject { get; set; }

        public InstaTvUser Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var user = new InstaTvUser
            {
                AllowedCommenterType = SourceObject.AllowedCommenterType,
                Biography = SourceObject.Biography,
                BiographyWithEntities = SourceObject.BiographyWithEntities,
                CanBoostPost = SourceObject.CanBoostPost,
                CanLinkEntitiesInBio = SourceObject.CanLinkEntitiesInBio,
                CanSeeOrganicInsights = SourceObject.CanSeeOrganicInsights,
                ExternalLynxUrl = SourceObject.ExternalLynxUrl,
                ExternalUrl = SourceObject.ExternalUrl,
                FollowerCount = SourceObject.FollowerCount,
                FollowingCount = SourceObject.FollowingCount,
                FollowingTagCount = SourceObject.FollowingTagCount,
                FullName = SourceObject.FullName,
                GeoMediaCount = SourceObject.GeoMediaCount,
                HasAnonymousProfilePicture = SourceObject.HasAnonymousProfilePicture,
                HasBiographyTranslation = SourceObject.HasBiographyTranslation,
                HasPlacedOrders = SourceObject.HasPlacedOrders,
                IsPrivate = SourceObject.IsPrivate,
                IsVerified = SourceObject.IsVerified,
                MediaCount = SourceObject.MediaCount,
                Pk = SourceObject.Pk,
                ProfilePicId = SourceObject.ProfilePicId,
                ProfilePicUrl = SourceObject.ProfilePicUrl,
                ReelAutoArchive = SourceObject.ReelAutoArchive,
                ShowInsightsTerms = SourceObject.ShowInsightsTerms,
                TotalIgtvVideosCount = SourceObject.TotalIgtvVideosCount,
                Username = SourceObject.Username
            };
            try
            {
                user.FriendshipStatus = InstaConvertersFabric.Instance
                    .GetFriendShipStatusConverter(SourceObject.FriendshipStatus)
                    .Convert();
            }
            catch
            {
            }

            return user;
        }
    }
}