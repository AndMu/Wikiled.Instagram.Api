﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Collection;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Collection;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Collection api functions.
    /// </summary>
    internal class InstaCollectionProcessor : ICollectionProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaCollectionProcessor(
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
        ///     Adds items to collection asynchronous.
        /// </summary>
        /// <param name="collectionId">Collection identifier.</param>
        /// <param name="mediaIds">Media id list.</param>
        public async Task<IResult<InstaCollectionItem>> AddItemsToCollectionAsync(
            long collectionId,
            params string[] mediaIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (mediaIds?.Length < 1)
                {
                    return Result.Fail<InstaCollectionItem>("Provide at least one media id to add to collection");
                }

                var editCollectionUri = InstaUriCreator.GetEditCollectionUri(collectionId);

                var data = new JObject
                {
                    { "module_name", InstaApiConstants.FeedSavedAddToCollectionModule },
                    { "added_media_ids", JsonConvert.SerializeObject(mediaIds) },
                    { "radio_type", "wifi-none" },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken }
                };

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Get, editCollectionUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaCollectionItem>(response, json);
                }

                var newCollectionResponse = JsonConvert.DeserializeObject<InstaCollectionItemResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetCollectionConverter(newCollectionResponse);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaCollectionItem), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaCollectionItem>(exception);
            }
        }

        /// <summary>
        ///     Create a new collection
        /// </summary>
        /// <param name="collectionName">The name of the new collection</param>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.InstaCollectionItem" />
        /// </returns>
        public async Task<IResult<InstaCollectionItem>> CreateCollectionAsync(string collectionName)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var createCollectionUri = InstaUriCreator.GetCreateCollectionUri();

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "name", collectionName },
                    { "module_name", InstaApiConstants.CollectionCreateModule }
                };

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Get, createCollectionUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var newCollectionResponse = JsonConvert.DeserializeObject<InstaCollectionItemResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetCollectionConverter(newCollectionResponse);

                return response.StatusCode != HttpStatusCode.OK
                    ? Result.UnExpectedResponse<InstaCollectionItem>(response, json)
                    : Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaCollectionItem), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaCollectionItem>(exception);
            }
        }

        /// <summary>
        ///     Delete your collection for given collection id
        /// </summary>
        /// <param name="collectionId">Collection ID to delete</param>
        /// <returns>true if succeed</returns>
        public async Task<IResult<bool>> DeleteCollectionAsync(long collectionId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var createCollectionUri = InstaUriCreator.GetDeleteCollectionUri(collectionId);

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "module_name", "collection_editor" }
                };

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Get, createCollectionUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Result.Success(true);
                }

                return Result.UnExpectedResponse<bool>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception.Message, false);
            }
        }

        /// <summary>
        ///     Edit a collection
        /// </summary>
        /// <param name="collectionId">Collection ID to edit</param>
        /// <param name="name">New name for giving collection (set null if you don't want to change it)</param>
        /// <param name="photoCoverMediaId">
        ///     New photo cover media Id (get it from <see cref="InstaMedia.Identifier" />) => Optional
        ///     <para>Important note: media id must be exists in giving collection!</para>
        /// </param>
        public async Task<IResult<InstaCollectionItem>> EditCollectionAsync(
            long collectionId,
            string name,
            string photoCoverMediaId = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var collection = await GetSingleCollection(collectionId, PaginationParameters.MaxPagesToLoad(1)).ConfigureAwait(false);
                if (collection.Succeeded && string.IsNullOrEmpty(name))
                {
                    name = collection.Value.CollectionName;
                }

                var editCollectionUri = InstaUriCreator.GetEditCollectionUri(collectionId);

                var data = new JObject
                {
                    { "name", name ?? string.Empty },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken }
                };
                if (!string.IsNullOrEmpty(photoCoverMediaId))
                {
                    data.Add("cover_media_id", photoCoverMediaId);
                }

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Get, editCollectionUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaCollectionItem>(response, json);
                }

                var newCollectionResponse = JsonConvert.DeserializeObject<InstaCollectionItemResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetCollectionConverter(newCollectionResponse);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaCollectionItem), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaCollectionItem>(exception);
            }
        }

        /// <summary>
        ///     Get your collections
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next max id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.InstaCollections" />
        /// </returns>
        public async Task<IResult<InstaCollections>> GetCollectionsAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaCollections Convert(InstaCollectionsResponse instaCollectionsResponse)
                {
                    return InstaConvertersFabric.Instance.GetCollectionsConverter(instaCollectionsResponse).Convert();
                }

                var collections = await GetCollections(paginationParameters).ConfigureAwait(false);

                if (!collections.Succeeded)
                {
                    return Result.Fail(collections.Info, default(InstaCollections));
                }

                var collectionsResponse = collections.Value;
                paginationParameters.NextMaxId = collectionsResponse.NextMaxId;
                var pagesLoaded = 1;

                while (collectionsResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(collectionsResponse.NextMaxId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextCollection = await GetCollections(paginationParameters).ConfigureAwait(false);

                    if (!nextCollection.Succeeded)
                    {
                        return Result.Fail(nextCollection.Info, Convert(nextCollection.Value));
                    }

                    collectionsResponse.NextMaxId = paginationParameters.NextMaxId = nextCollection.Value.NextMaxId;
                    collectionsResponse.MoreAvailable = nextCollection.Value.MoreAvailable;
                    collectionsResponse.AutoLoadMoreEnabled = nextCollection.Value.AutoLoadMoreEnabled;
                    collectionsResponse.Status = nextCollection.Value.Status;
                    collectionsResponse.Items.AddRange(nextCollection.Value.Items);
                    pagesLoaded++;
                }

                var converter = InstaConvertersFabric.Instance.GetCollectionsConverter(collectionsResponse);

                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaCollections), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaCollections>(exception);
            }
        }

        /// <summary>
        ///     Get your collection for given collection id
        /// </summary>
        /// <param name="collectionId">Collection ID</param>
        /// <param name="paginationParameters">Pagination parameters: next max id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.InstaCollectionItem" />
        /// </returns>
        public async Task<IResult<InstaCollectionItem>> GetSingleCollectionAsync(
            long collectionId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaCollectionItem Convert(InstaCollectionItemResponse instaCollectionItemResponse)
                {
                    return InstaConvertersFabric.Instance.GetCollectionConverter(instaCollectionItemResponse).Convert();
                }

                var collectionList = await GetSingleCollection(collectionId, paginationParameters).ConfigureAwait(false);
                if (!collectionList.Succeeded)
                {
                    return Result.Fail(collectionList.Info, default(InstaCollectionItem));
                }

                var collectionsListResponse = collectionList.Value;
                paginationParameters.NextMaxId = collectionsListResponse.NextMaxId;
                var pagesLoaded = 1;

                while (collectionsListResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(collectionsListResponse.NextMaxId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextCollectionList = await GetSingleCollection(collectionId, paginationParameters).ConfigureAwait(false);

                    if (!nextCollectionList.Succeeded)
                    {
                        return Result.Fail(nextCollectionList.Info, Convert(nextCollectionList.Value));
                    }

                    collectionsListResponse.NextMaxId =
                        paginationParameters.NextMaxId = nextCollectionList.Value.NextMaxId;
                    collectionsListResponse.MoreAvailable = nextCollectionList.Value.MoreAvailable;
                    collectionsListResponse.AutoLoadMoreEnabled = nextCollectionList.Value.AutoLoadMoreEnabled;
                    collectionsListResponse.Status = nextCollectionList.Value.Status;
                    collectionsListResponse.Media.Medias.AddRange(nextCollectionList.Value.Media.Medias);
                    pagesLoaded++;
                }

                var converter = InstaConvertersFabric.Instance.GetCollectionConverter(collectionsListResponse);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaCollectionItem), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaCollectionItem>(exception);
            }
        }

        private async Task<IResult<InstaCollectionsResponse>> GetCollections(PaginationParameters paginationParameters)
        {
            try
            {
                var collectionUri = InstaUriCreator.GetCollectionsUri(paginationParameters?.NextMaxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, collectionUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaCollectionsResponse>(response, json);
                }

                var collectionsResponse = JsonConvert.DeserializeObject<InstaCollectionsResponse>(json);
                return Result.Success(collectionsResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaCollectionsResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaCollectionsResponse>(exception);
            }
        }

        private async Task<IResult<InstaCollectionItemResponse>> GetSingleCollection(
            long collectionId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var collectionUri = InstaUriCreator.GetCollectionUri(collectionId, paginationParameters?.NextMaxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, collectionUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaCollectionItemResponse>(response, json);
                }

                var collectionsListResponse =
                    JsonConvert.DeserializeObject<InstaCollectionItemResponse>(
                        json,
                        new InstaCollectionDataConverter());
                return Result.Success(collectionsListResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaCollectionItemResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaCollectionItemResponse>(exception);
            }
        }
    }
}