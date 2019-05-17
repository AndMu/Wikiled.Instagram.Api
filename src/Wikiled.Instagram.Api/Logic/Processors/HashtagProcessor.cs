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
using Wikiled.Instagram.Api.Logger;

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

        private readonly InstaUserAuthValidate userAuthValidate;

        public InstaHashtagProcessor(
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
        ///     Follow a hashtag
        /// </summary>
        /// <param name="tagname">Tag name</param>
        public async Task<IResult<bool>> FollowHashtagAsync(string tagname)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetFollowHashtagUri(tagname);

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
        ///     Get following hashtags information
        /// </summary>
        /// <param name="userId">User identifier (pk)</param>
        /// <returns>
        ///     List of hashtags
        /// </returns>
        public async Task<IResult<InstaHashtagSearch>> GetFollowingHashtagsInfoAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var tags = new InstaHashtagSearch();
            try
            {
                var userUri = InstaUriCreator.GetFollowingTagsInfoUri(userId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHashtagSearch>(response, json);
                }

                var tagsResponse = JsonConvert.DeserializeObject<InstaHashtagSearchResponse>(
                    json,
                    new InstaHashtagSuggestedDataConverter());

                tags = InstaConvertersFabric.Instance.GetHashTagsSearchConverter(tagsResponse).Convert();
                return InstaResult.Success(tags);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHashtagSearch), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, tags);
            }
        }

        /// <summary>
        ///     Gets the hashtag information by user tagname.
        /// </summary>
        /// <param name="tagname">Tagname</param>
        /// <returns>Hashtag information</returns>
        public async Task<IResult<InstaHashtag>> GetHashtagInfoAsync(string tagname)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userUri = InstaUriCreator.GetTagInfoUri(tagname);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHashtag>(response, json);
                }

                var tagInfoResponse = JsonConvert.DeserializeObject<InstaHashtagResponse>(json);
                var tagInfo = InstaConvertersFabric.Instance.GetHashTagConverter(tagInfoResponse).Convert();

                return InstaResult.Success(tagInfo);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHashtag), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaHashtag>(exception);
            }
        }

        /// <summary>
        ///     Get stories of an hashtag
        /// </summary>
        /// <param name="tagname">Tag name</param>
        public async Task<IResult<InstaHashtagStory>> GetHashtagStoriesAsync(string tagname)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetHashtagStoryUri(tagname);

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHashtagStory>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaHashtagStoryContainerResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetHashtagStoryConverter(obj.Story).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHashtagStory), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaHashtagStory>(exception);
            }
        }

        /// <summary>
        ///     Get recent hashtag media list
        /// </summary>
        /// <param name="tagname">Tag name</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaSectionMedia>> GetRecentHashtagMediaListAsync(
            string tagname,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaSectionMedia Convert(InstaSectionMediaListResponse hashtagMediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetHashtagMediaListConverter(hashtagMediaListResponse).Convert();
                }

                var mediaResponse = await GetHashtagSection(
                    tagname,
                    Guid.NewGuid().ToString(),
                    paginationParameters.NextMaxId,
                    true).ConfigureAwait(false);
                if (!mediaResponse.Succeeded)
                {
                    if (mediaResponse.Value != null)
                    {
                        InstaResult.Fail(mediaResponse.Info, Convert(mediaResponse.Value));
                    }
                    else
                    {
                        InstaResult.Fail(mediaResponse.Info, default(InstaSectionMedia));
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
                        tagname,
                        Guid.NewGuid().ToString(),
                        paginationParameters.NextMaxId,
                        true).ConfigureAwait(false);
                    if (!moreMedias.Succeeded)
                    {
                        if (mediaResponse.Value.Sections != null && mediaResponse.Value.Sections.Any())
                        {
                            return InstaResult.Success(Convert(mediaResponse.Value));
                        }

                        return InstaResult.Fail(moreMedias.Info, Convert(mediaResponse.Value));
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

                return InstaResult.Success(InstaConvertersFabric.Instance.GetHashtagMediaListConverter(mediaResponse.Value)
                                          .Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaSectionMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaSectionMedia>(exception);
            }
        }

        /// <summary>
        ///     Get suggested hashtags
        /// </summary>
        /// <returns>
        ///     List of hashtags
        /// </returns>
        public async Task<IResult<InstaHashtagSearch>> GetSuggestedHashtagsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var tags = new InstaHashtagSearch();
            try
            {
                var userUri = InstaUriCreator.GetSuggestedTagsUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHashtagSearch>(response, json);
                }

                var tagsResponse = JsonConvert.DeserializeObject<InstaHashtagSearchResponse>(
                    json,
                    new InstaHashtagSuggestedDataConverter());

                tags = InstaConvertersFabric.Instance.GetHashTagsSearchConverter(tagsResponse).Convert();
                return InstaResult.Success(tags);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHashtagSearch), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, tags);
            }
        }

        /// <summary>
        ///     Get top (ranked) hashtag media list
        /// </summary>
        /// <param name="tagname">Tag name</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaSectionMedia>> GetTopHashtagMediaListAsync(
            string tagname,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaSectionMedia Convert(InstaSectionMediaListResponse hashtagMediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetHashtagMediaListConverter(hashtagMediaListResponse).Convert();
                }

                var mediaResponse = await GetHashtagSection(
                    tagname,
                    Guid.NewGuid().ToString(),
                    paginationParameters.NextMaxId).ConfigureAwait(false);

                if (!mediaResponse.Succeeded)
                {
                    if (mediaResponse.Value != null)
                    {
                        InstaResult.Fail(mediaResponse.Info, Convert(mediaResponse.Value));
                    }
                    else
                    {
                        InstaResult.Fail(mediaResponse.Info, default(InstaSectionMedia));
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
                        tagname,
                        Guid.NewGuid().ToString(),
                        paginationParameters.NextMaxId).ConfigureAwait(false);
                    if (!moreMedias.Succeeded)
                    {
                        if (mediaResponse.Value.Sections != null && mediaResponse.Value.Sections.Any())
                        {
                            return InstaResult.Success(Convert(mediaResponse.Value));
                        }

                        return InstaResult.Fail(moreMedias.Info, Convert(mediaResponse.Value));
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

                return InstaResult.Success(InstaConvertersFabric.Instance.GetHashtagMediaListConverter(mediaResponse.Value)
                                          .Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaSectionMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaSectionMedia>(exception);
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
        public async Task<IResult<InstaHashtagSearch>> SearchHashtagAsync(
            string query,
            IEnumerable<long> excludeList,
            string rankToken)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var requestHeaderFieldsTooLarge = (HttpStatusCode)431;
            var count = 50;
            var tags = new InstaHashtagSearch();

            try
            {
                var userUri = InstaUriCreator.GetSearchTagUri(query, count, excludeList, rankToken);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == requestHeaderFieldsTooLarge)
                {
                    return InstaResult.Success(tags);
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaHashtagSearch>(response, json);
                }

                var tagsResponse = JsonConvert.DeserializeObject<InstaHashtagSearchResponse>(
                    json,
                    new InstaHashtagSearchDataConverter());
                tags = InstaConvertersFabric.Instance.GetHashTagsSearchConverter(tagsResponse).Convert();

                if (tags.Any() && excludeList != null && excludeList.Contains(tags.First().Id))
                {
                    tags.RemoveAt(0);
                }

                if (!tags.Any())
                {
                    tags = new InstaHashtagSearch();
                }

                return InstaResult.Success(tags);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaHashtagSearch), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, tags);
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

        private async Task<IResult<InstaSectionMediaListResponse>> GetHashtagRecentMedia(
            string tagname,
            string rankToken = null,
            string maxId = null,
            int? page = null,
            List<long> nextMediaIds = null)
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
                    return InstaResult.UnExpectedResponse<InstaSectionMediaListResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaSectionMediaListResponse>(json);

                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaSectionMediaListResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaSectionMediaListResponse>(exception);
            }
        }

        private async Task<IResult<InstaSectionMediaListResponse>> GetHashtagSection(
            string tagname,
            string rankToken = null,
            string maxId = null,
            bool recent = false)
        {
            try
            {
                var instaUri = InstaUriCreator.GetHashtagSectionUri(tagname);

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

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaSectionMediaListResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaSectionMediaListResponse>(json);

                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaSectionMediaListResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaSectionMediaListResponse>(exception);
            }
        }
    }
}