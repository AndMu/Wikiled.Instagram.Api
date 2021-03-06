﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.Models.Business;
using Wikiled.Instagram.Api.Classes.Models.Collection;
using Wikiled.Instagram.Api.Classes.Models.Comment;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.Models.Feed;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Highlight;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.Shopping;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.Models.Web;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Business;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Collection;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Web;
using Wikiled.Instagram.Api.Converters.Activities;
using Wikiled.Instagram.Api.Converters.Broadcast;
using Wikiled.Instagram.Api.Converters.Business;
using Wikiled.Instagram.Api.Converters.Collections;
using Wikiled.Instagram.Api.Converters.Directs;
using Wikiled.Instagram.Api.Converters.Discover;
using Wikiled.Instagram.Api.Converters.Feeds;
using Wikiled.Instagram.Api.Converters.Hashtags;
using Wikiled.Instagram.Api.Converters.Highlights;
using Wikiled.Instagram.Api.Converters.Location;
using Wikiled.Instagram.Api.Converters.Media;
using Wikiled.Instagram.Api.Converters.Other;
using Wikiled.Instagram.Api.Converters.Shopping;
using Wikiled.Instagram.Api.Converters.Stories;
using Wikiled.Instagram.Api.Converters.TV;
using Wikiled.Instagram.Api.Converters.Users;
using Wikiled.Instagram.Api.Converters.Web;

namespace Wikiled.Instagram.Api.Converters
{
    internal class InstaConvertersFabric
    {
        private static readonly Lazy<InstaConvertersFabric> LazyInstance =
            new Lazy<InstaConvertersFabric>(() => new InstaConvertersFabric());

        public static InstaConvertersFabric Instance => LazyInstance.Value;

        public IObjectConverter<InstaAccountDetails, InstaAccountDetailsResponse> GetAccountDetailsConverter(
            InstaAccountDetailsResponse response)
        {
            return new InstaAccountDetailsConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastAddToPostLive, InstaBroadcastAddToPostLiveResponse> GetAddToPostLiveConverter(InstaBroadcastAddToPostLiveResponse response)
        {
            return new InstaBroadcastAddToPostLiveConverter { SourceObject = response };
        }

        public IObjectConverter<InstaAdsInfo, InstaAdsInfoResponse> GetAdsInfoConverter(InstaAdsInfoResponse response)
        {
            return new InstaAdsInfoConverter { SourceObject = response };
        }

        public IObjectConverter<InstaAnimatedImage, InstaAnimatedImageResponse> GetAnimatedImageConverter(InstaAnimatedImageResponse response)
        {
            return new InstaAnimatedImageConverter { SourceObject = response };
        }

        public IObjectConverter<InstaAnimatedImageMedia, InstaAnimatedImageMediaResponse> GetAnimatedImageMediaConverter(InstaAnimatedImageMediaResponse response)
        {
            return new InstaAnimatedImageMediaConverter { SourceObject = response };
        }

        public IObjectConverter<InstaAnimatedImageUser, InstaAnimatedImageUserResponse> GetAnimatedImageUserConverter(InstaAnimatedImageUserResponse response)
        {
            return new InstaAnimatedImageUserConverter { SourceObject = response };
        }

        public IObjectConverter<InstaAudio, InstaAudioResponse> GetAudioConverter(InstaAudioResponse response)
        {
            return new InstaAudioConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserShortList, InstaBlockedCommentersResponse> GetBlockedCommentersConverter(InstaBlockedCommentersResponse response)
        {
            return new InstaBlockedCommentersConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBlockedUserInfo, InstaBlockedUserInfoResponse> GetBlockedUserInfoConverter(InstaBlockedUserInfoResponse instaresponse)
        {
            return new InstaBlockedUserInfoConverter { SourceObject = instaresponse };
        }

        public IObjectConverter<InstaBlockedUsers, InstaBlockedUsersResponse> GetBlockedUsersConverter(InstaBlockedUsersResponse response)
        {
            return new InstaBlockedUsersConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBrandedContent, InstaBrandedContentResponse> GetBrandedContentConverter(InstaBrandedContentResponse response)
        {
            return new InstaBrandedContentConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastComment, InstaBroadcastCommentResponse> GetBroadcastCommentConverter(InstaBroadcastCommentResponse response)
        {
            return new InstaBroadcastCommentConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastCommentEnableDisable, InstaBroadcastCommentEnableDisableResponse> GetBroadcastCommentEnableDisableConverter(InstaBroadcastCommentEnableDisableResponse response)
        {
            return new InstaBroadcastCommentEnableDisableConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastCommentList, InstaBroadcastCommentListResponse> GetBroadcastCommentListConverter(InstaBroadcastCommentListResponse response)
        {
            return new InstaBroadcastCommentListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcast, InstaBroadcastResponse> GetBroadcastConverter(InstaBroadcastResponse response)
        {
            return new InstaBroadcastConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastCreate, InstaBroadcastCreateResponse> GetBroadcastCreateConverter(InstaBroadcastCreateResponse response)
        {
            return new InstaBroadcastCreateConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastInfo, InstaBroadcastInfoResponse> GetBroadcastInfoConverter(InstaBroadcastInfoResponse response)
        {
            return new InstaBroadcastInfoConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastLike, InstaBroadcastLikeResponse> GetBroadcastLikeConverter(InstaBroadcastLikeResponse response)
        {
            return new InstaBroadcastLikeConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastList, List<InstaBroadcastResponse>> GetBroadcastListConverter(List<InstaBroadcastResponse> response)
        {
            return new InstaBroadcastListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastLiveHeartBeatViewerCount, InstaBroadcastLiveHeartBeatViewerCountResponse>GetBroadcastLiveHeartBeatViewerCountConverter(InstaBroadcastLiveHeartBeatViewerCountResponse response)
        {
            return new InstaBroadcastLiveHeartBeatViewerCountConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastNotifyFriends, InstaBroadcastNotifyFriendsResponse> GetBroadcastNotifyFriendsConverter(InstaBroadcastNotifyFriendsResponse response)
        {
            return new InstaBroadcastNotifyFriendsConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastPinUnpin, InstaBroadcastPinUnpinResponse> GetBroadcastPinUnpinConverter(InstaBroadcastPinUnpinResponse response)
        {
            return new InstaBroadcastPinUnpinConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastPostLive, InstaBroadcastPostLiveResponse> GetBroadcastPostLiveConverter(
            InstaBroadcastPostLiveResponse response)
        {
            return new InstaBroadcastPostLiveConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastSendComment, InstaBroadcastSendCommentResponse>
            GetBroadcastSendCommentConverter(
                InstaBroadcastSendCommentResponse response)
        {
            return new InstaBroadcastSendCommentConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastStart, InstaBroadcastStartResponse> GetBroadcastStartConverter(
            InstaBroadcastStartResponse response)
        {
            return new InstaBroadcastStartConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastStatusItem, InstaBroadcastStatusItemResponse>
            GetBroadcastStatusItemConverter(
                InstaBroadcastStatusItemResponse response)
        {
            return new InstaBroadcastStatusItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBroadcastTopLiveStatusList, InstaBroadcastTopLiveStatusResponse>
            GetBroadcastTopLiveStatusListConverter(
                InstaBroadcastTopLiveStatusResponse response)
        {
            return new InstaBroadcastTopLiveStatusListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaBusinessUser, InstaBusinessUserContainerResponse> GetBusinessUserConverter(
            InstaBusinessUserContainerResponse response)
        {
            return new InstaBusinessUserConverter { SourceObject = response };
        }

        public IObjectConverter<Caption, InstaCaptionResponse> GetCaptionConverter(
            InstaCaptionResponse captionResponse)
        {
            return new InstaCaptionConverter { SourceObject = captionResponse };
        }

        public IObjectConverter<InstaCarousel, InstaCarouselResponse> GetCarouselConverter(
            InstaCarouselResponse carousel)
        {
            return new InstaCarouselConverter { SourceObject = carousel };
        }

        public IObjectConverter<InstaCarouselItem, InstaCarouselItemResponse> GetCarouselItemConverter(
            InstaCarouselItemResponse carouselItem)
        {
            return new InstaCarouselItemConverter { SourceObject = carouselItem };
        }

        public IObjectConverter<InstaChannel, InstaChannelResponse> GetChannelConverter(
            InstaChannelResponse response)
        {
            return new InstaChannelConverter { SourceObject = response };
        }

        public IObjectConverter<InstaCollectionItem, InstaCollectionItemResponse> GetCollectionConverter(
            InstaCollectionItemResponse response)
        {
            return new InstaCollectionConverter { SourceObject = response };
        }

        public IObjectConverter<InstaCollections, InstaCollectionsResponse> GetCollectionsConverter(
            InstaCollectionsResponse response)
        {
            return new InstaCollectionsConverter { SourceObject = response };
        }

        public IObjectConverter<InstaComment, InstaCommentResponse> GetCommentConverter(
            InstaCommentResponse comment)
        {
            return new InstaCommentConverter { SourceObject = comment };
        }

        public IObjectConverter<InstaCommentList, InstaCommentListResponse> GetCommentListConverter(
            InstaCommentListResponse commentList)
        {
            return new InstaCommentListConverter { SourceObject = commentList };
        }

        public IObjectConverter<InstaCommentShort, InstaCommentShortResponse> GetCommentShortConverter(
            InstaCommentShortResponse response)
        {
            return new InstaCommentShortConverter { SourceObject = response };
        }

        public IObjectConverter<InstaCoverMedia, InstaCoverMediaResponse> GetCoverMediaConverter(
            InstaCoverMediaResponse response)
        {
            return new InstaCoverMediaConverter { SourceObject = response };
        }

        public IObjectConverter<CurrentUser, InstaCurrentUserResponse> GetCurrentUserConverter(
            InstaCurrentUserResponse instaresponse)
        {
            return new InstaCurrentUserConverter { SourceObject = instaresponse };
        }

        public IObjectConverter<InstaDirectBroadcast, InstaDirectBroadcastResponse> GetDirectBroadcastConverter(
            InstaDirectBroadcastResponse response)
        {
            return new InstaDirectBroadcastConverter { SourceObject = response };
        }

        public IObjectConverter<DirectHashtag, DirectHashtagResponse> GetDirectHashtagConverter(
            DirectHashtagResponse response)
        {
            return new DirectHashtagConverter { SourceObject = response };
        }

        public IObjectConverter<InstaDirectInboxContainer, InstaDirectInboxContainerResponse>
            GetDirectInboxConverter(InstaDirectInboxContainerResponse inbox)
        {
            return new InstaDirectInboxConverter { SourceObject = inbox };
        }

        public IObjectConverter<InstaDirectInboxSubscription, InstaDirectInboxSubscriptionResponse>
            GetDirectSubscriptionConverter(InstaDirectInboxSubscriptionResponse subscription)
        {
            return new InstaDirectInboxSubscriptionConverter { SourceObject = subscription };
        }

        public IObjectConverter<InstaDirectInboxThread, InstaDirectInboxThreadResponse> GetDirectThreadConverter(
            InstaDirectInboxThreadResponse thread)
        {
            return new InstaDirectThreadConverter { SourceObject = thread };
        }

        public IObjectConverter<InstaDirectInboxItem, InstaDirectInboxItemResponse> GetDirectThreadItemConverter(
            InstaDirectInboxItemResponse threadItem)
        {
            return new InstaDirectThreadItemConverter { SourceObject = threadItem };
        }

        public IObjectConverter<InstaDiscoverRecentSearches, InstaDiscoverRecentSearchesResponse>
            GetDiscoverRecentSearchesConverter(
                InstaDiscoverRecentSearchesResponse response)
        {
            return new InstaDiscoverRecentSearchesConverter { SourceObject = response };
        }

        public IObjectConverter<InstaDiscoverSearches, InstaDiscoverSearchesResponse> GetDiscoverSearchesConverter(
            InstaDiscoverSearchesResponse response)
        {
            return new InstaDiscoverSearchesConverter { SourceObject = response };
        }

        public IObjectConverter<InstaDiscoverSearchResult, InstaDiscoverSearchResultResponse>
            GetDiscoverSearchResultConverter(
                InstaDiscoverSearchResultResponse response)
        {
            return new InstaDiscoverSearchResultConverter { SourceObject = response };
        }

        public IObjectConverter<InstaDiscoverSuggestedSearches, InstaDiscoverSuggestedSearchesResponse>
            GetDiscoverSuggestedSearchesConverter(
                InstaDiscoverSuggestedSearchesResponse response)
        {
            return new InstaDiscoverSuggestedSearchesConverter { SourceObject = response };
        }

        public IObjectConverter<InstaDiscoverTopLive, InstaDiscoverTopLiveResponse> GetDiscoverTopLiveConverter(
            InstaDiscoverTopLiveResponse response)
        {
            return new InstaDiscoverTopLiveConverter { SourceObject = response };
        }

        public IObjectConverter<InstaDiscoverTopSearches, InstaDiscoverTopSearchesResponse>
            GetDiscoverTopSearchesConverter(
                InstaDiscoverTopSearchesResponse response)
        {
            return new InstaDiscoverTopSearchesConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTopicalExploreCluster, InstaTopicalExploreClusterResponse>
            GetExploreClusterConverter(
                InstaTopicalExploreClusterResponse response)
        {
            return new InstaTopicalExploreClusterConverter { SourceObject = response };
        }

        public IObjectConverter<InstaExploreFeed, InstaExploreFeedResponse> GetExploreFeedConverter(
            InstaExploreFeedResponse feedResponse)
        {
            return new InstaExploreFeedConverter { SourceObject = feedResponse };
        }

        public IObjectConverter<InstaFeed, InstaFeedResponse> GetFeedConverter(
            InstaFeedResponse feedResponse)
        {
            return new InstaFeedConverter { SourceObject = feedResponse };
        }

        public IObjectConverter<InstaFriendshipFullStatus, InstaFriendshipFullStatusResponse>
            GetFriendshipFullStatusConverter(
                InstaFriendshipFullStatusResponse response)
        {
            return new InstaFriendshipFullStatusConverter { SourceObject = response };
        }

        public IObjectConverter<InstaFriendshipShortStatusList, InstaFriendshipShortStatusListResponse>
            GetFriendshipShortStatusListConverter(
                InstaFriendshipShortStatusListResponse response)
        {
            return new InstaFriendshipShortStatusListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaFriendshipStatus, InstaFriendshipStatusResponse>
            GetFriendShipStatusConverter(InstaFriendshipStatusResponse friendshipStatusResponse)
        {
            return new InstaFriendshipStatusConverter { SourceObject = friendshipStatusResponse };
        }

        public IObjectConverter<InstaFullMediaInsights, InstaFullMediaInsightsResponse> GetFullMediaInsightsConverter(
            InstaFullMediaInsightsResponse response)
        {
            return new InstaFullMediaInsightsConverter { SourceObject = response };
        }

        public IObjectConverter<InstaFullUserInfo, InstaFullUserInfoResponse> GetFullUserInfoConverter(
            InstaFullUserInfoResponse response)
        {
            return new InstaFullUserInfoConverter { SourceObject = response };
        }

        public IObjectConverter<ApiHashtag, HashtagResponse> GetHashTagConverter(
            HashtagResponse response)
        {
            return new HashtagConverter { SourceObject = response };
        }

        public IObjectConverter<SectionMedia, SectionMediaListResponse> GetHashtagMediaListConverter(
            SectionMediaListResponse response)
        {
            return new HashtagMediaConverter(new NullLogger<HashtagMediaConverter>()) { SourceObject = response };
        }

        public IObjectConverter<HashtagOwner, HashtagOwnerResponse> GetHashtagOwnerConverter(
            HashtagOwnerResponse response)
        {
            return new HashtagOwnerConverter { SourceObject = response };
        }

        public IObjectConverter<HashtagSearch, HashtagSearchResponse> GetHashTagsSearchConverter(
            HashtagSearchResponse response)
        {
            return new HashtagSearchConverter { SourceObject = response };
        }

        public IObjectConverter<HashtagStory, HashtagStoryResponse> GetHashtagStoryConverter(
            HashtagStoryResponse response)
        {
            return new HashtagStoryConverter { SourceObject = response };
        }

        public IObjectConverter<InstaHighlightFeeds, InstaHighlightFeedsResponse> GetHighlightFeedsConverter(
            InstaHighlightFeedsResponse response)
        {
            return new InstaHighlightConverter { SourceObject = response };
        }

        public IObjectConverter<InstaHighlightSingleFeed, InstaHighlightReelResponse> GetHighlightReelConverter(
            InstaHighlightReelResponse response)
        {
            return new InstaHighlightReelConverter { SourceObject = response };
        }

        public IObjectConverter<InstaHighlightShortList, InstaHighlightShortListResponse>
            GetHighlightShortListConverter(
                InstaHighlightShortListResponse response)
        {
            return new InstaHighlightShortListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaImage, InstaImageResponse> GetImageConverter(InstaImageResponse imageResponse)
        {
            return new InstaMediaImageConverter { SourceObject = imageResponse };
        }

        public IObjectConverter<InstaInboxMedia, InstaInboxMediaResponse> GetInboxMediaConverter(
            InstaInboxMediaResponse response)
        {
            return new InstaInboxMediaConverter { SourceObject = response };
        }

        public IObjectConverter<InstaInlineCommentList, InstaInlineCommentListResponse> GetInlineCommentsConverter(
            InstaInlineCommentListResponse response)
        {
            return new InstaInlineCommentListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaInsightsDataNode, InstaInsightsDataNodeResponse> GetInsightsDataNodeConverter(
            InstaInsightsDataNodeResponse response)
        {
            return new InstaInsightsDataNodeConverter { SourceObject = response };
        }

        public IObjectConverter<Classes.Models.Location.Location, LocationResponse> GetLocationConverter(
            LocationResponse response)
        {
            return new LocationConverter { SourceObject = response };
        }

        public IObjectConverter<LocationFeed, LocationFeedResponse> GetLocationFeedConverter(
            LocationFeedResponse response)
        {
            return new InstaLocationFeedConverter { SourceObject = response };
        }

        public IObjectConverter<LocationShort, LocationShortResponse> GetLocationShortConverter(
            LocationShortResponse response)
        {
            return new InstaLocationShortConverter { SourceObject = response };
        }

        public IObjectConverter<LocationShortList, LocationSearchResponse> GetLocationsSearchConverter(
            LocationSearchResponse response)
        {
            return new InstaLocationSearchConverter { SourceObject = response };
        }

        public IObjectConverter<InstaMediaList, InstaMediaListResponse> GetMediaListConverter(
            InstaMediaListResponse mediaResponse)
        {
            return new InstaMediaListConverter { SourceObject = mediaResponse };
        }

        public IObjectConverter<InstaMediaShort, InstaMediaShortResponse> GetMediaShortConverter(
            InstaMediaShortResponse response)
        {
            return new InstaMediaShortConverter { SourceObject = response };
        }

        public IObjectConverter<InstaReelMention, InstaReelMentionResponse> GetMentionConverter(
            InstaReelMentionResponse response)
        {
            return new InstaReelMentionConverter { SourceObject = response };
        }

        public IObjectConverter<InstaMerchant, InstaMerchantResponse> GetMerchantConverter(
            InstaMerchantResponse response)
        {
            return new InstaMerchantConverter { SourceObject = response };
        }

        public IObjectConverter<Place, PlaceResponse> GetPlaceConverter(
            PlaceResponse response)
        {
            return new InstaPlaceConverter { SourceObject = response };
        }

        public IObjectConverter<PlaceList, PlaceListResponse> GetPlaceListConverter(
            PlaceListResponse response)
        {
            return new InstaPlaceListConverter { SourceObject = response };
        }

        public IObjectConverter<PlaceShort, PlaceShortResponse> GetPlaceShortConverter(
            PlaceShortResponse response)
        {
            return new InstaPlaceShortConverter { SourceObject = response };
        }

        public IObjectConverter<InstaPresence, InstaPresenceResponse> GetPresenceConverter(
            InstaPresenceResponse response)
        {
            return new InstaPresenceConverter { SourceObject = response };
        }

        public IObjectConverter<InstaPrimaryCountryInfo, InstaPrimaryCountryInfoResponse>
            GetPrimaryCountryInfoConverter(
                InstaPrimaryCountryInfoResponse response)
        {
            return new InstaPrimaryCountryInfoConverter { SourceObject = response };
        }

        public IObjectConverter<InstaProduct, InstaProductResponse> GetProductConverter(
            InstaProductResponse response)
        {
            return new InstaProductConverter { SourceObject = response };
        }

        public IObjectConverter<InstaProductInfo, InstaProductInfoResponse> GetProductInfoConverter(
            InstaProductInfoResponse response)
        {
            return new InstaProductInfoConverter { SourceObject = response };
        }

        public IObjectConverter<InstaProductMediaList, InstaProductMediaListResponse> GetProductMediaListConverter(
            InstaProductMediaListResponse response)
        {
            return new InstaProductMediaListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaProductTag, InstaProductContainerResponse> GetProductTagContainerConverter(
            InstaProductContainerResponse response)
        {
            return new InstaProductContainerConverter { SourceObject = response };
        }

        public IObjectConverter<InstaRecipients, IInstaRecipientsResponse> GetRecipientsConverter(
            IInstaRecipientsResponse recipients)
        {
            return new InstaRecipientsConverter { SourceObject = recipients };
        }

        public IObjectConverter<InstaReelFeed, InstaReelFeedResponse> GetReelFeedConverter(
            InstaReelFeedResponse response)
        {
            return new InstaReelFeedConverter { SourceObject = response };
        }

        public IObjectConverter<InstaReelShare, InstaReelShareResponse> GetReelShareConverter(
            InstaReelShareResponse response)
        {
            return new InstaReelShareConverter { SourceObject = response };
        }

        public IObjectConverter<InstaReelStoryMediaViewers, InstaReelStoryMediaViewersResponse>
            GetReelStoryMediaViewersConverter(
                InstaReelStoryMediaViewersResponse response)
        {
            return new InstaReelStoryMediaViewersConverter { SourceObject = response };
        }

        public IObjectConverter<RelatedHashtag, RelatedHashtagResponse> GetRelatedHashtagConverter(
            RelatedHashtagResponse response)
        {
            return new RelatedHashtagConverter { SourceObject = response };
        }

        public IObjectConverter<InstaFriendshipShortStatus, InstaFriendshipShortStatusResponse>
            GetSingleFriendshipShortStatusConverter(
                InstaFriendshipShortStatusResponse response)
        {
            return new InstaFriendshipShortStatusConverter { SourceObject = response };
        }

        public IObjectConverter<InstaHighlightShort, InstaHighlightShortResponse> GetSingleHighlightShortConverter(
            InstaHighlightShortResponse response)
        {
            return new InstaHighlightShortConverter { SourceObject = response };
        }

        public IObjectConverter<InstaMedia, InstaMediaItemResponse> GetSingleMediaConverter(
            InstaMediaItemResponse responseMedia)
        {
            return new InstaMediaConverter { SourceObject = responseMedia };
        }

        public IObjectConverter<InstaMedia, InstaMediaAlbumResponse> GetSingleMediaFromAlbumConverter(
            InstaMediaAlbumResponse responseMedia)
        {
            return new InstaMediaAlbumConverter { SourceObject = responseMedia };
        }

        public IObjectConverter<InstaRecentActivityFeed, InstaRecentActivityFeedResponse>
            GetSingleRecentActivityConverter(InstaRecentActivityFeedResponse feedResponse)
        {
            return new InstaRecentActivityConverter { SourceObject = feedResponse };
        }

        public IObjectConverter<InstaStory, InstaStoryResponse> GetSingleStoryConverter(
            InstaStoryResponse storyResponse)
        {
            return new InstaStoryConverter { SourceObject = storyResponse };
        }

        public IObjectConverter<InstaTranslate, InstaTranslateResponse> GetSingleTranslateConverter(
            InstaTranslateResponse response)
        {
            return new InstaTranslateConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserChaining, InstaUserChainingResponse> GetSingleUserChainingConverter(
            InstaUserChainingResponse response)
        {
            return new InstaUserChainingConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserContact, InstaUserContactResponse> GetSingleUserContactConverter(
            InstaUserContactResponse response)
        {
            return new InstaUserContactConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserPresence, InstaUserPresenceResponse> GetSingleUserPresenceConverter(InstaUserPresenceResponse response)
        {
            return new InstaSingleUserPresenceConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStatistics, InstaStatisticsRootResponse> GetStatisticsConverter(InstaStatisticsRootResponse response)
        {
            return new InstaStatisticsConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStatisticsDataPointItem, InstaStatisticsDataPointItemResponse> GetStatisticsDataPointConverter(InstaStatisticsDataPointItemResponse response)
        {
            return new InstaStatisticsDataPointConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStory, InstaStoryResponse> GetStoryConverter(InstaStoryResponse storyItem)
        {
            return new InstaStoryConverter { SourceObject = storyItem };
        }

        public IObjectConverter<InstaStoryCountdownItem, InstaStoryCountdownItemResponse>
            GetStoryCountdownItemConverter(
                InstaStoryCountdownItemResponse response)
        {
            return new InstaStoryCountdownItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryCountdownList, InstaStoryCountdownListResponse>
            GetStoryCountdownListConverter(
                InstaStoryCountdownListResponse response)
        {
            return new InstaStoryCountdownListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryCountdownStickerItem, InstaStoryCountdownStickerItemResponse>
            GetStoryCountdownStickerItemConverter(
                InstaStoryCountdownStickerItemResponse response)
        {
            return new InstaStoryCountdownStickerItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryCta, InstaStoryCtaResponse> GetStoryCtaConverter(
            InstaStoryCtaResponse storyCta)
        {
            return new InstaStoryCtaConverter { SourceObject = storyCta };
        }

        public IObjectConverter<InstaStoryFeed, InstaStoryFeedResponse> GetStoryFeedConverter(
            InstaStoryFeedResponse response)
        {
            return new InstaStoryFeedConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryFeedMedia, InstaStoryFeedMediaResponse> GetStoryFeedMediaConverter(
            InstaStoryFeedMediaResponse storyItem)
        {
            return new InstaStoryFeedMediaConverter { SourceObject = storyItem };
        }

        public IObjectConverter<InstaStoryFriendshipStatus, InstaStoryFriendshipStatusResponse>
            GetStoryFriendshipStatusConverter(
                InstaStoryFriendshipStatusResponse response)
        {
            return new InstaStoryFriendshipStatusConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryFriendshipStatusShort, InstaStoryFriendshipStatusShortResponse>
            GetStoryFriendshipStatusShortConverter(
                InstaStoryFriendshipStatusShortResponse response)
        {
            return new InstaStoryFriendshipStatusShortConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryItem, InstaStoryItemResponse> GetStoryItemConverter(
            InstaStoryItemResponse storyItem)
        {
            return new InstaStoryItemConverter { SourceObject = storyItem };
        }

        public IObjectConverter<InstaStoryLocation, InstaStoryLocationResponse> GetStoryLocationConverter(
            InstaStoryLocationResponse response)
        {
            return new InstaStoryLocationConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryMedia, InstaStoryMediaResponse> GetStoryMediaConverter(
            InstaStoryMediaResponse storyMedia)
        {
            return new InstaStoryMediaConverter { SourceObject = storyMedia };
        }

        public IObjectConverter<InstaStoryPollItem, InstaStoryPollItemResponse> GetStoryPollItemConverter(
            InstaStoryPollItemResponse response)
        {
            return new InstaStoryPollItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryPollStickerItem, InstaStoryPollStickerItemResponse>
            GetStoryPollStickerItemConverter(
                InstaStoryPollStickerItemResponse response)
        {
            return new InstaStoryPollStickerItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryPollVoterInfoItem, InstaStoryPollVoterInfoItemResponse>
            GetStoryPollVoterInfoItemConverter(
                InstaStoryPollVoterInfoItemResponse response)
        {
            return new InstaStoryPollVoterInfoItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryVoterItem, InstaStoryVoterItemResponse> GetStoryPollVoterItemConverter(
            InstaStoryVoterItemResponse response)
        {
            return new InstaStoryPollVoterItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryPollVotersList, InstaStoryPollVotersListResponse>
            GetStoryPollVotersListConverter(
                InstaStoryPollVotersListResponse response)
        {
            return new InstaStoryPollVotersListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryQuestionInfo, InstaStoryQuestionInfoResponse> GetStoryQuestionInfoConverter(
            InstaStoryQuestionInfoResponse response)
        {
            return new InstaStoryQuestionInfoConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryQuestionItem, InstaStoryQuestionItemResponse> GetStoryQuestionItemConverter(
            InstaStoryQuestionItemResponse response)
        {
            return new InstaStoryQuestionItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryQuestionResponder, InstaStoryQuestionResponderResponse>
            GetStoryQuestionResponderConverter(
                InstaStoryQuestionResponderResponse response)
        {
            return new InstaStoryQuestionResponderConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryQuestionStickerItem, InstaStoryQuestionStickerItemResponse>
            GetStoryQuestionStickerItemConverter(
                InstaStoryQuestionStickerItemResponse response)
        {
            return new InstaStoryQuestionStickerItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStorySliderItem, InstaStorySliderItemResponse> GetStorySliderItemConverter(
            InstaStorySliderItemResponse response)
        {
            return new InstaStorySliderItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStorySliderStickerItem, InstaStorySliderStickerItemResponse>
            GetStorySliderStickerItemConverter(
                InstaStorySliderStickerItemResponse response)
        {
            return new InstaStorySliderStickerItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStorySliderVoterInfoItem, InstaStorySliderVoterInfoItemResponse>
            GetStorySliderVoterInfoItemConverter(
                InstaStorySliderVoterInfoItemResponse response)
        {
            return new InstaStorySliderVoterInfoItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryTalliesItem, InstaStoryTalliesItemResponse> GetStoryTalliesItemConverter(
            InstaStoryTalliesItemResponse response)
        {
            return new InstaStoryTalliesItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaStoryTray, InstaStoryTrayResponse> GetStoryTrayConverter(
            InstaStoryTrayResponse storyTray)
        {
            return new InstaStoryTrayConverter { SourceObject = storyTray };
        }

        public IObjectConverter<InstaSuggestionItem, InstaSuggestionItemResponse> GetSuggestionItemConverter(
            InstaSuggestionItemResponse response)
        {
            return new InstaSuggestionItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaSuggestionItemList, InstaSuggestionItemListResponse>
            GetSuggestionItemListConverter(
                InstaSuggestionItemListResponse response)
        {
            return new InstaSuggestionItemListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaSuggestions, InstaSuggestionUserContainerResponse> GetSuggestionsConverter(
            InstaSuggestionUserContainerResponse response)
        {
            return new InstaSuggestionsConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTagFeed, InstaTagFeedResponse> GetTagFeedConverter(
            InstaTagFeedResponse feedResponse)
        {
            return new InstaTagFeedConverter { SourceObject = feedResponse };
        }

        public IObjectConverter<InstaTopicalExploreFeed, InstaTopicalExploreFeedResponse>
            GetTopicalExploreFeedConverter(
                InstaTopicalExploreFeedResponse response)
        {
            return new InstaTopicalExploreFeedConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTopLive, InstaTopLiveResponse> GetTopLiveConverter(
            InstaTopLiveResponse response)
        {
            return new InstaTopLiveConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTranslateList, InstaTranslateContainerResponse> GetTranslateContainerConverter(
            InstaTranslateContainerResponse response)
        {
            return new InstaTranslateContainerConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTvChannel, InstaTvChannelResponse> GetTvChannelConverter(
            InstaTvChannelResponse response)
        {
            return new InstaTvChannelConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTv, InstaTvResponse> GetTvConverter(
            InstaTvResponse response)
        {
            return new InstaTvConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTvSearch, InstaTvSearchResponse> GetTvSearchConverter(
            InstaTvSearchResponse response)
        {
            return new InstaTvSearchConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTvSearchResult, InstaTvSearchResultResponse> GetTvSearchResultConverter(
            InstaTvSearchResultResponse response)
        {
            return new InstaTvSearchResultConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTvSelfChannel, InstaTvSelfChannelResponse> GetTvSelfChannelConverter(
            InstaTvSelfChannelResponse response)
        {
            return new InstaTvSelfChannelConverter { SourceObject = response };
        }

        public IObjectConverter<InstaTvUser, InstaTvUserResponse> GetTvUserConverter(
            InstaTvUserResponse response)
        {
            return new InstaTvUserConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserChainingList, InstaUserChainingContainerResponse> GetUserChainingListConverter(
            InstaUserChainingContainerResponse response)
        {
            return new InstaUserChainingListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaContactUserList, InstaContactUserListResponse> GetUserContactListConverter(
            InstaContactUserListResponse response)
        {
            return new InstaUserContactListConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUser, InstaUserResponse> GetUserConverter(InstaUserResponse instaresponse)
        {
            return new InstaUserConverter { SourceObject = instaresponse };
        }

        public IObjectConverter<InstaUserInfo, InstaUserInfoContainerResponse> GetUserInfoConverter(
            InstaUserInfoContainerResponse response)
        {
            return new InstaUserInfoConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserLookup, InstaUserLookupResponse> GetUserLookupConverter(
            InstaUserLookupResponse response)
        {
            return new InstaUserLookupConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserPresenceList, InstaUserPresenceContainerResponse> GetUserPresenceListConverter(
            InstaUserPresenceContainerResponse response)
        {
            return new InstaUserPresenceListConverter { SourceObject = response };
        }

        public IObjectConverter<UserShortDescription, InstaUserShortResponse> GetUserShortConverter(
            InstaUserShortResponse instaresponse)
        {
            return new InstaUserShortConverter { SourceObject = instaresponse };
        }

        public IObjectConverter<InstaUserShortDescriptionFriendship, InstaUserShortFriendshipResponse>
            GetUserShortFriendshipConverter(
                InstaUserShortFriendshipResponse response)
        {
            return new InstaUserShortFriendshipConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserShortDescriptionFriendshipFull, InstaUserShortFriendshipFullResponse>
            GetUserShortFriendshipFullConverter(
                InstaUserShortFriendshipFullResponse response)
        {
            return new InstaUserShortFriendshipFullConverter { SourceObject = response };
        }

        public IObjectConverter<InstaUserTag, InstaUserTagResponse> GetUserTagConverter(InstaUserTagResponse tag)
        {
            return new InstaUserTagConverter { SourceObject = tag };
        }

        public IObjectConverter<InstaVisualMediaContainer, InstaVisualMediaContainerResponse>
            GetVisualMediaContainerConverter(
                InstaVisualMediaContainerResponse response)
        {
            return new InstaVisualMediaContainerConverter { SourceObject = response };
        }

        public IObjectConverter<InstaVisualMedia, InstaVisualMediaResponse> GetVisualMediaConverter(
            InstaVisualMediaResponse response)
        {
            return new InstaVisualMediaConverter { SourceObject = response };
        }

        public IObjectConverter<InstaVoice, InstaVoiceResponse> GetVoiceConverter(
            InstaVoiceResponse response)
        {
            return new InstaVoiceConverter { SourceObject = response };
        }

        public IObjectConverter<InstaVoiceMedia, InstaVoiceMediaResponse> GetVoiceMediaConverter(
            InstaVoiceMediaResponse response)
        {
            return new InstaVoiceMediaConverter { SourceObject = response };
        }

        public IObjectConverter<InstaWebAccountInfo, InstaWebSettingsPageResponse> GetWebAccountInfoConverter(
            InstaWebSettingsPageResponse response)
        {
            return new InstaWebAccountInfoConverter { SourceObject = response };
        }

        public IObjectConverter<InstaWebData, InstaWebSettingsPageResponse> GetWebDataConverter(
            InstaWebSettingsPageResponse response)
        {
            return new InstaWebDataConverter { SourceObject = response };
        }

        public IObjectConverter<InstaWebDataItem, InstaWebDataItemResponse> GetWebDataItemConverter(
            InstaWebDataItemResponse response)
        {
            return new InstaWebDataItemConverter { SourceObject = response };
        }

        public IObjectConverter<InstaWebTextData, InstaWebSettingsPageResponse> GetWebTextDataListConverter(
            InstaWebSettingsPageResponse response)
        {
            return new InstaWebTextDataConverter { SourceObject = response };
        }
    }
}