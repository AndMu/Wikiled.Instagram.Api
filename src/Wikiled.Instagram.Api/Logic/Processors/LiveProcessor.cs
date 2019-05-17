using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.Models.Comment;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Live api functions.
    /// </summary>
    internal class InstaLiveProcessor : ILiveProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaLiveProcessor(
            AndroidDevice deviceInfo,
            UserSessionData user,
            IHttpRequestProcessor httpRequestProcessor,
            ILogger logger,
            UserAuthValidate userAuthValidate,
            InstaApi instaApi,
            InstaHttpHelper httpHelper)
        {
            this.deviceInfo = deviceInfo;
            this.user = user;
            this.httpRequestProcessor = httpRequestProcessor;
            this.logger = logger;
            this.userAuthValidate = userAuthValidate;
            this.instaApi = instaApi;
            this.httpHelper = httpHelper;
        }

        /// <summary>
        ///     Add an broadcast to post live.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        public async Task<IResult<InstaBroadcastAddToPostLive>> AddToPostLiveAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastAddToPostLiveUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastAddToPostLive>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastAddToPostLiveResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetAddToPostLiveConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastAddToPostLive), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastAddToPostLive>(exception);
            }
        }

        /// <summary>
        ///     Post a new comment to broadcast.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        /// <param name="commentText">Comment text</param>
        public async Task<IResult<InstaComment>> CommentAsync(string broadcastId, string commentText)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastPostCommentUri(broadcastId);
                var breadcrumb = InstaCryptoHelper.GetCommentBreadCrumbEncoded(commentText);
                var data = new JObject
                {
                    { "user_breadcrumb", commentText },
                    { "idempotence_token", Guid.NewGuid().ToString() },
                    { "comment_text", commentText },
                    { "live_or_vod", "1" },
                    { "offset_to_video_start", " 0" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaComment>(response, json);
                }

                var commentResponse = JsonConvert.DeserializeObject<InstaCommentResponse>(
                    json,
                    new InstaCommentDataConverter());
                var converter = InstaConvertersFabric.Instance.GetCommentConverter(commentResponse);
                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaComment), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaComment>(exception);
            }
        }

        // create, start, end broadcast
        /// <summary>
        ///     Create live broadcast. After create an live broadcast you must call StartAsync.
        /// </summary>
        /// <param name="previewWidth">Preview width</param>
        /// <param name="previewHeight">Preview height</param>
        /// <param name="broadcastMessage">Broadcast start message</param>
        public async Task<IResult<InstaBroadcastCreate>> CreateAsync(int previewWidth = 720,
                                                                     int previewHeight = 1184,
                                                                     string broadcastMessage = "")
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastCreateUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "preview_height", previewHeight },
                    { "preview_width", previewWidth },
                    { "broadcast_message", broadcastMessage },
                    { "broadcast_type", "RTMP" },
                    { "internal_only", 0 }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastCreate>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastCreateResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastCreateConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastCreate), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastCreate>(exception);
            }
        }

        /// <summary>
        ///     Delete an broadcast from post live.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        public async Task<IResult<bool>> DeletePostLiveAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastDeletePostLiveUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                return obj.Status == "ok" ? InstaResult.Success(true) : InstaResult.UnExpectedResponse<bool>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<bool>(exception);
            }
        }

        /// <summary>
        ///     Disable broadcast comments.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        public async Task<IResult<InstaBroadcastCommentEnableDisable>> DisableCommentsAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastDisableCommenstUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastCommentEnableDisable>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastCommentEnableDisableResponse>(json);
                return InstaResult.Success(
                    InstaConvertersFabric.Instance.GetBroadcastCommentEnableDisableConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaBroadcastCommentEnableDisable),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastCommentEnableDisable>(exception);
            }
        }

        /// <summary>
        ///     Enable broadcast comments.
        /// </summary>
        /// <param name="broadcastId"></param>
        public async Task<IResult<InstaBroadcastCommentEnableDisable>> EnableCommentsAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastEnableCommenstUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastCommentEnableDisable>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastCommentEnableDisableResponse>(json);
                return InstaResult.Success(
                    InstaConvertersFabric.Instance.GetBroadcastCommentEnableDisableConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaBroadcastCommentEnableDisable),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastCommentEnableDisable>(exception);
            }
        }

        /// <summary>
        ///     End live broadcast
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        /// <param name="endAfterCopyrightWarning">Copyright warning</param>
        public async Task<IResult<bool>> EndAsync(string broadcastId, bool endAfterCopyrightWarning = false)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastEndUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.UserName },
                    { "end_after_copyright_warning", endAfterCopyrightWarning.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                return obj.Status == "ok" ? InstaResult.Success(true) : InstaResult.UnExpectedResponse<bool>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<bool>(exception);
            }
        }

        /// <summary>
        ///     Get broadcast comments.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        /// <param name="lastCommentTs">Last comment time stamp</param>
        /// <param name="commentsRequested">Comments requested count</param>
        public async Task<IResult<InstaBroadcastCommentList>> GetCommentsAsync(
            string broadcastId,
            string lastCommentTs = "",
            int commentsRequested = 4)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastCommentUri(broadcastId, lastCommentTs);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastCommentList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastCommentListResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastCommentListConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastCommentList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastCommentList>(exception);
            }
        }

        /// <summary>
        ///     Get discover top live.
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaDiscoverTopLive>> GetDiscoverTopLiveAsync(
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var topLive = new InstaDiscoverTopLive();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaDiscoverTopLive Convert(InstaDiscoverTopLiveResponse instaDiscoverTop)
                {
                    return InstaConvertersFabric.Instance.GetDiscoverTopLiveConverter(instaDiscoverTop).Convert();
                }

                var topLiveResult = await GetDiscoverTopLive(paginationParameters.NextMaxId).ConfigureAwait(false);
                if (!topLiveResult.Succeeded)
                {
                    return InstaResult.Fail(topLiveResult.Info, topLive);
                }

                var topLiveResponse = topLiveResult.Value;
                topLive = Convert(topLiveResponse);
                topLive.NextMaxId = paginationParameters.NextMaxId = topLiveResponse.NextMaxId;
                paginationParameters.PagesLoaded++;

                while (topLiveResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    paginationParameters.PagesLoaded++;
                    var nextTop = await GetDiscoverTopLive(paginationParameters.NextMaxId).ConfigureAwait(false);
                    if (!nextTop.Succeeded)
                    {
                        return InstaResult.Fail(nextTop.Info, topLive);
                    }

                    var convertedTopLive = Convert(nextTop.Value);
                    topLive.NextMaxId = paginationParameters.NextMaxId = nextTop.Value.NextMaxId;
                    topLive.MoreAvailable = topLiveResponse.MoreAvailable = nextTop.Value.MoreAvailable;
                    topLive.AutoLoadMoreEnabled = nextTop.Value.AutoLoadMoreEnabled;
                    topLive.Broadcasts.AddRange(convertedTopLive.Broadcasts);
                    topLive.PostLiveBroadcasts.AddRange(convertedTopLive.PostLiveBroadcasts);
                    paginationParameters.PagesLoaded++;
                }

                return InstaResult.Success(topLive);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, topLive, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, topLive);
            }
        }

        /// <summary>
        ///     Get final viewer list.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        public async Task<IResult<InstaUserShortList>> GetFinalViewerListAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var viewers = new InstaUserShortList();
            try
            {
                var instaUri = InstaUriCreator.GetLiveFinalViewerListUri(broadcastId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserShortList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserListShortResponse>(json);
                viewers.AddRange(
                    obj.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));

                return InstaResult.Success(viewers);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, viewers, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, viewers);
            }
        }

        /// <summary>
        ///     Get heart beat and viewer count.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        public async Task<IResult<InstaBroadcastLiveHeartBeatViewerCount>> GetHeartBeatAndViewerCountAsync(
            string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetLiveHeartbeatAndViewerCountUri(broadcastId);
                var uploadId = ApiRequestMessage.GenerateUploadId();
                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    { new StringContent(user.CsrfToken), "\"_csrftoken\"" },
                    { new StringContent(deviceInfo.DeviceGuid.ToString()), "\"_uuid\"" },
                    { new StringContent("offset_to_video_start"), "30" }
                };
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = requestContent;
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastLiveHeartBeatViewerCount>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastLiveHeartBeatViewerCountResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastLiveHeartBeatViewerCountConverter(obj)
                                          .Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaBroadcastLiveHeartBeatViewerCount),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastLiveHeartBeatViewerCount>(exception);
            }
        }

        /// <summary>
        ///     Get broadcast information.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        public async Task<IResult<InstaBroadcastInfo>> GetInfoAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastInfoUri(broadcastId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastInfo>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastInfoResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastInfoConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastInfo), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastInfo>(exception);
            }
        }

        /// <summary>
        ///     Get join requests to current live broadcast
        /// </summary>
        /// <param name="broadcastId">Broadcast</param>
        public async Task<IResult<InstaUserShortList>> GetJoinRequestsAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var viewers = new InstaUserShortList();
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastJoinRequestsUri(broadcastId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserShortList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserListShortResponse>(json);
                viewers.AddRange(
                    obj.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                return InstaResult.Success(viewers);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, viewers, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, viewers);
            }
        }

        /// <summary>
        ///     Get broadcast like count.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        /// <param name="likeTs">Like time stamp</param>
        public async Task<IResult<InstaBroadcastLike>> GetLikeCountAsync(string broadcastId, int likeTs = 0)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetLiveLikeCountUri(broadcastId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastLike>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastLikeResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastLikeConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastLike), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastLike>(exception);
            }
        }

        /// <summary>
        ///     Get post live viewer list.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        /// <param name="maxId">Max id</param>
        public async Task<IResult<InstaUserShortList>> GetPostLiveViewerListAsync(string broadcastId, int? maxId = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var viewers = new InstaUserShortList();
            try
            {
                var instaUri = InstaUriCreator.GetPostLiveViewersListUri(broadcastId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserShortList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserListShortResponse>(json);
                viewers.AddRange(
                    obj.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                return InstaResult.Success(viewers);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, viewers, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, viewers);
            }
        }

        /// <summary>
        ///     Get suggested broadcasts
        /// </summary>
        public async Task<IResult<InstaBroadcastList>> GetSuggestedBroadcastsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSuggestedBroadcastsUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastSuggestedResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastListConverter(obj?.Broadcasts).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastList>(exception);
            }
        }

        /// <summary>
        ///     Get top live status.
        /// </summary>
        /// <param name="broadcastIds">Broadcast ids</param>
        public async Task<IResult<InstaBroadcastTopLiveStatusList>> GetTopLiveStatusAsync(params string[] broadcastIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            if (broadcastIds == null)
            {
                return InstaResult.Fail<InstaBroadcastTopLiveStatusList>("broadcast ids must be set");
            }

            try
            {
                var instaUri = InstaUriCreator.GetDiscoverTopLiveStatusUri();
                var data = new JObject { { "broadcast_ids", new JArray(broadcastIds) } };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastTopLiveStatusList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastTopLiveStatusResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastTopLiveStatusListConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaBroadcastTopLiveStatusList),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastTopLiveStatusList>(exception);
            }
        }

        /// <summary>
        ///     Get broadcast viewer list.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        public async Task<IResult<InstaUserShortList>> GetViewerListAsync(string broadcastId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var viewers = new InstaUserShortList();
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastViewerListUri(broadcastId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserShortList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserListShortResponse>(json);
                viewers.AddRange(
                    obj.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                return InstaResult.Success(viewers);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, viewers, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, viewers);
            }
        }

        /// <summary>
        ///     Like broadcast.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        /// <param name="likeCount">Like count (from 1 to 6)</param>
        public async Task<IResult<InstaBroadcastLike>> LikeAsync(string broadcastId, int likeCount = 1)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetLikeLiveUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "user_like_count", likeCount }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastLike>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastLikeResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastLikeConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastLike), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastLike>(exception);
            }
        }

        /// <summary>
        ///     Pin comment from broadcast.
        /// </summary>
        /// <param name="broadcastId"></param>
        /// <param name="commentId"></param>
        public async Task<IResult<InstaBroadcastPinUnpin>> PinCommentAsync(string broadcastId, string commentId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastPinCommentUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "comment_id", commentId },
                    { "offset_to_video_start", 0 }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastPinUnpin>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastPinUnpinResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastPinUnpinConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastPinUnpin), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastPinUnpin>(exception);
            }
        }

        /// <summary>
        ///     Share an live broadcast to direct recipients
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="broadcastId">Broadcast id to send ( <see cref="InstaBroadcast.Id" /> )</param>
        /// <param name="recipients">Recipients ids</param>
        public Task<IResult<bool>> ShareLiveToDirectRecipientAsync(string text, string broadcastId, params string[] recipients)
        {
            return ShareLiveToDirectThreadAsync(text, broadcastId, null, recipients);
        }

        /// <summary>
        ///     Share an live broadcast to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="broadcastId">Broadcast id to send ( <see cref="InstaBroadcast.Id" /> )</param>
        /// <param name="threadIds">Thread ids</param>
        public Task<IResult<bool>> ShareLiveToDirectThreadAsync(string text, string broadcastId, params string[] threadIds)
        {
            return ShareLiveToDirectThreadAsync(text, broadcastId, threadIds, null);
        }

        /// <summary>
        ///     Share an live broadcast to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="broadcastId">Broadcast id to send ( <see cref="InstaBroadcast.Id" /> )</param>
        /// <param name="threadIds">Thread ids</param>
        /// <param name="recipients">Recipients ids</param>
        public async Task<IResult<bool>> ShareLiveToDirectThreadAsync(string text,
                                                                      string broadcastId,
                                                                      string[] threadIds,
                                                                      string[] recipients)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetShareLiveToDirectUri();
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "text", text ?? string.Empty },
                    { "broadcast_id", broadcastId },
                    { "action", "send_item" },
                    { "client_context", clientContext },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                if (threadIds?.Length > 0)
                {
                    data.Add("thread_ids", $"[{threadIds.EncodeList(false)}]");
                }

                if (recipients?.Length > 0)
                {
                    data.Add("recipient_users", "[[" + recipients.EncodeList(false) + "]]");
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                return obj.Status.ToLower() == "ok"
                    ? InstaResult.Success(true)
                    : InstaResult.UnExpectedResponse<bool>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<bool>(exception);
            }
        }

        /// <summary>
        ///     Start live broadcast. NOTE: YOU MUST CREATE AN BROADCAST FIRST(CreateAsync) AND THEN CALL THIS METHOD.
        /// </summary>
        /// <param name="broadcastId">Broadcast id</param>
        /// <param name="sendNotifications">Send notifications</param>
        public async Task<IResult<InstaBroadcastStart>> StartAsync(string broadcastId, bool sendNotifications)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastStartUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "should_send_notifications", sendNotifications }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaBroadcastStartResponse>(json);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastStart>(response, json);
                }

                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastStartConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastStart), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastStart>(exception);
            }
        }

        /// <summary>
        ///     Unpin comment from broadcast.
        /// </summary>
        /// <param name="broadcastId"></param>
        /// <param name="commentId"></param>
        public async Task<IResult<InstaBroadcastPinUnpin>> UnPinCommentAsync(string broadcastId, string commentId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastUnPinCommentUri(broadcastId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "comment_id", commentId },
                    { "offset_to_video_start", 0 }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBroadcastPinUnpin>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBroadcastPinUnpinResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBroadcastPinUnpinConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBroadcastPinUnpin), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastPinUnpin>(exception);
            }
        }

        /// <summary>
        ///     NOT COMPLETE
        /// </summary>
        /// <returns></returns>
        public async Task<IResult<object>> GetPostLiveCommentsAsync(string broadcastId,
                                                                    int startingOffset = 0,
                                                                    string encodingTag = "instagram_dash_remuxed")
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                // kamel nist
                var instaUri = InstaUriCreator.GetBroadcastPostLiveCommentUri(broadcastId, startingOffset, encodingTag);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<object>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<object>(json);
                return InstaResult.Success(json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(string), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<string>(exception);
            }
        }

        /// <summary>
        ///     NOT COMPLETE
        /// </summary>
        /// <returns></returns>
        public async Task<IResult<object>> GetPostLiveLikesAsync(string broadcastId,
                                                                 int startingOffset = 0,
                                                                 string encodingTag = "instagram_dash_remuxed")
        {
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastPostLiveLikesUri(broadcastId, startingOffset, encodingTag);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<object>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<object>(json);
                return InstaResult.Success(json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(string), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<string>(exception);
            }
        }

        /// <summary>
        ///     NOT COMPLETE
        /// </summary>
        /// <returns></returns>
        public async Task<IResult<object>> NotifyToFriendsAsync()
        {
            try
            {
                var instaUri = InstaUriCreator.GetLiveNotifyToFriendsUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<object>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<object>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(object), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<object>(exception);
            }
        }

        /// <summary>
        ///     NOT COMPLETE
        /// </summary>
        /// <returns></returns>
        public async Task<IResult<object>> SeenBroadcastAsync(string broadcastId, string pk)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BaseInstagramApiUrl + "media/seen/?reel=1&live_vod=0");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "live_vods_skipped", new JObject() },
                    { "nuxes_skipped", new JObject() },
                    { "nuxes", new JObject() },
                    { "reels", new JObject { { broadcastId, new JArray(pk) } } },
                    { "live_vods", new JObject() },
                    { "reel_media_skipped", new JObject() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<object>(response, json);
                }

                return InstaResult.Success(json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaBroadcastLiveHeartBeatViewerCountResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBroadcastLiveHeartBeatViewerCountResponse>(exception);
            }
        }

        private async Task<IResult<InstaDiscoverTopLiveResponse>> GetDiscoverTopLive(string maxId)
        {
            try
            {
                var instaUri = InstaUriCreator.GetDiscoverTopLiveUri(maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDiscoverTopLiveResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDiscoverTopLiveResponse>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDiscoverTopLiveResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDiscoverTopLiveResponse>(exception);
            }
        }
    }
}