using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Discover api functions.
    /// </summary>
    internal class InstaDiscoverProcessor : IDiscoverProcessor
    {
        private readonly InstaAndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly InstaUserAuthValidate userAuthValidate;

        public InstaDiscoverProcessor(
            InstaAndroidDevice deviceInfo,
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
        ///     Clear Recent searches
        /// </summary>
        public async Task<IResult<bool>> ClearRecentSearchsAsync()
        {
            try
            {
                var instaUri = InstaUriCreator.GetClearSearchHistoryUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken }, { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                return obj.Status == "ok" ? InstaResult.Success(true) : InstaResult.UnExpectedResponse<bool>(response, json);
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
        ///     Get discover user chaining list
        /// </summary>
        public async Task<IResult<InstaUserChainingList>> GetChainingUsersAsync()
        {
            try
            {
                var instaUri = InstaUriCreator.GetDiscoverChainingUri(user.LoggedInUser.Pk);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserChainingList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserChainingContainerResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetUserChainingListConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserChainingList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUserChainingList>(exception);
            }
        }

        /// <summary>
        ///     Get recent searches
        /// </summary>
        public async Task<IResult<InstaDiscoverRecentSearches>> GetRecentSearchesAsync()
        {
            try
            {
                var instaUri = InstaUriCreator.GetRecentSearchUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDiscoverRecentSearches>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDiscoverRecentSearchesResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetDiscoverRecentSearchesConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDiscoverRecentSearches), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDiscoverRecentSearches>(exception);
            }
        }

        /// <summary>
        ///     Get suggested searches
        /// </summary>
        /// <param name="searchType">Search type(only blended and users works)</param>
        public async Task<IResult<InstaDiscoverSuggestedSearches>> GetSuggestedSearchesAsync(
            InstaDiscoverSearchType searchType =
                InstaDiscoverSearchType.Users)
        {
            try
            {
                var instaUri = InstaUriCreator.GetSuggestedSearchUri(searchType);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDiscoverSuggestedSearches>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDiscoverSuggestedSearchesResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetDiscoverSuggestedSearchesConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDiscoverSuggestedSearches), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDiscoverSuggestedSearches>(exception);
            }
        }

        /// <summary>
        ///     Get top searches
        /// </summary>
        /// <param name="querry">querry string of the search</param>
        /// <param name="searchType">Search type(only blended and users works)</param>
        /// <param name="timezoneOffset">
        ///     Timezone offset of the search region (GMT Offset * 60 * 60 - Like Tehran GMT +3:30 = 3.5*
        ///     60*60 = 12600)
        /// </param>
        /// <returns></returns>
        public async Task<IResult<InstaDiscoverTopSearches>> GetTopSearchesAsync(
            string querry = "",
            InstaDiscoverSearchType searchType = InstaDiscoverSearchType.Users,
            int timezoneOffset = 12600)
        {
            try
            {
                var instaUri = InstaUriCreator.GetTopSearchUri(user.RankToken, querry, searchType, timezoneOffset);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDiscoverTopSearches>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDiscoverTopSearchesResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetDiscoverTopSearchesConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDiscoverTopSearches), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDiscoverTopSearches>(exception);
            }
        }

        /// <summary>
        ///     Search user people
        /// </summary>
        /// <param name="query">Text to search</param>
        /// <param name="count">Count</param>
        public async Task<IResult<InstaDiscoverSearchResult>> SearchPeopleAsync(string query, int count = 30)
        {
            try
            {
                var instaUri = InstaUriCreator.GetSearchUserUri(query, count);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDiscoverSearchResult>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDiscoverSearchResultResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetDiscoverSearchResultConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDiscoverSearchResult), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDiscoverSearchResult>(exception);
            }
        }

        /// <summary>
        ///     Sync your phone contact list to instagram
        ///     <para>Note:You can find your friends in instagram with this function</para>
        /// </summary>
        /// <param name="instaContacts">Contact list</param>
        public async Task<IResult<InstaContactUserList>> SyncContactsAsync(params InstaContact[] instaContacts)
        {
            try
            {
                var contacts = new InstaContactList();
                contacts.AddRange(instaContacts);
                return await SyncContactsAsync(contacts).ConfigureAwait(false);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaContactUserList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaContactUserList>(exception);
            }
        }

        /// <summary>
        ///     Sync your phone contact list to instagram
        ///     <para>Note:You can find your friends in instagram with this function</para>
        /// </summary>
        /// <param name="instaContacts">Contact list</param>
        public async Task<IResult<InstaContactUserList>> SyncContactsAsync(InstaContactList instaContacts)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSyncContactsUri();

                var jsonContacts = JsonConvert.SerializeObject(instaContacts);

                var fields = new Dictionary<string, string> { { "contacts", jsonContacts } };

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, fields);

                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaContactUserList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaContactUserListResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetUserContactListConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaContactUserList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaContactUserList>(exception);
            }
        }

        /// <summary>
        ///     NOT COMPLETE
        /// </summary>
        /// <returns></returns>
        private async Task<IResult<object>> DiscoverPeopleAsync()
        {
            try
            {
                var instaUri = InstaUriCreator.GetDiscoverPeopleUri();
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "phone_id", deviceInfo.DeviceGuid.ToString() },
                    { "module", "discover_people" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "paginate", "true" }

                    //{"_uid", _user.LoggedInUder.Pk.ToString()},
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDefaultResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefaultResponse>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDefaultResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDefaultResponse>(exception);
            }
        }
    }
}