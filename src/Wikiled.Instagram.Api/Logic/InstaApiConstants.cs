using System;
using Newtonsoft.Json.Linq;

namespace Wikiled.Instagram.Api.Logic
{
    /// <summary>
    ///     Place of every endpoints, headers and other constants and variables.
    /// </summary>
    internal static class InstaApiConstants
    {
        public const string AcceptEncoding = "gzip, deflate, sdch";

        public const string Api = "/api";

        public const string ApiSuffix = Api + ApiVersion;

        public const string ApiSuffixV2 = Api + ApiVersionV2;

        public const string ApiVersion = "/v1";

        public const string ApiVersionV2 = "/v2";

        public const string BaseInstagramApiUrl = InstagramUrl + ApiSuffix + "/";

        public const string CommentBreadcrumbKey = "iN4$aGr0m";

        public const string Csrftoken = "csrftoken";

        public const string HeaderAcceptEncoding = "gzip, deflate, sdch";

        public const string HeaderAcceptLanguage = "Accept-Language";

        public const string HeaderCount = "count";

        public const string HeaderExcludeList = "exclude_list";

        public const string HeaderIgAppId = "X-IG-App-ID";

        public const string HeaderIgCapabilities = "X-IG-Capabilities";

        public const string HeaderIgConnectionType = "X-IG-Connection-Type";

        public const string HeaderIgSignature = "signed_body";

        public const string HeaderIgSignatureKeyVersion = "ig_sig_key_version";

        public const string HeaderMaxId = "max_id";

        public const string HeaderPhoneId = "phone_id";

        public const string HeaderQuery = "q";

        public const string HeaderRankToken = "rank_token";

        public const string HeaderTimezone = "timezone_offset";

        public const string HeaderUserAgent = "User-Agent";

        public const string HeaderXInstagramAjax = "X-Instagram-AJAX";

        public const string HeaderXRequestedWith = "X-Requested-With";

        public const string HeaderXcsrfToken = "X-CSRFToken";

        public const string HeaderXgoogleAdIde = "X-Google-AD-ID";

        public const string HeaderXmlHttpRequest = "XMLHttpRequest";

        public const string IgAppId = "567067343352427";

        public const string IgConnectionType = "WIFI";

        public const string IgSignatureKeyVersion = "4";

        public const string InstagramUrl = "https://i.instagram.com";

        public const string PSuffix = "p/";

        public const string SupportedCapabalitiesHeader = "supported_capabilities_new";

        public const string UserAgent =
            "Instagram {6} Android ({7}/{8}; {0}; {1}; {2}; {3}; {4}; {5}; en_US; {9})";

        public const string UserAgentDefault =
            "Instagram 44.0.0.9.93 Android (24/7.0; 640dpi; 1440x2560; samsung; SM-G935F; hero2lte; samsungexynos8890; en_US; 107092322)";

        public const string FacebookLoginUri =
            "https://m.facebook.com/v2.3/dialog/oauth?access_token=&client_id=124024574287414&e2e={0}&scope=email&default_audience=friends&redirect_uri=fbconnect://success&display=touch&response_type=token,signed_request&return_scopes=true";

        public const string FacebookToken =
            "https://graph.facebook.com/me?access_token={0}&fields=id,is_employee,name";

        public const string FacebookTokenPicture = "https://graph.facebook.com/me?access_token={0}&fields=picture";

        public const string FacebookUserAgent =
            "Mozilla/5.0 (Linux; Android {0}; {1} Build/{2}; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/69.0.3497.100 Mobile Safari/537.36";

        public const string FacebookUserAgentDefault =
            "Mozilla/5.0 (Linux; Android 7.0; PRA-LA1 Build/HONORPRA-LA1; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/69.0.3497.100 Mobile Safari/537.36";

        public const string WebUserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 OPR/57.0.3098.116";

        public const string ErrorOccurred = "Oops, an error occurred";

        public const string Accounts2FaLogin = ApiSuffix + "/accounts/two_factor_login/";

        public const string Accounts2FaLoginAgain = ApiSuffix + "/accounts/send_two_factor_login_sms/";

        public const string AccountsChangeProfilePicture = ApiSuffix + "/accounts/change_profile_picture/";

        public const string AccountsCheckPhoneNumber = ApiSuffix + "/accounts/check_phone_number/";

        public const string AccountsContactPointPrefill = ApiSuffix + "/accounts/contact_point_prefill/";

        public const string AccountsCreate = ApiSuffix + "/accounts/create/";

        public const string AccountsCreateValidated = ApiSuffix + "/accounts/create_validated/";

        public const string AccountsDisableSmsTwoFactor = ApiSuffix + "/accounts/disable_sms_two_factor/";

        public const string AccountsEditProfile = ApiSuffix + "/accounts/edit_profile/";

        public const string AccountsEnableSmsTwoFactor = ApiSuffix + "/accounts/enable_sms_two_factor/";

        public const string AccountsGetCommentFilter = ApiSuffix + "/accounts/get_comment_filter/";

        public const string AccountsLogin = ApiSuffix + "/accounts/login/";

        public const string AccountsLogout = ApiSuffix + "/accounts/logout/";

        public const string AccountsReadMsisdnHeader = ApiSuffix + "/accounts/read_msisdn_header/";

        public const string AccountsRegenBackupCodes = ApiSuffix + "/accounts/regen_backup_codes/";

        public const string AccountsRemoveProfilePicture = ApiSuffix + "/accounts/remove_profile_picture/";

        public const string AccountsRequestProfileEdit = ApiSuffix + "/accounts/current_user/?edit=true";

        public const string AccountsSecurityInfo = ApiSuffix + "/accounts/account_security_info/";

        public const string AccountsSendConfirmEmail = ApiSuffix + "/accounts/send_confirm_email/";

        public const string AccountsSendRecoveryEmail = ApiSuffix + "/accounts/send_recovery_flow_email/";

        public const string AccountsSendSignupSmsCode = ApiSuffix + "/accounts/send_signup_sms_code/";

        public const string AccountsSendSmsCode = ApiSuffix + "/accounts/send_sms_code/";

        public const string AccountsSendTwoFactorEnableSms = ApiSuffix + "/accounts/send_two_factor_enable_sms/";

        public const string AccountsSetBiography = ApiSuffix + "/accounts/set_biography/";

        public const string AccountsSetPhoneAndName = ApiSuffix + "/accounts/set_phone_and_name/";

        public const string AccountsSetPresenceDisabled = ApiSuffix + "/accounts/set_presence_disabled/";

        public const string AccountsUpdateBusinessInfo = ApiSuffix + "/accounts/update_business_info/";

        public const string AccountsUsernameSuggestions = ApiSuffix + "/accounts/username_suggestions/";

        public const string AccountsValidateSignupSmsCode = ApiSuffix + "/accounts/validate_signup_sms_code/";

        public const string AccountsVerifySmsCode = ApiSuffix + "/accounts/verify_sms_code/";

        public const string ChangePassword = ApiSuffix + "/accounts/change_password/";

        public const string Currentuser = ApiSuffix + "/accounts/current_user?edit=true";

        public const string SetAccountPrivate = ApiSuffix + "/accounts/set_private/";

        public const string SetAccountPublic = ApiSuffix + "/accounts/set_public/";

        public const string AccountsConvertToPersonal = ApiSuffix + "/accounts/convert_to_personal/";

        public const string AccountsCreateBusinessInfo = ApiSuffix + "/accounts/create_business_info/";

        public const string AccountsGetPresence = ApiSuffix + "/accounts/get_presence_disabled/";

        public const string AccountsGetBlockedCommenters = ApiSuffix + "/accounts/get_blocked_commenters/";

        public const string AccountsSetBlockedCommenters = ApiSuffix + "/accounts/set_blocked_commenters/";

        /// <summary>
        ///     /api/v1/business/instant_experience/get_ix_partners_bundle/?signed_body=b941ff07b83716087710019790b3529ab123c8deabfb216e056651e9cf4b4ca7.{}
        ///     &ig_sig_key_version=4
        /// </summary>
        public const string BusinessInstantExperience = ApiSuffix +
            "/business/instant_experience/get_ix_partners_bundle/?signed_body={0}&ig_sig_key_version={1}";

        public const string BusinessSetCategory = ApiSuffix + "/business/account/set_business_category/";

        public const string BusinessValidateUrl = ApiSuffix + "/business/instant_experience/ix_validate_url/";

        public const string BusinessBrandedGetSettings =
            ApiSuffix + "/business/branded_content/get_whitelist_settings/";

        public const string BusinessBrandedUserSearch =
            ApiSuffix + "/users/search/?q={0}&count={1}&branded_content_creator_only=true";

        public const string BusinessBrandedUpdateSettings =
            ApiSuffix + "/business/branded_content/update_whitelist_settings/";

        public const string BusinessConvertToBusinessAccount =
            ApiSuffix + "/business_conversion/get_business_convert_social_context/";

        public const string CollectionCreateModule = ApiSuffix + "collection_create";

        public const string CreateCollection = ApiSuffix + "/collections/create/";

        public const string DeleteCollection = ApiSuffix + "/collections/{0}/delete/";

        public const string EditCollection = ApiSuffix + "/collections/{0}/edit/";

        public const string FeedSavedAddToCollectionModule = "feed_saved_add_to_collection";

        public const string GetListCollections = ApiSuffix + "/collections/list/";

        public const string ConsentNewUserFlow = ApiSuffix + "/consent/new_user_flow/";

        public const string ConsentNewUserFlowBegins = ApiSuffix + "/consent/new_user_flow_begins/";

        public const string DirectBroadcastConfigureVideo =
            ApiSuffix + "/direct_v2/threads/broadcast/configure_video/";

        public const string DirectBroadcastLink = ApiSuffix + "/direct_v2/threads/broadcast/link/";

        public const string DirectBroadcastThreadLike = ApiSuffix + "/direct_v2/threads/broadcast/like/";

        public const string DirectBroadcastLocation = ApiSuffix + "/direct_v2/threads/broadcast/location/";

        public const string DirectBroadcastMediaShare =
            ApiSuffix + "/direct_v2/threads/broadcast/media_share/?media_type={0}";

        public const string DirectBroadcastProfile = ApiSuffix + "/direct_v2/threads/broadcast/profile/";

        public const string DirectBroadcastReaction = ApiSuffix + "/direct_v2/threads/broadcast/reaction/";

        public const string DirectBroadcastReelShare = ApiSuffix + "/direct_v2/threads/broadcast/reel_share/";

        public const string DirectBroadcastUploadPhoto = ApiSuffix + "/direct_v2/threads/broadcast/upload_photo/";

        public const string DirectBroadcastHashtag = ApiSuffix + "/direct_v2/threads/broadcast/hashtag/";

        public const string DirectBroadcastLiveViewerInvite =
            ApiSuffix + "/direct_v2/threads/broadcast/live_viewer_invite/";

        /// <summary>
        ///     post data:
        ///     <para>use_unified_inbox=true</para>
        ///     <para>recipient_users= user ids split with comma.: ["user id1","user id2","..."]</para>
        /// </summary>
        public const string DirectCreateGroup = ApiSuffix + "/direct_v2/create_group_thread/";

        public const string DirectPresence = ApiSuffix + "/direct_v2/get_presence/";

        public const string DirectShare = ApiSuffix + "/direct_share/inbox/";

        public const string DirectStar = ApiSuffix + "/direct_v2/threads/{0}/label/";

        public const string DirectThreadHide = ApiSuffix + "/direct_v2/threads/{0}/hide/";

        public const string DirectThreadAddUser = ApiSuffix + "/direct_v2/threads/{0}/add_user/";

        /// <summary>
        ///     deprecated
        /// </summary>
        public const string DirectThreadItemSeen = ApiSuffix + "/direct_v2/visual_threads/{0}/item_seen/";

        public const string DirectThreadSeen = ApiSuffix + "/direct_v2/threads/{0}/items/{1}/seen/";

        public const string DirectThreadLeave = ApiSuffix + "/direct_v2/threads/{0}/leave/";

        public const string DirectThreadMute = ApiSuffix + "/direct_v2/threads/{0}/mute/";

        public const string DirectThreadUnmute = ApiSuffix + "/direct_v2/threads/{0}/unmute/";

        public const string DirectThreadUpdateTitle = ApiSuffix + "/direct_v2/threads/{0}/update_title/";

        public const string DirectUnstar = ApiSuffix + "/direct_v2/threads/{0}/unlabel/";

        public const string GetDirectInbox = ApiSuffix + "/direct_v2/inbox/";

        public const string GetDirectPendingInbox = ApiSuffix + "/direct_v2/pending_inbox/";

        public const string GetDirectShareUser = ApiSuffix + "/direct_v2/threads/broadcast/profile/";

        public const string GetDirectTextBroadcast = ApiSuffix + "/direct_v2/threads/broadcast/text/";

        public const string GetDirectThread = ApiSuffix + "/direct_v2/threads/{0}";

        public const string GetDirectThreadApprove = ApiSuffix + "/direct_v2/threads/{0}/approve/";

        public const string GetDirectThreadApproveMultiple = ApiSuffix + "/direct_v2/threads/approve_multiple/";

        public const string GetDirectThreadDecline = ApiSuffix + "/direct_v2/threads/{0}/decline/";

        public const string GetDirectThreadDeclineMultiple = ApiSuffix + "/direct_v2/threads/decline_multiple/";

        public const string GetDirectThreadDeclineall = ApiSuffix + "/direct_v2/threads/decline_all/";

        public const string GetParticipantsRecipientUser =
            ApiSuffix + "/direct_v2/threads/get_by_participants/?recipient_users=[{0}]";

        public const string GetRankRecipientsByUsername = ApiSuffix +
            "/direct_v2/ranked_recipients/?mode=raven&show_threads=true&query={0}&use_unified_inbox=true";

        public const string GetRankedRecipients = ApiSuffix + "/direct_v2/ranked_recipients";

        public const string GetRecentRecipients = ApiSuffix + "/direct_share/recent_recipients/";

        public const string StoryShare = ApiSuffix + "/direct_v2/threads/broadcast/story_share/?media_type={0}";

        public const string DirectThreadDeleteMessage = ApiSuffix + "/direct_v2/threads/{0}/items/{1}/delete/";

        public const string DiscoverAyml = ApiSuffix + "/discover/ayml/";

        public const string DiscoverChaining = ApiSuffix + "/discover/chaining/?target_id={0}";

        public const string DiscoverExplore = ApiSuffix + "/discover/explore/";

        public const string DiscoverTopicalExplore = ApiSuffix + "/discover/topical_explore/";

        public const string DiscoverFetchSuggestionDetails =
            ApiSuffix + "/discover/fetch_suggestion_details/?target_id={0}&chained_ids={1}";

        public const string DiscoverTopLive = ApiSuffix + "/discover/top_live/";

        public const string DiscoverTopLiveStatus = ApiSuffix + "/discover/top_live_status/";

        public const string FbsearchClearSearchHistory = ApiSuffix + "/fbsearch/clear_search_history";

        public const string FbsearchGetHiddenSearchEntities = ApiSuffix + "/fbsearch/get_hidden_search_entities/";

        /// <summary>
        ///     post data:
        ///     <para>section=suggested</para>
        ///     <para>user=["1 user id"]</para>
        /// </summary>
        public const string FbsearchHideSearchEntities = ApiSuffix + "/fbsearch/hide_search_entities/";

        /// <summary>
        ///     get nearby places
        /// </summary>
        public const string FbsearchPlaces = ApiSuffix + "/fbsearch/places/";

        public const string FbsearchProfileSearch = ApiSuffix + "/fbsearch/profile_link_search/?q={0}&count={1}";

        public const string FbsearchRecentSearches = ApiSuffix + "/fbsearch/recent_searches/";

        public const string FbsearchSuggestedSearchs = ApiSuffix + "/fbsearch/suggested_searches/?type={0}";

        public const string FbsearchTopsearch = ApiSuffix + "/fbsearch/topsearch/";

        public const string FbsearchTopsearchFalt = ApiSuffix + "/fbsearch/topsearch_flat/";

        public const string FbsearchTopsearchFaltParameter = ApiSuffix +
            "/fbsearch/topsearch_flat/?rank_token={0}&timezone_offset={1}&query={2}&context={3}";

        public const string FbEntrypointInfo = ApiSuffix + "/fb/fb_entrypoint_info/";

        public const string FbFacebookSignup = ApiSuffix + "/fb/facebook_signup/";

        public const string FbGetInviteSuggestions = ApiSuffix + "/fb/get_invite_suggestions/";

        public const string FeedOnlyMeFeed = ApiSuffix + "/feed/only_me_feed/";

        /// <summary>
        ///     {0} = rank token <<<<< this endpoint is deprecated
        /// </summary>
        public const string FeedPopular =
            ApiVersion + "/feed/popular/?people_teaser_supported=1&rank_token={0}&ranked_content=true";

        public const string FeedPromotableMedia = ApiSuffix + "/feed/promotable_media/";

        public const string FeedReelMedia = ApiSuffix + "/feed/reels_media/";

        public const string FeedSaved = ApiSuffix + "/feed/saved/";

        public const string GetCollection = ApiSuffix + "/feed/collection/{0}/";

        public const string GetStoryTray = ApiSuffix + "/feed/reels_tray/";

        public const string GetTagFeed = ApiSuffix + "/feed/tag/{0}";

        public const string GetUserStory = ApiSuffix + "/feed/user/{0}/reel_media/";

        public const string GetUserTags = ApiSuffix + "/usertags/{0}/feed/";

        public const string LikeFeed = ApiSuffix + "/feed/liked/";

        public const string Timelinefeed = ApiSuffix + "/feed/timeline";

        public const string UserReelFeed = ApiSuffix + "/feed/user/{0}/reel_media/";

        public const string Userefeed = ApiSuffix + "/feed/user/";

        public const string FriendshipsApprove = ApiSuffix + "/friendships/approve/{0}/";

        public const string FriendshipsAutocompleteUserList = ApiSuffix + "/friendships/autocomplete_user_list/";

        public const string FriendshipsBlockUser = ApiSuffix + "/friendships/block/{0}/";

        public const string FriendshipsFollowUser = ApiSuffix + "/friendships/create/{0}/";

        public const string FriendshipsIgnore = ApiSuffix + "/friendships/ignore/{0}/";

        public const string FriendshipsPendingRequests =
            ApiSuffix + "/friendships/pending/?rank_mutual=0&rank_token={0}";

        public const string FriendshipsRemoveFollower = ApiSuffix + "/friendships/remove_follower/{0}/";

        /// <summary>
        ///     hide your stories from specific users
        /// </summary>
        public const string FriendshipsSetReelBlockStatus = ApiSuffix + "/friendships/set_reel_block_status/";

        public const string FriendshipsShowMany = ApiSuffix + "/friendships/show_many/";

        public const string FriendshipsUnblockUser = ApiSuffix + "/friendships/unblock/{0}/";

        public const string FriendshipsFavorite = ApiSuffix + "/friendships/favorite/{0}/";

        public const string FriendshipsUnfavorite = ApiSuffix + "/friendships/unfavorite/{0}/";

        public const string FriendshipsFavoriteForStories = ApiSuffix + "/friendships/favorite_for_stories/{0}/";

        public const string FriendshipsUnfavoriteForStories =
            ApiSuffix + "/friendships/unfavorite_for_stories/{0}/";

        public const string FriendshipsUnfollowUser = ApiSuffix + "/friendships/destroy/{0}/";

        public const string FriendshipsUserFollowers = ApiSuffix + "/friendships/{0}/followers/?rank_token={1}";

        public const string FriendshipsUserFollowersMutualfirst =
            ApiSuffix + "/friendships/{0}/followers/?rank_token={1}&rank_mutual={2}";

        public const string FriendshipsUserFollowing = ApiSuffix + "/friendships/{0}/following/?rank_token={1}";

        public const string Friendshipstatus = ApiSuffix + "/friendships/show/";

        public const string FriendshipsMarkUserOverage = ApiSuffix + "/friendships/mark_user_overage/{0}/feed/";

        public const string FriendshipsMutePostStory = ApiSuffix + "/friendships/mute_posts_or_story_from_follow/";

        public const string FriendshipsUnmutePostStory =
            ApiSuffix + "/friendships/unmute_posts_or_story_from_follow/";

        public const string FriendshipsBlockFriendReel = ApiSuffix + "/friendships/block_friend_reel/{0}/";

        public const string FriendshipsUnblockFriendReel = ApiSuffix + "/friendships/unblock_friend_reel/{0}/";

        public const string FriendshipsMuteFriendReel = ApiSuffix + "/friendships/mute_friend_reel/{0}/";

        public const string FriendshipsUnmuteFriendReel = ApiSuffix + "/friendships/unmute_friend_reel/{0}/";

        public const string FriendshipsBlockedReel = ApiSuffix + "/friendships/blocked_reels/";

        public const string FriendshipsBesties = ApiSuffix + "/friendships/besties/";

        public const string FriendshipsBestiesSuggestions = ApiSuffix + "/friendships/bestie_suggestions/";

        public const string FriendshipsSetBesties = ApiSuffix + "/friendships/set_besties/";

        public const string GraphQl = ApiSuffix + "/ads/graphql/";

        public const string GraphQlStatistics = GraphQl + "?locale={0}&vc_policy=insights_policy&surface={1}";

        public const string InsightsMedia =
            ApiSuffix + "/insights/account_organic_insights/?show_promotions_in_landing_page=true&first={0}";

        public const string InsightsMediaSingle = ApiSuffix + "/insights/media_organic_insights/{0}?{1}={2}";

        public const string HighlightCreateReel = ApiSuffix + "/highlights/create_reel/";

        public const string HighlightDeleteReel = ApiSuffix + "/highlights/{0}/delete_reel/";

        public const string HighlightEditReel = ApiSuffix + "/highlights/{0}/edit_reel/";

        public const string HighlightTray = ApiSuffix + "/highlights/{0}/highlights_tray/";

        public const string IgtvChannel = ApiSuffix + "/igtv/channel/";

        public const string IgtvSearch = ApiSuffix + "/igtv/search/?query={0}";

        public const string IgtvSuggestedSearches = ApiSuffix + "/igtv/suggested_searches/";

        public const string IgtvTvGuide = ApiSuffix + "/igtv/tv_guide/";

        public const string MediaConfigureToIgtv = ApiSuffix + "/media/configure_to_igtv/";

        public const string LanguageTranslate = ApiSuffix + "/language/translate/?id={0}&type=3";

        public const string LanguageTranslateComment = ApiSuffix + "/language/bulk_translate/?comment_ids={0}";

        public const string LiveAddToPostLive = ApiSuffix + "/live/{0}/add_to_post_live/";

        public const string LiveComment = ApiSuffix + "/live/{0}/comment/";

        public const string LiveCreate = ApiSuffix + "/live/create/";

        public const string LiveDeletePostLive = ApiSuffix + "/live/{0}/delete_post_live/";

        public const string LiveEnd = ApiSuffix + "/live/{0}/end_broadcast/";

        public const string LiveGetComment = ApiSuffix + "/live/{0}/get_comment/";

        public const string LiveGetCommentLastcommentts = ApiSuffix + "/live/{0}/get_comment/?last_comment_ts={1}";

        public const string LiveGetFinalViewerList = ApiSuffix + "/live/{0}/get_final_viewer_list/";

        public const string LiveGetJoinRequests = ApiSuffix + "/live/{0}/get_join_requests/";

        public const string LiveGetLikeCount = ApiSuffix + "/live/{0}/get_like_count/";

        public const string LiveGetLivePresence = ApiSuffix + "/live/get_live_presence/?presence_type=30min";

        public const string LiveGetPostLiveComment =
            ApiSuffix + "/live/{0}/get_post_live_comments/?starting_offset={1}&encoding_tag={2}";

        public const string LiveGetPostLiveViewersList = ApiSuffix + "/live/{0}/get_post_live_viewers_list/";

        public const string LiveGetSuggestedBroadcasts = ApiSuffix + "/live/get_suggested_broadcasts/";

        public const string LiveGetViewerList = ApiSuffix + "/live/{0}/get_viewer_list/";

        public const string LiveHeartbeatAndGetViewerCount =
            ApiSuffix + "/live/{0}/heartbeat_and_get_viewer_count/";

        public const string LiveInfo = ApiSuffix + "/live/{0}/info/";

        public const string LiveLike = ApiSuffix + "/live/{0}/like/";

        public const string LiveMuteComments = ApiSuffix + "/live/{0}/mute_comment/";

        public const string LivePinComment = ApiSuffix + "/live/{0}/pin_comment/";

        public const string LivePostLiveLikes =
            ApiSuffix + "/live/{0}/get_post_live_likes/?starting_offset={1}&encoding_tag={2}";

        public const string LiveStart = ApiSuffix + "/live/{0}/start/";

        public const string LiveUnmuteComments = ApiSuffix + "/live/{0}/unmute_comment/";

        public const string LiveUnpinComment = ApiSuffix + "/live/{0}/unpin_comment/";

        /// <summary>
        ///     It seems deprecated and can't get feeds, only stories will recieve
        /// </summary>
        public const string LocationFeed = ApiSuffix + "/feed/location/{0}/";

        public const string LocationSection = ApiSuffix + "/locations/{0}/sections/";

        public const string LocationSearch = ApiSuffix + "/location_search/";

        public const string LocationsInfo = ApiSuffix + "/locations/{0}/info/";

        /// <summary>
        ///     {0} => external id, NOT WORKING
        /// </summary>
        public const string LocationsReleated = ApiSuffix + "/locations/{0}/related/";

        public const string AllowMediaComments = ApiSuffix + "/media/{0}/enable_comments/";

        public const string DeleteComment = ApiSuffix + "/media/{0}/comment/{1}/delete/";

        public const string DeleteMedia = ApiSuffix + "/media/{0}/delete/?media_type={1}";

        public const string DeleteMultipleComment = ApiSuffix + "/media/{0}/comment/bulk_delete/";

        public const string DisableMediaComments = ApiSuffix + "/media/{0}/disable_comments/";

        public const string EditMedia = ApiSuffix + "/media/{0}/edit_media/";

        public const string GetMedia = ApiSuffix + "/media/{0}/info/";

        public const string GetShareLink = ApiSuffix + "/media/{0}/permalink/";

        public const string LikeComment = ApiSuffix + "/media/{0}/comment_like/";

        public const string LikeMedia = ApiSuffix + "/media/{0}/like/";

        public const string MaxMediaIdPostfix = "/media/?max_id=";

        public const string Media = "/media/";

        public const string MediaAlbumConfigure = ApiSuffix + "/media/configure_sidecar/";

        public const string MediaCommentLikers = ApiSuffix + "/media/{0}/comment_likers/";

        public const string MediaComments = ApiSuffix + "/media/{0}/comments/?can_support_threading=true";

        public const string MediaConfigure = ApiSuffix + "/media/configure/";

        public const string MediaConfigureVideo = ApiSuffix + "/media/configure/?video=1";

        public const string MediaUploadFinish = ApiSuffix + "/media/upload_finish/?video=1";

        public const string MediaInfos = ApiSuffix +
            "/media/infos/?_uuid={0}&_csrftoken={1}&media_ids={2}&ranked_content=true&include_inactive_reel=true";

        public const string MediaConfigureNametag = ApiSuffix + "/media/configure_to_nametag/";

        public const string MediaInlineComments = ApiSuffix + "/media/{0}/comments/{1}/inline_child_comments/";

        public const string MediaLikers = ApiSuffix + "/media/{0}/likers/";

        public const string MediaReport = ApiSuffix + "/media/{0}/flag_media/";

        public const string MediaReportComment = ApiSuffix + "/media/{0}/comment/{1}/flag/";

        public const string MediaSave = ApiSuffix + "/media/{0}/save/";

        public const string MediaUnsave = ApiSuffix + "/media/{0}/unsave/";

        public const string MediaValidateReelUrl = ApiSuffix + "/media/validate_reel_url/";

        public const string PostComment = ApiSuffix + "/media/{0}/comment/";

        public const string SeenMedia = ApiSuffix + "/media/seen/";

        public const string SeenMediaStory = ApiSuffixV2 + "/media/seen/?reel=1&live_vod=0";

        public const string StoryConfigure = ApiSuffix + "/media/configure_to_reel/";

        public const string StoryConfigureVideo = ApiSuffix + "/media/configure_to_story/?video=1";

        public const string StoryConfigureVideo2 = ApiSuffix + "/media/configure_to_story/";

        public const string StoryMediaInfoUpload = ApiSuffix + "/media/mas_opt_in_info/";

        public const string UnlikeComment = ApiSuffix + "/media/{0}/comment_unlike/";

        public const string UnlikeMedia = ApiSuffix + "/media/{0}/unlike/";

        public const string MediaStoryViewers = ApiSuffix + "/media/{0}/list_reel_media_viewer/";

        public const string MediaBlocked = ApiSuffix + "/media/blocked/";

        public const string MediaArchive = ApiSuffix + "/media/{0}/only_me/";

        public const string MediaUnarchive = ApiSuffix + "/media/{0}/undo_only_me/";

        public const string MediaStoryPollVoters = ApiSuffix + "/media/{0}/{1}/story_poll_voters/";

        public const string MediaStoryPollVote = ApiSuffix + "/media/{0}/{1}/story_poll_vote/";

        public const string MediaStorySliderVote = ApiSuffix + "/media/{0}/{1}/story_slider_vote/";

        public const string MediaStoryQuestionResponse = ApiSuffix + "/media/{0}/{1}/story_question_response/";

        public const string MediaStoryCountdowns = ApiSuffix + "/media/story_countdowns/";

        public const string MediaFollowCountdown = ApiSuffix + "/media/{0}/follow_story_countdown/";

        public const string MediaUnfollowCountdown = ApiSuffix + "/media/{0}/unfollow_story_countdown/";

        public const string GetFollowingRecentActivity = ApiSuffix + "/news/";

        public const string GetRecentActivity = ApiSuffix + "/news/inbox/";

        /// <summary>
        ///     post params:
        ///     <para>"action":"click"</para>
        /// </summary>
        public const string NewsLog = ApiSuffix + "/news/log/";

        public const string NotificationBadge = ApiSuffix + "/notifications/badge/";

        public const string PushRegister = ApiSuffix + "/push/register/";

        public const string UserShoppableMedia = ApiSuffix + "/feed/user/{0}/shoppable_media/";

        public const string CommerceProductInfo =
            ApiSuffix + "/commerce/products/{0}/?media_id={1}&device_width={2}";

        public const string GetTagInfo = ApiSuffix + "/tags/{0}/info/";

        public const string SearchTags = ApiSuffix + "/tags/search/?q={0}&count={1}";

        public const string TagFollow = ApiSuffix + "/tags/follow/{0}/";

        public const string TagRanked = ApiSuffix + "/tags/{0}/ranked_sections/";

        public const string TagRecent = ApiSuffix + "/tags/{0}/recent_sections/";

        public const string TagSection = ApiSuffix + "/tags/{0}/sections/";

        /// <summary>
        ///     queries:
        ///     <para>visited = [{"id":"TAG ID","type":"hashtag"}]</para>
        ///     <para>related_types = ["location","hashtag"]</para>
        /// </summary>
        public const string TagRelated = ApiSuffix + "/tags/{0}/related/";

        public const string TagStory = ApiSuffix + "/tags/{0}/story/";

        public const string TagSuggested = ApiSuffix + "/tags/suggested/";

        public const string TagUnfollow = ApiSuffix + "/tags/unfollow/{0}/";

        public const string AccountsLookupPhone = ApiSuffix + "/users/lookup_phone/";

        public const string GetUserInfoById = ApiSuffix + "/users/{0}/info/";

        public const string GetUserInfoByUsername = ApiSuffix + "/users/{0}/usernameinfo/";

        public const string SearchUsers = ApiSuffix + "/users/search";

        public const string UsersCheckEmail = ApiSuffix + "/users/check_email/";

        public const string UsersCheckUsername = ApiSuffix + "/users/check_username/";

        public const string UsersLookup = ApiSuffix + "/users/lookup/";

        public const string UsersNametagConfig = ApiSuffix + "/users/nametag_config/";

        public const string UsersReelSettings = ApiSuffix + "/users/reel_settings/";

        public const string UsersReport = ApiSuffix + "/users/{0}/flag_user/";

        public const string UsersSearch = ApiSuffix + "/users/search/?timezone_offset={0}&q={1}&count={2}";

        public const string UsersSetReelSettings = ApiSuffix + "/users/set_reel_settings/";

        public const string UsersFollowingTagInfo = ApiSuffix + "/users/{0}/following_tags_info/";

        public const string UsersFullDetailInfo = ApiSuffix + "/users/{0}/full_detail_info/";

        public const string UsersNametagLookup = ApiSuffix + "/users/nametag_lookup/";

        public const string UsersBlockedList = ApiSuffix + "/users/blocked_list/";

        public const string UsersAccountDetails = ApiSuffix + "/users/{0}/account_details/";

        public const string UploadPhoto = InstagramUrl + "/rupload_igphoto/{0}_0_{1}";

        public const string UploadPhotoOld = ApiSuffix + "/upload/photo/";

        public const string UploadVideo = InstagramUrl + "/rupload_igvideo/{0}_0_{1}";

        public const string UploadVideoOld = ApiSuffix + "/upload/video/";

        public const string AddressbookLink = ApiSuffix + "/address_book/link/?include=extra_display_name,thumbnails";

        public const string ArchiveReelDayShells = ApiSuffix + "/archive/reel/day_shells/?include_cover=0";

        public const string DyiRequestDownloadData = ApiSuffix + "/dyi/request_download_data/";

        public const string DyiCheckDataState = ApiSuffix + "/dyi/check_data_state/";

        public const string DynamicOnboardingGetSteps = ApiSuffix + "/dynamic_onboarding/get_steps/";

        public const string GetMediaid = ApiSuffix + "/oembed/?url={0}";

        public const string MegaphoneLog = ApiSuffix + "/megaphone/log/";

        public const string QeExpose = ApiSuffix + "/qe/expose/";

        public const string Challenge = ApiSuffix + "/challenge/";

        public static string Timezone = "Asia/Tehran";

        public static int TimezoneOffset = 16200;

        public static readonly JArray SupportedCapabalities = new JArray
        {
            new JObject
            {
                { "name", "SUPPORTED_SDK_VERSIONS" },
                {
                    "value", "13.0,14.0,15.0,16.0,17.0,18.0,19.0,20.0," +
                    "21.0,22.0,23.0,24.0,25.0,26.0,27.0,28.0,29.0,30.0,31.0,32.0,33.0," +
                    "34.0,35.0,36.0,37.0,38.0,39.0,40.0,41.0,42.0,43.0,44.0,45.0,46.0,47.0,48.0,49.0,50.0,51.0,52.0,53.0"
                }
            },
            new JObject { { "name", "FACE_TRACKER_VERSION" }, { "value", "12" } },
            new JObject { { "name", "segmentation" }, { "value", "segmentation_enabled" } },
            new JObject { { "name", "COMPRESSION" }, { "value", "ETC2_COMPRESSION" } },
            new JObject { { "name", "WORLD_TRACKER" }, { "value", "WORLD_TRACKER_ENABLED" } }
        };

        public static string AcceptLanguage = "en-US";

        public static readonly Uri BaseInstagramUri = new Uri(BaseInstagramApiUrl);

        public static string WebAddress = "https://www.instagram.com";

        public static string WebAccounts = "/accounts";

        public static string WebAccountData = WebAccounts + "/access_tool";

        public static string WebCurrentFollowRequests = WebAccountData + "/current_follow_requests";

        public static string WebFormerEmails = WebAccountData + "/former_emails";

        public static string WebFormerPhones = WebAccountData + "/former_phones";

        public static string WebFormerUsernames = WebAccountData + "/former_usernames";

        public static string WebFormerFullNames = WebAccountData + "/former_full_names";

        public static string WebFormerBioTexts = WebAccountData + "/former_bio_texts";

        public static string WebFormerBioLinks = WebAccountData + "/former_links_in_bio";

        public static string WebCursor = "__a=1&cursor={0}";

        public static readonly Uri InstagramWebUri = new Uri(WebAddress);
    }
}