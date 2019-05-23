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
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Hashtag api functions.
    /// </summary>
    internal class InstaHashtagProcessor : IHashtagProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaHashtagProcessor(
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
        ///     Follow a hashtag
        /// </summary>
        /// <param name="tagName">Tag name</param>
        public async Task<IResult<bool>> FollowHashtagAsync(string tagName)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetFollowHashtagUri(tagName);

                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                return obj.Status.ToLower() == "ok"
                    ? Result.Success(true)
                    : Result.UnExpectedResponse<bool>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<bool>(exception);
            }
        }

        /// <summary>
        ///     Get following hashtags information
        /// </summary>
        /// <param name="userId">User identifier (pk)</param>
        /// <returns>
        ///     List of hashtags
        /// </returns>
        public async Task<IResult<HashtagSearch>> GetFollowingHashtagsInfoAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var tags = new HashtagSearch();
            try
            {
                var userUri = InstaUriCreator.GetFollowingTagsInfoUri(userId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<HashtagSearch>(response, json);
                }

                var tagsResponse = JsonConvert.DeserializeObject<HashtagSearchResponse>(
                    json,
                    new InstaHashtagSuggestedDataConverter());

                tags = InstaConvertersFabric.Instance.GetHashTagsSearchConverter(tagsResponse).Convert();
                return Result.Success(tags);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(HashtagSearch), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, tags);
            }
        }

        /// <summary>
        ///     Gets the hashtag information by user tagName.
        /// </summary>
        /// <param name="tagName">Tagname</param>
        /// <returns>Hashtag information</returns>
        public async Task<IResult<ApiHashtag>> GetHashtagInfoAsync(string tagName)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userUri = InstaUriCreator.GetTagInfoUri(tagName);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<ApiHashtag>(response, json);
                }

                var tagInfoResponse = JsonConvert.DeserializeObject<HashtagResponse>(json);
                var tagInfo = InstaConvertersFabric.Instance.GetHashTagConverter(tagInfoResponse).Convert();

                return Result.Success(tagInfo);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(ApiHashtag), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<ApiHashtag>(exception);
            }
        }

        /// <summary>
        ///     Get stories of an hashtag
        /// </summary>
        /// <param name="tagName">Tag name</param>
        public async Task<IResult<HashtagStory>> GetHashtagStoriesAsync(string tagName)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetHashtagStoryUri(tagName);

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<HashtagStory>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<HashtagStoryContainerResponse>(json);

                return Result.Success(InstaConvertersFabric.Instance.GetHashtagStoryConverter(obj.Story).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(HashtagStory), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<HashtagStory>(exception);
            }
        }

        /// <summary>
        ///     Get recent hashtag media list
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<SectionMedia>> GetRecentHashtagMediaListAsync(
            string tagName,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                SectionMedia Convert(SectionMediaListResponse hashtagMediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetHashtagMediaListConverter(hashtagMediaListResponse).Convert();
                }

                var mediaResponse = await GetHashtagSection(
                    tagName,
                    Guid.NewGuid().ToString(),
                    paginationParameters.NextMaxId,
                    true).ConfigureAwait(false);
                if (!mediaResponse.Succeeded)
                {
                    if (mediaResponse.Value != null)
                    {
                        Result.Fail(mediaResponse.Info, Convert(mediaResponse.Value));
                    }
                    else
                    {
                        Result.Fail(mediaResponse.Info, default(SectionMedia));
                    }
                }

                paginationParameters.NextMediaIds = mediaResponse.Value.NextMediaIds;
                paginationParameters.NextPage = mediaResponse.Value.NextPage;
                paginationParameters.NextMaxId = mediaResponse.Value.NextMaxId;
                while (mediaResponse.Value.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var moreMedias = await GetHashtagSection(
                        tagName,
                        Guid.NewGuid().ToString(),
                        paginationParameters.NextMaxId,
                        true).ConfigureAwait(false);
                    if (!moreMedias.Succeeded)
                    {
                        if (mediaResponse.Value.Sections != null && mediaResponse.Value.Sections.Any())
                        {
                            return Result.Success(Convert(mediaResponse.Value));
                        }

                        return Result.Fail(moreMedias.Info, Convert(mediaResponse.Value));
                    }

                    mediaResponse.Value.MoreAvailable = moreMedias.Value.MoreAvailable;
                    mediaResponse.Value.NextMaxId = paginationParameters.NextMaxId = moreMedias.Value.NextMaxId;
                    mediaResponse.Value.AutoLoadMoreEnabled = moreMedias.Value.AutoLoadMoreEnabled;
                    mediaResponse.Value.NextMediaIds =
                        paginationParameters.NextMediaIds = moreMedias.Value.NextMediaIds;
                    mediaResponse.Value.NextPage = paginationParameters.NextPage = moreMedias.Value.NextPage;
                    mediaResponse.Value.Sections.AddRange(moreMedias.Value.Sections);
                    paginationParameters.PagesLoaded++;
                }

                return Result.Success(InstaConvertersFabric.Instance.GetHashtagMediaListConverter(mediaResponse.Value)
                                          .Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(SectionMedia), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<SectionMedia>(exception);
            }
        }

        /// <summary>
        ///     Get suggested hashtags
        /// </summary>
        /// <returns>
        ///     List of hashtags
        /// </returns>
        public async Task<IResult<HashtagSearch>> GetSuggestedHashtagsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var tags = new HashtagSearch();
            try
            {
                var userUri = InstaUriCreator.GetSuggestedTagsUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<HashtagSearch>(response, json);
                }

                var tagsResponse = JsonConvert.DeserializeObject<HashtagSearchResponse>(
                    json,
                    new InstaHashtagSuggestedDataConverter());

                tags = InstaConvertersFabric.Instance.GetHashTagsSearchConverter(tagsResponse).Convert();
                return Result.Success(tags);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(HashtagSearch), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, tags);
            }
        }

        /// <summary>
        ///     Get top (ranked) hashtag media list
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<SectionMedia>> GetTopHashtagMediaListAsync(string tagName, PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                SectionMedia Convert(SectionMediaListResponse hashtagMediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetHashtagMediaListConverter(hashtagMediaListResponse).Convert();
                }

                var mediaResponse = await GetHashtagSection(
                    tagName,
                    Guid.NewGuid().ToString(),
                    paginationParameters.NextMaxId).ConfigureAwait(false);

                if (!mediaResponse.Succeeded)
                {
                    Result.Fail(mediaResponse.Info, mediaResponse.Value != null ? Convert(mediaResponse.Value) : default(SectionMedia));
                }

                paginationParameters.NextMediaIds = mediaResponse.Value.NextMediaIds;
                paginationParameters.NextPage = mediaResponse.Value.NextPage;
                paginationParameters.NextMaxId = mediaResponse.Value.NextMaxId;
                while (mediaResponse.Value.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var moreMedias = await GetHashtagSection(
                        tagName,
                        Guid.NewGuid().ToString(),
                        paginationParameters.NextMaxId).ConfigureAwait(false);
                    if (!moreMedias.Succeeded)
                    {
                        if (mediaResponse.Value.Sections != null && mediaResponse.Value.Sections.Any())
                        {
                            return Result.Success(Convert(mediaResponse.Value));
                        }

                        return Result.Fail(moreMedias.Info, Convert(mediaResponse.Value));
                    }

                    mediaResponse.Value.MoreAvailable = moreMedias.Value.MoreAvailable;
                    mediaResponse.Value.NextMaxId = paginationParameters.NextMaxId = moreMedias.Value.NextMaxId;
                    mediaResponse.Value.AutoLoadMoreEnabled = moreMedias.Value.AutoLoadMoreEnabled;
                    mediaResponse.Value.NextMediaIds =
                        paginationParameters.NextMediaIds = moreMedias.Value.NextMediaIds;
                    mediaResponse.Value.NextPage = paginationParameters.NextPage = moreMedias.Value.NextPage;
                    mediaResponse.Value.Sections.AddRange(moreMedias.Value.Sections);
                    paginationParameters.PagesLoaded++;
                }

                return Result.Success(InstaConvertersFabric.Instance.GetHashtagMediaListConverter(mediaResponse.Value)
                                          .Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(SectionMedia), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<SectionMedia>(exception);
            }
        }

        /// <summary>
        ///     Searches for specific hashtag by search query.
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="excludeList">
        ///     Array of numerical hashtag IDs (ie "17841562498105353") to exclude from the response,
        ///     allowing you to skip tags from a previous call to get more results
        /// </param>
        /// <param name="rankToken">The rank token from the previous page's response</param>
        /// <returns>
        ///     List of hashtags
        /// </returns>
        public async Task<IResult<HashtagSearch>> SearchHashtagAsync(string query, IEnumerable<long> excludeList, string rankToken)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var requestHeaderFieldsTooLarge = (HttpStatusCode)431;
            var count = 50;
            var tags = new HashtagSearch();

            try
            {
                var userUri = InstaUriCreator.GetSearchTagUri(query, count, excludeList, rankToken);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == requestHeaderFieldsTooLarge)
                {
                    return Result.Success(tags);
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<HashtagSearch>(response, json);
                }

                var tagsResponse = JsonConvert.DeserializeObject<HashtagSearchResponse>(
                    json,
                    new InstaHashtagSearchDataConverter());
                tags = InstaConvertersFabric.Instance.GetHashTagsSearchConverter(tagsResponse).Convert();

                if (tags.Any() && excludeList != null && excludeList.Contains(tags.First().Id))
                {
                    tags.RemoveAt(0);
                }

                if (!tags.Any())
                {
                    tags = new HashtagSearch();
                }

                return Result.Success(tags);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(HashtagSearch), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, tags);
            }
        }

        /// <summary>
        ///     Unfollow a hashtag
        /// </summary>
        /// <param name="tagname">Tag name</param>
        public async Task<IResult<bool>> UnFollowHashtagAsync(string tagname)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUnFollowHashtagUri(tagname);

                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                return obj.Status.ToLower() == "ok"
                    ? Result.Success(true)
                    : Result.UnExpectedResponse<bool>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<bool>(exception);
            }
        }

        private async Task<IResult<SectionMediaListResponse>> GetHashtagRecentMedia(string tagname, string rankToken = null, string maxId = null, int? page = null, List<long> nextMediaIds = null)
        {
            try
            {
                var instaUri = InstaUriCreator.GetHashtagRecentMediaUri(
                    tagname,
                    rankToken,
                    maxId,
                    page,
                    nextMediaIds);

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<SectionMediaListResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<SectionMediaListResponse>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(SectionMediaListResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<SectionMediaListResponse>(exception);
            }
        }

        private async Task<IResult<SectionMediaListResponse>> GetHashtagSection(string tagName, string rankToken = null, string maxId = null, bool recent = false)
        {
            try
            {
                var instaUri = InstaUriCreator.GetHashtagSectionUri(tagName);

                var data = new Dictionary<string, string>
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "include_persistent", !recent ? "true" : "false" },
                    { "rank_token", rankToken }
                };

                if (recent)
                {
                    data.Add("tab", "recent");
                }
                else
                {
                    data.Add("supported_tabs", new JArray("top", "recent", "places", "discover").ToString());
                }

                if (!string.IsNullOrEmpty(maxId))
                {
                    data.Add("max_id", maxId);
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<SectionMediaListResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<SectionMediaListResponse>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(SectionMediaListResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<SectionMediaListResponse>(exception);
            }
        }
    }
}