using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Logic;

namespace Wikiled.Instagram.Api.Helpers
{
    internal class InstaUriCreator
    {
        private static readonly Uri BaseInstagramUri = new Uri(InstaApiConstants.InstagramUrl);

        public static Uri GetAcceptFriendshipUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsApprove, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for accept friendship");
            }

            return instaUri;
        }

        public static Uri GetAccount2FaLoginAgainUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.Accounts2FaLoginAgain, out var instaUri))
            {
                throw new Exception("Cant create URI for Account 2FA Login Again");
            }

            return instaUri;
        }

        public static Uri GetAccountDetailsUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.UsersAccountDetails, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for account details");
            }

            return instaUri;
        }

        public static Uri GetAccountGetCommentFilterUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsGetCommentFilter, out var instaUri))
            {
                throw new Exception("Cant create URI for accounts get comment filter");
            }

            return instaUri;
        }

        public static Uri GetAccountRecoverPhoneUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsLookupPhone, out var instaUri))
            {
                throw new Exception("Cant create URI for Account Recovery phone");
            }

            return instaUri;
        }

        public static Uri GetAccountRecoveryEmailUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSendRecoveryEmail, out var instaUri))
            {
                throw new Exception("Cant create URI for Account Recovery Email");
            }

            return instaUri;
        }

        public static Uri GetAccountSecurityInfoUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSecurityInfo, out var instaUri))
            {
                throw new Exception("Cant create URI for accounts security info");
            }

            return instaUri;
        }

        public static Uri GetAccountSendConfirmEmailUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSendConfirmEmail, out var instaUri))
            {
                throw new Exception("Cant create URI for accounts send confirm email");
            }

            return instaUri;
        }

        public static Uri GetAccountSendSmsCodeUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSendSmsCode, out var instaUri))
            {
                throw new Exception("Cant create URI for accounts send sms code");
            }

            return instaUri;
        }

        public static Uri GetAccountSetPresenseDisabledUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSetPresenceDisabled, out var instaUri))
            {
                throw new Exception("Cant create URI for accounts set presence disabled");
            }

            return instaUri;
        }

        public static Uri GetAccountVerifySmsCodeUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsVerifySmsCode, out var instaUri))
            {
                throw new Exception("Cant create URI for accounts verify sms code");
            }

            return instaUri;
        }

        public static Uri GetAddUserToDirectThreadUri(string threadId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.DirectThreadAddUser, threadId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for add users to direct thread");
            }

            return instaUri;
        }

        public static Uri GetAllowMediaCommetsUri(string mediaId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.AllowMediaComments, mediaId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI to allow comments on media");
            }

            return instaUri;
        }

        public static Uri GetApprovePendingDirectRequestUri(string threadId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.GetDirectThreadApprove, threadId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for approve inbox thread");
            }

            return instaUri;
        }

        public static Uri GetApprovePendingMultipleDirectRequestUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.GetDirectThreadApproveMultiple,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for approve multiple inbox threads");
            }

            return instaUri;
        }

        public static Uri GetArchivedMediaFeedsListUri(string nextId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FeedOnlyMeFeed, out var instaUri))
            {
                throw new Exception("Cant create URI for arhcived media feeds");
            }

            return !string.IsNullOrEmpty(nextId)
                ? new UriBuilder(instaUri) { Query = $"max_id={nextId}" }.Uri
                : instaUri;
        }

        public static Uri GetArchiveMediaUri(string mediaId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.MediaArchive, mediaId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for archive an post");
            }

            return instaUri;
        }

        public static Uri GetBestFriendsUri(string maxId)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FriendshipsBesties, out var instaUri))
            {
                throw new Exception("Cant create URI for user besties");
            }

            return !string.IsNullOrEmpty(maxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={maxId}" }.Uri
                : instaUri;
        }

        public static Uri GetBestiesSuggestionUri(string maxId)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FriendshipsBestiesSuggestions, out var instaUri))
            {
                throw new Exception("Cant create URI for user besties suggestions");
            }

            return !string.IsNullOrEmpty(maxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={maxId}" }.Uri
                : instaUri;
        }

        public static Uri GetBlockedCommentersUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsGetBlockedCommenters, out var instaUri))
            {
                throw new Exception("Cant create URI for blocked commenters");
            }

            return instaUri;
        }

        public static Uri GetBlockedMediaUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.MediaBlocked, out var instaUri))
            {
                throw new Exception("Cant create URI for blocked media");
            }

            return instaUri;
        }

        public static Uri GetBlockedStoriesUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FriendshipsBlockedReel, out var instaUri))
            {
                throw new Exception("Cant create URI for blocked stories");
            }

            return instaUri;
        }

        public static Uri GetBlockedUsersUri(string maxId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersBlockedList, out var instaUri))
            {
                throw new Exception("Cant create URI for blocked users");
            }

            return !string.IsNullOrEmpty(maxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={maxId}" }.Uri
                : instaUri;
        }

        public static Uri GetBlockUserUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsBlockUser, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media likers");
            }

            return instaUri;
        }

        public static Uri GetBroadcastAddToPostLiveUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveAddToPostLive, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast add to post live");
            }

            return instaUri;
        }

        public static Uri GetBroadcastCommentUri(string broadcastId, string lastcommentts = "")
        {
            if (lastcommentts == "")
            {
                if (!Uri.TryCreate(BaseInstagramUri,
                                   string.Format(InstaApiConstants.LiveGetComment, broadcastId),
                                   out var instaUri))
                {
                    throw new Exception("Cant create URI for broadcast get comments");
                }

                return instaUri;
            }
            else
            {
                if (!Uri.TryCreate(BaseInstagramUri,
                                   string.Format(InstaApiConstants.LiveGetCommentLastcommentts,
                                                 broadcastId,
                                                 lastcommentts),
                                   out var instaUri))
                {
                    throw new Exception("Cant create URI for broadcast get comments");
                }

                return instaUri;
            }
        }

        public static Uri GetBroadcastCreateUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.LiveCreate, out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast create");
            }

            return instaUri;
        }

        public static Uri GetBroadcastDeletePostLiveUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveDeletePostLive, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast delete post live");
            }

            return instaUri;
        }

        public static Uri GetBroadcastDisableCommenstUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveMuteComments, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast disable comments");
            }

            return instaUri;
        }

        public static Uri GetBroadcastEnableCommenstUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveUnmuteComments, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast enable comments");
            }

            return instaUri;
        }

        public static Uri GetBroadcastEndUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveEnd, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast end");
            }

            return instaUri;
        }

        public static Uri GetBroadcastInfoUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveInfo, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for get broadcast info");
            }

            return instaUri;
        }

        public static Uri GetBroadcastJoinRequestsUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveGetJoinRequests, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast join requests");
            }

            return instaUri;
        }

        public static Uri GetBroadcastPinCommentUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LivePinComment, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast pin comment");
            }

            return instaUri;
        }

        public static Uri GetBroadcastPostCommentUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveComment, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast comments");
            }

            return instaUri;
        }

        public static Uri GetBroadcastPostLiveCommentUri(string broadcastId, int startingOffset, string encodingTag)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveGetPostLiveComment,
                                             broadcastId,
                                             startingOffset,
                                             encodingTag),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast post live comment");
            }

            return instaUri;
        }

        public static Uri GetBroadcastPostLiveLikesUri(string broadcastId, int startingOffset, string encodingTag)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LivePostLiveLikes,
                                             broadcastId,
                                             startingOffset,
                                             encodingTag),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast post live likes");
            }

            return instaUri;
        }

        public static Uri GetBroadcastReelShareUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastReelShare, out var instaUri))
            {
                throw new Exception("Cant create URI for direct broadcast reel share");
            }

            return instaUri;
        }

        public static Uri GetBroadcastStartUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveStart, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast start");
            }

            return instaUri;
        }

        public static Uri GetBroadcastUnPinCommentUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveUnpinComment, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast unpin comments");
            }

            return instaUri;
        }

        public static Uri GetBroadcastViewerListUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveGetViewerList, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for get broadcast viewer list");
            }

            return instaUri;
        }

        public static Uri GetBusinessBrandedSearchUserUri(string query, int count)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.BusinessBrandedUserSearch, query, count),
                out var instaUri))
            {
                throw new Exception("Cant create URI for business branded user search");
            }

            return instaUri;
        }

        public static Uri GetBusinessBrandedSettingsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.BusinessBrandedGetSettings, out var instaUri))
            {
                throw new Exception("Cant create URI for business branded settings");
            }

            return instaUri;
        }

        public static Uri GetBusinessBrandedUpdateSettingsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.BusinessBrandedUpdateSettings, out var instaUri))
            {
                throw new Exception("Cant create URI for business branded update settings");
            }

            return instaUri;
        }

        public static Uri GetBusinessGraphQlUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.GraphQl,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for business graph ql");
            }

            return instaUri;
        }

        public static Uri GetBusinessInstantExperienceUri(string data)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(
                        InstaApiConstants.BusinessInstantExperience,
                        data,
                        InstaApiConstants.IgSignatureKeyVersion),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for business instant experience");
            }

            return instaUri;
        }

        public static Uri GetBusinessValidateUrlUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.BusinessValidateUrl,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for business validate url");
            }

            return instaUri;
        }

        public static Uri GetChallengeReplayUri(string apiPath)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               InstaApiConstants.ApiSuffix + apiPath.Replace("challenge/", "challenge/replay/"),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for challenge require url");
            }

            return instaUri;
        }

        public static Uri GetChallengeRequireFirstUri(string apiPath, string guid, string deviceId)
        {
            if (!apiPath.EndsWith("/"))
            {
                apiPath = apiPath + "/";
            }

            if (!Uri.TryCreate(
                BaseInstagramUri,
                InstaApiConstants.ApiSuffix +
                apiPath +
                $"?guid={guid}&device_id={deviceId}",
                out var instaUri))
            {
                throw new Exception("Cant create URI for challenge require url");
            }

            return instaUri;
        }

        public static Uri GetChallengeRequireUri(string apiPath)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.ApiSuffix + apiPath, out var instaUri))
            {
                throw new Exception("Cant create URI for challenge require url");
            }

            return instaUri;
        }

        public static Uri GetChallengeUri()
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                InstaApiConstants.Challenge,
                out var instaUri))
            {
                throw new Exception("Cant create URI for challenge url");
            }

            return instaUri;
        }

        public static Uri GetChangePasswordUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.ChangePassword, out var instaUri))
            {
                throw new Exception("Can't create URI for changing password");
            }

            return instaUri;
        }

        public static Uri GetChangeProfilePictureUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsChangeProfilePicture, out var instaUri))
            {
                throw new Exception("Cant create URI for change profile picture");
            }

            return instaUri;
        }

        public static Uri GetCheckEmailUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersCheckEmail, out var instaUri))
            {
                throw new Exception("Cant create URI for check email");
            }

            return instaUri;
        }

        public static Uri GetCheckPhoneNumberUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsCheckPhoneNumber, out var instaUri))
            {
                throw new Exception("Cant create URI for check phone number");
            }

            return instaUri;
        }

        public static Uri GetCheckUsernameUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersCheckUsername, out var instaUri))
            {
                throw new Exception("Cant create URI for check username");
            }

            return instaUri;
        }

        public static Uri GetClearSearchHistoryUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FbsearchClearSearchHistory, out var instaUri))
            {
                throw new Exception("Cant create URI for clear search history");
            }

            return instaUri;
        }

        public static Uri GetCollectionsUri(string nextMaxId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                InstaApiConstants.GetListCollections,
                out var instaUri))
            {
                throw new Exception("Can't create URI for getting collections");
            }

            return !string.IsNullOrEmpty(nextMaxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={nextMaxId}" }.Uri
                : instaUri;
        }

        public static Uri GetCollectionUri(long collectionId, string nextMaxId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.GetCollection, collectionId),
                out var instaUri))
            {
                throw new Exception("Can't create URI for getting collection");
            }

            return !string.IsNullOrEmpty(nextMaxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={nextMaxId}" }.Uri
                : instaUri;
        }

        public static Uri GetConsentNewUserFlowBeginsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.ConsentNewUserFlowBegins, out var instaUri))
            {
                throw new Exception("Cant create URI for request for consent new user flow begins.");
            }

            return instaUri;
        }

        public static Uri GetConsentNewUserFlowUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.ConsentNewUserFlow, out var instaUri))
            {
                throw new Exception("Cant create URI for request for consent new user flow.");
            }

            return instaUri;
        }

        public static Uri GetConvertToBusinessAccountUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               InstaApiConstants.BusinessConvertToBusinessAccount,
                               out var instaUri))
            {
                throw new Exception("Cant create URI for convert to business account");
            }

            return instaUri;
        }

        public static Uri GetConvertToPersonalAccountUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsConvertToPersonal, out var instaUri))
            {
                throw new Exception("Cant create URI for account convert to personal account");
            }

            return instaUri;
        }

        public static Uri GetCreateAccountUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsCreate, out var instaUri))
            {
                throw new Exception("Cant create URI for user creation");
            }

            return instaUri;
        }

        public static Uri GetCreateBusinessInfoUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsCreateBusinessInfo, out var instaUri))
            {
                throw new Exception("Cant create URI for account create business info");
            }

            return instaUri;
        }

        public static Uri GetCreateCollectionUri()
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                InstaApiConstants.CreateCollection,
                out var instaUri))
            {
                throw new Exception("Can't create URI for creating collection");
            }

            return instaUri;
        }

        public static Uri GetCreateValidatedUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsCreateValidated, out var instaUri))
            {
                throw new Exception("Cant create URI for accounbts create validated");
            }

            return instaUri;
        }

        public static Uri GetCurrentUserUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.Currentuser, out var instaUri))
            {
                throw new Exception("Cant create URI for current user info");
            }

            return instaUri;
        }

        public static Uri GetDeclineAllPendingDirectRequestsUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.GetDirectThreadDeclineall,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for decline all pending direct requests");
            }

            return instaUri;
        }

        public static Uri GetDeclineMultplePendingDirectRequestsUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.GetDirectThreadDeclineMultiple,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for decline all pending direct requests");
            }

            return instaUri;
        }

        public static Uri GetDeclinePendingDirectRequestUri(string threadId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.GetDirectThreadDecline, threadId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for decline pending direct request");
            }

            return instaUri;
        }

        public static Uri GetDeleteCollectionUri(long collectionId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.DeleteCollection, collectionId),
                out var instaUri))
            {
                throw new Exception("Can't create URI for deleting collection");
            }

            return instaUri;
        }

        public static Uri GetDeleteCommentUri(string mediaId, string commentId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DeleteComment, mediaId, commentId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for delete comment");
            }

            return instaUri;
        }

        public static Uri GetDeleteDirectMessageUri(string threadId, string itemId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.DirectThreadHide, threadId, itemId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for delete direct message");
            }

            return instaUri;
        }

        public static Uri GetDeleteMediaUri(string mediaId, InstaMediaType mediaType)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.DeleteMedia, mediaId, mediaType.ToString().ToUpper()),
                out var instaUri))
            {
                throw new Exception("Can't create URI for deleting media");
            }

            return instaUri;
        }

        public static Uri GetDeleteMultipleCommentsUri(string mediaId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DeleteMultipleComment, mediaId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for delete multiple comments");
            }

            return instaUri;
        }

        public static Uri GetDeleteStoryMediaUri(string mediaId, InstaSharingType mediaType)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.DeleteMedia, mediaId, mediaType.ToString().ToUpper()),
                out var instaUri))
            {
                throw new Exception("Can't create URI for deleting media story");
            }

            return instaUri;
        }

        public static Uri GetDenyFriendshipUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsIgnore, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for deny friendship");
            }

            return instaUri;
        }

        public static Uri GetDirectConfigVideoUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastConfigureVideo, out var instaUri))
            {
                throw new Exception("Cant create URI for direct config video");
            }

            return instaUri;
        }

        public static Uri GetDirectInboxThreadUri(string threadId, string nextId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.GetDirectThread, threadId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for get inbox thread by id");
            }

            return !string.IsNullOrEmpty(nextId)
                ? new UriBuilder(instaUri) { Query = $"use_unified_inbox=true&cursor={nextId}&direction=older" }.Uri
                : new UriBuilder(instaUri) { Query = "use_unified_inbox=true" }.Uri;
        }

        public static Uri GetDirectInboxUri(string nextId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetDirectInbox, out var instaUri))
            {
                throw new Exception("Cant create URI for get inbox");
            }

            return !string.IsNullOrEmpty(nextId)
                ? new UriBuilder(instaUri)
                {
                    Query = $"persistentBadging=true&use_unified_inbox=true&cursor={nextId}&direction=older"
                }.Uri
                : new UriBuilder(instaUri) { Query = "persistentBadging=true&use_unified_inbox=true" }.Uri;

            //: instaUri;
            //        return instaUri
            ////GET /api/v1/direct_v2/inbox/?visual_message_return_type=unseen&persistentBadging=true&use_unified_inbox=true
            //.AddQueryParameterIfNotEmpty("visual_message_return_type", "unseen")
            //.AddQueryParameterIfNotEmpty("persistentBadging", "true")
            //.AddQueryParameterIfNotEmpty("use_unified_inbox", "true")
            //.AddQueryParameterIfNotEmpty("cursor", NextId);
        }

        public static Uri GetDirectPendingInboxUri(string nextId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetDirectPendingInbox, out var instaUri))
            {
                throw new Exception("Cant create URI for get pending inbox");
            }

            return !string.IsNullOrEmpty(nextId)
                ? new UriBuilder(instaUri) { Query = $"cursor={nextId}" }.Uri
                : instaUri;
        }

        public static Uri GetDirectPresenceUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.DirectPresence,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for direct presence");
            }

            return instaUri;
        }

        public static Uri GetDirectSendMessageUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetDirectTextBroadcast, out var instaUri))
            {
                throw new Exception("Cant create URI for sending message");
            }

            return instaUri;
        }

        public static Uri GetDirectSendPhotoUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastUploadPhoto, out var instaUri))
            {
                throw new Exception("Cant create URI for sending photo to direct");
            }

            return instaUri;
        }

        public static Uri GetDirectThreadBroadcastLikeUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastThreadLike, out var instaUri))
            {
                throw new Exception("Cant create URI for broadcast post live likes");
            }

            return instaUri;
        }

        public static Uri GetDirectThreadSeenUri(string threadId, string itemId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.DirectThreadSeen, threadId, itemId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for seen thread");
            }

            return instaUri;
        }

        public static Uri GetDirectThreadUpdateTitleUri(string threadId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.DirectThreadUpdateTitle, threadId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for update thread title");
            }

            return instaUri;
        }

        public static Uri GetDisableMediaCommetsUri(string mediaId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DisableMediaComments, mediaId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI to disable comments on media");
            }

            return instaUri;
        }

        public static Uri GetDisableSmsTwoFactorUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsDisableSmsTwoFactor, out var instaUri))
            {
                throw new Exception("Cant create URI for disable sms two factor");
            }

            return instaUri;
        }

        public static Uri GetDiscoverChainingUri(long userId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DiscoverChaining, userId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for discover chaining");
            }

            return instaUri;
        }

        public static Uri GetDiscoverPeopleUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DiscoverAyml, out var instaUri))
            {
                throw new Exception("Cant create URI for discover people");
            }

            return instaUri;
        }

        public static Uri GetDiscoverSuggestionDetailsUri(long userId, List<long> chainedIds)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(
                        InstaApiConstants.DiscoverFetchSuggestionDetails,
                        userId,
                        string.Join(",", chainedIds)),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for discover suggestion details");
            }

            return instaUri;
        }

        public static Uri GetDiscoverTopLiveStatusUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DiscoverTopLiveStatus, out var instaUri))
            {
                throw new Exception("Cant create URI for discover top live status");
            }

            return instaUri;
        }

        public static Uri GetDiscoverTopLiveUri(string maxId)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DiscoverTopLive, out var instaUri))
            {
                throw new Exception("Cant create URI for discover top live");
            }

            return instaUri.AddQueryParameterIfNotEmpty("max_id", maxId);
        }

        public static Uri GetEditCollectionUri(long collectionId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.EditCollection, collectionId),
                out var instaUri))
            {
                throw new Exception("Can't create URI for editing collection");
            }

            return instaUri;
        }

        public static Uri GetEditMediaUri(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.EditMedia, mediaId),
                out var instaUri))
            {
                throw new Exception("Can't create URI for editing media");
            }

            return instaUri;
        }

        public static Uri GetEditProfileUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsEditProfile, out var instaUri))
            {
                throw new Exception("Cant create URI for edit profile");
            }

            return instaUri;
        }

        public static Uri GetEnableSmsTwoFactorUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsEnableSmsTwoFactor, out var instaUri))
            {
                throw new Exception("Cant create URI for enable sms two factor");
            }

            return instaUri;
        }

        public static Uri GetExploreUri(string maxId = null, string rankToken = null)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DiscoverExplore, out var instaUri))
            {
                throw new Exception("Cant create URI for explore posts");
            }

            var query =
                $"is_prefetch=false&is_from_promote=true&timezone_offset={InstaApiConstants.TimezoneOffset}&supported_capabilities_new={JsonConvert.SerializeObject(InstaApiConstants.SupportedCapabalities)}";
            if (!string.IsNullOrEmpty(maxId))
            {
                query += $"&max_id={maxId}&session_id={rankToken}";
            }

            var uriBuilder = new UriBuilder(instaUri) { Query = query };
            return uriBuilder.Uri;
        }

        public static Uri GetFacebookSignUpUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FbFacebookSignup, out var instaUri))
            {
                throw new Exception("Cant create URI for facebook sign up url");
            }

            return instaUri;
        }

        public static Uri GetFavoriteForUserStoriesUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsFavoriteForStories, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for favorite user stories");
            }

            return instaUri;
        }

        public static Uri GetFavoriteUserUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsFavorite, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for favorite user");
            }

            return instaUri;
        }

        public static Uri GetFollowHashtagUri(string hashtag)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.TagFollow, hashtag),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for follow hashtag");
            }

            return instaUri;
        }

        public static Uri GetFollowingRecentActivityUri(string maxId = null)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetFollowingRecentActivity, out var instaUri))
            {
                throw new Exception("Cant create URI (get following recent activity");
            }

            var query = string.Empty;
            if (!string.IsNullOrEmpty(maxId))
            {
                query += $"max_id={maxId}";
            }

            var uriBuilder = new UriBuilder(instaUri) { Query = query };
            return uriBuilder.Uri;
        }

        public static Uri GetFollowingTagsInfoUri(long userId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(
                        InstaApiConstants.UsersFollowingTagInfo,
                        userId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for suggested tags");
            }

            return instaUri;
        }

        public static Uri GetFollowUserUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsFollowUser, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media likers");
            }

            return instaUri;
        }

        public static Uri GetFriendshipPendingRequestsUri(string rankToken)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsPendingRequests, rankToken),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for friendship pending requests");
            }

            return instaUri;
        }

        public static Uri GetFriendshipShowManyUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.FriendshipsShowMany,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for friendship show many");
            }

            return instaUri;
        }

        public static Uri GetFullUserInfoUri(long userId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.UsersFullDetailInfo, userId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for full user info");
            }

            return instaUri;
        }

        public static Uri GetGraphStatisticsUri(string locale,
                                                InstaInsightSurfaceType surfaceType = InstaInsightSurfaceType.Account)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.GraphQlStatistics, locale, surfaceType.ToString().ToLower()),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for graph ql statistics");
            }

            return instaUri;
        }

        public static Uri GetHashtagRankedMediaUri(
            string hashtag,
            string rankToken = null,
            string nextId = null,
            int? page = null,
            IEnumerable<long> nextMediaIds = null)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.TagRanked, hashtag.EncodeUri()),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for hashtag ranked(top) media");
            }

            if (!string.IsNullOrEmpty(rankToken))
            {
                instaUri = instaUri.AddQueryParameter("rank_token", rankToken);
            }

            if (!string.IsNullOrEmpty(nextId))
            {
                instaUri = instaUri
                    .AddQueryParameter("max_id", nextId);
            }

            if (page != null && page > 0)
            {
                instaUri = instaUri
                    .AddQueryParameter("page", page.ToString());
            }

            if (nextMediaIds != null && nextMediaIds.Any())
            {
                var mediaIds = $"[{string.Join(",", nextMediaIds)}]";
                instaUri = instaUri
                    .AddQueryParameter("next_media_ids", mediaIds.EncodeUri());
            }

            return instaUri;
        }

        public static Uri GetHashtagRecentMediaUri(
            string hashtag,
            string rankToken = null,
            string nextId = null,
            int? page = null,
            IEnumerable<long> nextMediaIds = null)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.TagRecent, hashtag.EncodeUri()),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for hashtag recent media");
            }

            if (!string.IsNullOrEmpty(nextId))
            {
                instaUri = instaUri
                    .AddQueryParameter("max_id", nextId.EncodeUri());
            }

            if (page != null && page > 0)
            {
                instaUri = instaUri
                    .AddQueryParameter("page", page.ToString());
            }

            if (!string.IsNullOrEmpty(rankToken))
            {
                if (rankToken.Contains("_"))
                {
                    instaUri = instaUri.AddQueryParameter("rank_token", rankToken.Split('_')[1]);
                }
                else
                {
                    instaUri = instaUri.AddQueryParameter("rank_token", rankToken);
                }
            }

            if (nextMediaIds != null && nextMediaIds.Any())
            {
                var mediaIds = $"[{string.Join(",", nextMediaIds)}]";
                instaUri = instaUri
                    .AddQueryParameter("next_media_ids", mediaIds.EncodeUri());
            }

            return instaUri;
        }

        public static Uri GetHashtagSectionUri(string hashtag)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.TagSection, hashtag),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for hashtag section");
            }

            return instaUri;
        }

        public static Uri GetHashtagStoryUri(string hashtag)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.TagStory, hashtag),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for hashtag story");
            }

            return instaUri;
        }

        public static Uri GetHideDirectThreadUri(string threadId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.DirectThreadHide, threadId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for hide direct thread");
            }

            return instaUri;
        }

        public static Uri GetHideMyStoryFromUserUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsBlockFriendReel, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for hide my story from specific user");
            }

            return instaUri;
        }

        public static Uri GetHighlightCreateUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.HighlightCreateReel, out var instaUri))
            {
                throw new Exception("Cant create URI for highlight create reel");
            }

            return instaUri;
        }

        public static Uri GetHighlightEditUri(string highlightId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.HighlightEditReel, highlightId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for highlight edit reel");
            }

            return instaUri;
        }

        public static Uri GetHighlightFeedsUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.HighlightTray, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for highlight feeds");
            }

            return instaUri;
        }

        public static Uri GetHighlightsArchiveUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.ArchiveReelDayShells,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for highlights archive");
            }

            return instaUri;
        }

        public static Uri GetIgtvChannelUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.IgtvChannel, out var instaUri))
            {
                throw new Exception("Cant create URI for igtv channel");
            }

            return instaUri;
        }

        public static Uri GetIgtvGuideUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.IgtvTvGuide, out var instaUri))
            {
                throw new Exception("Cant create URI for igtv tv guide");
            }

            return instaUri;
        }

        public static Uri GetIgtvSearchUri(string query)
        {
            if (!Uri.TryCreate(BaseInstagramUri, string.Format(InstaApiConstants.IgtvSearch, query), out var instaUri))
            {
                throw new Exception("Cant create URI for igtv search");
            }

            return instaUri;
        }

        public static Uri GetIgtvSuggestedSearchesUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.IgtvSuggestedSearches, out var instaUri))
            {
                throw new Exception("Cant create URI for igtv suggested searches");
            }

            return instaUri;
        }

        public static Uri GetLeaveThreadUri(string threadId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DirectThreadLeave, threadId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for leave group thread");
            }

            return instaUri;
        }

        public static Uri GetLikeCommentUri(string commentId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.LikeComment, commentId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for like comment");
            }

            return instaUri;
        }

        public static Uri GetLikeLiveUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveLike, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for like live");
            }

            return instaUri;
        }

        public static Uri GetLikeMediaUri(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.LikeMedia, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for like media");
            }

            return instaUri;
        }

        public static Uri GetLikeUnlikeDirectMessageUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastReaction, out var instaUri))
            {
                throw new Exception("Cant create URI for like direct message");
            }

            return instaUri;
        }

        public static Uri GetLiveFinalViewerListUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveGetFinalViewerList, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for get final viewer list");
            }

            return instaUri;
        }

        public static Uri GetLiveHeartbeatAndViewerCountUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveHeartbeatAndGetViewerCount, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for live heartbeat and get viewer count");
            }

            return instaUri;
        }

        public static Uri GetLiveLikeCountUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveGetLikeCount, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for live like count");
            }

            return instaUri;
        }

        public static Uri GetLiveNotifyToFriendsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.LiveGetLivePresence, out var instaUri))
            {
                throw new Exception("Cant create URI for get live presence");
            }

            return instaUri;
        }

        public static Uri GetLocationFeedUri(string locationId, string maxId = null)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.LocationFeed, locationId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for get location feed");
            }

            return instaUri
                .AddQueryParameterIfNotEmpty("max_id", maxId);
        }

        public static Uri GetLocationInfoUri(string externalId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.LocationsInfo, externalId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for location info");
            }

            return instaUri;
        }

        public static Uri GetLocationSearchUri()
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                InstaApiConstants.LocationSearch,
                out var instaUri))
            {
                throw new Exception("Cant create URI for location search");
            }

            return instaUri;
        }

        public static Uri GetLocationSectionUri(string locationId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.LocationSection, locationId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for get location section");
            }

            return instaUri;
        }

        public static Uri GetLoginUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsLogin, out var instaUri))
            {
                throw new Exception("Cant create URI for user login");
            }

            return instaUri;
        }

        public static Uri GetLogoutUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsLogout, out var instaUri))
            {
                throw new Exception("Cant create URI for user logout");
            }

            return instaUri;
        }

        public static Uri GetMarkUserOverageUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsMarkUserOverage, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for mark user overage");
            }

            return instaUri;
        }

        public static Uri GetMediaAlbumConfigureUri()
        {
            if (
                !Uri.TryCreate(BaseInstagramUri, InstaApiConstants.MediaAlbumConfigure, out var instaUri))
            {
                throw new Exception("Cant create URI for configuring media album");
            }

            return instaUri;
        }

        public static Uri GetMediaCommentsMinIdUri(string mediaId, string nextMinId = "")
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaComments, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media comments");
            }

            return !string.IsNullOrEmpty(nextMinId)
                ? new UriBuilder(instaUri) { Query = $"can_support_threading=true&min_id={nextMinId}" }.Uri
                : new UriBuilder(instaUri) { Query = "can_support_threading=true" }.Uri;
        }

        public static Uri GetMediaCommentsUri(string mediaId, string nextMaxId = "")
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaComments, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media comments");
            }

            return !string.IsNullOrEmpty(nextMaxId)
                ? new UriBuilder(instaUri) { Query = $"can_support_threading=true&max_id={nextMaxId}" }.Uri
                : new UriBuilder(instaUri) { Query = "can_support_threading=true" }.Uri;
        }

        public static Uri GetMediaCommetLikersUri(string mediaId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.MediaCommentLikers, mediaId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI to media comments likers");
            }

            return instaUri;
        }

        public static Uri GetMediaConfigureToIgtvUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.MediaConfigureToIgtv, out var instaUri))
            {
                throw new Exception("Cant create URI for media configure igtv");
            }

            return instaUri;
        }

        public static Uri GetMediaConfigureUri(bool video = false)
        {
            if (
                !Uri.TryCreate(BaseInstagramUri,
                               video ? InstaApiConstants.MediaConfigureVideo : InstaApiConstants.MediaConfigure,
                               out var instaUri))
            {
                throw new Exception("Cant create URI for configuring media");
            }

            return instaUri;
        }

        public static Uri GetMediaIdFromUrlUri(Uri uri)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.GetMediaid, uri.AbsoluteUri),
                out var instaUri))
            {
                throw new Exception("Can't create URI for getting media id");
            }

            return instaUri;
        }

        public static Uri GetMediaInfoByMultipleMediaIdsUri(string[] mediaIds, string uuid, string csrfToken)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(
                    InstaApiConstants.MediaInfos,
                    uuid,
                    csrfToken,
                    string.Join(",", mediaIds)),
                out var instaUri))
            {
                throw new Exception("Cant create URI for media info by multiple media ids");
            }

            return instaUri;
        }

        public static Uri GetMediaInlineCommentsUri(string mediaId, string targetCommentId, string nextMaxId = "")
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaInlineComments, mediaId, targetCommentId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media comments replies with max id");
            }

            return !string.IsNullOrEmpty(nextMaxId)

                //? new UriBuilder(instaUri) { Query = $"min_id={nextId}" }.Uri
                ? new UriBuilder(instaUri) { Query = $"max_id={nextMaxId}" }.Uri
                : instaUri;
        }

        public static Uri GetMediaInlineCommentsWithMinIdUri(string mediaId,
                                                             string targetCommentId,
                                                             string nextMinId = "")
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaInlineComments, mediaId, targetCommentId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media comment replies with min id");
            }

            return !string.IsNullOrEmpty(nextMinId)
                ? new UriBuilder(instaUri) { Query = $"min_id={nextMinId}" }.Uri
                : instaUri;
        }

        public static Uri GetMediaInsightsUri(string unixTime)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.InsightsMedia, unixTime),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for media insights");
            }

            return instaUri;
        }

        public static Uri GetMediaLikersUri(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaLikers, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media likers");
            }

            return instaUri;
        }

        public static Uri GetMediaNametagConfigureUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.MediaConfigureNametag, out var instaUri))
            {
                throw new Exception("Cant create URI for media nametag configure");
            }

            return instaUri;
        }

        public static Uri GetMediaShareUri(InstaMediaType mediaType)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DirectBroadcastMediaShare, mediaType.ToString().ToLower()),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for media share");
            }

            return instaUri;
        }

        public static Uri GetMediaSingleInsightsUri(string mediaPk)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(
                        InstaApiConstants.InsightsMediaSingle,
                        mediaPk,
                        InstaApiConstants.HeaderIgSignatureKeyVersion,
                        InstaApiConstants.IgSignatureKeyVersion),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for single media insights");
            }

            return instaUri;
        }

        public static Uri GetMediaUploadFinishUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.MediaUploadFinish, out var instaUri))
            {
                throw new Exception("Cant create URI for media upload finish");
            }

            return instaUri;
        }

        public static Uri GetMediaUri(string mediaId)
        {
            return Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.GetMedia, mediaId),
                out var instaUri)
                ? instaUri
                : null;
        }

        public static Uri GetMuteDirectThreadUri(string threadId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.DirectThreadMute, threadId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for mute direct thread");
            }

            return instaUri;
        }

        public static Uri GetMuteFriendStoryUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsMuteFriendReel, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for mute friend story");
            }

            return instaUri;
        }

        public static Uri GetMuteUserMediaStoryUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FriendshipsMutePostStory, out var instaUri))
            {
                throw new Exception("Cant create URI for mute user media or story");
            }

            return instaUri;
        }

        public static Uri GetOnboardingStepsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DynamicOnboardingGetSteps, out var instaUri))
            {
                throw new Exception("Cant create URI for dynamic onboarding get steps");
            }

            return instaUri;
        }

        public static Uri GetParticipantRecipientUserUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.GetParticipantsRecipientUser, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for get participants recipient user");
            }

            return instaUri;
        }

        public static Uri GetPostCommetUri(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.PostComment, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for posting comment");
            }

            return instaUri;
        }

        public static Uri GetPostLiveViewersListUri(string broadcastId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.LiveGetPostLiveViewersList, broadcastId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for get post live viewer list");
            }

            return instaUri;
        }

        public static Uri GetPresenceUri(string signedKey)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsGetPresence, out var instaUri))
            {
                throw new Exception("Cant create URI for get presence disabled");
            }

            //?signed_body=b941ff07b83716087710019790b3529ab123c8deabfb216e056651e9cf4b4ca7.{}&ig_sig_key_version=4
            var signedBody = signedKey + ".{}";
            var query =
                $"{InstaApiConstants.HeaderIgSignature}={signedBody}&{InstaApiConstants.HeaderIgSignatureKeyVersion}={InstaApiConstants.IgSignatureKeyVersion}";
            var uriBuilder = new UriBuilder(instaUri) { Query = query };
            return uriBuilder.Uri;
        }

        public static Uri GetProductInfoUri(long productId, string mediaPk, int deviceWidth)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(
                    InstaApiConstants.CommerceProductInfo,
                    productId,
                    mediaPk,
                    deviceWidth),
                out var instaUri))
            {
                throw new Exception("Cant create URI for product info");
            }

            return instaUri;
        }

        public static Uri GetProfileSearchUri(string query, int count)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FbsearchProfileSearch, query, count),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for profile search");
            }

            return instaUri;
        }

        public static Uri GetProfileSetPhoneAndNameUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSetPhoneAndName, out var instaUri))
            {
                throw new Exception("Cant create URI for sets phone and number");
            }

            return instaUri;
        }

        public static Uri GetPromotableMediaFeedsUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.FeedPromotableMedia,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for promotable media feeds");
            }

            return instaUri;
        }

        public static Uri GetPushRegisterUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, string.Format(InstaApiConstants.PushRegister), out var instaUri))
            {
                throw new Exception("Cant create URI for live heartbeat and get viewer count");
            }

            return instaUri;
        }

        public static Uri GetRankedRecipientsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetRankedRecipients, out var instaUri))
            {
                throw new Exception("Cant create URI (get ranked recipients)");
            }

            return instaUri;
        }

        public static Uri GetRankRecipientsByUserUri(string username)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.GetRankRecipientsByUsername, username),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for get rank recipients by username");
            }

            return instaUri;
        }

        public static Uri GetRecentActivityUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetRecentActivity, out var instaUri))
            {
                throw new Exception("Cant create URI (get recent activity)");
            }

            var query = "activity_module=all";
            var uriBuilder = new UriBuilder(instaUri) { Query = query };
            return uriBuilder.Uri;
        }

        public static Uri GetRecentRecipientsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetRecentRecipients, out var instaUri))
            {
                throw new Exception("Cant create URI (get recent recipients)");
            }

            return instaUri;
        }

        public static Uri GetRecentSearchUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FbsearchRecentSearches, out var instaUri))
            {
                throw new Exception("Cant create URI for facebook recent searches");
            }

            return instaUri;
        }

        public static Uri GetReelMediaUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.FeedReelMedia,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for reel media");
            }

            return instaUri;
        }

        public static Uri GetRegenerateTwoFactorBackUpCodeUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsRegenBackupCodes, out var instaUri))
            {
                throw new Exception("Cant create URI for regenerate two factor backup codes");
            }

            return instaUri;
        }

        public static Uri GetRemoveFollowerUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsRemoveFollower, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for remove follower");
            }

            return instaUri;
        }

        public static Uri GetRemoveProfilePictureUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsRemoveProfilePicture, out var instaUri))
            {
                throw new Exception("Cant create URI for remove profile picture");
            }

            return instaUri;
        }

        public static Uri GetReportCommetUri(string mediaId, string commentId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.MediaReportComment, mediaId, commentId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for report comment");
            }

            return instaUri;
        }

        public static Uri GetReportMediaUri(string mediaId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.MediaReport, mediaId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for report media");
            }

            return instaUri;
        }

        public static Uri GetReportUserUri(long userId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.UsersReport, userId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for report user");
            }

            return instaUri;
        }

        public static Uri GetRequestForDownloadDataUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DyiRequestDownloadData, out var instaUri))
            {
                throw new Exception("Cant create URI for request for download data.");
            }

            return instaUri;
        }

        public static Uri GetRequestForEditProfileUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsRequestProfileEdit, out var instaUri))
            {
                throw new Exception("Cant create URI for request editing profile");
            }

            return instaUri;
        }

        public static Uri GetResetChallengeRequireUri(string apiPath)
        {
            apiPath = apiPath.Replace("/challenge/", "/challenge/reset/");
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.ApiSuffix + apiPath, out var instaUri))
            {
                throw new Exception("Cant create URI for challenge require url");
            }

            return instaUri;
        }

        public static Uri GetSavedFeedUri(string maxId)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FeedSaved, out var instaUri))
            {
                throw new Exception("Cant create URI for get saved feed");
            }

            return !string.IsNullOrEmpty(maxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={maxId}" }.Uri
                : instaUri;
        }

        public static Uri GetSaveMediaUri(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaSave, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for save media");
            }

            return instaUri;
        }

        public static Uri GetSearchPlacesUri(int timezoneOffset,
                                             double lat,
                                             double lng,
                                             string query,
                                             string rankToken,
                                             List<long> excludeList)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FbsearchPlaces, out var instaUri))
            {
                throw new Exception("Cant create URI for search places");
            }

            var parameters =
                $"timezone_offset={timezoneOffset}&lat={lat.ToString(CultureInfo.InvariantCulture)}&lng={lng.ToString(CultureInfo.InvariantCulture)}";

            if (!string.IsNullOrEmpty(query))
            {
                parameters += $"&query={query}";
            }

            if (!string.IsNullOrEmpty(rankToken))
            {
                parameters += $"&rank_token={rankToken}";
            }

            if (excludeList?.Count > 0)
            {
                parameters += $"&exclude_list=[{string.Join(",", excludeList)}]";
            }

            return new UriBuilder(instaUri) { Query = parameters }.Uri;
        }

        public static Uri GetSearchTagUri(string tag, int count, IEnumerable<long> excludeList, string rankToken)
        {
            excludeList = excludeList ?? new List<long>();
            var excludeListStr = $"[{string.Join(",", excludeList)}]";
            if (!Uri.TryCreate(BaseInstagramUri, string.Format(InstaApiConstants.SearchTags, tag, count), out var instaUri))
            {
                throw new Exception("Cant create search tag URI");
            }

            return instaUri
                .AddQueryParameter("exclude_list", excludeListStr)
                .AddQueryParameter("rank_token", rankToken);
        }

        public static Uri GetSearchUserUri(string text, int count = 30)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(
                    InstaApiConstants.UsersSearch,
                    TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalSeconds,
                    text,
                    count),
                out var instaUri))
            {
                throw new Exception("Cant create URI for search user");
            }

            return instaUri;
        }

        public static Uri GetSeenMediaStoryUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.SeenMediaStory, out var instaUri))
            {
                throw new Exception("Cant create URI for seen media story");
            }

            return instaUri;
        }

        public static Uri GetSeenMediaUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.SeenMedia, out var instaUri))
            {
                throw new Exception("Cant create URI for seen media");
            }

            return instaUri;
        }

        public static Uri GetSendDirectHashtagUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastHashtag, out var instaUri))
            {
                throw new Exception("Cant create URI for send hashtag to direct thread");
            }

            return instaUri;
        }

        public static Uri GetSendDirectLinkUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastLink, out var instaUri))
            {
                throw new Exception("Cant create URI for send link to direct thread");
            }

            return instaUri;
        }

        public static Uri GetSendDirectLocationUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastLocation, out var instaUri))
            {
                throw new Exception("Cant create URI for send location to direct thread");
            }

            return instaUri;
        }

        public static Uri GetSendDirectProfileUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DirectBroadcastProfile, out var instaUri))
            {
                throw new Exception("Cant create URI for send profile to direct thread");
            }

            return instaUri;
        }

        public static Uri GetSendTwoFactorEnableSmsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               InstaApiConstants.AccountsSendTwoFactorEnableSms,
                               out var instaUri))
            {
                throw new Exception("Cant create URI for send two factor enable sms");
            }

            return instaUri;
        }

        public static Uri GetSetBestFriendsUri()
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                InstaApiConstants.FriendshipsSetBesties,
                out var instaUri))
            {
                throw new Exception("Cant create URI for set best friends");
            }

            return instaUri;
        }

        public static Uri GetSetBiographyUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSetBiography, out var instaUri))
            {
                throw new Exception("Cant create URI for set biography");
            }

            return instaUri;
        }

        public static Uri GetSetBlockedCommentersUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSetBlockedCommenters, out var instaUri))
            {
                throw new Exception("Cant create URI for set blocked commenters");
            }

            return instaUri;
        }

        public static Uri GetSetBusinessCategoryUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.BusinessSetCategory,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for set business category");
            }

            return instaUri;
        }

        public static Uri GetSetReelSettingsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersSetReelSettings, out var instaUri))
            {
                throw new Exception("Cant create URI for set reel settings");
            }

            return instaUri;
        }

        public static Uri GetShareLinkFromMediaId(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.GetShareLink, mediaId),
                out var instaUri))
            {
                throw new Exception("Can't create URI for getting share link");
            }

            return instaUri;
        }

        public static Uri GetShareLiveToDirectUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               InstaApiConstants.DirectBroadcastLiveViewerInvite,
                               out var instaUri))
            {
                throw new Exception("Cant create URI for share live to direct");
            }

            return instaUri;
        }

        public static Uri GetShareUserUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetDirectShareUser, out var instaUri))
            {
                throw new Exception("Cant create URI for share user");
            }

            return instaUri;
        }

        public static Uri GetSignUpSmsCodeUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsSendSignupSmsCode, out var instaUri))
            {
                throw new Exception("Cant create URI for send signup sms code");
            }

            return instaUri;
        }

        public static Uri GetStarThreadUri(string threadId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DirectStar, threadId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for star thread");
            }

            return instaUri;
        }

        public static Uri GetStoryConfigureUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.StoryConfigure, out var instaUri))
            {
                throw new Exception("Can't create URI for configuring story media");
            }

            return instaUri;
        }

        public static Uri GetStoryCountdownMediaUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.MediaStoryCountdowns, out var instaUri))
            {
                throw new Exception("Cant create URI for story countdown media");
            }

            return instaUri;
        }

        public static Uri GetStoryFeedUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.GetStoryTray, out var instaUri))
            {
                throw new Exception("Can't create URI for getting story tray");
            }

            return instaUri;
        }

        public static Uri GetStoryFollowCountdownUri(long countdownId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.MediaFollowCountdown, countdownId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for story follow countdown");
            }

            return instaUri;
        }

        public static Uri GetStoryMediaInfoUploadUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.StoryMediaInfoUpload, out var instaUri))
            {
                throw new Exception("Cant create URI for story media info");
            }

            return instaUri;
        }

        public static Uri GetStoryMediaViewersUri(string storyMediaId, string nextId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.MediaStoryViewers, storyMediaId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for story media viewers");
            }

            return !string.IsNullOrEmpty(nextId)
                ? new UriBuilder(instaUri) { Query = $"max_id={nextId}" }.Uri
                : instaUri;
        }

        public static Uri GetStoryPollVotersUri(string storyMediaId, string pollId, string maxId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaStoryPollVoters, storyMediaId, pollId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for get story poll voters list");
            }

            return !string.IsNullOrEmpty(maxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={maxId}" }.Uri
                : instaUri;
        }

        public static Uri GetStoryPollVoteUri(string storyMediaId, string pollId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaStoryPollVote, storyMediaId, pollId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for get story poll vote");
            }

            return instaUri;
        }

        public static Uri GetStoryQuestionResponseUri(string storyId, long questionid)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaStoryQuestionResponse, storyId, questionid),
                out var instaUri))
            {
                throw new Exception("Cant create URI for story question answer");
            }

            return instaUri;
        }

        public static Uri GetStorySettingsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersReelSettings, out var instaUri))
            {
                throw new Exception("Cant create URI for story settings");
            }

            return instaUri;
        }

        public static Uri GetStoryShareUri(string mediaType)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.StoryShare, mediaType),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for story share");
            }

            return instaUri;
        }

        public static Uri GetStoryUnFollowCountdownUri(long countdownId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.MediaUnfollowCountdown, countdownId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for story unfollow countdown");
            }

            return instaUri;
        }

        public static Uri GetStoryUploadPhotoUri(string uploadId, int fileHashCode)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.UploadPhoto, uploadId, fileHashCode),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for story upload photo");
            }

            return instaUri;
        }

        public static Uri GetStoryUploadVideoUri(string uploadId, int fileHashCode)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.UploadVideo, uploadId, fileHashCode),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for story upload video");
            }

            return instaUri;
        }

        public static Uri GetSuggestedBroadcastsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.LiveGetSuggestedBroadcasts, out var instaUri))
            {
                throw new Exception("Cant create URI for get suggested broadcasts");
            }

            return instaUri;
        }

        public static Uri GetSuggestedSearchUri(InstaDiscoverSearchType searchType)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FbsearchSuggestedSearchs,
                                             searchType.ToString().ToLower()),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for suggested search");
            }

            return instaUri;
        }

        public static Uri GetSuggestedTagsUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.TagSuggested,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for suggested tags");
            }

            return instaUri;
        }

        public static Uri GetSyncContactsUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.AddressbookLink,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for sync contacts");
            }

            return instaUri;
        }

        public static Uri GetTagFeedUri(string tag, string maxId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri, string.Format(InstaApiConstants.GetTagFeed, tag), out var instaUri))
            {
                throw new Exception("Cant create URI for discover tag feed");
            }

            return !string.IsNullOrEmpty(maxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={maxId}" }.Uri
                : instaUri;
        }

        public static Uri GetTagInfoUri(string tag)
        {
            if (!Uri.TryCreate(BaseInstagramUri, string.Format(InstaApiConstants.GetTagInfo, tag), out var instaUri))
            {
                throw new Exception("Cant create tag info URI");
            }

            return instaUri;
        }

        public static Uri GetTimelineWithMaxIdUri(string nextId)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.Timelinefeed, out var instaUri))
            {
                throw new Exception("Cant create search URI for timeline");
            }

            var uriBuilder = new UriBuilder(instaUri) { Query = $"max_id={nextId}" };
            return uriBuilder.Uri;
        }

        public static Uri GetTopicalExploreUri(string sessionId, string maxId = null, string clusterId = null)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.DiscoverTopicalExplore, out var instaUri))
            {
                throw new Exception("Cant create URI for topical explore");
            }

            instaUri = instaUri
                .AddQueryParameter("is_prefetch", "false")
                .AddQueryParameter("module", "explore_popular")
                .AddQueryParameter("use_sectional_payload", "true")
                .AddQueryParameter("timezone_offset", InstaApiConstants.TimezoneOffset.ToString())
                .AddQueryParameter("session_id", sessionId)
                .AddQueryParameter("include_fixed_destinations", "false");

            if (clusterId.ToLower() == "explore_all:0" || clusterId.ToLower() == "explore_all%3A0")
            {
                if (!string.IsNullOrEmpty(maxId))
                {
                    instaUri = instaUri.AddQueryParameter("max_id", maxId);
                    instaUri = instaUri.AddQueryParameter("cluster_id", "explore_all%3A0");
                }
            }
            else
            {
                instaUri = instaUri.AddQueryParameter("cluster_id", clusterId);
                instaUri = instaUri.AddQueryParameter("max_id", maxId);
            }

            return instaUri;
        }

        public static Uri GetTopSearchUri(string rankToken,
                                          string querry = "",
                                          InstaDiscoverSearchType searchType = InstaDiscoverSearchType.Users,
                                          int timezoneOffset = 12600)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FbsearchTopsearchFaltParameter,
                              rankToken,
                              timezoneOffset,
                              querry,
                              searchType.ToString().ToLower()),
                out var instaUri))
            {
                throw new Exception("Cant create URI for suggested search");
            }

            return instaUri;
        }

        public static Uri GetTranslateBiographyUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.LanguageTranslate, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for translate bio");
            }

            return instaUri;
        }

        public static Uri GetTranslateCommentsUri(string commendIds)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.LanguageTranslateComment, commendIds),
                out var instaUri))
            {
                throw new Exception("Cant create URI for translate comments");
            }

            return instaUri;
        }

        public static Uri GetTwoFactorLoginUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.Accounts2FaLogin, out var instaUri))
            {
                throw new Exception("Cant create URI for user 2FA login");
            }

            return instaUri;
        }

        public static Uri GetUnArchiveMediaUri(string mediaId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.MediaUnarchive, mediaId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for unarchive an post");
            }

            return instaUri;
        }

        public static Uri GetUnBlockUserUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsUnblockUser, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media likers");
            }

            return instaUri;
        }

        public static Uri GetUnFavoriteForUserStoriesUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsUnfavoriteForStories, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for unfavorite user stories");
            }

            return instaUri;
        }

        public static Uri GetUnFavoriteUserUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.FriendshipsUnfavorite, userId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for unfavorite user");
            }

            return instaUri;
        }

        public static Uri GetUnFollowHashtagUri(string hashtag)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.TagUnfollow, hashtag),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for unfollow hashtag");
            }

            return instaUri;
        }

        public static Uri GetUnFollowUserUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsUnfollowUser, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for getting media likers");
            }

            return instaUri;
        }

        public static Uri GetUnHideMyStoryFromUserUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsUnblockFriendReel, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for unhide my story from from specific user");
            }

            return instaUri;
        }

        public static Uri GetUnLikeCommentUri(string commentId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.UnlikeComment, commentId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for unlike comment");
            }

            return instaUri;
        }

        public static Uri GetUnLikeMediaUri(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.UnlikeMedia, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for unlike media");
            }

            return instaUri;
        }

        public static Uri GetUnMuteDirectThreadUri(string threadId)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.DirectThreadUnmute, threadId),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for unmute direct thread");
            }

            return instaUri;
        }

        public static Uri GetUnMuteFriendStoryUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsUnmuteFriendReel, userId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for unmute friend story");
            }

            return instaUri;
        }

        public static Uri GetUnMuteUserMediaStoryUri(long userId)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FriendshipsUnmutePostStory, out var instaUri))
            {
                throw new Exception("Cant create URI for unmute user media or story");
            }

            return instaUri;
        }

        public static Uri GetUnSaveMediaUri(string mediaId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaUnsave, mediaId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for unsave media");
            }

            return instaUri;
        }

        public static Uri GetUnStarThreadUri(string threadId)
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.DirectUnstar, threadId),
                    out var instaUri))
            {
                throw new Exception("Cant create URI for unstar thread");
            }

            return instaUri;
        }

        public static Uri GetUpdateBusinessInfoUri()
        {
            if (
                !Uri.TryCreate(
                    BaseInstagramUri,
                    InstaApiConstants.AccountsUpdateBusinessInfo,
                    out var instaUri))
            {
                throw new Exception("Cant create URI for update business info");
            }

            return instaUri;
        }

        public static Uri GetUploadPhotoUri()
        {
            if (
                !Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UploadPhotoOld, out var instaUri))
            {
                throw new Exception("Cant create URI for upload photo");
            }

            return instaUri;
        }

        public static Uri GetUploadVideoUri()
        {
            if (
                !Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UploadVideoOld, out var instaUri))
            {
                throw new Exception("Cant create URI for upload video");
            }

            return instaUri;
        }

        public static Uri GetUriSetAccountPrivate()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.SetAccountPrivate, out var instaUri))
            {
                throw new Exception("Cant create URI for set account private");
            }

            return instaUri;
        }

        public static Uri GetUriSetAccountPublic()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.SetAccountPublic, out var instaUri))
            {
                throw new Exception("Cant create URI for set account public");
            }

            return instaUri;
        }

        public static Uri GetUserFeedUri(string maxId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.Timelinefeed, out var instaUri))
            {
                throw new Exception("Cant create timeline feed URI");
            }

            return !string.IsNullOrEmpty(maxId)
                ? new UriBuilder(instaUri) { Query = $"max_id={maxId}" }.Uri
                : instaUri;
        }

        public static Uri GetUserFollowersUri(long userPk,
                                              string rankToken,
                                              string searchQuery,
                                              bool mutualsfirst = false,
                                              string maxId = "")
        {
            Uri instaUri = null;
            if (!mutualsfirst)
            {
                if (!Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.FriendshipsUserFollowers, userPk, rankToken),
                    out instaUri))
                {
                    throw new Exception("Cant create URI for user followers");
                }
            }
            else
            {
                if (!Uri.TryCreate(
                    BaseInstagramUri,
                    string.Format(InstaApiConstants.FriendshipsUserFollowersMutualfirst, userPk, rankToken, "1"),
                    out instaUri))
                {
                    throw new Exception("Cant create URI for user followers");
                }
            }

            return instaUri
                .AddQueryParameterIfNotEmpty("max_id", maxId)
                .AddQueryParameterIfNotEmpty("query", searchQuery);
        }

        public static Uri GetUserFollowingUri(long userPk, string rankToken, string searchQuery, string maxId = "")
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.FriendshipsUserFollowing, userPk, rankToken),
                out var instaUri))
            {
                throw new Exception("Cant create URI for user following");
            }

            return instaUri
                .AddQueryParameterIfNotEmpty("max_id", maxId)
                .AddQueryParameterIfNotEmpty("query", searchQuery);
        }

        public static Uri GetUserFriendshipUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Concat(InstaApiConstants.Friendshipstatus, userId, "/"),
                out var instaUri))
            {
                throw new Exception("Can't create URI for getting friendship status");
            }

            return instaUri;
        }

        public static Uri GetUserInfoByIdUri(long pk)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.GetUserInfoById, pk),
                               out var instaUri))
            {
                throw new Exception("Cant create user info by identifier URI");
            }

            return instaUri;
        }

        public static Uri GetUserInfoByUsernameUri(string username)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.GetUserInfoByUsername, username),
                               out var instaUri))
            {
                throw new Exception("Cant create user info by username URI");
            }

            return instaUri;
        }

        public static Uri GetUserLikeFeedUri(string maxId = null)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.LikeFeed, out var instaUri))
            {
                throw new Exception("Can't create URI for getting like feed");
            }

            var query = string.Empty;
            if (!string.IsNullOrEmpty(maxId))
            {
                query += $"max_id={maxId}";
            }

            var uriBuilder = new UriBuilder(instaUri) { Query = query };
            return uriBuilder.Uri;
        }

        public static Uri GetUserMediaListUri(long userPk, string nextId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.Userefeed + userPk, out var instaUri))
            {
                throw new Exception("Cant create URI for user media retrieval");
            }

            return !string.IsNullOrEmpty(nextId)
                ? new UriBuilder(instaUri) { Query = $"max_id={nextId}" }.Uri
                : instaUri;
        }

        public static Uri GetUsernameSuggestionsUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsUsernameSuggestions, out var instaUri))
            {
                throw new Exception("Cant create URI for username suggestions");
            }

            return instaUri;
        }

        public static Uri GetUserReelFeedUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.UserReelFeed, userId),
                out var instaUri))
            {
                throw new Exception("Can't create URI for getting user reel feed");
            }

            return instaUri;
        }

        public static Uri GetUserSearchByLocationUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.FbsearchTopsearchFalt, out var instaUri))
            {
                throw new Exception("Cant create URI for user search by location");
            }

            return instaUri;
        }

        public static Uri GetUserShoppableMediaListUri(long userPk, string nextId = "")
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               string.Format(InstaApiConstants.UserShoppableMedia, userPk),
                               out var instaUri))
            {
                throw new Exception("Cant create URI for user shoppable media");
            }

            return !string.IsNullOrEmpty(nextId)
                ? new UriBuilder(instaUri) { Query = $"max_id={nextId}" }.Uri
                : instaUri;
        }

        public static Uri GetUsersLookupUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersLookup, out var instaUri))
            {
                throw new Exception("Cant create URI for user lookup");
            }

            return instaUri;
        }

        public static Uri GetUsersNametagConfigUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersNametagConfig, out var instaUri))
            {
                throw new Exception("Cant create URI for users nametag config");
            }

            return instaUri;
        }

        public static Uri GetUsersNametagLookupUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.UsersNametagLookup, out var instaUri))
            {
                throw new Exception("Cant create URI for users nametag lookup");
            }

            return instaUri;
        }

        public static Uri GetUserStoryUri(long userId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.GetUserStory, userId),
                out var instaUri))
            {
                throw new Exception("Can't create URI for getting user's story");
            }

            return instaUri;
        }

        public static Uri GetUserTagsUri(long userPk, string rankToken, string maxId = null)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.GetUserTags, userPk),
                out var instaUri))
            {
                throw new Exception("Cant create URI for get user tags");
            }

            var query = $"rank_token={rankToken}&ranked_content=true";
            if (!string.IsNullOrEmpty(maxId))
            {
                query += $"&max_id={maxId}";
            }

            var uriBuilder = new UriBuilder(instaUri) { Query = query };
            return uriBuilder.Uri;
        }

        public static Uri GetUserUri(string username)
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.SearchUsers, out var instaUri))
            {
                throw new Exception("Cant create search user URI");
            }

            var userUriBuilder = new UriBuilder(instaUri) { Query = $"q={username}" };
            return userUriBuilder.Uri;
        }

        public static Uri GetValidateReelLinkAddressUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.MediaValidateReelUrl, out var instaUri))
            {
                throw new Exception("Cant create URI for request for validate reel url");
            }

            return instaUri;
        }

        public static Uri GetValidateSignUpSmsCodeUri()
        {
            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.AccountsValidateSignupSmsCode, out var instaUri))
            {
                throw new Exception("Cant create URI for validate signup sms code");
            }

            return instaUri;
        }

        public static Uri GetVerifyEmailUri(Uri uri)
        {
            var u = uri.ToString();
            if (u.Contains("?"))
            {
                u = u.Substring(0, u.IndexOf("?"));
            }

            u = u.Substring(u.IndexOf("/accounts/"));

            if (!Uri.TryCreate(BaseInstagramUri, InstaApiConstants.ApiSuffix + u, out var instaUri))
            {
                throw new Exception("Cant create URI for verify email");
            }

            return instaUri;
        }

        public static Uri GetVideoStoryConfigureUri(bool isVideo = false)
        {
            if (!Uri.TryCreate(BaseInstagramUri,
                               isVideo
                                   ? InstaApiConstants.StoryConfigureVideo
                                   : InstaApiConstants.StoryConfigureVideo2,
                               out var instaUri))
            {
                throw new Exception("Can't create URI for configuring story media");
            }

            return instaUri;
        }

        public static Uri GetVoteStorySliderUri(string storyMediaId, string pollId)
        {
            if (!Uri.TryCreate(
                BaseInstagramUri,
                string.Format(InstaApiConstants.MediaStorySliderVote, storyMediaId, pollId),
                out var instaUri))
            {
                throw new Exception("Cant create URI for vote story slider");
            }

            return instaUri;
        }
    }
}