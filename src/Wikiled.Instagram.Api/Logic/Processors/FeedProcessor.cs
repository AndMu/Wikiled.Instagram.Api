using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Feed;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Feed api functions.
    /// </summary>
    internal class InstaFeedProcessor : IFeedProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly InstaUserAuthValidate userAuthValidate;

        public InstaFeedProcessor(
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
        ///     Get user explore feed (Explore tab info) asynchronously
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaExploreFeed" />
        /// </returns>
        public async Task<IResult<InstaExploreFeed>> GetExploreFeedAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var exploreFeed = new InstaExploreFeed();
            try
            {
                InstaExploreFeed Convert(InstaExploreFeedResponse exploreFeedResponse)
                {
                    return InstaConvertersFabric.Instance.GetExploreFeedConverter(exploreFeedResponse).Convert();
                }

                var feeds = await GetExploreFeed(paginationParameters).ConfigureAwait(false);
                if (!feeds.Succeeded)
                {
                    if (feeds.Value != null)
                    {
                        return InstaResult.Fail(feeds.Info, Convert(feeds.Value));
                    }

                    return InstaResult.Fail(feeds.Info, (InstaExploreFeed)null);
                }

                var feedResponse = feeds.Value;
                paginationParameters.NextMaxId = feedResponse.MaxId;
                paginationParameters.RankToken = feedResponse.RankToken;

                while (feedResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextFeed = await GetExploreFeed(paginationParameters).ConfigureAwait(false);
                    if (!nextFeed.Succeeded)
                    {
                        return InstaResult.Fail(nextFeed.Info, Convert(feeds.Value));
                    }

                    feedResponse.NextMaxId = paginationParameters.NextMaxId = nextFeed.Value.MaxId;
                    feedResponse.RankToken = paginationParameters.RankToken = nextFeed.Value.RankToken;
                    feedResponse.MoreAvailable = nextFeed.Value.MoreAvailable;
                    feedResponse.AutoLoadMoreEnabled = nextFeed.Value.AutoLoadMoreEnabled;
                    feedResponse.NextMaxId = nextFeed.Value.NextMaxId;
                    feedResponse.ResultsCount = nextFeed.Value.ResultsCount;
                    feedResponse.Items.Channel = nextFeed.Value.Items.Channel;
                    feedResponse.Items.Medias.AddRange(nextFeed.Value.Items.Medias);
                    if (nextFeed.Value.Items.StoryTray == null)
                    {
                        feedResponse.Items.StoryTray = nextFeed.Value.Items.StoryTray;
                    }
                    else
                    {
                        feedResponse.Items.StoryTray.Id = nextFeed.Value.Items.StoryTray.Id;
                        feedResponse.Items.StoryTray.IsPortrait = nextFeed.Value.Items.StoryTray.IsPortrait;

                        feedResponse.Items.StoryTray.Tray.AddRange(nextFeed.Value.Items.StoryTray.Tray);
                        if (nextFeed.Value.Items.StoryTray.TopLive == null)
                        {
                            feedResponse.Items.StoryTray.TopLive = nextFeed.Value.Items.StoryTray.TopLive;
                        }
                        else
                        {
                            feedResponse.Items.StoryTray.TopLive.RankedPosition =
                                nextFeed.Value.Items.StoryTray.TopLive.RankedPosition;
                            feedResponse.Items.StoryTray.TopLive.BroadcastOwners.AddRange(
                                nextFeed.Value.Items.StoryTray.TopLive.BroadcastOwners);
                        }
                    }

                    paginationParameters.PagesLoaded++;
                }

                exploreFeed = Convert(feedResponse);
                exploreFeed.Medias.Pages = paginationParameters.PagesLoaded;
                exploreFeed.Medias.PageSize = feedResponse.ResultsCount;
                return InstaResult.Success(exploreFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaExploreFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, exploreFeed);
            }
        }

        /// <summary>
        ///     Get activity of following asynchronously
        /// </summary>
        /// <param name="paginationParameters"></param>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.InstaActivityFeed" />
        /// </returns>
        public Task<IResult<InstaActivityFeed>> GetFollowingRecentActivityFeedAsync(
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var uri = InstaUriCreator.GetFollowingRecentActivityUri();
            return GetRecentActivityInternalAsync(uri, paginationParameters);
        }

        /// <summary>
        ///     Get feed of media your liked.
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public async Task<IResult<InstaMediaList>> GetLikedFeedAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaMediaList Convert(InstaMediaListResponse mediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetMediaListConverter(mediaListResponse).Convert();
                }

                var mediaResult = await GetAnyFeeds(InstaUriCreator.GetUserLikeFeedUri(paginationParameters.NextMaxId)).ConfigureAwait(false);
                if (!mediaResult.Succeeded)
                {
                    if (mediaResult.Value != null)
                    {
                        return InstaResult.Fail(mediaResult.Info, Convert(mediaResult.Value));
                    }

                    return InstaResult.Fail(mediaResult.Info, default(InstaMediaList));
                }

                var mediaResponse = mediaResult.Value;
                var mediaList = Convert(mediaResponse);
                mediaList.NextMaxId = paginationParameters.NextMaxId = mediaResponse.NextMaxId;
                paginationParameters.PagesLoaded++;
                while (mediaResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var result = await GetAnyFeeds(InstaUriCreator.GetUserLikeFeedUri(paginationParameters.NextMaxId)).ConfigureAwait(false);
                    if (!result.Succeeded)
                    {
                        return InstaResult.Fail(result.Info, mediaList);
                    }

                    var convertedResult = Convert(result.Value);
                    paginationParameters.PagesLoaded++;
                    mediaList.NextMaxId = paginationParameters.NextMaxId = result.Value.NextMaxId;
                    mediaResponse.MoreAvailable = result.Value.MoreAvailable;
                    mediaResponse.ResultsCount += result.Value.ResultsCount;
                    mediaResponse.TotalCount += result.Value.TotalCount;
                    mediaList.AddRange(convertedResult);
                }

                mediaList.PageSize = mediaResponse.ResultsCount;
                mediaList.Pages = paginationParameters.PagesLoaded;
                return InstaResult.Success(mediaList);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMediaList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMediaList>(exception);
            }
        }

        /// <summary>
        ///     Get recent activity info asynchronously
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.InstaActivityFeed" />
        /// </returns>
        public Task<IResult<InstaActivityFeed>> GetRecentActivityFeedAsync(
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var uri = InstaUriCreator.GetRecentActivityUri();
            return GetRecentActivityInternalAsync(uri, paginationParameters);
        }

        /// <summary>
        ///     Get saved media feeds asynchronously
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public async Task<IResult<InstaMediaList>> GetSavedFeedAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaMediaList Convert(InstaMediaListResponse mediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetMediaListConverter(mediaListResponse).Convert();
                }

                var mediaFeedsResult = await GetAnyFeeds(InstaUriCreator.GetSavedFeedUri(paginationParameters?.NextMaxId)).ConfigureAwait(false);
                if (!mediaFeedsResult.Succeeded)
                {
                    if (mediaFeedsResult.Value != null)
                    {
                        return InstaResult.Fail(mediaFeedsResult.Info, Convert(mediaFeedsResult.Value));
                    }

                    return InstaResult.Fail(mediaFeedsResult.Info, default(InstaMediaList));
                }

                var mediaResponse = mediaFeedsResult.Value;
                paginationParameters.NextMaxId = mediaResponse.NextMaxId;
                paginationParameters.PagesLoaded++;
                while (mediaResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var result = await GetAnyFeeds(InstaUriCreator.GetSavedFeedUri(paginationParameters?.NextMaxId)).ConfigureAwait(false);
                    if (!result.Succeeded)
                    {
                        return InstaResult.Fail(result.Info, Convert(mediaResponse));
                    }

                    mediaResponse.NextMaxId = paginationParameters.NextMaxId = result.Value.NextMaxId;
                    mediaResponse.MoreAvailable = result.Value.MoreAvailable;
                    mediaResponse.AutoLoadMoreEnabled = result.Value.AutoLoadMoreEnabled;
                    mediaResponse.ResultsCount += result.Value.ResultsCount;
                    mediaResponse.TotalCount += result.Value.TotalCount;
                    mediaResponse.Medias.AddRange(result.Value.Medias);
                    paginationParameters.PagesLoaded++;
                }

                var mediaList = Convert(mediaResponse);
                mediaList.PageSize = mediaResponse.ResultsCount;
                mediaList.Pages = paginationParameters.PagesLoaded;
                return InstaResult.Success(mediaList);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMediaList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMediaList>(exception);
            }
        }

        /// <summary>
        ///     Get tag feed by tag value asynchronously
        /// </summary>
        /// <param name="tag">Tag value</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaTagFeed" />
        /// </returns>
        public async Task<IResult<InstaTagFeed>> GetTagFeedAsync(string tag, PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var tagFeed = new InstaTagFeed();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaTagFeed Convert(InstaTagFeedResponse instaTagFeedResponse)
                {
                    return InstaConvertersFabric.Instance.GetTagFeedConverter(instaTagFeedResponse).Convert();
                }

                var tags = await GetTagFeed(tag, paginationParameters).ConfigureAwait(false);
                if (!tags.Succeeded)
                {
                    if (tags.Value != null)
                    {
                        return InstaResult.Fail(tags.Info, Convert(tags.Value));
                    }

                    return InstaResult.Fail(tags.Info, default(InstaTagFeed));
                }

                var feedResponse = tags.Value;

                tagFeed = Convert(feedResponse);

                paginationParameters.NextMaxId = feedResponse.NextMaxId;
                paginationParameters.PagesLoaded++;

                while (feedResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextFeed = await GetTagFeed(tag, paginationParameters).ConfigureAwait(false);
                    if (!nextFeed.Succeeded)
                    {
                        return InstaResult.Fail(nextFeed.Info, tagFeed);
                    }

                    var convertedFeeds = Convert(nextFeed.Value);
                    tagFeed.NextMaxId = paginationParameters.NextMaxId = nextFeed.Value.NextMaxId;
                    tagFeed.Medias.AddRange(convertedFeeds.Medias);
                    tagFeed.Stories.AddRange(convertedFeeds.Stories);
                    feedResponse.MoreAvailable = nextFeed.Value.MoreAvailable;
                    paginationParameters.PagesLoaded++;
                }

                return InstaResult.Success(tagFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTagFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, tagFeed);
            }
        }

        /// <summary>
        ///     Get user topical explore feeds asynchronously
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <param name="clusterId">Cluster id</param>
        /// <returns>
        ///     <see cref="InstaTopicalExploreFeed" />
        /// </returns>
        public async Task<IResult<InstaTopicalExploreFeed>> GetTopicalExploreFeedAsync(
            PaginationParameters paginationParameters,
            string clusterId = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var topicalExploreFeed = new InstaTopicalExploreFeed();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaTopicalExploreFeed Convert(InstaTopicalExploreFeedResponse topicalExploreFeedResponse)
                {
                    return InstaConvertersFabric.Instance.GetTopicalExploreFeedConverter(topicalExploreFeedResponse)
                        .Convert();
                }

                var feeds = await GetTopicalExploreFeed(paginationParameters, clusterId).ConfigureAwait(false);
                if (!feeds.Succeeded)
                {
                    if (feeds.Value != null)
                    {
                        return InstaResult.Fail(feeds.Info, Convert(feeds.Value));
                    }

                    return InstaResult.Fail(feeds.Info, (InstaTopicalExploreFeed)null);
                }

                var feedResponse = feeds.Value;
                paginationParameters.NextMaxId = feedResponse.MaxId;
                paginationParameters.RankToken = feedResponse.RankToken;

                while (feedResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextFeed = await GetTopicalExploreFeed(paginationParameters, clusterId).ConfigureAwait(false);
                    if (!nextFeed.Succeeded)
                    {
                        return InstaResult.Fail(nextFeed.Info, Convert(feeds.Value));
                    }

                    feedResponse.NextMaxId = paginationParameters.NextMaxId = nextFeed.Value.MaxId;
                    feedResponse.RankToken = paginationParameters.RankToken = nextFeed.Value.RankToken;
                    feedResponse.MoreAvailable = nextFeed.Value.MoreAvailable;
                    feedResponse.AutoLoadMoreEnabled = nextFeed.Value.AutoLoadMoreEnabled;
                    feedResponse.NextMaxId = nextFeed.Value.NextMaxId;
                    feedResponse.ResultsCount = nextFeed.Value.ResultsCount;
                    feedResponse.Channel = nextFeed.Value.Channel;
                    feedResponse.Medias.AddRange(nextFeed.Value.Medias);
                    feedResponse.TvChannels.AddRange(nextFeed.Value.TvChannels);
                    feedResponse.Clusters.AddRange(nextFeed.Value.Clusters);
                    feedResponse.MaxId = nextFeed.Value.MaxId;
                    feedResponse.HasShoppingChannelContent = nextFeed.Value.HasShoppingChannelContent;
                    paginationParameters.PagesLoaded++;
                }

                topicalExploreFeed = Convert(feedResponse);
                topicalExploreFeed.Medias.Pages = paginationParameters.PagesLoaded;
                topicalExploreFeed.Medias.PageSize = feedResponse.ResultsCount;
                return InstaResult.Success(topicalExploreFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTopicalExploreFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, topicalExploreFeed);
            }
        }

        /// <summary>
        ///     Get user timeline feed (feed of recent posts from users you follow) asynchronously.
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <param name="seenMediaIds">Id of the posts seen till now</param>
        /// <param name="refreshRequest">Request refresh feeds</param>
        /// <returns>
        ///     <see cref="InstaFeed" />
        /// </returns>
        public async Task<IResult<InstaFeed>> GetUserTimelineFeedAsync(PaginationParameters paginationParameters,
                                                                       string[] seenMediaIds = null,
                                                                       bool refreshRequest = false)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var feed = new InstaFeed();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaFeed Convert(InstaFeedResponse instaFeedResponse)
                {
                    return InstaConvertersFabric.Instance.GetFeedConverter(instaFeedResponse).Convert();
                }

                var timelineFeeds = await GetUserTimelineFeed(paginationParameters, seenMediaIds, refreshRequest).ConfigureAwait(false);
                if (!timelineFeeds.Succeeded)
                {
                    return InstaResult.Fail(timelineFeeds.Info, feed);
                }

                var feedResponse = timelineFeeds.Value;

                feed = Convert(feedResponse);
                paginationParameters.NextMaxId = feed.NextMaxId;
                paginationParameters.PagesLoaded++;

                while (feedResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextFeed = await GetUserTimelineFeed(paginationParameters).ConfigureAwait(false);
                    if (!nextFeed.Succeeded)
                    {
                        return InstaResult.Fail(nextFeed.Info, feed);
                    }

                    var convertedFeed = Convert(nextFeed.Value);
                    feed.Medias.AddRange(convertedFeed.Medias);
                    feed.Stories.AddRange(convertedFeed.Stories);
                    feedResponse.MoreAvailable = nextFeed.Value.MoreAvailable;
                    paginationParameters.NextMaxId = nextFeed.Value.NextMaxId;
                    paginationParameters.PagesLoaded++;
                }

                return InstaResult.Success(feed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, feed);
            }
        }

        private async Task<IResult<InstaMediaListResponse>> GetAnyFeeds(Uri instaUri)
        {
            try
            {
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaMediaListResponse>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaListResponse>(
                    json,
                    new InstaMediaListDataConverter());

                return InstaResult.Success(mediaResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMediaListResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMediaListResponse>(exception);
            }
        }

        private async Task<IResult<InstaExploreFeedResponse>> GetExploreFeed(PaginationParameters paginationParameters)
        {
            try
            {
                var exploreUri =
                    InstaUriCreator.GetExploreUri(paginationParameters.NextMaxId, paginationParameters.RankToken);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, exploreUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaExploreFeedResponse>(response, json);
                }

                var feedResponse = JsonConvert.DeserializeObject<InstaExploreFeedResponse>(
                    json,
                    new InstaExploreFeedDataConverter());
                return InstaResult.Success(feedResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaExploreFeedResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaExploreFeedResponse>(exception);
            }
        }

        private async Task<IResult<InstaRecentActivityResponse>> GetFollowingActivityWithMaxIdAsync(string maxId)
        {
            try
            {
                var uri = InstaUriCreator.GetFollowingRecentActivityUri(maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaRecentActivityResponse>(response, json);
                }

                var followingActivity = JsonConvert.DeserializeObject<InstaRecentActivityResponse>(
                    json,
                    new InstaRecentActivityConverter());
                return InstaResult.Success(followingActivity);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaRecentActivityResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaRecentActivityResponse>(exception);
            }
        }

        private async Task<IResult<InstaActivityFeed>> GetRecentActivityInternalAsync(
            Uri uri,
            PaginationParameters paginationParameters)
        {
            try
            {
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
                var activityFeed = new InstaActivityFeed();
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaActivityFeed>(response, json);
                }

                var feedPage = JsonConvert.DeserializeObject<InstaRecentActivityResponse>(
                    json,
                    new InstaRecentActivityConverter());
                activityFeed.IsOwnActivity = feedPage.IsOwnActivity;
                var nextId = feedPage.NextMaxId;
                activityFeed.Items.AddRange(
                    feedPage.Stories.Select(InstaConvertersFabric.Instance.GetSingleRecentActivityConverter)
                        .Select(converter => converter.Convert()));
                paginationParameters.PagesLoaded++;
                activityFeed.NextMaxId = paginationParameters.NextMaxId = feedPage.NextMaxId;
                while (!string.IsNullOrEmpty(nextId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextFollowingFeed = await GetFollowingActivityWithMaxIdAsync(nextId).ConfigureAwait(false);
                    if (!nextFollowingFeed.Succeeded)
                    {
                        return InstaResult.Fail(nextFollowingFeed.Info, activityFeed);
                    }

                    nextId = nextFollowingFeed.Value.NextMaxId;
                    activityFeed.Items.AddRange(
                        feedPage.Stories.Select(InstaConvertersFabric.Instance.GetSingleRecentActivityConverter)
                            .Select(converter => converter.Convert()));
                    paginationParameters.PagesLoaded++;
                    activityFeed.NextMaxId = paginationParameters.NextMaxId = nextId;
                }

                return InstaResult.Success(activityFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaActivityFeed), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaActivityFeed>(exception);
            }
        }

        private async Task<IResult<InstaTagFeedResponse>> GetTagFeed(string tag,
                                                                     PaginationParameters paginationParameters)
        {
            try
            {
                var userFeedUri = InstaUriCreator.GetTagFeedUri(tag, paginationParameters?.NextMaxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userFeedUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaTagFeedResponse>(response, json);
                }

                var feedResponse = JsonConvert.DeserializeObject<InstaTagFeedResponse>(
                    json,
                    new InstaTagFeedDataConverter());
                return InstaResult.Success(feedResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTagFeedResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, default(InstaTagFeedResponse));
            }
        }

        private async Task<IResult<InstaTopicalExploreFeedResponse>> GetTopicalExploreFeed(
            PaginationParameters paginationParameters,
            string clusterId)
        {
            try
            {
                if (string.IsNullOrEmpty(clusterId))
                {
                    clusterId = "explore_all:0";
                }

                var exploreUri =
                    InstaUriCreator.GetTopicalExploreUri(deviceInfo.GoogleAdId.ToString(),
                                                    paginationParameters?.NextMaxId,
                                                    clusterId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, exploreUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaTopicalExploreFeedResponse>(response, json);
                }

                var feedResponse = JsonConvert.DeserializeObject<InstaTopicalExploreFeedResponse>(
                    json,
                    new InstaTopicalExploreFeedDataConverter());

                return InstaResult.Success(feedResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaTopicalExploreFeedResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaTopicalExploreFeedResponse>(exception);
            }
        }

        private async Task<IResult<InstaFeedResponse>> GetUserTimelineFeed(
            PaginationParameters paginationParameters,
            string[] seenMediaIds = null,
            bool refreshRequest = false)
        {
            try
            {
                var userFeedUri = InstaUriCreator.GetUserFeedUri(paginationParameters?.NextMaxId);

                var data = new Dictionary<string, string>
                {
                    { "is_prefetch", "0" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.PhoneGuid.ToString() },
                    { "phone_id", deviceInfo.RankToken.ToString() },
                    { "client_session_id", Guid.NewGuid().ToString() },
                    { "timezone_offset", instaApi.GetTimezoneOffset().ToString() },
                    { "rti_delivery_backend", "0" }
                };

                if (seenMediaIds != null)
                {
                    data.Add("seen_posts", seenMediaIds.EncodeList(false));
                }

                if (refreshRequest)
                {
                    data.Add("reason", "pull_to_refresh");
                    data.Add("is_pull_to_refresh", "1");
                }
                else
                {
                    data.Add("reason", "warm_start_fetch");
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, userFeedUri, deviceInfo, data);
                request.Headers.Add("X-Ads-Opt-Out", "0");
                request.Headers.Add("X-Google-AD-ID", deviceInfo.GoogleAdId.ToString());
                request.Headers.Add("X-DEVICE-ID", deviceInfo.DeviceGuid.ToString());

                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaFeedResponse>(response, json);
                }

                var feedResponse = JsonConvert.DeserializeObject<InstaFeedResponse>(
                    json,
                    new InstaFeedResponseDataConverter());
                return InstaResult.Success(feedResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaFeedResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, default(InstaFeedResponse));
            }
        }
    }
}