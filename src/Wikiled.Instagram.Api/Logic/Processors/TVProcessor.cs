using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Instagram TV api functions.
    /// </summary>
    internal class InstaTvProcessor : ITvProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly InstaUserAuthValidate userAuthValidate;

        public InstaTvProcessor(
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
        ///     Get channel by user id (pk) => channel owner
        /// </summary>
        /// <param name="userId">User id (pk) => channel owner</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public Task<IResult<InstaTvChannel>> GetChannelByIdAsync(long userId, PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return GetChannel(null, userId, paginationParameters);
        }

        /// <summary>
        ///     Get channel by <seealso cref="InstaTvChannelType" />
        /// </summary>
        /// <param name="channelType">Channel type</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public Task<IResult<InstaTvChannel>> GetChannelByTypeAsync(InstaTvChannelType channelType, PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return GetChannel(channelType, null, paginationParameters);
        }

        /// <summary>
        ///     Get suggested searches
        /// </summary>
        public async Task<IResult<InstaTvSearch>> GetSuggestedSearchesAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetIgtvSuggestedSearchesUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaTvSearch>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaTvSearchResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetTvSearchConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTvSearch), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaTvSearch>(exception);
            }
        }

        /// <summary>
        ///     Get TV Guide (gets popular and suggested channels)
        /// </summary>
        public async Task<IResult<InstaTv>> GetTvGuideAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetIgtvGuideUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaTv>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaTvResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetTvConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTv), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaTv>(exception);
            }
        }

        /// <summary>
        ///     Search channels
        /// </summary>
        /// <param name="query">Channel or username</param>
        public async Task<IResult<InstaTvSearch>> SearchAsync(string query)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetIgtvSearchUri(query);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaTvSearch>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaTvSearchResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetTvSearchConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTvSearch), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaTvSearch>(exception);
            }
        }

        /// <summary>
        ///     Upload video to Instagram TV
        /// </summary>
        /// <param name="video">
        ///     Video to upload (aspect ratio is very important for thumbnail and video | range 0.5 - 1.0 | Width =
        ///     480, Height = 852)
        /// </param>
        /// <param name="title">Title</param>
        /// <param name="caption">Caption</param>
        public Task<IResult<InstaMedia>> UploadVideoAsync(InstaVideoUpload video, string title, string caption)
        {
            return UploadVideoAsync(null, video, title, caption);
        }

        /// <summary>
        ///     Upload video to Instagram TV with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">
        ///     Video to upload (aspect ratio is very important for thumbnail and video | range 0.5 - 1.0 | Width =
        ///     480, Height = 852)
        /// </param>
        /// <param name="title">Title</param>
        /// <param name="caption">Caption</param>
        public Task<IResult<InstaMedia>> UploadVideoAsync(Action<InstaUploaderProgress> progress, InstaVideoUpload video, string title, string caption)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return instaApi.HelperProcessor.SendIgtvVideoAsync(progress, video, title, caption);
        }

        private async Task<IResult<InstaTvChannel>> GetChannel(InstaTvChannelType? channelType,
                                                               long? userId,
                                                               PaginationParameters paginationParameters)
        {
            try
            {
                var instaUri = InstaUriCreator.GetIgtvChannelUri();
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                if (channelType != null)
                {
                    data.Add("id", channelType.Value.GetRealChannelType());
                }
                else
                {
                    data.Add("id", $"user_{userId}");
                }

                if (paginationParameters != null && !string.IsNullOrEmpty(paginationParameters.NextMaxId))
                {
                    data.Add("max_id", paginationParameters.NextMaxId);
                }

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaTvChannel>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaTvChannelResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetTvChannelConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTvChannel), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaTvChannel>(exception);
            }
        }
    }
}