using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Comment;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Comments api functions.
    /// </summary>
    internal class InstaCommentProcessor : ICommentProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaCommentProcessor(
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
        ///     Block an user from commenting to medias
        /// </summary>
        /// <param name="userIds">User ids (pk)</param>
        public Task<IResult<bool>> BlockUserCommentingAsync(params long[] userIds)
        {
            return BlockUnblockCommenting(true, userIds);
        }

        /// <summary>
        ///     Comment media
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="text">Comment text</param>
        public async Task<IResult<InstaComment>> CommentMediaAsync(string mediaId, string text)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetPostCommetUri(mediaId);
                var breadcrumb = InstaCryptoHelper.GetCommentBreadCrumbEncoded(text);
                var fields = new Dictionary<string, string>
                {
                    { "user_breadcrumb", breadcrumb },
                    { "idempotence_token", Guid.NewGuid().ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "comment_text", text },
                    { "containermodule", "comments_feed_timeline" },
                    { "radio_type", "wifi-none" }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
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

        /// <summary>
        ///     Delete media comment
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="commentId">Comment id</param>
        public async Task<IResult<bool>> DeleteCommentAsync(string mediaId, string commentId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDeleteCommentUri(mediaId, commentId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Delete media comments(multiple)
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="commentIds">Comment id</param>
        public async Task<IResult<bool>> DeleteMultipleCommentsAsync(string mediaId, params string[] commentIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDeleteMultipleCommentsUri(mediaId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "comment_ids_to_delete", commentIds.EncodeList(false) }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Disable media comments
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<bool>> DisableMediaCommentAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDisableMediaCommetsUri(mediaId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Allow media comments
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<bool>> EnableMediaCommentAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAllowMediaCommetsUri(mediaId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Get blocked users from commenting
        /// </summary>
        public async Task<IResult<InstaUserShortList>> GetBlockedCommentersAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBlockedCommentersUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserShortList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBlockedCommentersResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetBlockedCommentersConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserShortList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUserShortList>(exception);
            }
        }

        /// <summary>
        ///     Get media comments likers
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<InstaLikersList>> GetMediaCommentLikersAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetMediaCommetLikersUri(mediaId);
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaLikersList>(response, json);
                }

                var likers = new InstaLikersList();
                var likersResponse = JsonConvert.DeserializeObject<InstaMediaLikersResponse>(json);
                likers.UsersCount = likersResponse.UsersCount;
                likers.AddRange(
                    likersResponse.Users.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                return InstaResult.Success(likers);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaLikersList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaLikersList>(exception);
            }
        }

        /// <summary>
        ///     Get media comments
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaCommentList>> GetMediaCommentsAsync(
            string mediaId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                var commentsUri = InstaUriCreator.GetMediaCommentsUri(mediaId, paginationParameters.NextMaxId);
                if (!string.IsNullOrEmpty(paginationParameters.NextMinId))
                {
                    commentsUri = InstaUriCreator.GetMediaCommentsMinIdUri(mediaId, paginationParameters.NextMinId);
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, commentsUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaCommentList>(response, json);
                }

                var commentListResponse = JsonConvert.DeserializeObject<InstaCommentListResponse>(json);
                var pagesLoaded = 1;

                InstaCommentList Convert(InstaCommentListResponse commentsResponse)
                {
                    return InstaConvertersFabric.Instance.GetCommentListConverter(commentsResponse).Convert();
                }

                while (commentListResponse.MoreCommentsAvailable &&
                    !string.IsNullOrEmpty(commentListResponse.NextMaxId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad ||
                    commentListResponse.MoreHeadLoadAvailable &&
                    !string.IsNullOrEmpty(commentListResponse.NextMinId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    IResult<InstaCommentListResponse> nextComments;
                    if (!string.IsNullOrEmpty(commentListResponse.NextMaxId))
                    {
                        nextComments = await GetCommentListWithMaxIdAsync(mediaId, commentListResponse.NextMaxId, null).ConfigureAwait(false);
                    }
                    else
                    {
                        nextComments = await GetCommentListWithMaxIdAsync(mediaId, null, commentListResponse.NextMinId).ConfigureAwait(false);
                    }

                    if (!nextComments.Succeeded)
                    {
                        return InstaResult.Fail(nextComments.Info, Convert(commentListResponse));
                    }

                    commentListResponse.NextMaxId = nextComments.Value.NextMaxId;
                    commentListResponse.NextMinId = nextComments.Value.NextMinId;
                    commentListResponse.MoreCommentsAvailable = nextComments.Value.MoreCommentsAvailable;
                    commentListResponse.MoreHeadLoadAvailable = nextComments.Value.MoreHeadLoadAvailable;
                    commentListResponse.Comments.AddRange(nextComments.Value.Comments);
                    paginationParameters.NextMaxId = nextComments.Value.NextMaxId;
                    paginationParameters.NextMinId = nextComments.Value.NextMinId;
                    pagesLoaded++;
                }

                paginationParameters.NextMaxId = commentListResponse.NextMaxId;
                paginationParameters.NextMinId = commentListResponse.NextMinId;
                var converter = InstaConvertersFabric.Instance.GetCommentListConverter(commentListResponse);
                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaCommentList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaCommentList>(exception);
            }
        }

        /// <summary>
        ///     Get media inline comments
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="targetCommentId">Target comment id</param>
        /// <param name="paginationParameters">Maximum amount of pages to load and start id</param>
        /// <returns></returns>
        public async Task<IResult<InstaInlineCommentList>> GetMediaRepliesCommentsAsync(
            string mediaId,
            string targetCommentId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                var commentsUri =
                    InstaUriCreator.GetMediaInlineCommentsUri(mediaId, targetCommentId, paginationParameters.NextMaxId);
                if (!string.IsNullOrEmpty(paginationParameters.NextMinId))
                {
                    commentsUri =
                        InstaUriCreator.GetMediaInlineCommentsWithMinIdUri(mediaId,
                                                                      targetCommentId,
                                                                      paginationParameters.NextMinId);
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, commentsUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaInlineCommentList>(response, json);
                }

                var commentListResponse = JsonConvert.DeserializeObject<InstaInlineCommentListResponse>(json);

                var pagesLoaded = 1;

                InstaInlineCommentList Convert(InstaInlineCommentListResponse commentsResponse)
                {
                    return InstaConvertersFabric.Instance.GetInlineCommentsConverter(commentsResponse).Convert();
                }

                while (commentListResponse.HasMoreTailChildComments &&
                    !string.IsNullOrEmpty(commentListResponse.NextMaxId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad ||
                    commentListResponse.HasMoreHeadChildComments &&
                    !string.IsNullOrEmpty(commentListResponse.NextMinId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    IResult<InstaInlineCommentListResponse> nextComments;
                    if (!string.IsNullOrEmpty(commentListResponse.NextMaxId))
                    {
                        nextComments =
                            await GetInlineCommentListWithMaxIdAsync(mediaId,
                                                                     targetCommentId,
                                                                     commentListResponse.NextMaxId,
                                                                     null).ConfigureAwait(false);
                    }
                    else
                    {
                        nextComments =
                            await GetInlineCommentListWithMaxIdAsync(mediaId,
                                                                     targetCommentId,
                                                                     null,
                                                                     commentListResponse.NextMinId).ConfigureAwait(false);
                    }

                    if (!nextComments.Succeeded)
                    {
                        return InstaResult.Fail(nextComments.Info, Convert(commentListResponse));
                    }

                    commentListResponse.NextMaxId = nextComments.Value.NextMaxId;
                    commentListResponse.NextMinId = nextComments.Value.NextMinId;
                    commentListResponse.HasMoreHeadChildComments = nextComments.Value.HasMoreHeadChildComments;
                    commentListResponse.HasMoreTailChildComments = nextComments.Value.HasMoreTailChildComments;
                    commentListResponse.ChildComments.AddRange(nextComments.Value.ChildComments);
                    paginationParameters.NextMaxId = nextComments.Value.NextMaxId;
                    paginationParameters.NextMinId = nextComments.Value.NextMinId;
                    pagesLoaded++;
                }

                paginationParameters.NextMaxId = commentListResponse.NextMaxId;
                paginationParameters.NextMinId = commentListResponse.NextMinId;
                var comments = InstaConvertersFabric.Instance.GetInlineCommentsConverter(commentListResponse).Convert();
                return InstaResult.Success(comments);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaInlineCommentList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaInlineCommentList>(exception);
            }
        }

        /// <summary>
        ///     Like media comment
        /// </summary>
        /// <param name="commentId">Comment id</param>
        public async Task<IResult<bool>> LikeCommentAsync(string commentId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetLikeCommentUri(commentId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Inline comment media
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="targetCommentId">Target comment id</param>
        /// <param name="text">Comment text</param>
        public async Task<IResult<InstaComment>> ReplyCommentMediaAsync(
            string mediaId,
            string targetCommentId,
            string text)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetPostCommetUri(mediaId);
                var breadcrumb = InstaCryptoHelper.GetCommentBreadCrumbEncoded(text);
                var fields = new Dictionary<string, string>
                {
                    { "user_breadcrumb", breadcrumb },
                    { "idempotence_token", Guid.NewGuid().ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "replied_to_comment_id", targetCommentId },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "comment_text", text },
                    { "containermodule", "comments_feed_timeline" },
                    { "radio_type", "wifi-none" }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
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

        /// <summary>
        ///     Report media comment
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="commentId">Comment id</param>
        public async Task<IResult<bool>> ReportCommentAsync(string mediaId, string commentId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetReportCommetUri(mediaId, commentId);
                var fields = new Dictionary<string, string>
                {
                    { "media_id", mediaId },
                    { "comment_id", commentId },
                    { "reason", "1" },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Translate comment or captions
        ///     <para>Note: use this function to translate captions too! (i.e: <see cref="InstaCaption.Pk" />)</para>
        /// </summary>
        /// <param name="commentIds">Comment id(s) (Array of <see cref="InstaComment.Pk" />)</param>
        public async Task<IResult<InstaTranslateList>> TranslateCommentAsync(params long[] commentIds)
        {
            try
            {
                if (commentIds == null || commentIds != null && !commentIds.Any())
                {
                    throw new ArgumentException("At least one comment id require");
                }

                var instaUri = InstaUriCreator.GetTranslateCommentsUri(string.Join(",", commentIds));

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(json))
                {
                    return InstaResult.UnExpectedResponse<InstaTranslateList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaTranslateContainerResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetTranslateContainerConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTranslateList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaTranslateList>(exception);
            }
        }

        /// <summary>
        ///     Unblock an user from commenting to medias
        /// </summary>
        /// <param name="userIds">User ids (pk)</param>
        public Task<IResult<bool>> UnblockUserCommentingAsync(params long[] userIds)
        {
            return BlockUnblockCommenting(false, userIds);
        }

        /// <summary>
        ///     Unlike media comment
        /// </summary>
        /// <param name="commentId">Comment id</param>
        public async Task<IResult<bool>> UnlikeCommentAsync(string commentId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUnLikeCommentUri(commentId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        private async Task<IResult<bool>> BlockUnblockCommenting(bool block, long[] userIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (userIds == null || userIds?.Length == 0)
                {
                    InstaResult.Fail<bool>("At least one user id (pk) is require");
                }

                var instaUri = InstaUriCreator.GetSetBlockedCommentersUri();

                //var blockedUsersResponse = await GetBlockedCommentersAsync();
                //var blockedUsers = new List<long>();
                //if (blockedUsersResponse.Succeeded && blockedUsersResponse.Value?.Count > 0)
                //{
                //    foreach (var u in blockedUsersResponse.Value)
                //    {
                //        foreach (var id in userIds)
                //            if (u.Pk == id)
                //                blockedUsers.Add(u.Pk);
                //    }
                //}

                //{
                //	"_csrftoken": "UBPgM6BG1Qr95lO4ofLYpgJXtbVvVnvs",
                //	"_uid": "7405924766",
                //	"_uuid": "6324ecb2-e663-4dc8-a3a1-289c699cc876",
                //	"commenter_block_status": {
                //		"block": [9013775990, 9013775990],
                //		"unblock": [9013775990]
                //	}
                //}
                var commenterBlockStatus = new JObject();
                if (block)
                {
                    commenterBlockStatus.Add("block", new JArray(userIds));
                    commenterBlockStatus.Add("unblock", new JArray());
                }
                else
                {
                    commenterBlockStatus.Add("block", new JArray());
                    commenterBlockStatus.Add("unblock", new JArray(userIds));
                }

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "commenter_block_status", commenterBlockStatus }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        private async Task<IResult<InstaCommentListResponse>> GetCommentListWithMaxIdAsync(
            string mediaId,
            string nextMaxId,
            string nextMinId)
        {
            try
            {
                var commentsUri = InstaUriCreator.GetMediaCommentsUri(mediaId, nextMaxId);
                if (!string.IsNullOrEmpty(nextMinId))
                {
                    commentsUri = InstaUriCreator.GetMediaCommentsMinIdUri(mediaId, nextMinId);
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, commentsUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaCommentListResponse>(response, json);
                }

                var comments = JsonConvert.DeserializeObject<InstaCommentListResponse>(json);
                return InstaResult.Success(comments);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaCommentListResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaCommentListResponse>(exception);
            }
        }

        private async Task<IResult<InstaInlineCommentListResponse>> GetInlineCommentListWithMaxIdAsync(
            string mediaId,
            string targetCommandId,
            string nextMaxId,
            string nextMinId)
        {
            try
            {
                var commentsUri = InstaUriCreator.GetMediaInlineCommentsUri(mediaId, targetCommandId, nextMaxId);
                if (!string.IsNullOrEmpty(nextMinId))
                {
                    commentsUri = InstaUriCreator.GetMediaInlineCommentsWithMinIdUri(mediaId, targetCommandId, nextMinId);
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, commentsUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaInlineCommentListResponse>(response, json);
                }

                var commentListResponse = JsonConvert.DeserializeObject<InstaInlineCommentListResponse>(json);
                return InstaResult.Success(commentListResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaInlineCommentListResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaInlineCommentListResponse>(exception);
            }
        }
    }
}