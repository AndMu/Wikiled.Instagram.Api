﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Shopping;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Shopping and commerce api functions.
    /// </summary>
    internal class InstaShoppingProcessor : IShoppingProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaShoppingProcessor(
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
        ///     Get product info
        /// </summary>
        /// <param name="productId">Product id (get it from <see cref="InstaProduct.ProductId" /> )</param>
        /// <param name="mediaPk">Media Pk (get it from <see cref="InstaMedia.Pk" />)</param>
        /// <param name="deviceWidth">Device width (pixel)</param>
        public async Task<IResult<InstaProductInfo>> GetProductInfoAsync(
            long productId,
            string mediaPk,
            int deviceWidth = 720)
        {
            try
            {
                var instaUri = InstaUriCreator.GetProductInfoUri(productId, mediaPk, deviceWidth);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaProductInfo>(response, json);
                }

                var productInfoResponse = JsonConvert.DeserializeObject<InstaProductInfoResponse>(json);
                var converted = InstaConvertersFabric.Instance.GetProductInfoConverter(productInfoResponse).Convert();

                return Result.Success(converted);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaProductInfo), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaProductInfo>(exception);
            }
        }

        /// <summary>
        ///     Get all user shoppable media by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public async Task<IResult<InstaMediaList>> GetUserShoppableMediaAsync(
            string username,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var user = await instaApi.UserProcessor.GetUserAsync(username).ConfigureAwait(false);
            if (!user.Succeeded)
            {
                return Result.Fail<InstaMediaList>("Unable to get user to load shoppable media");
            }

            return await GetUserShoppableMedia(user.Value.Pk, paginationParameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get all user shoppable media by user id (pk)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public Task<IResult<InstaMediaList>> GetUserShoppableMediaByIdAsync(long userId, PaginationParameters paginationParameters)
        {
            return GetUserShoppableMedia(userId, paginationParameters);
        }

        public async Task<IResult<InstaProductInfo>> GetCatalogsAsync()
        {
            try
            {
                var instaUri =
                    new Uri(
                        $"https://i.instagram.com/api/v1/wwwgraphql/ig/query/?locale={InstaApiConstants.AcceptLanguage.Replace("-", "_")}");

                var sources = new JObject { { "sources", null } };

                var data = new Dictionary<string, string>
                {
                    { "access_token", "undefined" },
                    { "fb_api_caller_class", "RelayModern" },
                    { "variables", sources.ToString(Formatting.Indented) },
                    { "doc_id", "1742970149122229" }
                };

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                //{"data":{"me":{"taggable_catalogs":{"edges":[],"page_info":{"has_next_page":false,"end_cursor":null}},"id":"17841407343005740"}}}
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaProductInfo>(response, json);
                }

                var productInfoResponse = JsonConvert.DeserializeObject<InstaProductInfoResponse>(json);
                var converted = InstaConvertersFabric.Instance.GetProductInfoConverter(productInfoResponse).Convert();

                return Result.Success(converted);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaProductInfo), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaProductInfo>(exception);
            }
        }

        private async Task<IResult<InstaMediaListResponse>> GetShoppableMedia(
            long userId,
            PaginationParameters paginationParameters)
        {
            try
            {
                var instaUri = InstaUriCreator.GetUserShoppableMediaListUri(userId, paginationParameters.NextMaxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaMediaListResponse>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaListResponse>(
                    json,
                    new InstaMediaListDataConverter());

                return Result.Success(mediaResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaMediaListResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, default(InstaMediaListResponse));
            }
        }

        private async Task<IResult<InstaMediaList>> GetUserShoppableMedia(
            long userId,
            PaginationParameters paginationParameters)
        {
            var mediaList = new InstaMediaList();
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

                var mediaResult = await GetShoppableMedia(userId, paginationParameters).ConfigureAwait(false);
                if (!mediaResult.Succeeded)
                {
                    if (mediaResult.Value != null)
                    {
                        return Result.Fail(mediaResult.Info, Convert(mediaResult.Value));
                    }

                    return Result.Fail(mediaResult.Info, mediaList);
                }

                var mediaResponse = mediaResult.Value;
                mediaList = InstaConvertersFabric.Instance.GetMediaListConverter(mediaResponse).Convert();
                mediaList.NextMaxId = paginationParameters.NextMaxId = mediaResponse.NextMaxId;
                paginationParameters.PagesLoaded++;

                while (mediaResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextMedia = await GetShoppableMedia(userId, paginationParameters).ConfigureAwait(false);
                    if (!nextMedia.Succeeded)
                    {
                        return Result.Fail(nextMedia.Info, mediaList);
                    }

                    mediaList.NextMaxId = paginationParameters.NextMaxId = nextMedia.Value.NextMaxId;
                    mediaList.AddRange(Convert(nextMedia.Value));
                    mediaResponse.MoreAvailable = nextMedia.Value.MoreAvailable;
                    mediaResponse.ResultsCount += nextMedia.Value.ResultsCount;
                    paginationParameters.PagesLoaded++;
                }

                mediaList.Pages = paginationParameters.PagesLoaded;
                mediaList.PageSize = mediaResponse.ResultsCount;
                return Result.Success(mediaList);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaMediaList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, mediaList);
            }
        }
    }
}