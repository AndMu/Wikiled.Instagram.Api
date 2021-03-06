﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Location api functions.
    /// </summary>
    internal class InstaLocationProcessor : ILocationProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaLocationProcessor(
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
        ///     Get location(place) information by external id or facebook places id
        ///     <para>
        ///         Get external id from this function:
        ///         <see cref="ILocationProcessor.SearchLocationAsync(double, double, string)" />
        ///     </para>
        ///     <para>
        ///         Get facebook places id from this function:
        ///         <see cref="ILocationProcessor.SearchPlacesAsync(double, double, string)(double, double, string)" />
        ///     </para>
        /// </summary>
        /// <param name="externalIdOrFacebookPlacesId">
        ///     External id or facebook places id of an location/place
        ///     <para>
        ///         Get external id from this function:
        ///         <see cref="ILocationProcessor.SearchLocationAsync(double, double, string)" />
        ///     </para>
        ///     <para>
        ///         Get facebook places id from this function:
        ///         <see cref="ILocationProcessor.SearchPlacesAsync(double, double, string)(double, double, string)" />
        ///     </para>
        /// </param>
        public async Task<IResult<PlaceShort>> GetLocationInfoAsync(string externalIdOrFacebookPlacesId)
        {
            try
            {
                var instaUri = InstaUriCreator.GetLocationInfoUri(externalIdOrFacebookPlacesId);

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<PlaceShort>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<PlaceResponse>(json);

                return Result.Success(InstaConvertersFabric.Instance.GetPlaceShortConverter(obj.Location).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(PlaceShort), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<PlaceShort>(exception);
            }
        }

        /// <summary>
        ///     Gets the stories of particular location.
        /// </summary>
        /// <param name="locationId">Location identifier (location pk, external id, facebook id)</param>
        /// <returns>
        ///     Location stories
        /// </returns>
        public async Task<IResult<InstaStory>> GetLocationStoriesAsync(long locationId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var uri = InstaUriCreator.GetLocationFeedUri(locationId.ToString());
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaStory>(response, json);
                }

                var feedResponse = JsonConvert.DeserializeObject<LocationFeedResponse>(json);
                var feed = InstaConvertersFabric.Instance.GetLocationFeedConverter(feedResponse).Convert();

                return Result.Success(feed.Story);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaStory), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaStory>(exception);
            }
        }

        /// <summary>
        ///     Get recent location media feeds.
        ///     <para>Important note: Be careful of using this function, because it's an POST request</para>
        /// </summary>
        /// <param name="locationId">Location identifier (location pk, external id, facebook id)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public Task<IResult<SectionMedia>> GetRecentLocationFeedsAsync(long locationId, PaginationParameters paginationParameters)
        {
            return GetSectionAsync(locationId, paginationParameters, InstaSectionType.Recent);
        }

        /// <summary>
        ///     Get top (ranked) location media feeds.
        ///     <para>Important note: Be careful of using this function, because it's an POST request</para>
        /// </summary>
        /// <param name="locationId">Location identifier (location pk, external id, facebook id)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public Task<IResult<SectionMedia>> GetTopLocationFeedsAsync(long locationId, PaginationParameters paginationParameters)
        {
            return GetSectionAsync(locationId, paginationParameters, InstaSectionType.Ranked);
        }

        /// <summary>
        ///     Searches for specific location by provided geo-data or search query.
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="query">Search query</param>
        /// <returns>
        ///     List of locations (short format)
        /// </returns>
        public async Task<IResult<LocationShortList>> SearchLocationAsync(double latitude, double longitude, string query)
        {
            logger.LogDebug("SearchLocationAsync {0} {1} {2}", latitude, longitude, query);
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var uri = InstaUriCreator.GetLocationSearchUri();

                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "latitude", latitude.ToString(CultureInfo.InvariantCulture) },
                    { "longitude", longitude.ToString(CultureInfo.InvariantCulture) },
                    { "rank_token", user.RankToken }
                };

                if (!string.IsNullOrEmpty(query))
                {
                    fields.Add("search_query", query);
                }
                else
                {
                    fields.Add("timestamp", InstaDateTimeHelper.GetUnixTimestampSeconds().ToString());
                }

                if (!Uri.TryCreate(uri, fields.AsQueryString(), out var newuri))
                {
                    return Result.Fail<LocationShortList>("Unable to create uri for location search");
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, newuri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<LocationShortList>(response, json);
                }

                var locations = JsonConvert.DeserializeObject<LocationSearchResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetLocationsSearchConverter(locations);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(LocationShortList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<LocationShortList>(exception);
            }
        }

        /// <summary>
        ///     Search places in facebook
        ///     <para>Note: This works for non-facebook accounts too!</para>
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="PlaceList" />
        /// </returns>
        public Task<IResult<PlaceList>> SearchPlacesAsync(double latitude, double longitude, PaginationParameters paginationParameters)
        {
            return SearchPlacesAsync(latitude, longitude, null, paginationParameters);
        }

        /// <summary>
        ///     Search places in facebook
        ///     <para>Note: This works for non-facebook accounts too!</para>
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="query">Query to search (city, country or ...)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="PlaceList" />
        /// </returns>
        public async Task<IResult<PlaceList>> SearchPlacesAsync(double latitude,
                                                                     double longitude,
                                                                     string query,
                                                                     PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                PlaceList Convert(PlaceListResponse placelistResponse)
                {
                    return InstaConvertersFabric.Instance.GetPlaceListConverter(placelistResponse).Convert();
                }

                var places = await SearchPlaces(latitude, longitude, query, paginationParameters).ConfigureAwait(false);
                if (!places.Succeeded)
                {
                    return Result.Fail(places.Info, default(PlaceList));
                }

                var placesResponse = places.Value;
                paginationParameters.NextMaxId = placesResponse.RankToken;
                paginationParameters.ExcludeList = placesResponse.ExcludeList;
                var pagesLoaded = 1;
                while (placesResponse.HasMore != null &&
                    placesResponse.HasMore.Value &&
                    !string.IsNullOrEmpty(placesResponse.RankToken) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextPlaces = await SearchPlaces(latitude, longitude, query, paginationParameters).ConfigureAwait(false);

                    if (!nextPlaces.Succeeded)
                    {
                        return Result.Fail(nextPlaces.Info, Convert(nextPlaces.Value));
                    }

                    placesResponse.RankToken = paginationParameters.NextMaxId = nextPlaces.Value.RankToken;
                    placesResponse.HasMore = nextPlaces.Value.HasMore;
                    placesResponse.Items.AddRange(nextPlaces.Value.Items);
                    placesResponse.Status = nextPlaces.Value.Status;
                    paginationParameters.ExcludeList = nextPlaces.Value.ExcludeList;
                    pagesLoaded++;
                }

                return Result.Success(InstaConvertersFabric.Instance.GetPlaceListConverter(placesResponse).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(PlaceList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<PlaceList>(exception);
            }
        }

        /// <summary>
        ///     Search user by location
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <param name="desireUsername">Desire username</param>
        /// <param name="count">Maximum user count</param>
        public async Task<IResult<InstaUserSearchLocation>> SearchUserByLocationAsync(
            double latitude,
            double longitude,
            string desireUsername,
            int count = 50)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var uri = InstaUriCreator.GetUserSearchByLocationUri();
                if (count <= 0)
                {
                    count = 30;
                }

                var fields = new Dictionary<string, string>
                {
                    { "timezone_offset", InstaApiConstants.TimezoneOffset.ToString() },
                    { "lat", latitude.ToString(CultureInfo.InvariantCulture) },
                    { "lng", longitude.ToString(CultureInfo.InvariantCulture) },
                    { "count", count.ToString() },
                    { "query", desireUsername },
                    { "context", "blended" },
                    { "rank_token", user.RankToken }
                };
                if (!Uri.TryCreate(uri, fields.AsQueryString(), out var newuri))
                {
                    return Result.Fail<InstaUserSearchLocation>("Unable to create uri for user search by location");
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, newuri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaUserSearchLocation>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserSearchLocation>(json);
                return obj.Status.ToLower() == "ok"
                    ? Result.Success(obj)
                    : Result.UnExpectedResponse<InstaUserSearchLocation>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUserSearchLocation), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaUserSearchLocation>(exception);
            }
        }

        private async Task<IResult<SectionMedia>> GetSectionAsync(
            long locationId,
            PaginationParameters paginationParameters,
            InstaSectionType sectionType)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                SectionMedia Convert(SectionMediaListResponse sectionMediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetHashtagMediaListConverter(sectionMediaListResponse).Convert();
                }

                var mediaResponse = await GetSectionMedia(
                    sectionType,
                    locationId,
                    paginationParameters.NextMaxId,
                    paginationParameters.NextPage,
                    paginationParameters.NextMediaIds).ConfigureAwait(false);

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
                    var moreMedias = await GetSectionMedia(
                        sectionType,
                        locationId,
                        paginationParameters.NextMaxId,
                        mediaResponse.Value.NextPage,
                        mediaResponse.Value.NextMediaIds).ConfigureAwait(false);
                    if (!moreMedias.Succeeded)
                    {
                        if (mediaResponse.Value.Sections?.Count > 0)
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

        private async Task<IResult<SectionMediaListResponse>> GetSectionMedia(
            InstaSectionType sectionType,
            long locationId,
            string maxId = null,
            int? page = null,
            List<long> nextMediaIds = null)
        {
            try
            {
                var instaUri = InstaUriCreator.GetLocationSectionUri(locationId.ToString());
                var data = new Dictionary<string, string>
                {
                    { "rank_token", deviceInfo.DeviceGuid.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "session_id", Guid.NewGuid().ToString() },
                    { "tab", sectionType.ToString().ToLower() }
                };

                if (!string.IsNullOrEmpty(maxId))
                {
                    data.Add("max_id", maxId);
                }

                if (page != null && page > 0)
                {
                    data.Add("page", page.ToString());
                }

                if (nextMediaIds?.Count > 0)
                {
                    var mediaIds = $"[{string.Join(",", nextMediaIds)}]";
                    if (sectionType == InstaSectionType.Ranked)
                    {
                        data.Add("next_media_ids", mediaIds.EncodeUri());
                    }
                    else
                    {
                        data.Add("next_media_ids", mediaIds);
                    }
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

        private async Task<IResult<PlaceListResponse>> SearchPlaces(
            double latitude,
            double longitude,
            string query,
            PaginationParameters paginationParameters)
        {
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                var instaUri = InstaUriCreator.GetSearchPlacesUri(
                    InstaApiConstants.TimezoneOffset,
                    latitude,
                    longitude,
                    query,
                    paginationParameters.NextMaxId,
                    paginationParameters.ExcludeList);

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<PlaceListResponse>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<PlaceListResponse>(response, json);
                }

                if (obj.Items?.Count > 0)
                {
                    foreach (var item in obj.Items)
                    {
                        obj.ExcludeList.Add(item.Location.Pk);
                    }
                }

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(PlaceListResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<PlaceListResponse>(exception);
            }
        }
    }
}