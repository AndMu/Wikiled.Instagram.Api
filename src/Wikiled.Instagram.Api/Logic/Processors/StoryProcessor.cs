using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.Models.Highlight;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Extensions;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Story api functions.
    /// </summary>
    internal class InstaStoryProcessor : IStoryProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly InstaUserAuthValidate userAuthValidate;

        public InstaStoryProcessor(
            AndroidDevice deviceInfo,
            UserSessionData user,
            IHttpRequestProcessor httpRequestProcessor,
            ILogger logger,
            InstaUserAuthValidate userAuthValidate,
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
        ///     Respond to an story question
        /// </summary>
        /// <param name="storyId">Story id (<see cref="InstaStoryItem.Id" />)</param>
        /// <param name="questionId">Question id (<see cref="InstaStoryQuestionStickerItem.QuestionId" />)</param>
        /// <param name="responseText">Text to respond</param>
        public async Task<IResult<bool>> AnswerToStoryQuestionAsync(string storyId,
                                                                    long questionId,
                                                                    string responseText)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetStoryQuestionResponseUri(storyId, questionId);
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "response", responseText ?? string.Empty },
                    { "type", "text" }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                return obj.Status.ToLower() == "ok"
                    ? InstaResult.Success(true)
                    : InstaResult.UnExpectedResponse<bool>(response, obj.Message, null);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, false, InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, false);
            }
        }

        /// <summary>
        ///     Create new highlight
        /// </summary>
        /// <param name="mediaId">Story media id</param>
        /// <param name="title">Highlight title</param>
        /// <param name="cropWidth">
        ///     Crop width It depends on the aspect ratio/size of device display and the aspect ratio of story
        ///     uploaded. must be in a range of 0-1, i.e: 0.19545822
        /// </param>
        /// <param name="cropHeight">
        ///     Crop height It depends on the aspect ratio/size of device display and the aspect ratio of
        ///     story uploaded. must be in a range of 0-1, i.e: 0.8037307
        /// </param>
        public async Task<IResult<InstaHighlightFeed>> CreateHighlightFeedAsync(
            string mediaId,
            string title,
            float cropWidth,
            float cropHeight)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var cover = new JObject
                {
                    { "media_id", mediaId },
                    { "crop_rect", new JArray { 0.0, cropWidth, 1.0, cropHeight }.ToString(Formatting.None) }
                }.ToString(Formatting.None);
                var data = new JObject
                {
                    { "source", "self_profile" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "cover", cover },
                    { "title", title },
                    { "media_ids", $"[{new[] { mediaId }.EncodeList()}]" }
                };

                var instaUri = InstaUriCreator.GetHighlightCreateUri();
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHighlightFeed>(response, json);
                }

                var highlightFeedResponse = JsonConvert.DeserializeObject<InstaHighlightReelResponse>(
                    json,
                    new InstaHighlightReelDataConverter());
                var highlightStoryFeed =
                    InstaConvertersFabric.Instance.GetHighlightReelConverter(highlightFeedResponse).Convert();
                return InstaResult.Success(highlightStoryFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHighlightFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaHighlightFeed>(exception);
            }
        }

        /// <summary>
        ///     Delete highlight feed
        /// </summary>
        /// <param name="highlightId">Highlight id</param>
        /// <param name="mediaId">Media id (CoverMedia.MediaId)</param>
        public async Task<IResult<bool>> DeleteHighlightFeedAsync(string highlightId, string mediaId)
        {
            return await AppendOrDeleteHighlight(highlightId, mediaId, true).ConfigureAwait(false);
        }

        /// <summary>
        ///     Delete a media story (photo or video)
        /// </summary>
        /// <param name="storyMediaId">Story media id</param>
        /// <param name="sharingType">The type of the media</param>
        /// <returns>Return true if the story media is deleted</returns>
        public async Task<IResult<bool>> DeleteStoryAsync(string storyMediaId,
                                                          InstaSharingType sharingType = InstaSharingType.Video)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var deleteMediaUri = InstaUriCreator.GetDeleteStoryMediaUri(storyMediaId, sharingType);

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "media_id", storyMediaId }
                };

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, deleteMediaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var deletedResponse = JsonConvert.DeserializeObject<InstaDeleteResponse>(json);
                return InstaResult.Success(deletedResponse.IsDeleted);
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
        ///     Follow countdown stories
        /// </summary>
        /// <param name="countdownId">Countdown id (<see cref="InstaStoryCountdownStickerItem.CountdownId" />)</param>
        public async Task<IResult<bool>> FollowCountdownStoryAsync(long countdownId)
        {
            return await FollowUnfollowCountdown(InstaUriCreator.GetStoryFollowCountdownUri(countdownId)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get list of users that blocked from seeing your stories
        /// </summary>
        public async Task<IResult<InstaUserShortList>> GetBlockedUsersFromStoriesAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var list = new InstaUserShortList();
            try
            {
                var instaUri = InstaUriCreator.GetBlockedStoriesUri();
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserShortList>(response, json);
                }

                var usersResponse = JsonConvert.DeserializeObject<InstaUserListShortResponse>(json);
                list.AddRange(
                    usersResponse.Items.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                return InstaResult.Success(list);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, list, InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, list);
            }
        }

        /// <summary>
        ///     Get stories countdowns for self accounts
        /// </summary>
        public async Task<IResult<InstaStoryCountdownList>> GetCountdownsStoriesAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetStoryCountdownMediaUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaStoryCountdownList>(response, json);
                }

                var countdownListResponse = JsonConvert.DeserializeObject<InstaStoryCountdownListResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetStoryCountdownListConverter(countdownListResponse)
                                          .Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryCountdownList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryCountdownList>(exception);
            }
        }

        /// <summary>
        ///     Get user highlight feeds by user id (pk)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<InstaHighlightFeeds>> GetHighlightFeedsAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetHighlightFeedsUri(userId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHighlightFeeds>(response, json);
                }

                var highlightFeedResponse = JsonConvert.DeserializeObject<InstaHighlightFeedsResponse>(json);
                var highlightStoryFeed = InstaConvertersFabric.Instance.GetHighlightFeedsConverter(highlightFeedResponse)
                    .Convert();
                return InstaResult.Success(highlightStoryFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHighlightFeeds), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaHighlightFeeds>(exception);
            }
        }

        /// <summary>
        ///     Get single highlight medias
        ///     <para>Note: get highlight id from <see cref="IStoryProcessor.GetHighlightFeedsAsync(long)" /></para>
        /// </summary>
        /// <param name="highlightId">Highlight id (Get it from <see cref="IStoryProcessor.GetHighlightFeedsAsync(long)" />)</param>
        public async Task<IResult<InstaHighlightSingleFeed>> GetHighlightMediasAsync(string highlightId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (string.IsNullOrEmpty(highlightId))
                {
                    throw new ArgumentNullException("highlightId cannot be null or empty");
                }

                var instaUri = InstaUriCreator.GetReelMediaUri();
                var data = new JObject
                {
                    {
                        InstaApiConstants.SupportedCapabalitiesHeader,
                        InstaApiConstants.SupportedCapabalities.ToString(Formatting.None)
                    },
                    { "source", "profile" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "user_ids", new JArray(highlightId) }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHighlightSingleFeed>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaHighlightReelResponse>(
                    json,
                    new InstaHighlightReelsListDataConverter());

                return obj?.Reel != null
                    ? InstaResult.Success(InstaConvertersFabric.Instance.GetHighlightReelConverter(obj).Convert())
                    : InstaResult.Fail<InstaHighlightSingleFeed>("No reels found");
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHighlightSingleFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaHighlightSingleFeed>(exception);
            }
        }

        /// <summary>
        ///     Get user highlights archive
        ///     <para>
        ///         Note: Use <see cref="IStoryProcessor.GetHighlightsArchiveMediasAsync(string)" /> to get hightlight medias of
        ///         an specific day.
        ///     </para>
        /// </summary>
        public async Task<IResult<InstaHighlightShortList>> GetHighlightsArchiveAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetHighlightsArchiveUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHighlightShortList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaHighlightShortListResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetHighlightShortListConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHighlightShortList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaHighlightShortList>(exception);
            }
        }

        /// <summary>
        ///     Get highlights archive medias
        ///     <para>Note: get highlight id from <see cref="IStoryProcessor.GetHighlightsArchiveAsync" /></para>
        /// </summary>
        /// <param name="highlightId">Highlight id (Get it from <see cref="IStoryProcessor.GetHighlightsArchiveAsync" />)</param>
        public async Task<IResult<InstaHighlightSingleFeed>> GetHighlightsArchiveMediasAsync(string highlightId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (string.IsNullOrEmpty(highlightId))
                {
                    throw new ArgumentNullException("highlightId cannot be null or empty");
                }

                var instaUri = InstaUriCreator.GetReelMediaUri();

                var data = new JObject
                {
                    {
                        InstaApiConstants.SupportedCapabalitiesHeader,
                        InstaApiConstants.SupportedCapabalities.ToString(Formatting.None)
                    },
                    { "source", "reel_highlights_gallery" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "user_ids", new JArray(highlightId) }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHighlightSingleFeed>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaHighlightReelResponse>(
                    json,
                    new InstaHighlightReelsListDataConverter());

                return obj?.Reel != null
                    ? InstaResult.Success(InstaConvertersFabric.Instance.GetHighlightReelConverter(obj).Convert())
                    : InstaResult.Fail<InstaHighlightSingleFeed>("No reels found");
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHighlightSingleFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaHighlightSingleFeed>(exception);
            }
        }

        /// <summary>
        ///     Get user story feed (stories from users followed by current user).
        /// </summary>
        public async Task<IResult<InstaStoryFeed>> GetStoryFeedAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var storyFeedUri = InstaUriCreator.GetStoryFeedUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, storyFeedUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaStoryFeed>(response, json);
                }

                var storyFeedResponse = JsonConvert.DeserializeObject<InstaStoryFeedResponse>(json);
                var instaStoryFeed = InstaConvertersFabric.Instance.GetStoryFeedConverter(storyFeedResponse).Convert();
                return InstaResult.Success(instaStoryFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryFeed>(exception);
            }
        }

        /// <summary>
        ///     Get story media viewers
        /// </summary>
        /// <param name="storyMediaId">Story media id</param>
        /// <param name="paginationParameters">Pagination parameters</param>
        public async Task<IResult<InstaReelStoryMediaViewers>> GetStoryMediaViewersAsync(
            string storyMediaId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaReelStoryMediaViewers Convert(InstaReelStoryMediaViewersResponse reelResponse)
                {
                    return InstaConvertersFabric.Instance.GetReelStoryMediaViewersConverter(reelResponse).Convert();
                }

                var storyMediaViewersResult = await GetStoryMediaViewers(storyMediaId, paginationParameters?.NextMaxId).ConfigureAwait(false);

                if (!storyMediaViewersResult.Succeeded)
                {
                    return InstaResult.Fail(storyMediaViewersResult.Info, default(InstaReelStoryMediaViewers));
                }

                var storyMediaViewersResponse = storyMediaViewersResult.Value;
                paginationParameters.NextMaxId = storyMediaViewersResponse.NextMaxId;

                while (!string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    paginationParameters.PagesLoaded++;
                    var nextStoryViewers = await GetStoryMediaViewers(storyMediaId, paginationParameters.NextMaxId).ConfigureAwait(false);
                    if (!nextStoryViewers.Succeeded)
                    {
                        return InstaResult.Fail(nextStoryViewers.Info, Convert(nextStoryViewers.Value));
                    }

                    storyMediaViewersResponse.NextMaxId =
                        paginationParameters.NextMaxId = nextStoryViewers.Value.NextMaxId;
                    storyMediaViewersResponse.Users.AddRange(nextStoryViewers.Value.Users);
                }

                return InstaResult.Success(Convert(storyMediaViewersResponse));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaReelStoryMediaViewers), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaReelStoryMediaViewers>(exception);
            }
        }

        /// <summary>
        ///     Get story poll voters
        /// </summary>
        /// <param name="storyMediaId">Story media id</param>
        /// <param name="pollId">Story poll id</param>
        /// <param name="paginationParameters">Pagination parameters</param>
        public async Task<IResult<InstaStoryPollVotersList>> GetStoryPollVotersAsync(
            string storyMediaId,
            string pollId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaStoryPollVotersList Convert(InstaStoryPollVotersListResponse storyVotersResponse)
                {
                    return InstaConvertersFabric.Instance.GetStoryPollVotersListConverter(storyVotersResponse).Convert();
                }

                var votersResult = await GetStoryPollVoters(storyMediaId, pollId, paginationParameters?.NextMaxId).ConfigureAwait(false);

                if (!votersResult.Succeeded)
                {
                    return InstaResult.Fail(votersResult.Info, default(InstaStoryPollVotersList));
                }

                var votersResponse = votersResult.Value;
                paginationParameters.NextMaxId = votersResponse.MaxId;

                while (votersResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    paginationParameters.PagesLoaded++;
                    var nextVoters = await GetStoryPollVoters(storyMediaId, pollId, paginationParameters.NextMaxId).ConfigureAwait(false);
                    if (!nextVoters.Succeeded)
                    {
                        return InstaResult.Fail(nextVoters.Info, Convert(nextVoters.Value));
                    }

                    votersResponse.MaxId = paginationParameters.NextMaxId = nextVoters.Value.MaxId;
                    votersResponse.Voters.AddRange(nextVoters.Value.Voters);
                    votersResponse.LatestPollVoteTime = nextVoters.Value.LatestPollVoteTime;
                    votersResponse.MoreAvailable = nextVoters.Value.MoreAvailable;
                }

                return InstaResult.Success(Convert(votersResponse));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryPollVotersList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryPollVotersList>(exception);
            }
        }

        /// <summary>
        ///     Get the story by userId
        /// </summary>
        /// <param name="userId">User Id</param>
        public async Task<IResult<InstaStory>> GetUserStoryAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userStoryUri = InstaUriCreator.GetUserStoryUri(userId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userStoryUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    InstaResult.UnExpectedResponse<InstaStory>(response, json);
                }

                var userStoryResponse = JsonConvert.DeserializeObject<InstaStoryResponse>(json);
                var userStory = InstaConvertersFabric.Instance.GetStoryConverter(userStoryResponse).Convert();
                return InstaResult.Success(userStory);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStory), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStory>(exception);
            }
        }

        /// <summary>
        ///     Get user story reel feed. Contains user info last story including all story items.
        /// </summary>
        /// <param name="userId">User identifier (PK)</param>
        public async Task<IResult<InstaReelFeed>> GetUserStoryFeedAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var feed = new InstaReelFeed();
            try
            {
                var userFeedUri = InstaUriCreator.GetUserReelFeedUri(userId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userFeedUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaReelFeed>(response, json);
                }

                var feedResponse = JsonConvert.DeserializeObject<InstaReelFeedResponse>(json);
                feed = InstaConvertersFabric.Instance.GetReelFeedConverter(feedResponse).Convert();
                return InstaResult.Success(feed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaReelFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, feed);
            }
        }

        /// <summary>
        ///     Seen highlight
        ///     <para>Get media id from <see cref="InstaHighlightFeed.CoverMedia.MediaId" /></para>
        /// </summary>
        /// <param name="mediaId">Media identifier (get it from <see cref="InstaHighlightFeed.CoverMedia.MediaId" />)</param>
        /// <param name="highlightId">Highlight id</param>
        /// <param name="takenAtUnix">Taken at unix</param>
        public async Task<IResult<bool>> MarkHighlightAsSeenAsync(string mediaId, string highlightId, long takenAtUnix)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSeenMediaStoryUri();
                var reelId = $"{mediaId}_{highlightId}";
                var dateTimeUnix = DateTime.UtcNow.ToUnixTime();

                var reel = new JObject { { reelId, new JArray($"{takenAtUnix}_{dateTimeUnix}") } };
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "container_module", "profile" },
                    { "live_vods_skipped", new JObject() },
                    { "nuxes_skipped", new JObject() },
                    { "nuxes", new JObject() },
                    { "reels", reel },
                    { "live_vods", new JObject() },
                    { "reel_media_skipped", new JObject() }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Seen story
        /// </summary>
        /// <param name="storyMediaId">Story media identifier</param>
        /// <param name="takenAtUnix">Taken at unix</param>
        public async Task<IResult<bool>> MarkStoryAsSeenAsync(string storyMediaId, long takenAtUnix)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSeenMediaStoryUri();
                var storyId = $"{storyMediaId}_{storyMediaId.Split('_')[1]}";
                var dateTimeUnix = DateTime.UtcNow.ToUnixTime();
                var reel = new JObject { { storyId, new JArray($"{takenAtUnix}_{dateTimeUnix}") } };
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "container_module", "feed_timeline" },
                    { "live_vods_skipped", new JObject() },
                    { "nuxes_skipped", new JObject() },
                    { "nuxes", new JObject() },
                    { "reels", reel },
                    { "live_vods", new JObject() },
                    { "reel_media_skipped", new JObject() }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Reply to story
        ///     <para>Note: Get story media id from <see cref="InstaMedia.Identifier" /></para>
        /// </summary>
        /// <param name="storyMediaId">Media id (get it from <see cref="InstaMedia.Identifier" />)</param>
        /// <param name="userId">Story owner user pk (get it from <see cref="InstaMedia.User.Pk" />)</param>
        /// <param name="text">Text to send</param>
        public async Task<IResult<bool>> ReplyToStoryAsync(string storyMediaId, long userId, string text)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBroadcastReelShareUri();
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "recipient_users", $"[[{userId}]]" },
                    { "action", "send_item" },
                    { "client_context", clientContext },
                    { "media_id", storyMediaId },
                    { "_csrftoken", user.CsrfToken },
                    { "text", text ?? string.Empty },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Share an media to story
        ///     <para>
        ///         Note 1: You must draw whatever you want in your image first!
        ///         Also it's on you to calculate clickable media but mostly is 0.5 for width and height
        ///     </para>
        ///     <para>
        ///         Note 2: Get media pk from <see cref="InstaMedia.Pk" />
        ///     </para>
        /// </summary>
        /// <param name="image">Photo to upload</param>
        /// <param name="mediaStoryUpload">
        ///     Media options
        ///     <para>
        ///         Note 1: You must draw whatever you want in your image first!
        ///         Also it's on you to calculate clickable media but mostly is 0.5 for width and height
        ///     </para>
        ///     <para>
        ///         Note 2: Get media pk from <see cref="InstaMedia.Pk" />
        ///     </para>
        /// </param>
        public async Task<IResult<InstaStoryMedia>> ShareMediaAsStoryAsync(
            InstaImage image,
            InstaMediaStoryUpload mediaStoryUpload)
        {
            return await ShareMediaAsStoryAsync(null, image, mediaStoryUpload).ConfigureAwait(false);
        }

        /// <summary>
        ///     Share an media to story with progress
        ///     <para>
        ///         Note 1: You must draw whatever you want in your image first!
        ///         Also it's on you to calculate clickable media but mostly is 0.5 for width and height
        ///     </para>
        ///     <para>
        ///         Note 2: Get media pk from <see cref="InstaMedia.Pk" />
        ///     </para>
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="image">Photo to upload</param>
        /// <param name="mediaStoryUpload">
        ///     Media options
        ///     <para>
        ///         Note 1: You must draw whatever you want in your image first!
        ///         Also it's on you to calculate clickable media but mostly is 0.5 for width and height
        ///     </para>
        ///     <para>
        ///         Note 2: Get media pk from <see cref="InstaMedia.Pk" />
        ///     </para>
        /// </param>
        public async Task<IResult<InstaStoryMedia>> ShareMediaAsStoryAsync(
            Action<InstaUploaderProgress> progress,
            InstaImage image,
            InstaMediaStoryUpload mediaStoryUpload)
        {
            if (image == null)
            {
                return InstaResult.Fail<InstaStoryMedia>("Image cannot be null");
            }

            if (mediaStoryUpload == null)
            {
                return InstaResult.Fail<InstaStoryMedia>("Media story upload option cannot be null");
            }

            return await UploadStoryPhotoWithUrlAsync(progress,
                                                      image,
                                                      string.Empty,
                                                      null,
                                                      new InstaStoryUploadOptions { MediaStory = mediaStoryUpload }).ConfigureAwait(false);
        }

        /// <summary>
        ///     Share story to someone
        /// </summary>
        /// <param name="reelId">Reel id</param>
        /// <param name="storyMediaId">Story media id</param>
        /// <param name="threadId">Thread id</param>
        /// <param name="text">Text to send (optional</param>
        /// <param name="sharingType">Sharing type</param>
        public async Task<IResult<InstaSharing>> ShareStoryAsync(string reelId,
                                                                 string storyMediaId,
                                                                 string threadId,
                                                                 string text,
                                                                 InstaSharingType sharingType = InstaSharingType.Video)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetStoryShareUri(sharingType.ToString().ToLower());
                var data = new JObject
                {
                    { "action", "send_item" },
                    { "thread_ids", $"[{threadId}]" },
                    { "unified_broadcast_format", "1" },
                    { "reel_id", reelId },
                    { "text", text ?? "" },
                    { "story_media_id", storyMediaId },
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaSharing>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaSharing>(json);

                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaSharing), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaSharing>(exception);
            }
        }

        /// <summary>
        ///     UnFollow countdown stories
        /// </summary>
        /// <param name="countdownId">Countdown id (<see cref="InstaStoryCountdownStickerItem.CountdownId" />)</param>
        public async Task<IResult<bool>> UnFollowCountdownStoryAsync(long countdownId)
        {
            return await FollowUnfollowCountdown(InstaUriCreator.GetStoryUnFollowCountdownUri(countdownId)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story photo
        /// </summary>
        /// <param name="image">Photo to upload</param>
        /// <param name="caption">Caption</param>
        /// param name="uploadOptions">Upload options => Optional
        /// </param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryPhotoAsync(
            InstaImage image,
            string caption,
            InstaStoryUploadOptions uploadOptions = null)
        {
            return await UploadStoryPhotoAsync(null, image, caption, uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story photo with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="image">Photo to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryPhotoAsync(
            Action<InstaUploaderProgress> progress,
            InstaImage image,
            string caption,
            InstaStoryUploadOptions uploadOptions = null)
        {
            return await UploadStoryPhotoWithUrlAsync(progress, image, caption, null, uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story photo with adding link address
        ///     <para>Note: this function only works with verified account or you have more than 10k followers.</para>
        /// </summary>
        /// <param name="image">Photo to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="uri">Uri to add</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryPhotoWithUrlAsync(
            InstaImage image,
            string caption,
            Uri uri,
            InstaStoryUploadOptions uploadOptions = null)
        {
            return await UploadStoryPhotoWithUrlAsync(null, image, caption, uri, uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story photo with adding link address (with progress)
        ///     <para>Note: this function only works with verified account or you have more than 10k followers.</para>
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="image">Photo to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="uri">Uri to add</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryPhotoWithUrlAsync(
            Action<InstaUploaderProgress> progress,
            InstaImage image,
            string caption,
            Uri uri,
            InstaStoryUploadOptions uploadOptions = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var upProgress = new InstaUploaderProgress
            {
                Caption = caption ?? string.Empty, UploadState = InstaUploadState.Preparing
            };
            try
            {
                if (uploadOptions?.Mentions?.Count > 0)
                {
                    var currentDelay = instaApi.GetRequestDelay();
                    instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));
                    foreach (var tag in uploadOptions.Mentions)
                    {
                        var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                        if (instaUser != null)
                        {
                            tag.Pk = instaUser.Pk;
                        }
                    }

                    instaApi.SetRequestDelay(currentDelay);
                }

                if (uploadOptions?.Questions?.Count > 0)
                {
                    try
                    {
                        var profilePicture = user.LoggedInUser.ProfilePicture;

                        var instaUser = await instaApi.UserProcessor.GetUserSafe(user.UserName.ToLower(), logger).ConfigureAwait(false);
                        if (instaUser != null)
                        {
                            profilePicture = instaUser.ProfilePicture;
                        }
                       

                        foreach (var question in uploadOptions.Questions)
                        {
                            question.ProfilePicture = profilePicture;
                        }
                    }
                    catch
                    {
                    }
                }

                var uploadId = ApiRequestMessage.GenerateRandomUploadId();
                var photoHashCode = Path.GetFileName(image.Uri ?? $"C:\\{13.GenerateRandomString()}.jpg").GetHashCode();

                var waterfallId = Guid.NewGuid().ToString();

                var photoEntityName = $"{uploadId}_0_{photoHashCode}";
                var photoUri = InstaUriCreator.GetStoryUploadPhotoUri(uploadId, photoHashCode);

                upProgress.UploadId = uploadId;
                progress?.Invoke(upProgress);
                var videoMediaInfoData = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    {
                        "media_info",
                        new JObject
                        {
                            { "capture_mode", "normal" },
                            { "media_type", 1 },
                            { "caption", caption },
                            { "mentions", new JArray() },
                            { "hashtags", new JArray() },
                            { "locations", new JArray() },
                            { "stickers", new JArray() }
                        }
                    }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post,
                                                           InstaUriCreator.GetStoryMediaInfoUploadUri(),
                                                           deviceInfo,
                                                           videoMediaInfoData);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var photoUploadParamsObj = new JObject
                {
                    { "upload_id", uploadId },
                    { "media_type", "1" },
                    {
                        "retry_context",
                        "{\"num_step_auto_retry\":0,\"num_reupload\":0,\"num_step_manual_retry\":0}"
                    },
                    { "image_compression", "{\"lib_name\":\"moz\",\"lib_version\":\"3.1.m\",\"quality\":\"95\"}" }
                };
                var photoUploadParams = JsonConvert.SerializeObject(photoUploadParamsObj);
                request = httpHelper.GetDefaultRequest(HttpMethod.Get, photoUri, deviceInfo);
                request.Headers.Add("X_FB_PHOTO_WATERFALL_ID", waterfallId);
                request.Headers.Add("X-Instagram-Rupload-Params", photoUploadParams);
                response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
                }

                upProgress.UploadState = InstaUploadState.Uploading;
                progress?.Invoke(upProgress);
                var imageBytes = image.ImageBytes ?? File.ReadAllBytes(image.Uri);
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.Add("Content-Transfer-Encoding", "binary");
                imageContent.Headers.Add("Content-Type", "application/octet-stream");
                request = httpHelper.GetDefaultRequest(HttpMethod.Post, photoUri, deviceInfo);
                request.Content = imageContent;
                request.Headers.Add("X-Entity-Type", "image/jpeg");
                request.Headers.Add("Offset", "0");
                request.Headers.Add("X-Instagram-Rupload-Params", photoUploadParams);
                request.Headers.Add("X-Entity-Name", photoEntityName);
                request.Headers.Add("X-Entity-Length", imageBytes.Length.ToString());
                request.Headers.Add("X_FB_PHOTO_WATERFALL_ID", waterfallId);
                response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    upProgress.UploadState = InstaUploadState.Uploaded;
                    progress?.Invoke(upProgress);
                    await Task.Delay(5000).ConfigureAwait(false);
                    return await ConfigureStoryPhotoAsync(progress,
                                                          upProgress,
                                                          image,
                                                          uploadId,
                                                          caption,
                                                          uri,
                                                          uploadOptions).ConfigureAwait(false);
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryMedia>(exception);
            }
        }

        /// <summary>
        ///     Upload story video (to self story)
        /// </summary>
        /// <param name="video">Video to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryVideoAsync(
            InstaVideoUpload video,
            string caption,
            InstaStoryUploadOptions uploadOptions = null)
        {
            return await UploadStoryVideoAsync(null, video, caption, uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story video (to self story) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryVideoAsync(
            Action<InstaUploaderProgress> progress,
            InstaVideoUpload video,
            string caption,
            InstaStoryUploadOptions uploadOptions = null)
        {
            return await UploadStoryVideoWithUrlAsync(progress, video, caption, null, uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story video (to self story)
        /// </summary>
        /// <param name="video">Video to upload</param>
        public async Task<IResult<bool>> UploadStoryVideoAsync(
            InstaVideoUpload video,
            InstaStoryType storyType = InstaStoryType.SelfStory,
            InstaStoryUploadOptions uploadOptions = null,
            params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendVideoAsync(null,
                                                                 false,
                                                                 false,
                                                                 "",
                                                                 InstaViewMode.Replayable,
                                                                 storyType,
                                                                 null,
                                                                 threadIds.EncodeList(),
                                                                 video,
                                                                 null,
                                                                 uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story video (to self story) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<bool>> UploadStoryVideoAsync(
            Action<InstaUploaderProgress> progress,
            InstaVideoUpload video,
            InstaStoryType storyType = InstaStoryType.SelfStory,
            InstaStoryUploadOptions uploadOptions = null,
            params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendVideoAsync(progress,
                                                                 false,
                                                                 false,
                                                                 "",
                                                                 InstaViewMode.Replayable,
                                                                 storyType,
                                                                 null,
                                                                 threadIds.EncodeList(),
                                                                 video,
                                                                 null,
                                                                 uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story video (to self story) with adding link address
        ///     <para>Note: this function only works with verified account or you have more than 10k followers.</para>
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="uri">Uri to add</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryVideoWithUrlAsync(
            InstaVideoUpload video,
            string caption,
            Uri uri,
            InstaStoryUploadOptions uploadOptions = null)
        {
            return await UploadStoryVideoWithUrlAsync(null, video, caption, uri, uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story video (to self story) with adding link address (with progress)
        ///     <para>Note: this function only works with verified account or you have more than 10k followers.</para>
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="uri">Uri to add</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<InstaStoryMedia>> UploadStoryVideoWithUrlAsync(
            Action<InstaUploaderProgress> progress,
            InstaVideoUpload video,
            string caption,
            Uri uri,
            InstaStoryUploadOptions uploadOptions = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var upProgress = new InstaUploaderProgress
            {
                Caption = caption ?? string.Empty, UploadState = InstaUploadState.Preparing
            };
            try
            {
                var uploadId = ApiRequestMessage.GenerateRandomUploadId();
                var videoHashCode = Path.GetFileName(video.Video.Uri ?? $"C:\\{13.GenerateRandomString()}.mp4")
                    .GetHashCode();
                var photoHashCode = Path.GetFileName(video.VideoThumbnail.Uri ?? $"C:\\{13.GenerateRandomString()}.jpg")
                    .GetHashCode();

                var waterfallId = Guid.NewGuid().ToString();

                var videoEntityName = $"{uploadId}_0_{videoHashCode}";
                var videoUri = InstaUriCreator.GetStoryUploadVideoUri(uploadId, videoHashCode);

                var photoEntityName = $"{uploadId}_0_{photoHashCode}";
                var photoUri = InstaUriCreator.GetStoryUploadPhotoUri(uploadId, photoHashCode);

                upProgress.UploadId = uploadId;
                progress?.Invoke(upProgress);
                var videoMediaInfoData = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    {
                        "media_info",
                        new JObject
                        {
                            { "capture_mode", "normal" },
                            { "media_type", 2 },
                            { "caption", caption },
                            { "mentions", new JArray() },
                            { "hashtags", new JArray() },
                            { "locations", new JArray() },
                            { "stickers", new JArray() }
                        }
                    }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post,
                                                           InstaUriCreator.GetStoryMediaInfoUploadUri(),
                                                           deviceInfo,
                                                           videoMediaInfoData);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var videoUploadParamsObj = new JObject
                {
                    { "upload_media_height", "0" },
                    { "upload_media_width", "0" },
                    { "upload_media_duration_ms", "46000" },
                    { "upload_id", uploadId },
                    { "for_album", "1" },
                    {
                        "retry_context",
                        "{\"num_step_auto_retry\":0,\"num_reupload\":0,\"num_step_manual_retry\":0}"
                    },
                    { "media_type", "2" }
                };
                var videoUploadParams = JsonConvert.SerializeObject(videoUploadParamsObj);
                request = httpHelper.GetDefaultRequest(HttpMethod.Get, videoUri, deviceInfo);
                request.Headers.Add("X_FB_VIDEO_WATERFALL_ID", waterfallId);
                request.Headers.Add("X-Instagram-Rupload-Params", videoUploadParams);
                response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
                }

                var videoBytes = video.Video.VideoBytes ?? File.ReadAllBytes(video.Video.Uri);
                var videoContent = new ByteArrayContent(videoBytes);
                videoContent.Headers.Add("Content-Transfer-Encoding", "binary");
                videoContent.Headers.Add("Content-Type", "application/octet-stream");

                //var progressContent = new ProgressableStreamContent(videoContent, 4096, progress)
                //{
                //    UploaderProgress = upProgress
                //};
                request = httpHelper.GetDefaultRequest(HttpMethod.Post, videoUri, deviceInfo);
                request.Content = videoContent;
                upProgress.UploadState = InstaUploadState.Uploading;
                progress?.Invoke(upProgress);
                var vidExt = Path.GetExtension(video.Video.Uri ?? $"C:\\{13.GenerateRandomString()}.mp4")
                    .Replace(".", "")
                    .ToLower();
                if (vidExt == "mov")
                {
                    request.Headers.Add("X-Entity-Type", "image/quicktime");
                }
                else
                {
                    request.Headers.Add("X-Entity-Type", "image/mp4");
                }

                request.Headers.Add("Offset", "0");
                request.Headers.Add("X-Instagram-Rupload-Params", videoUploadParams);
                request.Headers.Add("X-Entity-Name", videoEntityName);
                request.Headers.Add("X-Entity-Length", videoBytes.Length.ToString());
                request.Headers.Add("X_FB_VIDEO_WATERFALL_ID", waterfallId);
                response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
                }

                upProgress.UploadState = InstaUploadState.Uploaded;
                progress?.Invoke(upProgress);
                var photoUploadParamsObj = new JObject
                {
                    {
                        "retry_context", "{\"num_step_auto_retry\":0,\"num_reupload\":0,\"num_step_manual_retry\":0}"
                    },
                    { "media_type", "2" },
                    { "upload_id", uploadId },
                    { "image_compression", "{\"lib_name\":\"moz\",\"lib_version\":\"3.1.m\",\"quality\":\"95\"}" }
                };
                var photoUploadParams = JsonConvert.SerializeObject(photoUploadParamsObj);
                request = httpHelper.GetDefaultRequest(HttpMethod.Get, photoUri, deviceInfo);
                request.Headers.Add("X_FB_PHOTO_WATERFALL_ID", waterfallId);
                request.Headers.Add("X-Instagram-Rupload-Params", photoUploadParams);
                response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
                }

                upProgress.UploadState = InstaUploadState.UploadingThumbnail;
                progress?.Invoke(upProgress);
                var imageBytes = video.VideoThumbnail.ImageBytes ?? File.ReadAllBytes(video.VideoThumbnail.Uri);
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.Add("Content-Transfer-Encoding", "binary");
                imageContent.Headers.Add("Content-Type", "application/octet-stream");
                request = httpHelper.GetDefaultRequest(HttpMethod.Post, photoUri, deviceInfo);
                request.Content = imageContent;
                request.Headers.Add("X-Entity-Type", "image/jpeg");
                request.Headers.Add("Offset", "0");
                request.Headers.Add("X-Instagram-Rupload-Params", photoUploadParams);
                request.Headers.Add("X-Entity-Name", photoEntityName);
                request.Headers.Add("X-Entity-Length", imageBytes.Length.ToString());
                request.Headers.Add("X_FB_PHOTO_WATERFALL_ID", waterfallId);
                response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    //upProgress = progressContent?.UploaderProgress;
                    upProgress.UploadState = InstaUploadState.ThumbnailUploaded;
                    progress?.Invoke(upProgress);
                    await Task.Delay(30000).ConfigureAwait(false);
                    return await ConfigureStoryVideoAsync(progress,
                                                          upProgress,
                                                          video,
                                                          uploadId,
                                                          caption,
                                                          uri,
                                                          uploadOptions).ConfigureAwait(false);
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryMedia>(exception);
            }
        }

        /// <summary>
        ///     Upload story video [to self story, to direct threads or both(self and direct)] with adding link address
        ///     <para>Note: this function only works with verified account or you have more than 10k followers.</para>
        /// </summary>
        /// <param name="video">Video to upload</param>
        /// <param name="uri">Uri to add</param>
        /// <param name="storyType">Story type</param>
        /// <param name="threadIds">Thread ids</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<bool>> UploadStoryVideoWithUrlAsync(
            InstaVideoUpload video,
            Uri uri,
            InstaStoryType storyType = InstaStoryType.SelfStory,
            InstaStoryUploadOptions uploadOptions = null,
            params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendVideoAsync(null,
                                                                 false,
                                                                 false,
                                                                 "",
                                                                 InstaViewMode.Replayable,
                                                                 storyType,
                                                                 null,
                                                                 threadIds.EncodeList(),
                                                                 video,
                                                                 uri,
                                                                 uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload story video (to self story) with adding link address (with progress)
        ///     <para>Note: this function only works with verified account or you have more than 10k followers.</para>
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload</param>
        /// <param name="storyType">Story type</param>
        /// <param name="threadIds">Thread ids</param>
        /// <param name="uploadOptions">Upload options => Optional</param>
        public async Task<IResult<bool>> UploadStoryVideoWithUrlAsync(
            Action<InstaUploaderProgress> progress,
            InstaVideoUpload video,
            Uri uri,
            InstaStoryType storyType = InstaStoryType.SelfStory,
            InstaStoryUploadOptions uploadOptions = null,
            params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendVideoAsync(progress,
                                                                 false,
                                                                 false,
                                                                 "",
                                                                 InstaViewMode.Replayable,
                                                                 storyType,
                                                                 null,
                                                                 threadIds.EncodeList(),
                                                                 video,
                                                                 uri,
                                                                 uploadOptions).ConfigureAwait(false);
        }

        /// <summary>
        ///     Vote to an story poll
        /// </summary>
        /// <param name="storyMediaId">Story media id</param>
        /// <param name="pollId">Story poll id</param>
        /// <param name="pollVote">Your poll vote</param>
        public async Task<IResult<InstaStoryItem>> VoteStoryPollAsync(string storyMediaId,
                                                                      string pollId,
                                                                      InstaStoryPollVoteType pollVote)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetStoryPollVoteUri(storyMediaId, pollId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "radio_type", "wifi-none" },
                    { "vote", ((int)pollVote).ToString() }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaStoryItem>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaReelStoryMediaViewersResponse>(json);
                var covertedObj = InstaConvertersFabric.Instance.GetReelStoryMediaViewersConverter(obj).Convert();

                return InstaResult.Success(covertedObj.UpdatedMedia);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryItem), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryItem>(exception);
            }
        }

        /// <summary>
        ///     Vote to an story slider
        ///     <para>Note: slider vote must be between 0 and 1</para>
        /// </summary>
        /// <param name="storyMediaId">Story media id</param>
        /// <param name="pollId">Story poll id</param>
        /// <param name="sliderVote">Your slider vote (from 0 to 1)</param>
        public async Task<IResult<InstaStoryItem>> VoteStorySliderAsync(
            string storyMediaId,
            string pollId,
            double sliderVote = 0.5)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (sliderVote > 1)
                {
                    return InstaResult.Fail<InstaStoryItem>(
                        "sliderVote cannot be more than 1.\r\nIt must be between 0 and 1");
                }

                if (sliderVote < 0)
                {
                    return InstaResult.Fail<InstaStoryItem>(
                        "sliderVote cannot be less than 0.\r\nIt must be between 0 and 1");
                }

                var instaUri = InstaUriCreator.GetVoteStorySliderUri(storyMediaId, pollId);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "vote", sliderVote.ToString() }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaStoryItem>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaReelStoryMediaViewersResponse>(json);
                var covertedObj = InstaConvertersFabric.Instance.GetReelStoryMediaViewersConverter(obj).Convert();

                return InstaResult.Success(covertedObj.UpdatedMedia);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryItem), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryItem>(exception);
            }
        }

        /// <summary>
        ///     Append to existing highlight
        /// </summary>
        /// <param name="highlightId">Highlight id</param>
        /// <param name="mediaId">Media id (CoverMedia.MediaId)</param>
        public async Task<IResult<bool>> AppendToHighlightFeedAsync(string highlightId, string mediaId)
        {
            return await AppendOrDeleteHighlight(highlightId, mediaId, false).ConfigureAwait(false);
        }

        public async Task<IResult<bool>> FollowUnfollowCountdown(Uri instaUri)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var resp = JsonConvert.DeserializeObject<InstaDefaultResponse>(json);

                return resp.IsSucceed ? InstaResult.Success(true) : InstaResult.Fail<bool>("");
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
        ///     Validate url for adding to story link
        /// </summary>
        /// <param name="url">Url address</param>
        public async Task<IResult<bool>> ValidateUrlAsync(string url)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return InstaResult.Fail("Url cannot be null or empty.", false);
                }

                var instaUri = InstaUriCreator.GetValidateReelLinkAddressUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "url", url }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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

        private async Task<IResult<bool>> AppendOrDeleteHighlight(string highlightId, string mediaId, bool delete)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new JObject
                {
                    { "source", "story_viewer" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                if (delete)
                {
                    data.Add("added_media_ids", "[]");
                    data.Add("removed_media_ids", $"[{new[] { mediaId }.EncodeList()}]");
                }
                else
                {
                    data.Add("added_media_ids", $"[{new[] { mediaId }.EncodeList()}]");
                    data.Add("removed_media_ids", "[]");
                }

                var instaUri = InstaUriCreator.GetHighlightEditUri(highlightId);
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Configure story photo
        /// </summary>
        /// <param name="image">Photo to configure</param>
        /// <param name="uploadId">Upload id</param>
        /// <param name="caption">Caption</param>
        /// <param name="uri">Uri to add</param>
        private async Task<IResult<InstaStoryMedia>> ConfigureStoryPhotoAsync(
            Action<InstaUploaderProgress> progress,
            InstaUploaderProgress upProgress,
            InstaImage image,
            string uploadId,
            string caption,
            Uri uri,
            InstaStoryUploadOptions uploadOptions = null)
        {
            try
            {
                upProgress.UploadState = InstaUploadState.Configuring;
                progress?.Invoke(upProgress);
                var instaUri = InstaUriCreator.GetVideoStoryConfigureUri(); // UriCreator.GetStoryConfigureUri();
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "source_type", "3" },
                    { "caption", caption },
                    { "upload_id", uploadId },
                    { "edits", new JObject() },
                    { "disable_comments", false },
                    { "configure_mode", 1 },
                    { "camera_position", "unknown" }
                };
                if (uri != null)
                {
                    var webUri = new JArray { new JObject { { "webUri", uri.ToString() } } };
                    var storyCta = new JArray { new JObject { { "links", webUri } } };
                    data.Add("story_cta", storyCta.ToString(Formatting.None));
                }

                if (uploadOptions != null)
                {
                    if (uploadOptions.Hashtags?.Count > 0)
                    {
                        var hashtagArr = new JArray();
                        foreach (var item in uploadOptions.Hashtags)
                        {
                            hashtagArr.Add(item.ConvertToJson());
                        }

                        data.Add("story_hashtags", hashtagArr.ToString(Formatting.None));
                    }

                    if (uploadOptions.Locations?.Count > 0)
                    {
                        var locationArr = new JArray();
                        foreach (var item in uploadOptions.Locations)
                        {
                            locationArr.Add(item.ConvertToJson());
                        }

                        data.Add("story_locations", locationArr.ToString(Formatting.None));
                    }

                    if (uploadOptions.Slider != null)
                    {
                        var sliderArr = new JArray { uploadOptions.Slider.ConvertToJson() };

                        data.Add("story_sliders", sliderArr.ToString(Formatting.None));
                        if (uploadOptions.Slider.IsSticker)
                        {
                            data.Add("story_sticker_ids", $"{uploadOptions.Slider.Emoji}");
                        }
                    }
                    else
                    {
                        if (uploadOptions.Polls?.Count > 0)
                        {
                            var pollArr = new JArray();
                            foreach (var item in uploadOptions.Polls)
                            {
                                pollArr.Add(item.ConvertToJson());
                            }

                            data.Add("story_polls", pollArr.ToString(Formatting.None));
                        }

                        if (uploadOptions.Questions?.Count > 0)
                        {
                            var questionArr = new JArray();
                            foreach (var item in uploadOptions.Questions)
                            {
                                questionArr.Add(item.ConvertToJson());
                            }

                            data.Add("story_questions", questionArr.ToString(Formatting.None));
                        }
                    }

                    if (uploadOptions.MediaStory != null)
                    {
                        var mediaStory = new JArray { uploadOptions.MediaStory.ConvertToJson() };

                        data.Add("attached_media", mediaStory.ToString(Formatting.None));
                    }

                    if (uploadOptions.Mentions?.Count > 0)
                    {
                        var mentionArr = new JArray();
                        foreach (var item in uploadOptions.Mentions)
                        {
                            mentionArr.Add(item.ConvertToJson());
                        }

                        data.Add("reel_mentions", mentionArr.ToString(Formatting.None));
                    }

                    if (uploadOptions.Countdown != null)
                    {
                        var countdownArr = new JArray { uploadOptions.Countdown.ConvertToJson() };

                        data.Add("story_countdowns", countdownArr.ToString(Formatting.None));
                        data.Add("story_sticker_ids", "countdown_sticker_time");
                    }
                }

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var mediaResponse = JsonConvert.DeserializeObject<InstaStoryMediaResponse>(json);
                    var converter = InstaConvertersFabric.Instance.GetStoryMediaConverter(mediaResponse);
                    var obj = converter.Convert();
                    upProgress.UploadState = InstaUploadState.Configured;
                    progress?.Invoke(upProgress);

                    upProgress.UploadState = InstaUploadState.Completed;
                    progress?.Invoke(upProgress);
                    return InstaResult.Success(obj);
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryMedia>(exception);
            }
        }

        /// <summary>
        ///     Configure story video
        /// </summary>
        /// <param name="video">Video to configure</param>
        /// <param name="uploadId">Upload id</param>
        /// <param name="caption">Caption</param>
        /// <param name="uri">Uri to add</param>
        private async Task<IResult<InstaStoryMedia>> ConfigureStoryVideoAsync(
            Action<InstaUploaderProgress> progress,
            InstaUploaderProgress upProgress,
            InstaVideoUpload video,
            string uploadId,
            string caption,
            Uri uri,
            InstaStoryUploadOptions uploadOptions = null)
        {
            try
            {
                upProgress.UploadState = InstaUploadState.Configuring;
                progress?.Invoke(upProgress);
                var instaUri = InstaUriCreator.GetVideoStoryConfigureUri();
                var rnd = new Random();
                var data = new JObject
                {
                    { "filter_type", "0" },
                    { "timezone_offset", "16200" },
                    { "_csrftoken", user.CsrfToken },
                    {
                        "client_shared_at",
                        (long.Parse(ApiRequestMessage.GenerateUploadId()) - rnd.Next(25, 55)).ToString()
                    },
                    {
                        "story_media_creation_date",
                        (long.Parse(ApiRequestMessage.GenerateUploadId()) - rnd.Next(50, 70)).ToString()
                    },
                    { "media_folder", "Camera" },
                    { "configure_mode", "1" },
                    { "source_type", "4" },
                    { "video_result", "" },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "caption", caption },
                    { "date_time_original", DateTime.Now.ToString("yyyy-dd-MMTh:mm:ss-0fffZ") },
                    { "capture_type", "normal" },
                    { "mas_opt_in", "NOT_PROMPTED" },
                    { "upload_id", uploadId },
                    { "client_timestamp", ApiRequestMessage.GenerateUploadId() },
                    {
                        "device",
                        new JObject
                        {
                            { "manufacturer", deviceInfo.HardwareManufacturer },
                            { "model", deviceInfo.DeviceModelIdentifier },
                            { "android_release", deviceInfo.AndroidVer.VersionNumber },
                            { "android_version", deviceInfo.AndroidVer.ApiLevel }
                        }
                    },
                    { "length", 0 },
                    { "extra", new JObject { { "source_width", 0 }, { "source_height", 0 } } },
                    { "audio_muted", false },
                    { "poster_frame_index", 0 }
                };
                if (uri != null)
                {
                    var webUri = new JArray { new JObject { { "webUri", uri.ToString() } } };
                    var storyCta = new JArray { new JObject { { "links", webUri } } };
                    data.Add("story_cta", storyCta.ToString(Formatting.None));
                }

                if (uploadOptions != null)
                {
                    if (uploadOptions.Hashtags?.Count > 0)
                    {
                        var hashtagArr = new JArray();
                        foreach (var item in uploadOptions.Hashtags)
                        {
                            hashtagArr.Add(item.ConvertToJson());
                        }

                        data.Add("story_hashtags", hashtagArr.ToString(Formatting.None));
                    }

                    if (uploadOptions.Locations?.Count > 0)
                    {
                        var locationArr = new JArray();
                        foreach (var item in uploadOptions.Locations)
                        {
                            locationArr.Add(item.ConvertToJson());
                        }

                        data.Add("story_locations", locationArr.ToString(Formatting.None));
                    }

                    if (uploadOptions.Slider != null)
                    {
                        var sliderArr = new JArray { uploadOptions.Slider.ConvertToJson() };

                        data.Add("story_sliders", sliderArr.ToString(Formatting.None));
                        if (uploadOptions.Slider.IsSticker)
                        {
                            data.Add("story_sticker_ids", $"emoji_slider_{uploadOptions.Slider.Emoji}");
                        }
                    }
                    else
                    {
                        if (uploadOptions.Polls?.Count > 0)
                        {
                            var pollArr = new JArray();
                            foreach (var item in uploadOptions.Polls)
                            {
                                pollArr.Add(item.ConvertToJson());
                            }

                            data.Add("story_polls", pollArr.ToString(Formatting.None));
                        }

                        if (uploadOptions.Questions?.Count > 0)
                        {
                            var questionArr = new JArray();
                            foreach (var item in uploadOptions.Questions)
                            {
                                questionArr.Add(item.ConvertToJson());
                            }

                            data.Add("story_questions", questionArr.ToString(Formatting.None));
                        }
                    }

                    if (uploadOptions.Countdown != null)
                    {
                        var countdownArr = new JArray { uploadOptions.Countdown.ConvertToJson() };

                        data.Add("story_countdowns", countdownArr.ToString(Formatting.None));
                        data.Add("story_sticker_ids", "countdown_sticker_time");
                    }
                }

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var uploadParamsObj = new JObject
                {
                    { "num_step_auto_retry", 0 }, { "num_reupload", 0 }, { "num_step_manual_retry", 0 }
                };
                var uploadParams = JsonConvert.SerializeObject(uploadParamsObj);
                request.Headers.Add("retry_context", uploadParams);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var mediaResponse = JsonConvert.DeserializeObject<InstaStoryMediaResponse>(json);
                    var converter = InstaConvertersFabric.Instance.GetStoryMediaConverter(mediaResponse);
                    var obj = InstaResult.Success(converter.Convert());
                    upProgress.UploadState = InstaUploadState.Configured;
                    progress?.Invoke(upProgress);
                    upProgress.UploadState = InstaUploadState.Completed;
                    progress?.Invoke(upProgress);
                    return obj;
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryMedia>(exception);
            }
        }

        private async Task<IResult<InstaReelStoryMediaViewersResponse>> GetStoryMediaViewers(
            string storyMediaId,
            string maxId)
        {
            try
            {
                var directInboxUri = InstaUriCreator.GetStoryMediaViewersUri(storyMediaId, maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, directInboxUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaReelStoryMediaViewersResponse>(response, json);
                }

                var storyMediaViewersResponse = JsonConvert.DeserializeObject<InstaReelStoryMediaViewersResponse>(json);

                return InstaResult.Success(storyMediaViewersResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaReelStoryMediaViewersResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaReelStoryMediaViewersResponse>(exception);
            }
        }

        private async Task<IResult<InstaStoryPollVotersListResponse>> GetStoryPollVoters(
            string storyMediaId,
            string pollId,
            string maxId)
        {
            try
            {
                var directInboxUri = InstaUriCreator.GetStoryPollVotersUri(storyMediaId, pollId, maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, directInboxUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaStoryPollVotersListResponse>(response, json);
                }

                var storyVotersResponse =
                    JsonConvert.DeserializeObject<InstaStoryPollVotersListContainerResponse>(json);

                return InstaResult.Success(storyVotersResponse.VoterInfo);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaStoryPollVotersListResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryPollVotersListResponse>(exception);
            }
        }

        private async Task<IResult<InstaStoryMedia>> UploadStoryPhotoWithUrlAsyncOld(
            Action<InstaUploaderProgress> progress,
            InstaImage image,
            string caption,
            Uri uri,
            InstaStoryUploadOptions uploadOptions = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var upProgress = new InstaUploaderProgress
            {
                Caption = caption ?? string.Empty, UploadState = InstaUploadState.Preparing
            };
            try
            {
                if (uploadOptions?.Mentions?.Count > 0)
                {
                    var currentDelay = instaApi.GetRequestDelay();
                    instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));
                    foreach (var tag in uploadOptions.Mentions)
                    {
                        var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                        if (instaUser != null)
                        {
                            tag.Pk = instaUser.Pk;
                        }
                    }

                    instaApi.SetRequestDelay(currentDelay);
                }

                var instaUri = InstaUriCreator.GetUploadPhotoUri();
                var uploadId = ApiRequestMessage.GenerateUploadId();
                upProgress.UploadId = uploadId;
                progress?.Invoke(upProgress);
                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    { new StringContent(uploadId), "\"upload_id\"" },

                    //{new StringContent(_deviceInfo.DeviceGuid.ToString()), "\"_uuid\""},
                    //{new StringContent(_user.CsrfToken), "\"_csrftoken\""},
                    {
                        new StringContent("{\"lib_name\":\"jt\",\"lib_version\":\"1.3.0\",\"quality\":\"87\"}"),
                        "\"image_compression\""
                    }
                };
                byte[] imageBytes;
                if (image.ImageBytes == null)
                {
                    imageBytes = File.ReadAllBytes(image.Uri);
                }
                else
                {
                    imageBytes = image.ImageBytes;
                }

                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.Add("Content-Transfer-Encoding", "binary");
                imageContent.Headers.Add("Content-Type", "application/octet-stream");

                //var progressContent = new ProgressableStreamContent(imageContent, 4096, progress)
                //{
                //    UploaderProgress = upProgress
                //};
                upProgress.UploadState = InstaUploadState.Uploading;
                progress?.Invoke(upProgress);
                requestContent.Add(imageContent, "photo", $"pending_media_{ApiRequestMessage.GenerateUploadId()}.jpg");
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = requestContent;
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    upProgress.UploadState = InstaUploadState.Uploaded;
                    progress?.Invoke(upProgress);

                    //upProgress = progressContent?.UploaderProgress;
                    return await ConfigureStoryPhotoAsync(progress,
                                                          upProgress,
                                                          image,
                                                          uploadId,
                                                          caption,
                                                          uri,
                                                          uploadOptions).ConfigureAwait(false);
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<InstaStoryMedia>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStoryMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStoryMedia>(exception);
            }
        }
    }
}