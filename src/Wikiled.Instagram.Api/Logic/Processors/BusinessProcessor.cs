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
using Wikiled.Instagram.Api.Classes.Models.Business;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Business;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Business api functions
    ///     <para>Note: All functions of this interface only works with business accounts!</para>
    /// </summary>
    internal class InstaBusinessProcessor : IBusinessProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaBusinessProcessor(
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
        ///     Add button to your business account
        /// </summary>
        /// <param name="businessPartner">
        ///     Desire partner button (Use
        ///     <see cref="IBusinessProcessor.GetBusinessPartnersButtonsAsync" /> to get business buttons(instagram partner) list!)
        /// </param>
        /// <param name="uri">Uri (related to Business partner button)</param>
        public async Task<IResult<InstaBusinessUser>> AddOrChangeBusinessButtonAsync(
            InstaBusinessPartner businessPartner,
            Uri uri)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (businessPartner == null)
                {
                    return Result.Fail<InstaBusinessUser>("Business partner cannot be null");
                }

                if (uri == null)
                {
                    return Result.Fail<InstaBusinessUser>("Uri related to business partner cannot be null");
                }

                var validateUri = await ValidateUrlAsync(businessPartner, uri).ConfigureAwait(false);
                if (!validateUri.Succeeded)
                {
                    return Result.Fail<InstaBusinessUser>(validateUri.Info.Message);
                }

                var instaUri = InstaUriCreator.GetUpdateBusinessInfoUri();

                var data = new JObject
                {
                    { "ix_url", uri.ToString() },
                    { "ix_app_id", businessPartner.AppId },
                    { "is_call_to_action_enabled", "1" },
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
                    return Result.UnExpectedResponse<InstaBusinessUser>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessUserContainerResponse>(json);

                return Result.Success(InstaConvertersFabric.Instance.GetBusinessUserConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessUser), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessUser>(exception);
            }
        }

        /// <summary>
        ///     Add users to approval branded whitelist
        /// </summary>
        /// <param name="userIdsToAdd">User ids (pk) to add</param>
        public async Task<IResult<InstaBrandedContent>> AddUserToBrandedWhiteListAsync(params long[] userIdsToAdd)
        {
            if (userIdsToAdd == null || userIdsToAdd != null && !userIdsToAdd.Any())
            {
                return Result.Fail<InstaBrandedContent>("At least one user id is require.");
            }

            return await UpdateBrandedContent(null, userIdsToAdd).ConfigureAwait(false);
        }

        /// <summary>
        ///     Change business category
        ///     <para>Note: Get it from <see cref="IBusinessProcessor.GetSubCategoriesAsync(string)" /></para>
        /// </summary>
        /// <param name="subCategoryId">
        ///     Sub category id (Get it from <see cref="IBusinessProcessor.GetSubCategoriesAsync(string)" />)
        /// </param>
        public async Task<IResult<InstaBusinessUser>> ChangeBusinessCategoryAsync(string subCategoryId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (string.IsNullOrEmpty(subCategoryId))
                {
                    return Result.Fail<InstaBusinessUser>("Sub category id cannot be null or empty");
                }

                var instaUri = InstaUriCreator.GetSetBusinessCategoryUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "category_id", subCategoryId }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBusinessUser>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessUserContainerResponse>(json);

                return Result.Success(InstaConvertersFabric.Instance.GetBusinessUserConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessUser), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessUser>(exception);
            }
        }

        /// <summary>
        ///     Disable branded content approval
        /// </summary>
        public Task<IResult<InstaBrandedContent>> DisableBrandedContentApprovalAsync()
        {
            return UpdateBrandedContent(0);
        }

        /// <summary>
        ///     Enable branded content approval
        /// </summary>
        public Task<IResult<InstaBrandedContent>> EnableBrandedContentApprovalAsync()
        {
            return UpdateBrandedContent(1);
        }

        /// <summary>
        ///     Get account details for an business account ( like it's joined date )
        ///     <param name="userId">User id (pk)</param>
        /// </summary>
        public async Task<IResult<InstaAccountDetails>> GetAccountDetailsAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAccountDetailsUri(userId);

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaAccountDetails>(
                        response,
                        "Can't find account details for this user pk",
                        json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountDetailsResponse>(json);
                return Result.Success(InstaConvertersFabric.Instance.GetAccountDetailsConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaAccountDetails), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaAccountDetails>(exception);
            }
        }

        /// <summary>
        ///     Get branded content approval settings
        ///     <para>Note: Only approved partners can tag you in branded content when you require approvals.</para>
        /// </summary>
        public async Task<IResult<InstaBrandedContent>> GetBrandedContentApprovalAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBusinessBrandedSettingsUri();

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);

                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBrandedContent>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBrandedContentResponse>(json);
                return Result.Success(InstaConvertersFabric.Instance.GetBrandedContentConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBrandedContent), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBrandedContent>(exception);
            }
        }

        /// <summary>
        ///     Get logged in business account information
        /// </summary>
        public Task<IResult<InstaUserInfo>> GetBusinessAccountInformationAsync()
        {
            return instaApi.UserProcessor.GetUserInfoByIdAsync(user.LoggedInUser.Pk);
        }

        /// <summary>
        ///     Get business get buttons (partners)
        /// </summary>
        public async Task<IResult<InstaBusinessPartnersList>> GetBusinessPartnersButtonsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new JObject();
                var dataStr = httpHelper.GetSignature(data);
                var instaUri = InstaUriCreator.GetBusinessInstantExperienceUri(dataStr);

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBusinessPartnersList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessPartnerContainer>(json);
                var partners = new InstaBusinessPartnersList();

                foreach (var p in obj.Partners)
                {
                    partners.Add(p);
                }

                return Result.Success(partners);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessPartnersList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessPartnersList>(exception);
            }
        }

        /// <summary>
        ///     Get all categories
        /// </summary>
        public async Task<IResult<InstaBusinessCategoryList>> GetCategoriesAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBusinessGraphQlUri();

                var queryParams = new JObject { { "0", "-1" } };
                var data = new Dictionary<string, string>
                {
                    { "query_id", "425892567746558" },
                    { "locale", InstaApiConstants.AcceptLanguage.Replace("-", "_") },
                    { "vc_policy", "ads_viewer_context_policy" },
                    { "signed_body", $"{httpHelper.ApiVersion.SignatureKey}." },
                    {
                        InstaApiConstants.HeaderIgSignatureKeyVersion,
                        InstaApiConstants.IgSignatureKeyVersion
                    },
                    { "strip_nulls", "true" },
                    { "strip_defaults", "true" },
                    { "query_params", queryParams.ToString(Formatting.None) }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBusinessCategoryList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessCategoryList>(
                    json,
                    new InstaBusinessCategoryDataConverter());
                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessCategoryList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessCategoryList>(exception);
            }
        }

        /// <summary>
        ///     Get full media insights
        /// </summary>
        /// <param name="mediaId">Media id (<see cref="InstaMedia.Identifier" />)</param>
        public async Task<IResult<InstaFullMediaInsights>> GetFullMediaInsightsAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri =
                    InstaUriCreator.GetGraphStatisticsUri(InstaApiConstants.AcceptLanguage, InstaInsightSurfaceType.Post);

                var queryParamsData = new JObject { { "access_token", "" }, { "id", mediaId } };
                var variables = new JObject { { "query_params", queryParamsData } };
                var data = new Dictionary<string, string>
                {
                    { "access_token", "undefined" },
                    { "fb_api_caller_class", "RelayModern" },
                    { "variables", variables.ToString(Formatting.None) },
                    { "doc_id", "1527362987318283" }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaFullMediaInsights>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaFullMediaInsightsRootResponse>(json);
                return Result.Success(InstaConvertersFabric.Instance.GetFullMediaInsightsConverter(obj.Data.Media)
                                          .Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFullMediaInsights), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaFullMediaInsights>(exception);
            }
        }

        /// <summary>
        ///     Get media insights
        /// </summary>
        /// <param name="mediaPk">Media PK (<see cref="InstaMedia.Pk" />)</param>
        public async Task<IResult<InstaMediaInsights>> GetMediaInsightsAsync(string mediaPk)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetMediaSingleInsightsUri(mediaPk);
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaMediaInsights>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaMediaInsightsContainer>(json);
                return Result.Success(obj.MediaOrganicInsights);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaMediaInsights), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaMediaInsights>(exception);
            }
        }

        /// <summary>
        ///     Get promotable media feeds
        /// </summary>
        public async Task<IResult<InstaMediaList>> GetPromotableMediaFeedsAsync()
        {
            var mediaList = new InstaMediaList();
            try
            {
                var instaUri = InstaUriCreator.GetPromotableMediaFeedsUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaMediaList>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaListResponse>(
                    json,
                    new InstaMediaListDataConverter());

                mediaList = InstaConvertersFabric.Instance.GetMediaListConverter(mediaResponse).Convert();
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

        /// <summary>
        ///     Get statistics of current account
        /// </summary>
        public async Task<IResult<InstaStatistics>> GetStatisticsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetGraphStatisticsUri(InstaApiConstants.AcceptLanguage);
                var queryParamsData = new JObject
                {
                    { "access_token", "" }, { "id", user.LoggedInUser.Pk.ToString() }
                };
                var variables = new JObject
                {
                    { "query_params", queryParamsData },
                    { "timezone", InstaApiConstants.Timezone },
                    { "activityTab", true },
                    { "audienceTab", true },
                    { "contentTab", true }
                };
                var data = new Dictionary<string, string>
                {
                    { "access_token", "undefined" },
                    { "fb_api_caller_class", "RelayModern" },
                    { "variables", variables.ToString(Formatting.None) },
                    { "doc_id", "1926322010754880" }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaStatistics>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaStatisticsRootResponse>(json);
                return Result.Success(InstaConvertersFabric.Instance.GetStatisticsConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaStatistics), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaStatistics>(exception);
            }
        }

        /// <summary>
        ///     Get sub categories of an category
        /// </summary>
        /// <param name="categoryId">Category id (Use <see cref="IBusinessProcessor.GetCategoriesAsync" /> to get category id)</param>
        public async Task<IResult<InstaBusinessCategoryList>> GetSubCategoriesAsync(string categoryId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (string.IsNullOrEmpty(categoryId))
                {
                    return Result.Fail<InstaBusinessCategoryList>("Category id cannot be null or empty");
                }

                var instaUri = InstaUriCreator.GetBusinessGraphQlUri();

                var queryParams = new JObject { { "0", categoryId } };
                var data = new Dictionary<string, string>
                {
                    { "query_id", "425892567746558" },
                    { "locale", InstaApiConstants.AcceptLanguage.Replace("-", "_") },
                    { "vc_policy", "ads_viewer_context_policy" },
                    { "signed_body", $"{httpHelper.ApiVersion.SignatureKey}." },
                    {
                        InstaApiConstants.HeaderIgSignatureKeyVersion,
                        InstaApiConstants.IgSignatureKeyVersion
                    },
                    { "strip_nulls", "true" },
                    { "strip_defaults", "true" },
                    { "query_params", queryParams.ToString(Formatting.None) }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBusinessCategoryList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessCategoryList>(
                    json,
                    new InstaBusinessCategoryDataConverter());
                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessCategoryList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessCategoryList>(exception);
            }
        }

        /// <summary>
        ///     Get suggested categories
        /// </summary>
        public async Task<IResult<InstaBusinessSuggestedCategoryList>> GetSuggestedCategoriesAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBusinessGraphQlUri();

                var zero = new JObject { { "page_name", user.UserName.ToLower() }, { "num_result", "5" } };
                var queryParams = new JObject { { "0", zero } };
                var data = new Dictionary<string, string>
                {
                    { "query_id", "706774002864790" },
                    { "locale", InstaApiConstants.AcceptLanguage.Replace("-", "_") },
                    { "vc_policy", "ads_viewer_context_policy" },
                    { "signed_body", $"{httpHelper.ApiVersion.SignatureKey}." },
                    {
                        InstaApiConstants.HeaderIgSignatureKeyVersion,
                        InstaApiConstants.IgSignatureKeyVersion
                    },
                    { "strip_nulls", "true" },
                    { "strip_defaults", "true" },
                    { "query_params", queryParams.ToString(Formatting.None) }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBusinessSuggestedCategoryList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessSuggestedCategoryList>(
                    json,
                    new InstaBusinessSuggestedCategoryDataConverter());
                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException,
                                   default(InstaBusinessSuggestedCategoryList),
                                   ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessSuggestedCategoryList>(exception);
            }
        }

        /// <summary>
        ///     Remove button from your business account
        /// </summary>
        public async Task<IResult<InstaBusinessUser>> RemoveBusinessButtonAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUpdateBusinessInfoUri();

                var data = new JObject
                {
                    { "is_call_to_action_enabled", "0" },
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
                    return Result.UnExpectedResponse<InstaBusinessUser>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessUserContainerResponse>(json);

                return Result.Success(InstaConvertersFabric.Instance.GetBusinessUserConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessUser), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessUser>(exception);
            }
        }

        /// <summary>
        ///     Remove business location
        /// </summary>
        public Task<IResult<InstaBusinessUser>> RemoveBusinessLocationAsync()
        {
            return UpdateBusinessInfoAsync(null, null, null, null, null);
        }

        /// <summary>
        ///     Remove users from approval branded whitelist
        /// </summary>
        /// <param name="userIdsToRemove">User ids (pk) to remove</param>
        public async Task<IResult<InstaBrandedContent>> RemoveUserFromBrandedWhiteListAsync(
            params long[] userIdsToRemove)
        {
            if (userIdsToRemove == null || userIdsToRemove != null && !userIdsToRemove.Any())
            {
                return Result.Fail<InstaBrandedContent>("At least one user id is require.");
            }

            return await UpdateBrandedContent(null, null, userIdsToRemove).ConfigureAwait(false);
        }

        /// <summary>
        ///     Search branded users for adding to your branded whitelist
        /// </summary>
        /// <param name="query">Query(name, username or...) to search</param>
        /// <param name="count">Count</param>
        public async Task<IResult<InstaDiscoverSearchResult>> SearchBrandedUsersAsync(string query, int count = 85)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (count < 10)
                {
                    count = 10;
                }

                var instaUri = InstaUriCreator.GetBusinessBrandedSearchUserUri(query, count);

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);

                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaDiscoverSearchResult>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDiscoverSearchResultResponse>(json);
                return Result.Success(InstaConvertersFabric.Instance.GetDiscoverSearchResultConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaDiscoverSearchResult), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaDiscoverSearchResult>(exception);
            }
        }

        /// <summary>
        ///     Search city location for business account
        /// </summary>
        /// <param name="cityOrTown">City/town name</param>
        public async Task<IResult<InstaBusinessCityLocationList>> SearchCityLocationAsync(string cityOrTown)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (string.IsNullOrEmpty(cityOrTown))
                {
                    return Result.Fail<InstaBusinessCityLocationList>("CityOrTown cannot be null or empty");
                }

                var instaUri = InstaUriCreator.GetBusinessGraphQlUri();

                var queryParams = new JObject { { "0", cityOrTown } };
                var data = new Dictionary<string, string>
                {
                    { "query_id", "1860980127555904" },
                    { "locale", InstaApiConstants.AcceptLanguage.Replace("-", "_") },
                    { "vc_policy", "ads_viewer_context_policy" },
                    { "signed_body", $"{httpHelper.ApiVersion.SignatureKey}." },
                    {
                        InstaApiConstants.HeaderIgSignatureKeyVersion,
                        InstaApiConstants.IgSignatureKeyVersion
                    },
                    { "strip_nulls", "true" },
                    { "strip_defaults", "true" },
                    { "query_params", queryParams.ToString(Formatting.None) }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBusinessCityLocationList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessCityLocationList>(
                    json,
                    new InstaBusinessCityLocationDataConverter());
                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessCityLocationList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessCityLocationList>(exception);
            }
        }

        /// <summary>
        ///     Update business information
        /// </summary>
        /// <param name="phoneNumberWithCountryCode">Phone number with country code [set null if you don't want to change it]</param>
        /// <param name="cityLocation">
        ///     City Location (get it from <see cref="IBusinessProcessor.SearchCityLocationAsync(string)" />
        ///     )
        /// </param>
        /// <param name="streetAddress">Street address</param>
        /// <param name="zipCode">Zip code</param>
        /// <param name="businessContactType">
        ///     Phone contact type (<see cref="InstaUserInfo.BusinessContactMethod" />) [set null if
        ///     you don't want to change it]
        /// </param>
        public async Task<IResult<InstaBusinessUser>> UpdateBusinessInfoAsync(
            string phoneNumberWithCountryCode,
            InstaBusinessCityLocation cityLocation,
            string streetAddress,
            string zipCode,
            InstaBusinessContactType? businessContactType)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var info = await GetBusinessAccountInformationAsync().ConfigureAwait(false);
                if (!info.Succeeded)
                {
                    return Result.Fail<InstaBusinessUser>("Cannot get current business information");
                }

                var user = info.Value;
                if (!user.IsBusiness)
                {
                    return Result.Fail<InstaBusinessUser>($"'{user.Username}' is not a business account");
                }

                var instaUri = InstaUriCreator.GetUpdateBusinessInfoUri();

                if (phoneNumberWithCountryCode == null)
                {
                    phoneNumberWithCountryCode = $"{user.PublicPhoneCountryCode}{user.PublicPhoneNumber}";
                }

                if (businessContactType == null)
                {
                    businessContactType = user.BusinessContactMethod;
                }

                var publicPhoneContact = new JObject
                {
                    { "public_phone_number", phoneNumberWithCountryCode },
                    { "business_contact_method", businessContactType.ToString().ToUpper() }
                };

                var cityId = "0";
                if (cityLocation != null)
                {
                    cityId = cityLocation.Id;
                }

                var businessAddress = new JObject
                {
                    { "address_street", streetAddress ?? string.Empty },
                    { "city_id", cityId },
                    { "zip", zipCode ?? string.Empty }
                };

                var data = new JObject
                {
                    { "page_id", user.PageId.Value.ToString() },
                    { "_csrftoken", this.user.CsrfToken },
                    { "public_phone_contact", publicPhoneContact.ToString(Formatting.None) },
                    { "_uid", this.user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "public_email", user.PublicEmail ?? string.Empty },
                    { "business_address", businessAddress.ToString(Formatting.None) }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBusinessUser>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessUserContainerResponse>(json);

                return Result.Success(InstaConvertersFabric.Instance.GetBusinessUserConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBusinessUser), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBusinessUser>(exception);
            }
        }

        /// <summary>
        ///     Validate an uri for an button(instagram partner)
        ///     <para>
        ///         Note: Use <see cref="IBusinessProcessor.GetBusinessPartnersButtonsAsync" /> to get business buttons(instagram
        ///         partner) list!
        ///     </para>
        /// </summary>
        /// <param name="desirePartner">
        ///     Desire partner (Use <see cref="IBusinessProcessor.GetBusinessPartnersButtonsAsync" /> to
        ///     get business buttons(instagram partner) list!)
        /// </param>
        /// <param name="uri">Uri to check (Must be related to desire partner!)</param>
        public async Task<IResult<bool>> ValidateUrlAsync(InstaBusinessPartner desirePartner, Uri uri)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (desirePartner?.AppId == null)
                {
                    return Result.Fail<bool>("Desire partner cannot be null");
                }

                if (uri == null)
                {
                    return Result.Fail<bool>("Uri cannot be null");
                }

                var instaUri = InstaUriCreator.GetBusinessValidateUrlUri();

                var data = new JObject
                {
                    { "app_id", desirePartner.AppId },
                    { "_csrftoken", user.CsrfToken },
                    { "url", uri.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaBusinessValidateUrl>(json);
                return obj.IsValid ? Result.Success(true) : Result.Fail<bool>(obj.ErrorMessage);
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
        ///     Star direct thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> StarDirectThreadAsync(string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetStarThreadUri(threadId);

                var data = new Dictionary<string, string>
                {
                    { "thread_label", "1" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Unstar direct thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> UnStarDirectThreadAsync(string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUnStarThreadUri(threadId);

                var data = new Dictionary<string, string>
                {
                    { "_csrftoken", user.CsrfToken }, { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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

        private async Task<IResult<InstaBrandedContent>> UpdateBrandedContent(
            int? approval = null,
            long[] usersToAdd = null,
            long[] usersToRemove = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetBusinessBrandedUpdateSettingsUri();
                var data = new JObject
                {
                    { "require_approval", (approval ?? 1).ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var addArray = new JArray();
                var removeArray = new JArray();

                if (usersToAdd != null && usersToAdd.Any())
                {
                    foreach (var item in usersToAdd)
                    {
                        addArray.Add($"{item}");
                    }
                }

                if (usersToRemove != null && usersToRemove.Any())
                {
                    foreach (var item in usersToRemove)
                    {
                        removeArray.Add($"{item}");
                    }
                }

                data.Add("added_user_ids", addArray);
                data.Add("removed_user_ids", removeArray);

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);

                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBrandedContent>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBrandedContentResponse>(json);
                return Result.Success(InstaConvertersFabric.Instance.GetBrandedContentConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBrandedContent), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBrandedContent>(exception);
            }
        }
    }
}