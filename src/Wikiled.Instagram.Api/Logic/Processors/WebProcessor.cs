﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Web;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Web;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    internal class InstaWebProcessor : IWebProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaWebProcessor(
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
        ///     Get self account information like joined date or switched to business account date
        /// </summary>
        public async Task<IResult<InstaWebAccountInfo>> GetAccountInfoAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaWebUriCreator.GetAccountsDataUri();
                var request = httpHelper.GetWebRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.Fail($"Error! Status code: {response.StatusCode}", default(InstaWebAccountInfo));
                }

                var json = html.GetJson();
                if (json.IsEmpty())
                {
                    return Result.Fail("Json response isn't available.", default(InstaWebAccountInfo));
                }

                var obj = JsonConvert.DeserializeObject<InstaWebContainerResponse>(json);

                if (obj.Entry?.SettingsPages != null)
                {
                    var first = obj.Entry.SettingsPages.FirstOrDefault();
                    if (first != null)
                    {
                        return Result.Success(InstaConvertersFabric.Instance.GetWebAccountInfoConverter(first).Convert());
                    }
                }

                return Result.Fail("Date joined isn't available.", default(InstaWebAccountInfo));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaWebAccountInfo), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, default(InstaWebAccountInfo));
            }
        }

        /// <summary>
        ///     Get self account follow requests
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaWebTextData>> GetFollowRequestsAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var textDataList = new InstaWebTextData();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaWebTextData Convert(InstaWebSettingsPageResponse settingsPageResponse)
                {
                    return InstaConvertersFabric.Instance.GetWebTextDataListConverter(settingsPageResponse).Convert();
                }

                Uri CreateUri(string cursor = null)
                {
                    return InstaWebUriCreator.GetCurrentFollowRequestsUri(cursor);
                }

                var request = await GetRequest(CreateUri(paginationParameters?.NextMaxId)).ConfigureAwait(false);
                if (!request.Succeeded)
                {
                    if (request.Value != null)
                    {
                        return Result.Fail(request.Info, Convert(request.Value));
                    }

                    return Result.Fail(request.Info, textDataList);
                }

                var response = request.Value;

                paginationParameters.NextMaxId = response.Data.Cursor;

                while (!string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextRequest = await GetRequest(CreateUri(paginationParameters?.NextMaxId)).ConfigureAwait(false);
                    if (!nextRequest.Succeeded)
                    {
                        return Result.Fail(nextRequest.Info, Convert(response));
                    }

                    var nextResponse = nextRequest.Value;

                    if (nextResponse.Data != null)
                    {
                        response.Data.Data.AddRange(nextResponse.Data.Data);
                    }

                    response.Data.Cursor = paginationParameters.NextMaxId = nextResponse.Data?.Cursor;
                    paginationParameters.PagesLoaded++;
                }

                return Result.Success(Convert(response));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, textDataList, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, textDataList);
            }
        }

        /// <summary>
        ///     Get former biography texts
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaWebData>> GetFormerBiographyLinksAsync(PaginationParameters paginationParameters)
        {
            return await GetFormerAsync(InstaWebType.FormerLinksInBio, paginationParameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get former biography texts
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaWebData>> GetFormerBiographyTextsAsync(PaginationParameters paginationParameters)
        {
            return await GetFormerAsync(InstaWebType.FormerBioTexts, paginationParameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get former emails
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaWebData>> GetFormerEmailsAsync(PaginationParameters paginationParameters)
        {
            return await GetFormerAsync(InstaWebType.FormerEmails, paginationParameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get former full names
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaWebData>> GetFormerFullNamesAsync(PaginationParameters paginationParameters)
        {
            return await GetFormerAsync(InstaWebType.FormerFullNames, paginationParameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get former phone numbers
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaWebData>> GetFormerPhoneNumbersAsync(PaginationParameters paginationParameters)
        {
            return await GetFormerAsync(InstaWebType.FormerPhones, paginationParameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get former usernames
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaWebData>> GetFormerUsernamesAsync(PaginationParameters paginationParameters)
        {
            return await GetFormerAsync(InstaWebType.FormerUsernames, paginationParameters).ConfigureAwait(false);
        }

        private async Task<IResult<InstaWebData>> GetFormerAsync(InstaWebType type,
                                                                 PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var webData = new InstaWebData();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaWebData Convert(InstaWebSettingsPageResponse settingsPageResponse)
                {
                    return InstaConvertersFabric.Instance.GetWebDataConverter(settingsPageResponse).Convert();
                }

                Uri CreateUri(string cursor = null)
                {
                    switch (type)
                    {
                        case InstaWebType.FormerBioTexts:
                            return InstaWebUriCreator.GetFormerBiographyTextsUri(cursor);
                        case InstaWebType.FormerLinksInBio:
                            return InstaWebUriCreator.GetFormerBiographyLinksUri(cursor);
                        case InstaWebType.FormerUsernames:
                            return InstaWebUriCreator.GetFormerUsernamesUri(cursor);
                        case InstaWebType.FormerFullNames:
                            return InstaWebUriCreator.GetFormerFullNamesUri(cursor);
                        case InstaWebType.FormerPhones:
                            return InstaWebUriCreator.GetFormerPhoneNumbersUri(cursor);
                        case InstaWebType.FormerEmails:
                            return InstaWebUriCreator.GetFormerEmailsUri(cursor);
                        default:
                            return InstaWebUriCreator.GetFormerBiographyLinksUri(cursor);
                    }
                }

                var request = await GetRequest(CreateUri(paginationParameters?.NextMaxId)).ConfigureAwait(false);
                if (!request.Succeeded)
                {
                    if (request.Value != null)
                    {
                        return Result.Fail(request.Info, Convert(request.Value));
                    }

                    return Result.Fail(request.Info, webData);
                }

                var response = request.Value;

                paginationParameters.NextMaxId = response.Data.Cursor;

                while (!string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextRequest = await GetRequest(CreateUri(paginationParameters?.NextMaxId)).ConfigureAwait(false);
                    if (!nextRequest.Succeeded)
                    {
                        return Result.Fail(nextRequest.Info, Convert(response));
                    }

                    var nextResponse = nextRequest.Value;

                    if (nextResponse.Data != null)
                    {
                        response.Data.Data.AddRange(nextResponse.Data.Data);
                    }

                    response.Data.Cursor = paginationParameters.NextMaxId = nextResponse.Data?.Cursor;
                    paginationParameters.PagesLoaded++;
                }

                return Result.Success(Convert(response));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, webData, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, webData);
            }
        }

        private async Task<IResult<InstaWebSettingsPageResponse>> GetRequest(Uri instaUri)
        {
            try
            {
                var request = httpHelper.GetWebRequest(HttpMethod.Get, instaUri, deviceInfo);

                request.Headers.Add("upgrade-insecure-requests", "1");
                request.Headers.Add("accept",
                                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.Fail($"Error! Status code: {response.StatusCode}",
                                       default(InstaWebSettingsPageResponse));
                }

                if (instaUri.ToString().ToLower().Contains("a=1&cursor="))
                {
                    return Result.Success(JsonConvert.DeserializeObject<InstaWebSettingsPageResponse>(html));
                }

                var json = html.GetJson();
                if (json.IsEmpty())
                {
                    return Result.Fail("Json response isn't available.", default(InstaWebSettingsPageResponse));
                }

                var obj = JsonConvert.DeserializeObject<InstaWebContainerResponse>(json);

                if (obj.Entry?.SettingsPages != null)
                {
                    var first = obj.Entry.SettingsPages.FirstOrDefault();
                    if (first != null)
                    {
                        return Result.Success(first);
                    }
                }

                return Result.Fail("Data isn't available.", default(InstaWebSettingsPageResponse));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaWebSettingsPageResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, default(InstaWebSettingsPageResponse));
            }
        }
    }
}