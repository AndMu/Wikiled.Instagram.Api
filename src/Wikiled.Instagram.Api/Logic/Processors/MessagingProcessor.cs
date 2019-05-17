using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Messaging (direct) api functions.
    /// </summary>
    internal class InstaMessagingProcessor : IMessagingProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly InstaUserAuthValidate userAuthValidate;

        public InstaMessagingProcessor(
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
        ///     Add users to group thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="userIds">User ids (pk)</param>
        public async Task<IResult<InstaDirectInboxThread>> AddUserToGroupThreadAsync(
            string threadId,
            params long[] userIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (userIds == null || userIds != null && !userIds.Any())
                {
                    throw new ArgumentException("UserIds cannot be null or empty.\nAt least one user id require.");
                }

                var instaUri = InstaUriCreator.GetAddUserToDirectThreadUri(threadId);

                var data = new Dictionary<string, string>
                {
                    { "use_unified_inbox", "true" },
                    { "user_ids", $"[{userIds.EncodeList()}]" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDirectInboxThread>(response, json);
                }

                var threadResponse = JsonConvert.DeserializeObject<InstaDirectInboxThreadResponse>(
                    json,
                    new InstaThreadDataConverter());

                //Reverse for Chat Order
                threadResponse.Items.Reverse();
                var converter = InstaConvertersFabric.Instance.GetDirectThreadConverter(threadResponse);

                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDirectInboxThread), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxThread>(exception);
            }
        }

        /// <summary>
        ///     Approve direct pending request
        /// </summary>
        /// <param name="threadIds">Thread id</param>
        public async Task<IResult<bool>> ApproveDirectPendingRequestAsync(params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new Dictionary<string, string>
                {
                    { "_csrftoken", user.CsrfToken }, { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                Uri instaUri;
                if (threadIds.Length == 1)
                {
                    instaUri = InstaUriCreator.GetApprovePendingDirectRequestUri(threadIds.FirstOrDefault());
                }
                else
                {
                    instaUri = InstaUriCreator.GetApprovePendingMultipleDirectRequestUri();
                    data.Add("thread_ids", threadIds.EncodeList(false));
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefaultResponse>(json);
                if (obj.IsSucceed)
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.UnExpectedResponse<bool>(response, json);
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
        ///     Decline all direct pending requests
        /// </summary>
        public Task<IResult<bool>> DeclineAllDirectPendingRequestsAsync()
        {
            return DeclineDirectPendingRequests(true);
        }

        /// <summary>
        ///     Decline direct pending requests
        /// </summary>
        /// <param name="threadIds">Thread ids</param>
        public Task<IResult<bool>> DeclineDirectPendingRequestsAsync(params string[] threadIds)
        {
            return DeclineDirectPendingRequests(false, threadIds);
        }

        /// <summary>
        ///     Delete direct thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> DeleteDirectThreadAsync(string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetHideDirectThreadUri(threadId);

                var data = new Dictionary<string, string>
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "use_unified_inbox", "true" }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Delete self message in direct
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> DeleteSelfMessageAsync(string threadId, string itemId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDeleteDirectMessageUri(threadId, itemId);

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
        ///     Get direct inbox threads for current user asynchronously
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.InstaDirectInboxContainer" />
        /// </returns>
        public async Task<IResult<InstaDirectInboxContainer>> GetDirectInboxAsync(
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaDirectInboxContainer Convert(InstaDirectInboxContainerResponse inboxContainerResponse)
                {
                    return InstaConvertersFabric.Instance.GetDirectInboxConverter(inboxContainerResponse).Convert();
                }

                var inbox = await GetDirectInbox(paginationParameters.NextMaxId).ConfigureAwait(false);
                if (!inbox.Succeeded)
                {
                    return InstaResult.Fail(inbox.Info, default(InstaDirectInboxContainer));
                }

                var inboxResponse = inbox.Value;
                paginationParameters.NextMaxId = inboxResponse.Inbox.OldestCursor;
                var pagesLoaded = 1;
                while (inboxResponse.Inbox.HasOlder &&
                    !string.IsNullOrEmpty(inboxResponse.Inbox.OldestCursor) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextInbox = await GetDirectInbox(inboxResponse.Inbox.OldestCursor).ConfigureAwait(false);

                    if (!nextInbox.Succeeded)
                    {
                        return InstaResult.Fail(nextInbox.Info, Convert(nextInbox.Value));
                    }

                    inboxResponse.Inbox.OldestCursor =
                        paginationParameters.NextMaxId = nextInbox.Value.Inbox.OldestCursor;
                    inboxResponse.Inbox.HasOlder = nextInbox.Value.Inbox.HasOlder;
                    inboxResponse.Inbox.BlendedInboxEnabled = nextInbox.Value.Inbox.BlendedInboxEnabled;
                    inboxResponse.Inbox.UnseenCount = nextInbox.Value.Inbox.UnseenCount;
                    inboxResponse.Inbox.UnseenCountTs = nextInbox.Value.Inbox.UnseenCountTs;
                    inboxResponse.Inbox.Threads.AddRange(nextInbox.Value.Inbox.Threads);
                    pagesLoaded++;
                }

                return InstaResult.Success(InstaConvertersFabric.Instance.GetDirectInboxConverter(inboxResponse).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDirectInboxContainer), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxContainer>(exception);
            }
        }

        /// <summary>
        ///     Get direct inbox thread by its id asynchronously
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaDirectInboxThread" />
        /// </returns>
        public async Task<IResult<InstaDirectInboxThread>> GetDirectInboxThreadAsync(
            string threadId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                var thread = await GetDirectInboxThread(threadId, paginationParameters.NextMaxId).ConfigureAwait(false);
                if (!thread.Succeeded)
                {
                    return InstaResult.Fail(thread.Info, default(InstaDirectInboxThread));
                }

                InstaDirectInboxThread Convert(InstaDirectInboxThreadResponse inboxThreadResponse)
                {
                    return InstaConvertersFabric.Instance.GetDirectThreadConverter(inboxThreadResponse).Convert();
                }

                var threadResponse = thread.Value;
                paginationParameters.NextMaxId = threadResponse.OldestCursor;
                var pagesLoaded = 1;

                while (threadResponse.HasOlder &&
                    !string.IsNullOrEmpty(threadResponse.OldestCursor) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextThread = await GetDirectInboxThread(threadId, threadResponse.OldestCursor).ConfigureAwait(false);

                    if (!nextThread.Succeeded)
                    {
                        return InstaResult.Fail(nextThread.Info, Convert(nextThread.Value));
                    }

                    threadResponse.OldestCursor = paginationParameters.NextMaxId = nextThread.Value.OldestCursor;
                    threadResponse.HasOlder = nextThread.Value.HasOlder;
                    threadResponse.Canonical = nextThread.Value.Canonical;
                    threadResponse.ExpiringMediaReceiveCount = nextThread.Value.ExpiringMediaReceiveCount;
                    threadResponse.ExpiringMediaSendCount = nextThread.Value.ExpiringMediaSendCount;
                    threadResponse.HasNewer = nextThread.Value.HasNewer;
                    threadResponse.LastActivity = nextThread.Value.LastActivity;
                    threadResponse.LastSeenAt = nextThread.Value.LastSeenAt;
                    threadResponse.ReshareReceiveCount = nextThread.Value.ReshareReceiveCount;
                    threadResponse.ReshareSendCount = nextThread.Value.ReshareSendCount;
                    threadResponse.Status = nextThread.Value.Status;
                    threadResponse.Title = nextThread.Value.Title;
                    threadResponse.IsGroup = nextThread.Value.IsGroup;
                    threadResponse.IsSpam = nextThread.Value.IsSpam;
                    threadResponse.IsPin = nextThread.Value.IsPin;
                    threadResponse.Muted = nextThread.Value.Muted;
                    threadResponse.PendingScore = nextThread.Value.PendingScore;
                    threadResponse.Pending = nextThread.Value.Pending;
                    threadResponse.Users = nextThread.Value.Users;
                    threadResponse.ValuedRequest = nextThread.Value.ValuedRequest;
                    threadResponse.VcMuted = nextThread.Value.VcMuted;
                    threadResponse.VieweId = nextThread.Value.VieweId;
                    threadResponse.Items.AddRange(nextThread.Value.Items);
                    pagesLoaded++;
                }

                //Reverse for Chat Order
                threadResponse.Items.Reverse();
                var converter = InstaConvertersFabric.Instance.GetDirectThreadConverter(threadResponse);

                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDirectInboxThread), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxThread>(exception);
            }
        }

        /// <summary>
        ///     Get direct pending inbox threads for current user asynchronously
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.InstaDirectInboxContainer" />
        /// </returns>
        public async Task<IResult<InstaDirectInboxContainer>> GetPendingDirectAsync(
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaDirectInboxContainer Convert(InstaDirectInboxContainerResponse inboxContainerResponse)
                {
                    return InstaConvertersFabric.Instance.GetDirectInboxConverter(inboxContainerResponse).Convert();
                }

                var inbox = await GetPendingDirect(paginationParameters.NextMaxId).ConfigureAwait(false);
                if (!inbox.Succeeded)
                {
                    return InstaResult.Fail(inbox.Info, default(InstaDirectInboxContainer));
                }

                var inboxResponse = inbox.Value;
                paginationParameters.NextMaxId = inboxResponse.Inbox.OldestCursor;
                var pagesLoaded = 1;
                while (inboxResponse.Inbox.HasOlder &&
                    !string.IsNullOrEmpty(inboxResponse.Inbox.OldestCursor) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextInbox = await GetPendingDirect(inboxResponse.Inbox.OldestCursor).ConfigureAwait(false);

                    if (!nextInbox.Succeeded)
                    {
                        return InstaResult.Fail(nextInbox.Info, Convert(nextInbox.Value));
                    }

                    inboxResponse.Inbox.OldestCursor =
                        paginationParameters.NextMaxId = nextInbox.Value.Inbox.OldestCursor;
                    inboxResponse.Inbox.HasOlder = nextInbox.Value.Inbox.HasOlder;
                    inboxResponse.Inbox.Threads.AddRange(nextInbox.Value.Inbox.Threads);
                    inboxResponse.Inbox.BlendedInboxEnabled = nextInbox.Value.Inbox.BlendedInboxEnabled;
                    inboxResponse.Inbox.UnseenCount = nextInbox.Value.Inbox.UnseenCount;
                    inboxResponse.Inbox.UnseenCountTs = nextInbox.Value.Inbox.UnseenCountTs;
                    pagesLoaded++;
                }

                return InstaResult.Success(InstaConvertersFabric.Instance.GetDirectInboxConverter(inboxResponse).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDirectInboxContainer), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxContainer>(exception);
            }
        }

        /// <summary>
        ///     Get ranked recipients (threads and users) asynchronously
        /// </summary>
        /// <returns>
        ///     <see cref="InstaRecipients" />
        /// </returns>
        public Task<IResult<InstaRecipients>> GetRankedRecipientsAsync()
        {
            return GetRankedRecipientsByUsernameAsync(null);
        }

        /// <summary>
        ///     Get ranked recipients (threads and users) asynchronously
        ///     <para>Note: Some recipient has User, some recipient has Thread</para>
        /// </summary>
        /// <param name="username">Username to search</param>
        /// <returns>
        ///     <see cref="InstaRecipients" />
        /// </returns>
        public async Task<IResult<InstaRecipients>> GetRankedRecipientsByUsernameAsync(string username)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                Uri instaUri;
                if (string.IsNullOrEmpty(username))
                {
                    instaUri = InstaUriCreator.GetRankedRecipientsUri();
                }
                else
                {
                    instaUri = InstaUriCreator.GetRankRecipientsByUserUri(username);
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaRecipients>(response, json);
                }

                var responseRecipients = JsonConvert.DeserializeObject<InstaRankedRecipientsResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetRecipientsConverter(responseRecipients);
                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaRecipients), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaRecipients>(exception);
            }
        }

        /// <summary>
        ///     Get recent recipients (threads and users) asynchronously
        /// </summary>
        /// <returns>
        ///     <see cref="InstaRecipients" />
        /// </returns>
        public async Task<IResult<InstaRecipients>> GetRecentRecipientsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userUri = InstaUriCreator.GetRecentRecipientsUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaRecipients>(response, json);
                }

                var responseRecipients = JsonConvert.DeserializeObject<InstaRecentRecipientsResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetRecipientsConverter(responseRecipients);
                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaRecipients), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaRecipients>(exception);
            }
        }

        /// <summary>
        ///     Get direct users presence
        ///     <para>Note: You can use this function to find out who is online and who isn't.</para>
        /// </summary>
        public async Task<IResult<InstaUserPresenceList>> GetUsersPresenceAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDirectPresenceUri();

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserPresenceList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserPresenceContainerResponse>(
                    json,
                    new InstaUserPresenceContainerDataConverter());
                return InstaResult.Success(InstaConvertersFabric.Instance.GetUserPresenceListConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserPresenceList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUserPresenceList>(exception);
            }
        }

        /// <summary>
        ///     Leave from group thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> LeaveGroupThreadAsync(string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetLeaveThreadUri(threadId);
                var clientContext = Guid.NewGuid().ToString();
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
        ///     Like direct message in a thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="itemId">Item id (message id)</param>
        public async Task<IResult<bool>> LikeThreadMessageAsync(string threadId, string itemId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetLikeUnlikeDirectMessageUri();

                var data = new Dictionary<string, string>
                {
                    { "item_type", "reaction" },
                    { "reaction_type", "like" },
                    { "action", "send_item" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "thread_ids", $"[{threadId}]" },
                    { "client_context", Guid.NewGuid().ToString() },
                    { "node_type", "item" },
                    { "reaction_status", "created" },
                    { "item_id", itemId }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Mark direct message as seen
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="itemId">Message id (item id)</param>
        public async Task<IResult<bool>> MarkDirectThreadAsSeenAsync(string threadId, string itemId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDirectThreadSeenUri(threadId, itemId);

                var data = new Dictionary<string, string>
                {
                    { "thread_id", threadId },
                    { "action", "mark_seen" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "item_id", itemId },
                    { "use_unified_inbox", "true" }
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
        ///     Mute direct thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> MuteDirectThreadAsync(string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetMuteDirectThreadUri(threadId);

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
        ///     Send disappearing photo to direct thread (video will remove after user saw it)
        /// </summary>
        /// <param name="image">Image to upload</param>
        /// <param name="viewMode">View mode</param>
        /// <param name="threadIds">Thread ids</param>
        public Task<IResult<bool>> SendDirectDisappearingPhotoAsync(InstaImage image, InstaViewMode viewMode = InstaViewMode.Replayable, params string[] threadIds)
        {
            return SendDirectDisappearingPhotoAsync(null, image, viewMode, threadIds);
        }

        /// <summary>
        ///     Send disappearing photo to direct thread (video will remove after user saw it) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="image">Image to upload</param>
        /// <param name="viewMode">View mode</param>
        /// <param name="threadIds">Thread ids</param>
        public async Task<IResult<bool>> SendDirectDisappearingPhotoAsync(
            Action<InstaUploaderProgress> progress,
            InstaImage image,
            InstaViewMode viewMode = InstaViewMode.Replayable,
            params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendPhotoAsync(progress,
                                                                 false,
                                                                 true,
                                                                 "",
                                                                 viewMode,
                                                                 InstaStoryType.Direct,
                                                                 null,
                                                                 threadIds.EncodeList(),
                                                                 image).ConfigureAwait(false);
        }

        /// <summary>
        ///     Send disappearing video to direct thread (video will remove after user saw it)
        /// </summary>
        /// <param name="video">Video to upload</param>
        /// <param name="viewMode">View mode</param>
        /// <param name="threadIds">Thread ids</param>
        public Task<IResult<bool>> SendDirectDisappearingVideoAsync(InstaVideoUpload video, InstaViewMode viewMode = InstaViewMode.Replayable, params string[] threadIds)
        {
            return SendDirectDisappearingVideoAsync(null, video, viewMode, threadIds);
        }

        /// <summary>
        ///     Send disappearing video to direct thread (video will remove after user saw it) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload</param>
        /// <param name="viewMode">View mode</param>
        /// <param name="threadIds">Thread ids</param>
        public async Task<IResult<bool>> SendDirectDisappearingVideoAsync(
            Action<InstaUploaderProgress> progress,
            InstaVideoUpload video,
            InstaViewMode viewMode = InstaViewMode.Replayable,
            params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendVideoAsync(progress,
                                                                 false,
                                                                 true,
                                                                 "",
                                                                 viewMode,
                                                                 InstaStoryType.Direct,
                                                                 null,
                                                                 threadIds.EncodeList(),
                                                                 video).ConfigureAwait(false);
        }

        /// <summary>
        ///     Send hashtag to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="hashtag">Hashtag to send</param>
        /// <param name="threadIds">Thread ids</param>
        /// <returns>Returns True if hashtag sent</returns>
        public Task<IResult<bool>> SendDirectHashtagAsync(string text, string hashtag, params string[] threadIds)
        {
            return SendDirectHashtagAsync(text, hashtag, threadIds, null);
        }

        /// <summary>
        ///     Send hashtag to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="hashtag">Hashtag to send</param>
        /// <param name="threadIds">Thread ids</param>
        /// <param name="recipients">Recipients ids</param>
        /// <returns>Returns True if hashtag sent</returns>
        public async Task<IResult<bool>> SendDirectHashtagAsync(string text,
                                                                string hashtag,
                                                                string[] threadIds,
                                                                string[] recipients)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSendDirectHashtagUri();
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "text", text ?? string.Empty },
                    { "hashtag", hashtag },
                    { "action", "send_item" },
                    { "client_context", clientContext },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                if (threadIds?.Length > 0)
                {
                    data.Add("thread_ids", $"[{threadIds.EncodeList(false)}]");
                }

                if (recipients?.Length > 0)
                {
                    data.Add("recipient_users", "[[" + recipients.EncodeList(false) + "]]");
                }

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Send hashtag to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="hashtag">Hashtag to send</param>
        /// <param name="threadIds">Thread ids</param>
        /// <param name="recipients">Recipients ids</param>
        /// <returns>Returns True if hashtag sent</returns>
        public Task<IResult<bool>> SendDirectHashtagToRecipientsAsync(string text, string hashtag, params string[] recipients)
        {
            return SendDirectHashtagAsync(text, hashtag, null, recipients);
        }

        /// <summary>
        ///     Send a like to the conversation
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> SendDirectLikeAsync(string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDirectThreadBroadcastLikeUri();

                var data = new Dictionary<string, string>
                {
                    { "action", "send_item" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "thread_id", $"{threadId}" },
                    { "client_context", Guid.NewGuid().ToString() }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Send link address to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="link">Link to send</param>
        /// <param name="threadIds">Thread ids</param>
        public Task<IResult<bool>> SendDirectLinkAsync(string text, string link, params string[] threadIds)
        {
            return SendDirectLinkAsync(text, link, threadIds, null);
        }

        /// <summary>
        ///     Send link address to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="link">Link to send</param>
        /// <param name="threadIds">Thread ids</param>
        /// <param name="recipients">Recipients ids</param>
        public async Task<IResult<bool>> SendDirectLinkAsync(string text,
                                                             string link,
                                                             string[] threadIds,
                                                             string[] recipients)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSendDirectLinkUri();
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "link_text", text ?? string.Empty },
                    { "link_urls", $"[{new[] { link }.EncodeList()}]" },
                    { "action", "send_item" },
                    { "client_context", clientContext },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                if (threadIds?.Length > 0)
                {
                    data.Add("thread_ids", $"[{threadIds.EncodeList(false)}]");
                }

                if (recipients?.Length > 0)
                {
                    data.Add("recipient_users", "[[" + recipients.EncodeList(false) + "]]");
                }

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Send link address to direct thread
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="link">Link to send</param>
        /// <param name="recipients">Recipients ids</param>
        public Task<IResult<bool>> SendDirectLinkToRecipientsAsync(string text, string link, params string[] recipients)
        {
            return SendDirectLinkAsync(text, link, null, recipients);
        }

        /// <summary>
        ///     Send location to direct thread
        /// </summary>
        /// <param name="externalId">External id (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" /></param>
        /// <param name="threadIds">Thread ids</param>
        public async Task<IResult<bool>> SendDirectLocationAsync(string externalId, params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSendDirectLocationUri();
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "venue_id", externalId },
                    { "action", "send_item" },
                    { "thread_ids", $"[{threadIds.EncodeList(false)}]" },
                    { "client_context", clientContext },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Send photo to direct thread (single)
        /// </summary>
        /// <param name="image">Image to upload</param>
        /// <param name="threadId">Thread id</param>
        /// <returns>Returns True is sent</returns>
        public Task<IResult<bool>> SendDirectPhotoAsync(InstaImage image, string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return SendDirectPhotoAsync(null, image, threadId);
        }

        /// <summary>
        ///     Send photo to direct thread (single) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="image">Image to upload</param>
        /// <param name="threadId">Thread id</param>
        /// <returns>Returns True is sent</returns>
        public Task<IResult<bool>> SendDirectPhotoAsync(Action<InstaUploaderProgress> progress, InstaImage image, string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return SendDirectPhoto(progress, null, threadId, image);
        }

        /// <summary>
        ///     Send photo to multiple recipients (multiple user)
        /// </summary>
        /// <param name="image">Image to upload</param>
        /// <param name="recipients">Recipients (user ids/pk)</param>
        /// <returns>Returns True is sent</returns>
        public Task<IResult<bool>> SendDirectPhotoToRecipientsAsync(InstaImage image, params string[] recipients)
        {
            return SendDirectPhotoToRecipientsAsync(null, image, recipients);
        }

        /// <summary>
        ///     Send photo to multiple recipients (multiple user) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="image">Image to upload</param>
        /// <param name="recipients">Recipients (user ids/pk)</param>
        /// <returns>Returns True is sent</returns>
        public Task<IResult<bool>> SendDirectPhotoToRecipientsAsync(Action<InstaUploaderProgress> progress, InstaImage image, params string[] recipients)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return SendDirectPhoto(progress, string.Join(",", recipients), null, image);
        }

        /// <summary>
        ///     Send profile to direct thrad
        /// </summary>
        /// <param name="userIdToSend">User id to send</param>
        /// <param name="threadIds">Thread ids</param>
        public async Task<IResult<bool>> SendDirectProfileAsync(long userIdToSend, params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSendDirectProfileUri();
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "profile_user_id", userIdToSend.ToString() },
                    { "action", "send_item" },
                    { "thread_ids", $"[{threadIds.EncodeList(false)}]" },
                    { "client_context", clientContext },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Send profile to direct thrad
        /// </summary>
        /// <param name="userIdToSend">User id to send</param>
        /// <param name="threadIds">Thread ids</param>
        public async Task<IResult<bool>> SendDirectProfileToRecipientsAsync(long userIdToSend, string recipients)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSendDirectProfileUri();
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "profile_user_id", userIdToSend.ToString() },
                    { "action", "send_item" },
                    { "recipient_users", "[[" + recipients + "]]" },
                    { "client_context", clientContext },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Send direct text message to provided users and threads
        /// </summary>
        /// <param name="recipients">Comma-separated users PK</param>
        /// <param name="threadIds">Message thread ids</param>
        /// <param name="text">Message text</param>
        /// <returns>List of threads</returns>
        public async Task<IResult<InstaDirectInboxThreadList>> SendDirectTextAsync(
            string recipients,
            string threadIds,
            string text)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var threads = new InstaDirectInboxThreadList();
            try
            {
                var directSendMessageUri = InstaUriCreator.GetDirectSendMessageUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, directSendMessageUri, deviceInfo);
                var fields = new Dictionary<string, string> { { "text", text } };
                if (!string.IsNullOrEmpty(recipients))
                {
                    fields.Add("recipient_users", "[[" + recipients + "]]");
                }
                else
                {
                    fields.Add("recipient_users", "[]");
                }

                if (!string.IsNullOrEmpty(threadIds))
                {
                    fields.Add("thread_ids", "[" + threadIds + "]");
                }

                request.Content = new FormUrlEncodedContent(fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDirectInboxThreadList>(response, json);
                }

                var result = JsonConvert.DeserializeObject<InstaSendDirectMessageResponse>(json);
                if (!result.IsOk())
                {
                    return InstaResult.Fail<InstaDirectInboxThreadList>(result.Status);
                }

                threads.AddRange(
                    result.Threads.Select(
                        thread =>
                            InstaConvertersFabric.Instance.GetDirectThreadConverter(thread).Convert()));
                return InstaResult.Success(threads);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDirectInboxThreadList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxThreadList>(exception);
            }
        }

        /// <summary>
        ///     Send video to direct thread (single)
        /// </summary>
        /// <param name="video">Video to upload (no need to set thumbnail)</param>
        /// <param name="threadId">Thread id</param>
        public Task<IResult<bool>> SendDirectVideoAsync(InstaVideoUpload video, string threadId)
        {
            return SendDirectVideoAsync(null, video, threadId);
        }

        /// <summary>
        ///     Send video to direct thread (single) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload (no need to set thumbnail)</param>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> SendDirectVideoAsync(Action<InstaUploaderProgress> progress,
                                                              InstaVideoUpload video,
                                                              string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendVideoAsync(progress,
                                                                 true,
                                                                 false,
                                                                 "",
                                                                 InstaViewMode.Replayable,
                                                                 InstaStoryType.Both,
                                                                 null,
                                                                 threadId,
                                                                 video).ConfigureAwait(false);
        }

        /// <summary>
        ///     Send video to multiple recipients (multiple user)
        /// </summary>
        /// <param name="video">Video to upload (no need to set thumbnail)</param>
        /// <param name="recipients">Recipients (user ids/pk)</param>
        public Task<IResult<bool>> SendDirectVideoToRecipientsAsync(InstaVideoUpload video, params string[] recipients)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return SendDirectVideoToRecipientsAsync(null, video, recipients);
        }

        /// <summary>
        ///     Send video to multiple recipients (multiple user) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video to upload (no need to set thumbnail)</param>
        /// <param name="recipients">Recipients (user ids/pk)</param>
        public async Task<IResult<bool>> SendDirectVideoToRecipientsAsync(
            Action<InstaUploaderProgress> progress,
            InstaVideoUpload video,
            params string[] recipients)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendVideoAsync(progress,
                                                                 true,
                                                                 false,
                                                                 "",
                                                                 InstaViewMode.Replayable,
                                                                 InstaStoryType.Both,
                                                                 recipients.EncodeList(false),
                                                                 null,
                                                                 video).ConfigureAwait(false);
        }

        /// <summary>
        ///     Share media to direct thread
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="mediaType">Media type</param>
        /// <param name="text">Text to send</param>
        /// <param name="threadIds">Thread ids</param>
        public async Task<IResult<bool>> ShareMediaToThreadAsync(string mediaId, InstaMediaType mediaType, string text, params string[] threadIds)
        {
            try
            {
                if (threadIds == null || threadIds != null && !threadIds.Any())
                {
                    throw new ArgumentException("At least one thread id required");
                }

                return await ShareMedia(mediaId, mediaType, text, threadIds, null).ConfigureAwait(false);
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
        ///     Share media to user id
        /// </summary>
        /// <param name="mediaId">Media id</param>
        /// <param name="mediaType">Media type</param>
        /// <param name="text">Text to send</param>
        /// <param name="userIds">User ids (pk)</param>
        public async Task<IResult<bool>> ShareMediaToUserAsync(string mediaId, InstaMediaType mediaType, string text, params long[] userIds)
        {
            try
            {
                if (userIds == null || userIds != null && !userIds.Any())
                {
                    throw new ArgumentException("At least one user id required");
                }

                return await ShareMedia(mediaId, mediaType, text, null, userIds).ConfigureAwait(false);
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
        ///     UnLike direct message in a thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="itemId">Item id (message id)</param>
        public async Task<IResult<bool>> UnLikeThreadMessageAsync(string threadId, string itemId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetLikeUnlikeDirectMessageUri();

                var data = new Dictionary<string, string>
                {
                    { "item_type", "reaction" },
                    { "reaction_type", "like" },
                    { "action", "send_item" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "thread_ids", $"[{threadId}]" },
                    { "client_context", Guid.NewGuid().ToString() },
                    { "node_type", "item" },
                    { "reaction_status", "deleted" },
                    { "item_id", itemId }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
        ///     Unmute direct thread
        /// </summary>
        /// <param name="threadId">Thread id</param>
        public async Task<IResult<bool>> UnMuteDirectThreadAsync(string threadId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUnMuteDirectThreadUri(threadId);

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
        ///     Update direct thread title (for groups)
        /// </summary>
        /// <param name="threadId">Thread id</param>
        /// <param name="title">New title</param>
        public async Task<IResult<bool>> UpdateDirectThreadTitleAsync(string threadId, string title)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDirectThreadUpdateTitleUri(threadId);

                var data = new Dictionary<string, string>
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "title", title }
                };
                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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

        private async Task<IResult<bool>> DeclineDirectPendingRequests(bool all, params string[] threadIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDeclineAllPendingDirectRequestsUri();

                var data = new Dictionary<string, string>
                {
                    { "_csrftoken", user.CsrfToken }, { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                if (!all)
                {
                    if (threadIds.Length == 1)
                    {
                        instaUri = InstaUriCreator.GetDeclinePendingDirectRequestUri(threadIds.FirstOrDefault());
                    }
                    else
                    {
                        instaUri = InstaUriCreator.GetDeclineMultplePendingDirectRequestsUri();
                        data.Add("thread_ids", threadIds.EncodeList(false));
                    }
                }

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefaultResponse>(json);
                if (obj.IsSucceed)
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Fail("Error: " + obj.Message, false);
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

        private async Task<IResult<InstaDirectInboxContainerResponse>> GetDirectInbox(string maxId = null)
        {
            try
            {
                var directInboxUri = InstaUriCreator.GetDirectInboxUri(maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, directInboxUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDirectInboxContainerResponse>(response, json);
                }

                var inboxResponse = JsonConvert.DeserializeObject<InstaDirectInboxContainerResponse>(json);
                return InstaResult.Success(inboxResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaDirectInboxContainerResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxContainerResponse>(exception);
            }
        }

        private async Task<IResult<InstaDirectInboxThreadResponse>> GetDirectInboxThread(
            string threadId,
            string maxId = null)
        {
            try
            {
                var directInboxUri = InstaUriCreator.GetDirectInboxThreadUri(threadId, maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, directInboxUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDirectInboxThreadResponse>(response, json);
                }

                var threadResponse = JsonConvert.DeserializeObject<InstaDirectInboxThreadResponse>(
                    json,
                    new InstaThreadDataConverter());

                return InstaResult.Success(threadResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaDirectInboxThreadResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxThreadResponse>(exception);
            }
        }

        private async Task<IResult<InstaDirectInboxContainerResponse>> GetPendingDirect(string maxId = null)
        {
            try
            {
                var directInboxUri = InstaUriCreator.GetDirectPendingInboxUri(maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, directInboxUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaDirectInboxContainerResponse>(response, json);
                }

                var inboxResponse = JsonConvert.DeserializeObject<InstaDirectInboxContainerResponse>(json);
                return InstaResult.Success(inboxResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaDirectInboxContainerResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaDirectInboxContainerResponse>(exception);
            }
        }

        private async Task<IResult<bool>> SendDirectPhoto(Action<InstaUploaderProgress> progress,
                                                          string recipients,
                                                          string threadId,
                                                          InstaImage image)
        {
            var upProgress = new InstaUploaderProgress
            {
                Caption = string.Empty, UploadState = InstaUploadState.Preparing
            };
            try
            {
                var instaUri = InstaUriCreator.GetDirectSendPhotoUri();
                var uploadId = ApiRequestMessage.GenerateRandomUploadId();
                var clientContext = Guid.NewGuid();
                upProgress.UploadId = uploadId;
                progress?.Invoke(upProgress);
                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    { new StringContent("send_item"), "\"action\"" },
                    { new StringContent(clientContext.ToString()), "\"client_context\"" },
                    { new StringContent(user.CsrfToken), "\"_csrftoken\"" },
                    { new StringContent(deviceInfo.DeviceGuid.ToString()), "\"_uuid\"" }
                };
                if (!string.IsNullOrEmpty(recipients))
                {
                    requestContent.Add(new StringContent($"[[{recipients}]]"), "recipient_users");
                }
                else
                {
                    requestContent.Add(new StringContent($"[{threadId}]"), "thread_ids");
                }

                byte[] fileBytes;
                if (image.ImageBytes == null)
                {
                    fileBytes = File.ReadAllBytes(image.Uri);
                }
                else
                {
                    fileBytes = image.ImageBytes;
                }

                var imageContent = new ByteArrayContent(fileBytes);
                imageContent.Headers.Add("Content-Transfer-Encoding", "binary");
                imageContent.Headers.Add("Content-Type", "application/octet-stream");
                requestContent.Add(
                    imageContent,
                    "photo",
                    $"direct_temp_photo_{ApiRequestMessage.GenerateUploadId()}.jpg");

                //var progressContent = new ProgressableStreamContent(requestContent, 4096, progress)
                //{
                //    UploaderProgress = upProgress
                //};
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = requestContent;
                upProgress.UploadState = InstaUploadState.Uploading;
                progress?.Invoke(upProgress);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                upProgress.UploadState = InstaUploadState.Uploaded;
                progress?.Invoke(upProgress);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                if (obj.Status.ToLower() == "ok")
                {
                    upProgress.UploadState = InstaUploadState.Completed;
                    progress?.Invoke(upProgress);
                    return InstaResult.Success(true);
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<bool>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<bool>(exception);
            }
        }

        private async Task<IResult<bool>> ShareMedia(string mediaId,
                                                     InstaMediaType mediaType,
                                                     string text,
                                                     string[] threadIds,
                                                     long[] userIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetMediaShareUri(mediaType);
                var clientContext = Guid.NewGuid().ToString();
                var data = new Dictionary<string, string>
                {
                    { "action", "send_item" },
                    { "client_context", clientContext },
                    { "media_id", mediaId },
                    { "_csrftoken", user.CsrfToken },
                    { "unified_broadcast_format", "1" },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "text", text ?? string.Empty }
                };
                if (threadIds != null)
                {
                    data.Add("thread_ids", $"[{threadIds.EncodeList(false)}]");
                }
                else
                {
                    data.Add("recipient_users", $"[{userIds.EncodeRecipients()}]");
                }

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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
    }
}