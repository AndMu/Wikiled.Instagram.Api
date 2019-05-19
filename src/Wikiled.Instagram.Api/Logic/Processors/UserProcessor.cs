using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.Models.Feed;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     User api functions.
    /// </summary>
    internal class InstaUserProcessor : IUserProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaUserProcessor(
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
        ///     Accept user friendship requst.
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<InstaFriendshipStatus>> AcceptFriendshipRequestAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAcceptFriendshipUri(userId);
                var fields = new Dictionary<string, string>
                {
                    { "user_id", userId.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaFriendshipStatus>(response, json);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaFriendshipStatusResponse>(
                    json,
                    new InstaFriendShipDataConverter());
                var converter = InstaConvertersFabric.Instance.GetFriendShipStatusConverter(friendshipStatus);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFriendshipStatus), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<InstaFriendshipStatus>(ex);
            }
        }

        /// <summary>
        ///     Add new best friend (besties)
        /// </summary>
        /// <param name="userIds">User ids (pk) to add</param>
        public async Task<IResult<InstaFriendshipShortStatusList>> AddBestFriendsAsync(params long[] userIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            if (userIds?.Length == 0)
            {
                return Result.Fail<InstaFriendshipShortStatusList>("At least 1 user id is require");
            }

            return await AddBestFriends(userIds, null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Delete an user from your best friend (besties) lists
        /// </summary>
        /// <param name="userIds">User ids (pk) to add</param>
        public async Task<IResult<InstaFriendshipShortStatusList>> DeleteBestFriendsAsync(params long[] userIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            if (userIds?.Length == 0)
            {
                return Result.Fail<InstaFriendshipShortStatusList>("At least 1 user id is require");
            }

            return await AddBestFriends(null, userIds).ConfigureAwait(false);
        }

        /// <summary>
        ///     Block user
        /// </summary>
        /// <param name="userId">User id</param>
        public Task<IResult<InstaFriendshipFullStatus>> BlockUserAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return BlockUnblockUserInternal(userId, InstaUriCreator.GetBlockUserUri(userId));
        }

        /// <summary>
        ///     Favorite user (user must be in your following list)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public Task<IResult<bool>> FavoriteUserAsync(long userId)
        {
            return FavoriteUnfavoriteUser(InstaUriCreator.GetFavoriteUserUri(userId), userId);
        }

        /// <summary>
        ///     Favorite user stories (user must be in your following list)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public Task<IResult<bool>> FavoriteUserStoriesAsync(long userId)
        {
            return FavoriteUnfavoriteUser(InstaUriCreator.GetFavoriteForUserStoriesUri(userId), userId);
        }

        /// <summary>
        ///     Follow user
        /// </summary>
        /// <param name="userId">User id</param>
        public Task<IResult<InstaFriendshipFullStatus>> FollowUserAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return FollowUnfollowUserInternal(userId, InstaUriCreator.GetFollowUserUri(userId));
        }

        /// <summary>
        ///     Get self best friends (besties)
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public Task<IResult<InstaUserShortList>> GetBestFriendsAsync(PaginationParameters paginationParameters)
        {
            return GetBesties(paginationParameters);
        }

        /// <summary>
        ///     Get best friends (besties) suggestions
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public Task<IResult<InstaUserShortList>> GetBestFriendsSuggestionsAsync(PaginationParameters paginationParameters)
        {
            return GetBesties(paginationParameters, true);
        }

        /// <summary>
        ///     Get blocked users
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public async Task<IResult<InstaBlockedUsers>> GetBlockedUsersAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaBlockedUsers Convert(InstaBlockedUsersResponse instaBlockedUsers)
                {
                    return InstaConvertersFabric.Instance.GetBlockedUsersConverter(instaBlockedUsers).Convert();
                }

                var blockedUsersResponse = await GetBlockedUsers(paginationParameters?.NextMaxId).ConfigureAwait(false);
                if (!blockedUsersResponse.Succeeded)
                {
                    if (blockedUsersResponse.Value != null)
                    {
                        return Result.Fail(blockedUsersResponse.Info, Convert(blockedUsersResponse.Value));
                    }

                    return Result.Fail(blockedUsersResponse.Info, default(InstaBlockedUsers));
                }

                paginationParameters.NextMaxId = blockedUsersResponse.Value.MaxId;

                paginationParameters.PagesLoaded++;
                while (!string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var moreUsers = await GetBlockedUsers(paginationParameters.NextMaxId).ConfigureAwait(false);
                    if (!moreUsers.Succeeded)
                    {
                        return Result.Fail(moreUsers.Info, Convert(blockedUsersResponse.Value));
                    }

                    blockedUsersResponse.Value.BlockedList.AddRange(moreUsers.Value.BlockedList);
                    blockedUsersResponse.Value.PageSize = moreUsers.Value.PageSize;
                    blockedUsersResponse.Value.BigList = moreUsers.Value.BigList;
                    blockedUsersResponse.Value.MaxId = paginationParameters.NextMaxId = moreUsers.Value.MaxId;
                    paginationParameters.PagesLoaded++;
                }

                return Result.Success(Convert(blockedUsersResponse.Value));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBlockedUsers), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBlockedUsers>(exception);
            }
        }

        /// <summary>
        ///     Get currently logged in user info asynchronously
        /// </summary>
        /// <returns>
        ///     <see cref="CurrentUser" />
        /// </returns>
        public async Task<IResult<CurrentUser>> GetCurrentUserAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetCurrentUserUri();
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", this.user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", this.user.CsrfToken }
                };
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = new FormUrlEncodedContent(fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<CurrentUser>(response, json);
                }

                var user = JsonConvert.DeserializeObject<InstaCurrentUserResponse>(
                    json,
                    new InstaCurrentUserDataConverter());
                if (user.Pk < 1)
                {
                    Result.Fail<CurrentUser>("Pk is incorrect");
                }

                var converter = InstaConvertersFabric.Instance.GetCurrentUserConverter(user);
                var userConverted = converter.Convert();
                return Result.Success(userConverted);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(CurrentUser), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<CurrentUser>(exception);
            }
        }

        /// <summary>
        ///     Get followers list for currently logged in user asynchronously
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public Task<IResult<InstaUserShortList>> GetCurrentUserFollowersAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return GetUserFollowersAsync(user.UserName, paginationParameters, string.Empty);
        }

        public Task<IResult<InstaActivityFeed>> GetFollowingRecentActivityFeedAsync(PaginationParameters paginationParameters)
        {
            var uri = InstaUriCreator.GetFollowingRecentActivityUri(paginationParameters.NextMaxId);
            return GetRecentActivityInternalAsync(uri, paginationParameters);
        }

        /// <summary>
        ///     Get friendship status for given user id.
        /// </summary>
        /// <param name="userId">User identifier (PK)</param>
        /// <returns>
        ///     <see cref="InstaStoryFriendshipStatus" />
        /// </returns>
        public async Task<IResult<InstaStoryFriendshipStatus>> GetFriendshipStatusAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userUri = InstaUriCreator.GetUserFriendshipUri(userId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaStoryFriendshipStatus>(response, json);
                }

                var friendshipStatusResponse = JsonConvert.DeserializeObject<InstaStoryFriendshipStatusResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetStoryFriendshipStatusConverter(friendshipStatusResponse);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaStoryFriendshipStatus), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaStoryFriendshipStatus>(exception);
            }
        }

        /// <summary>
        ///     Get friendship status for multiple user ids.
        /// </summary>
        /// <param name="userIds">Array of user identifier (PK)</param>
        /// <returns>
        ///     <see cref="InstaFriendshipShortStatusList" />
        /// </returns>
        public async Task<IResult<InstaFriendshipShortStatusList>> GetFriendshipStatusesAsync(params long[] userIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (userIds == null || userIds != null && !userIds.Any())
                {
                    throw new ArgumentException("At least one user id is require.");
                }

                var userUri = InstaUriCreator.GetFriendshipShowManyUri();

                var data = new Dictionary<string, string>
                {
                    { "_csrftoken", user.CsrfToken },
                    { "user_ids", string.Join(",", userIds) },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, userUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaFriendshipShortStatusList>(response, json);
                }

                var friendshipStatusesResponse = JsonConvert.DeserializeObject<InstaFriendshipShortStatusListResponse>(
                    json,
                    new InstaFriendShipShortDataConverter());
                var converter =
                    InstaConvertersFabric.Instance.GetFriendshipShortStatusListConverter(friendshipStatusesResponse);

                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFriendshipShortStatusList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaFriendshipShortStatusList>(exception);
            }
        }

        /// <summary>
        ///     Get full user info (user info, feeds, stories, broadcasts)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<InstaFullUserInfo>> GetFullUserInfoAsync(long userId)
        {
            try
            {
                var instaUri = InstaUriCreator.GetFullUserInfoUri(userId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaFullUserInfo>(response, json);
                }

                var fullUserInfoResponse = JsonConvert.DeserializeObject<InstaFullUserInfoResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetFullUserInfoConverter(fullUserInfoResponse);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFullUserInfo), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaFullUserInfo>(exception);
            }
        }

        /// <summary>
        ///     Get pending friendship requests.
        /// </summary>
        public async Task<IResult<InstaPendingRequest>> GetPendingFriendRequestsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var cookies =
                    httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        httpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                user.CsrfToken = csrftoken;
                var instaUri = InstaUriCreator.GetFriendshipPendingRequestsUri(user.RankToken);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                request.Properties.Add(InstaApiConstants.HeaderIgSignatureKeyVersion,
                                       InstaApiConstants.IgSignatureKeyVersion);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jRes = JsonConvert.DeserializeObject<InstaPendingRequest>(json);
                    return Result.Success(jRes);
                }

                return Result.Fail<InstaPendingRequest>(response.StatusCode.ToString());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaPendingRequest), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<InstaPendingRequest>(ex);
            }
        }

        public Task<IResult<InstaActivityFeed>> GetRecentActivityFeedAsync(PaginationParameters paginationParameters)
        {
            var uri = InstaUriCreator.GetRecentActivityUri();
            return GetRecentActivityInternalAsync(uri, paginationParameters);
        }

        /// <summary>
        ///     Get suggestion details
        /// </summary>
        /// <param name="userIds">List of user ids (pk)</param>
        public async Task<IResult<InstaSuggestionItemList>> GetSuggestionDetailsAsync(params long[] userIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (userIds == null || userIds != null && !userIds.Any())
                {
                    throw new ArgumentException("At least one user id is require.");
                }

                var instaUri = InstaUriCreator.GetDiscoverSuggestionDetailsUri(user.LoggedInUser.Pk, userIds.ToList());

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaSuggestionItemList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaSuggestionItemListResponse>(
                    json,
                    new InstaSuggestionUserDetailDataConverter());
                return Result.Success(InstaConvertersFabric.Instance.GetSuggestionItemListConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaSuggestionItemList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaSuggestionItemList>(exception);
            }
        }

        /// <summary>
        ///     Get suggestion users
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        public async Task<IResult<InstaSuggestions>> GetSuggestionUsersAsync(PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaSuggestions Convert(InstaSuggestionUserContainerResponse suggestResponse)
                {
                    return InstaConvertersFabric.Instance.GetSuggestionsConverter(suggestResponse).Convert();
                }

                var suggestionsResponse = await GetSuggestionUsers(paginationParameters).ConfigureAwait(false);
                if (!suggestionsResponse.Succeeded)
                {
                    if (suggestionsResponse.Value != null)
                    {
                        return Result.Fail(suggestionsResponse.Info, Convert(suggestionsResponse.Value));
                    }

                    return Result.Fail(suggestionsResponse.Info, default(InstaSuggestions));
                }

                paginationParameters.NextMaxId = suggestionsResponse.Value.MaxId;

                paginationParameters.PagesLoaded++;
                while (suggestionsResponse.Value.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var moreSuggestions = await GetSuggestionUsers(paginationParameters).ConfigureAwait(false);
                    if (!moreSuggestions.Succeeded)
                    {
                        return Result.Fail(moreSuggestions.Info, Convert(suggestionsResponse.Value));
                    }

                    suggestionsResponse.Value.NewSuggestedUsers.Suggestions.AddRange(
                        moreSuggestions.Value.NewSuggestedUsers.Suggestions);
                    suggestionsResponse.Value.SuggestedUsers.Suggestions.AddRange(
                        moreSuggestions.Value.SuggestedUsers.Suggestions);
                    suggestionsResponse.Value.MoreAvailable = moreSuggestions.Value.MoreAvailable;
                    suggestionsResponse.Value.MaxId = paginationParameters.NextMaxId = moreSuggestions.Value.MaxId;
                    paginationParameters.PagesLoaded++;
                }

                return Result.Success(Convert(suggestionsResponse.Value));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaSuggestions), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaSuggestions>(exception);
            }
        }

        /// <summary>
        ///     Get user info by its user name asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>
        ///     <see cref="InstaUser" />
        /// </returns>
        public async Task<IResult<InstaUser>> GetUserAsync(string username)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userUri = InstaUriCreator.GetUserUri(username);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                request.Properties.Add(
                    new KeyValuePair<string, object>(
                        InstaApiConstants.HeaderTimezone,
                        InstaApiConstants.TimezoneOffset.ToString()));
                request.Properties.Add(new KeyValuePair<string, object>(InstaApiConstants.HeaderCount, "1"));
                request.Properties.Add(
                    new KeyValuePair<string, object>(InstaApiConstants.HeaderRankToken, this.user.RankToken));
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaUser>(response, json);
                }

                var userInfo = JsonConvert.DeserializeObject<InstaSearchUserResponse>(json);
                var user = userInfo.Users?.FirstOrDefault(
                    u => u.UserName.ToLower() == username.ToLower().Replace("@", ""));
                if (user == null)
                {
                    var errorMessage = $"Can't find this user: {username}";
                    logger?.LogInformation(errorMessage);
                    return Result.Fail<InstaUser>(errorMessage);
                }

                if (user.Pk < 1)
                {
                    Result.Fail<InstaUser>("Pk is incorrect");
                }

                var converter = InstaConvertersFabric.Instance.GetUserConverter(user);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUser), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaUser>(exception);
            }
        }

        /// <summary>
        ///     Get user from a nametag image
        /// </summary>
        /// <param name="nametagImage">Nametag image</param>
        public async Task<IResult<InstaUser>> GetUserFromNametagAsync(InstaImage nametagImage)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUsersNametagLookupUri();
                var uploadId = ApiRequestMessage.GenerateUploadId();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "gallery", "true" },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "waterfall_id", Guid.NewGuid().ToString() }
                };
                var signedBody = httpHelper.GetSignature(data);

                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    {
                        new StringContent(InstaApiConstants.IgSignatureKeyVersion),
                        InstaApiConstants.HeaderIgSignatureKeyVersion
                    },
                    { new StringContent(signedBody), "signed_body" }
                };
                byte[] fileBytes;
                if (nametagImage.ImageBytes == null)
                {
                    fileBytes = File.ReadAllBytes(nametagImage.Uri);
                }
                else
                {
                    fileBytes = nametagImage.ImageBytes;
                }

                var imageContent = new ByteArrayContent(fileBytes);
                imageContent.Headers.Add("Content-Transfer-Encoding", "binary");
                imageContent.Headers.Add("Content-Type", "application/octet-stream");
                requestContent.Add(imageContent, "photo_0", "photo_0");
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = requestContent;
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.Fail<InstaUser>("User not found.");
                }

                //{"message": "No user found for 3", "username": "3", "failure_code": 602, "failure_reason": "user_not_found_for_username", "status": "fail"}
                //{"message": "Scan was below 95, got 90.0779664516449", "confidence": 90.0779664516449, "username": "9", "failure_code": 601, "failure_reason": "confidence_below_threshold", "status": "fail"}

                var obj = JsonConvert.DeserializeObject<InstaUserContainerResponse>(json);

                var converter = InstaConvertersFabric.Instance.GetUserConverter(obj.User);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUser), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaUser>(exception);
            }
        }

        /// <summary>
        ///     Get followers list by username asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <param name="searchQuery">Search string to locate specific followers</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public async Task<IResult<InstaUserShortList>> GetUserFollowersAsync(
            string username,
            PaginationParameters paginationParameters,
            string searchQuery,
            bool mutualsfirst = false)
        {
            try
            {
                var user = await GetUserAsync(username).ConfigureAwait(false);
                if (user.Succeeded)
                {
                    if (user.Value.FriendshipStatus.IsPrivate &&
                        user.Value.UserName != this.user.LoggedInUser.UserName &&
                        !user.Value.FriendshipStatus.Following)
                    {
                        return Result.Fail(
                            "You must be a follower of private accounts to be able to get user's followers",
                            default(InstaUserShortList));
                    }

                    return await GetUserFollowersByIdAsync(user.Value.Pk,
                                                           paginationParameters,
                                                           searchQuery,
                                                           mutualsfirst).ConfigureAwait(false);
                }

                return Result.Fail(user.Info, default(InstaUserShortList));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUserShortList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, default(InstaUserShortList));
            }
        }

        /// <summary>
        ///     Get followers list by user id(pk) asynchronously
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <param name="searchQuery">Search string to locate specific followers</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public async Task<IResult<InstaUserShortList>> GetUserFollowersByIdAsync(
            long userId,
            PaginationParameters paginationParameters,
            string searchQuery,
            bool mutualsfirst = false)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var followers = new InstaUserShortList();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                var userFollowersUri =
                    InstaUriCreator.GetUserFollowersUri(
                        userId,
                        user.RankToken,
                        searchQuery,
                        mutualsfirst,
                        paginationParameters.NextMaxId);
                var followersResponse = await GetUserListByUriAsync(userFollowersUri).ConfigureAwait(false);
                if (!followersResponse.Succeeded)
                {
                    return Result.Fail(followersResponse.Info, (InstaUserShortList)null);
                }

                followers.AddRange(
                    followersResponse.Value.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                paginationParameters.NextMaxId = followers.NextMaxId = followersResponse.Value.NextMaxId;

                var pagesLoaded = 1;
                while (!string.IsNullOrEmpty(followersResponse.Value.NextMaxId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextFollowersUri =
                        InstaUriCreator.GetUserFollowersUri(
                            userId,
                            user.RankToken,
                            searchQuery,
                            mutualsfirst,
                            followersResponse.Value.NextMaxId);
                    followersResponse = await GetUserListByUriAsync(nextFollowersUri).ConfigureAwait(false);
                    if (!followersResponse.Succeeded)
                    {
                        return Result.Fail(followersResponse.Info, followers);
                    }

                    followers.AddRange(
                        followersResponse.Value.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                            .Select(converter => converter.Convert()));
                    pagesLoaded++;
                    paginationParameters.PagesLoaded = pagesLoaded;
                    paginationParameters.NextMaxId = followers.NextMaxId = followersResponse.Value.NextMaxId;
                }

                return Result.Success(followers);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, followers, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, followers);
            }
        }

        /// <summary>
        ///     Get following list by username asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <param name="searchQuery">Search string to locate specific followings</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public async Task<IResult<InstaUserShortList>> GetUserFollowingAsync(
            string username,
            PaginationParameters paginationParameters,
            string searchQuery)
        {
            try
            {
                var user = await GetUserAsync(username).ConfigureAwait(false);
                if (user.Succeeded)
                {
                    if (user.Value.FriendshipStatus.IsPrivate &&
                        user.Value.UserName != this.user.LoggedInUser.UserName &&
                        !user.Value.FriendshipStatus.Following)
                    {
                        return Result.Fail(
                            "You must be a follower of private accounts to be able to get user's followings",
                            default(InstaUserShortList));
                    }

                    return await GetUserFollowingByIdAsync(user.Value.Pk, paginationParameters, searchQuery).ConfigureAwait(false);
                }

                return Result.Fail(user.Info, default(InstaUserShortList));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUserShortList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, default(InstaUserShortList));
            }
        }

        /// <summary>
        ///     Get following list by user id(pk) asynchronously
        /// </summary>
        /// <param name="userId">User id(pk)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <param name="searchQuery">Search string to locate specific followings</param>
        /// <returns>
        ///     <see cref="InstaUserShortList" />
        /// </returns>
        public async Task<IResult<InstaUserShortList>> GetUserFollowingByIdAsync(
            long userId,
            PaginationParameters paginationParameters,
            string searchQuery)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var following = new InstaUserShortList();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                var uri = InstaUriCreator.GetUserFollowingUri(
                    userId,
                    user.RankToken,
                    searchQuery,
                    paginationParameters.NextMaxId);
                var userListResponse = await GetUserListByUriAsync(uri).ConfigureAwait(false);
                if (!userListResponse.Succeeded)
                {
                    return Result.Fail(userListResponse.Info, (InstaUserShortList)null);
                }

                following.AddRange(
                    userListResponse.Value.Items.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                paginationParameters.NextMaxId = following.NextMaxId = userListResponse.Value.NextMaxId;
                var pages = 1;
                while (!string.IsNullOrEmpty(following.NextMaxId) && pages < paginationParameters.MaximumPagesToLoad)
                {
                    var nextUri =
                        InstaUriCreator.GetUserFollowingUri(
                            userId,
                            user.RankToken,
                            searchQuery,
                            userListResponse.Value.NextMaxId);
                    userListResponse = await GetUserListByUriAsync(nextUri).ConfigureAwait(false);
                    if (!userListResponse.Succeeded)
                    {
                        return Result.Fail(userListResponse.Info, following);
                    }

                    following.AddRange(
                        userListResponse.Value.Items.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                            .Select(converter => converter.Convert()));
                    pages++;
                    paginationParameters.PagesLoaded = pages;
                    paginationParameters.NextMaxId = following.NextMaxId = userListResponse.Value.NextMaxId;
                }

                return Result.Success(following);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, following, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, following);
            }
        }

        /// <summary>
        ///     Gets the user extended information (followers count, following count, bio, etc) by user identifier.
        /// </summary>
        /// <param name="pk">User Id, like "123123123"</param>
        /// <returns></returns>
        public async Task<IResult<InstaUserInfo>> GetUserInfoByIdAsync(long pk)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userUri = InstaUriCreator.GetUserInfoByIdUri(pk);
                return await GetUserInfoAsync(userUri).ConfigureAwait(false);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUserInfo), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaUserInfo>(exception);
            }
        }

        /// <summary>
        ///     Gets the user extended information (followers count, following count, bio, etc) by username.
        /// </summary>
        /// <param name="username">Username, like "instagram"</param>
        /// <returns></returns>
        public async Task<IResult<InstaUserInfo>> GetUserInfoByUsernameAsync(string username)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var userUri = InstaUriCreator.GetUserInfoByUsernameUri(username);
                return await GetUserInfoAsync(userUri).ConfigureAwait(false);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUserInfo), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaUserInfo>(exception);
            }
        }

        /// <summary>
        ///     Get all user media by username asynchronously
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public IObservable<InstaMedia> GetUserMedia(string username, PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return Observable.Create<InstaMedia>(
                async obs =>
                {
                    var user = await GetUserAsync(username).ConfigureAwait(false);
                    if (!user.Succeeded)
                    {
                        obs.OnError(new Exception("Failed to load"));
                    }

                    return GetUserMediaById(user.Value.Pk, paginationParameters).Subscribe(obs);
                });
        }

        /// <summary>
        ///     Get all user media by user id (pk) asynchronously
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public IObservable<InstaMedia> GetUserMediaById(long userId, PaginationParameters paginationParameters)
        {
            return Observable.Create<InstaMedia>(
                async obs =>
                {
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

                        var mediaResult = await GetUserMedia(userId, paginationParameters).ConfigureAwait(false);
                        if (!mediaResult.Succeeded)
                        {
                            obs.OnError(new Exception("Failed to load"));
                        }

                        var mediaResponse = mediaResult.Value;

                        foreach (var item in Convert(mediaResult.Value))
                        {
                            obs.OnNext(item);
                        }

                        paginationParameters.PagesLoaded++;
                        paginationParameters.NextMaxId = mediaResponse.NextMaxId;
                        while (mediaResponse.MoreAvailable &&
                            !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                            paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                        {
                            var nextMedia = await GetUserMedia(userId, paginationParameters).ConfigureAwait(false);
                            if (!nextMedia.Succeeded)
                            {
                                obs.OnError(new Exception("Failed to load"));
                            }

                            paginationParameters.NextMaxId = nextMedia.Value.NextMaxId;
                            mediaResponse.MoreAvailable = nextMedia.Value.MoreAvailable;
                            mediaResponse.ResultsCount += nextMedia.Value.ResultsCount;
                            var data = Convert(nextMedia.Value);
                            foreach (var item in data)
                            {
                                obs.OnNext(item);
                            }
                          
                            paginationParameters.PagesLoaded++;
                        }

                        obs.OnCompleted();
                    }
                    catch (Exception exception)
                    {
                        logger?.LogError(exception, "Error");
                        obs.OnError(exception);
                    }
                });
        }

        /// <summary>
        ///     Get all user shoppable media by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public Task<IResult<InstaMediaList>> GetUserShoppableMediaAsync(string username, PaginationParameters paginationParameters)
        {
            return instaApi.ShoppingProcessor.GetUserShoppableMediaAsync(username, paginationParameters);
        }

        /// <summary>
        ///     Get user tags by username asynchronously
        ///     <remarks>Returns media list containing tags</remarks>
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public async Task<IResult<InstaMediaList>> GetUserTagsAsync(
            string username,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var user = await GetUserAsync(username).ConfigureAwait(false);
            if (!user.Succeeded)
            {
                return Result.Fail($"Unable to get user {username} to get tags", (InstaMediaList)null);
            }

            return await GetUserTagsAsync(user.Value.Pk, paginationParameters).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get user tags by username asynchronously
        ///     <remarks>Returns media list containing tags</remarks>
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        /// <param name="paginationParameters">Pagination parameters: next id and max amount of pages to load</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public async Task<IResult<InstaMediaList>> GetUserTagsAsync(
            long userId,
            PaginationParameters paginationParameters)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var userTags = new InstaMediaList();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                IEnumerable<InstaMedia> Convert(InstaMediaListResponse mediaListResponse)
                {
                    return mediaListResponse.Medias.Select(InstaConvertersFabric.Instance.GetSingleMediaConverter)
                        .Select(converter => converter.Convert());
                }

                var mediaTags = await GetUserTags(userId, paginationParameters).ConfigureAwait(false);
                if (!mediaTags.Succeeded)
                {
                    if (mediaTags.Value != null)
                    {
                        userTags.AddRange(Convert(mediaTags.Value));
                        return Result.Fail(mediaTags.Info, userTags);
                    }

                    return Result.Fail(mediaTags.Info, default(InstaMediaList));
                }

                var mediaResponse = mediaTags.Value;
                userTags.AddRange(Convert(mediaResponse));
                userTags.NextMaxId = paginationParameters.NextMaxId = mediaResponse.NextMaxId;
                paginationParameters.PagesLoaded++;

                while (mediaResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextMedia = await GetUserTags(userId, paginationParameters).ConfigureAwait(false);
                    if (!nextMedia.Succeeded)
                    {
                        return Result.Fail(nextMedia.Info, userTags);
                    }

                    userTags.AddRange(Convert(nextMedia.Value));
                    userTags.NextMaxId = paginationParameters.NextMaxId =
                        mediaResponse.NextMaxId = nextMedia.Value.NextMaxId;
                    mediaResponse.AutoLoadMoreEnabled = nextMedia.Value.AutoLoadMoreEnabled;
                    mediaResponse.MoreAvailable = nextMedia.Value.MoreAvailable;
                    mediaResponse.RankToken = nextMedia.Value.RankToken;
                    mediaResponse.TotalCount += nextMedia.Value.TotalCount;
                    mediaResponse.ResultsCount += nextMedia.Value.ResultsCount;
                }

                userTags.PageSize = mediaResponse.ResultsCount;
                userTags.Pages = paginationParameters.PagesLoaded;
                return Result.Success(userTags);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, userTags, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, userTags);
            }
        }

        /// <summary>
        ///     Ignore user friendship requst.
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<InstaFriendshipFullStatus>> IgnoreFriendshipRequestAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDenyFriendshipUri(userId);
                var fields = new Dictionary<string, string>
                {
                    { "user_id", userId.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaFriendshipFullStatus>(response, json);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaFriendshipFullStatusContainerResponse>(json);
                var converter =
                    InstaConvertersFabric.Instance.GetFriendshipFullStatusConverter(friendshipStatus.FriendshipStatus);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFriendshipFullStatus), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                return Result.Fail<InstaFriendshipFullStatus>(exception);
            }
        }

        /// <summary>
        ///     Hide my story from specific user
        /// </summary>
        /// <param name="userId">User id</param>
        public Task<IResult<InstaStoryFriendshipStatus>> HideMyStoryFromUserAsync(long userId)
        {
            return HideUnhideMyStoryFromUser(InstaUriCreator.GetHideMyStoryFromUserUri(userId));
        }

        /// <summary>
        ///     Mark user as overage
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<bool>> MarkUserAsOverageAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetMarkUserOverageUri(userId);

                var data = new JObject
                {
                    { "user_id", userId.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<bool>(response, obj.Message, null);
                }

                return obj.Status.ToLower() == "ok"
                    ? Result.Success(true)
                    : Result.UnExpectedResponse<bool>(response, obj.Message, null);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                return Result.Fail<bool>(exception);
            }
        }

        /// <summary>
        ///     Mute friend's stories, so you won't see their stories in latest stories tab
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public Task<IResult<InstaStoryFriendshipStatus>> MuteFriendStoryAsync(long userId)
        {
            return MuteUnMuteFriendStory(InstaUriCreator.GetMuteFriendStoryUri(userId));
        }

        /// <summary>
        ///     Mute user media (story, post or all)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        /// <param name="unmuteOption">Unmute option</param>
        public Task<IResult<InstaStoryFriendshipStatus>> MuteUserMediaAsync(long userId, InstaMuteOption unmuteOption)
        {
            return MuteUnMuteUserMedia(InstaUriCreator.GetMuteUserMediaStoryUri(userId), userId, unmuteOption);
        }

        /// <summary>
        ///     Report user
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<bool>> ReportUserAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetReportUserUri(userId);
                var fields = new Dictionary<string, string>
                {
                    { "user_id", userId.ToString() },
                    { "source_name", "profile" },
                    { "reason", "1" },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "is_spam", "true" }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return Result.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Stop block user
        /// </summary>
        /// <param name="userId">User id</param>
        public Task<IResult<InstaFriendshipFullStatus>> UnBlockUserAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return BlockUnblockUserInternal(userId, InstaUriCreator.GetUnBlockUserUri(userId));
        }

        /// <summary>
        ///     Unfavorite user (user must be in your following list)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public Task<IResult<bool>> UnFavoriteUserAsync(long userId)
        {
            return FavoriteUnfavoriteUser(InstaUriCreator.GetUnFavoriteUserUri(userId), userId);
        }

        /// <summary>
        ///     Unfavorite user stories (user must be in your following list)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public Task<IResult<bool>> UnFavoriteUserStoriesAsync(long userId)
        {
            return FavoriteUnfavoriteUser(InstaUriCreator.GetUnFavoriteForUserStoriesUri(userId), userId);
        }

        /// <summary>
        ///     Stop follow user
        /// </summary>
        /// <param name="userId">User id</param>
        public Task<IResult<InstaFriendshipFullStatus>> UnFollowUserAsync(long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return FollowUnfollowUserInternal(userId, InstaUriCreator.GetUnFollowUserUri(userId));
        }

        /// <summary>
        ///     Unhide my story from specific user
        /// </summary>
        /// <param name="userId">User id</param>
        public Task<IResult<InstaStoryFriendshipStatus>> UnHideMyStoryFromUserAsync(long userId)
        {
            return HideUnhideMyStoryFromUser(InstaUriCreator.GetUnHideMyStoryFromUserUri(userId));
        }

        /// <summary>
        ///     Unmute friend's stories, so you will be able to see their stories in latest stories tab once again
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public Task<IResult<InstaStoryFriendshipStatus>> UnMuteFriendStoryAsync(long userId)
        {
            return MuteUnMuteFriendStory(InstaUriCreator.GetUnMuteFriendStoryUri(userId));
        }

        /// <summary>
        ///     Unmute user media (story, post or all)
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        /// <param name="unmuteOption">Unmute option</param>
        public Task<IResult<InstaStoryFriendshipStatus>> UnMuteUserMediaAsync(long userId, InstaMuteOption unmuteOption)
        {
            return MuteUnMuteUserMedia(InstaUriCreator.GetUnMuteUserMediaStoryUri(userId), userId, unmuteOption);
        }

        /// <summary>
        ///     Remove an follower from your followers
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<InstaFriendshipStatus>> RemoveFollowerAsync(long userId)
        {
            try
            {
                var instaUri = InstaUriCreator.GetRemoveFollowerUri(userId);

                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "user_id", userId.ToString() },
                    { "radio_type", "wifi-none" },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(json))
                {
                    return Result.UnExpectedResponse<InstaFriendshipStatus>(response, json);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaFriendshipStatusResponse>(
                    json,
                    new InstaFriendShipDataConverter());
                var converter = InstaConvertersFabric.Instance.GetFriendShipStatusConverter(friendshipStatus);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFriendshipStatus), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaFriendshipStatus>(exception);
            }
        }

        /// <summary>
        ///     Translate biography of someone
        /// </summary>
        /// <param name="userId">User id (pk)</param>
        public async Task<IResult<string>> TranslateBiographyAsync(long userId)
        {
            try
            {
                var instaUri = InstaUriCreator.GetTranslateBiographyUri(userId);

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(json))
                {
                    return Result.UnExpectedResponse<string>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaTranslateBioResponse>(json);

                return obj.Status.ToLower() == "ok"
                    ? Result.Success(obj.Translation)
                    : Result.Fail<string>(obj.Message);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(string), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<string>(exception);
            }
        }

        private async Task<IResult<InstaFriendshipShortStatusList>> AddBestFriends(
            long[] userIdsToAdd,
            long[] userIdsToRemove)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSetBestFriendsUri();

                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "module", "favorites_home_list" },
                    { "source", "audience_manager" }
                };
                if (userIdsToAdd?.Length > 0)
                {
                    var jArr = new JArray { userIdsToAdd };
                    data.Add("add", jArr);
                    data.Add("remove", new JArray());
                }
                else
                {
                    var jArr = new JArray { userIdsToRemove };
                    data.Add("add", new JArray());
                    data.Add("remove", jArr);
                }

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaFriendshipShortStatusList>(response, json);
                }

                var friendshipStatusesResponse = JsonConvert.DeserializeObject<InstaFriendshipShortStatusListResponse>(
                    json,
                    new InstaFriendShipShortDataConverter());
                var converter =
                    InstaConvertersFabric.Instance.GetFriendshipShortStatusListConverter(friendshipStatusesResponse);

                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFriendshipShortStatusList), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaFriendshipShortStatusList>(exception);
            }
        }

        private async Task<IResult<InstaFriendshipFullStatus>> BlockUnblockUserInternal(long userId, Uri instaUri)
        {
            try
            {
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "user_id", userId.ToString() },
                    { "radio_type", "wifi-none" }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(json))
                {
                    return Result.UnExpectedResponse<InstaFriendshipFullStatus>(response, json);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaFriendshipFullStatusContainerResponse>(json);
                var converter =
                    InstaConvertersFabric.Instance.GetFriendshipFullStatusConverter(friendshipStatus.FriendshipStatus);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFriendshipFullStatus), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaFriendshipFullStatus>(exception);
            }
        }

        private async Task<IResult<InstaFriendshipFullStatus>> FollowUnfollowUserInternal(long userId, Uri instaUri)
        {
            try
            {
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "user_id", userId.ToString() },
                    { "radio_type", "wifi-none" }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(json))
                {
                    return Result.UnExpectedResponse<InstaFriendshipFullStatus>(response, json);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaFriendshipFullStatusContainerResponse>(json);
                var converter =
                    InstaConvertersFabric.Instance.GetFriendshipFullStatusConverter(friendshipStatus.FriendshipStatus);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaFriendshipFullStatus), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaFriendshipFullStatus>(exception);
            }
        }

        private async Task<IResult<InstaRecentActivityResponse>> GetFollowingActivityWithMaxIdAsync(string maxId)
        {
            try
            {
                var uri = InstaUriCreator.GetFollowingRecentActivityUri(maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaRecentActivityResponse>(response, json);
                }

                var followingActivity = JsonConvert.DeserializeObject<InstaRecentActivityResponse>(
                    json,
                    new InstaRecentActivityConverter());
                return Result.Success(followingActivity);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaRecentActivityResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaRecentActivityResponse>(exception);
            }
        }

        private async Task<IResult<InstaActivityFeed>> GetRecentActivityInternalAsync(
            Uri uri,
            PaginationParameters paginationParameters)
        {
            try
            {
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
                var activityFeed = new InstaActivityFeed();
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaActivityFeed>(response, json);
                }

                var feedPage = JsonConvert.DeserializeObject<InstaRecentActivityResponse>(
                    json,
                    new InstaRecentActivityConverter());
                activityFeed.IsOwnActivity = feedPage.IsOwnActivity;
                var nextId = feedPage.NextMaxId;
                activityFeed.Items.AddRange(
                    feedPage.Stories.Select(InstaConvertersFabric.Instance.GetSingleRecentActivityConverter)
                        .Select(converter => converter.Convert()));
                paginationParameters.PagesLoaded++;
                activityFeed.NextMaxId = paginationParameters.NextMaxId = feedPage.NextMaxId;
                while (!string.IsNullOrEmpty(nextId) &&
                    paginationParameters.PagesLoaded <= paginationParameters.MaximumPagesToLoad)
                {
                    var nextFollowingFeed = await GetFollowingActivityWithMaxIdAsync(nextId).ConfigureAwait(false);
                    if (!nextFollowingFeed.Succeeded)
                    {
                        return Result.Fail(nextFollowingFeed.Info, activityFeed);
                    }

                    nextId = nextFollowingFeed.Value.NextMaxId;
                    activityFeed.Items.AddRange(
                        feedPage.Stories.Select(InstaConvertersFabric.Instance.GetSingleRecentActivityConverter)
                            .Select(converter => converter.Convert()));
                    paginationParameters.PagesLoaded++;
                    activityFeed.NextMaxId = paginationParameters.NextMaxId = nextId;
                }

                return Result.Success(activityFeed);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaActivityFeed), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaActivityFeed>(exception);
            }
        }

        private async Task<IResult<InstaBlockedUsersResponse>> GetBlockedUsers(string maxId)
        {
            try
            {
                var instaUri = InstaUriCreator.GetBlockedUsersUri(maxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaBlockedUsersResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBlockedUsersResponse>(json);
                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaBlockedUsersResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaBlockedUsersResponse>(exception);
            }
        }

        private async Task<IResult<InstaSuggestionUserContainerResponse>> GetSuggestionUsers(
            PaginationParameters paginationParameters)
        {
            try
            {
                var instaUri = InstaUriCreator.GetDiscoverPeopleUri();

                var data = new Dictionary<string, string>
                {
                    { "phone_id", deviceInfo.PhoneGuid.ToString() },
                    { "module", "discover_people" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "paginate", "true" }
                };

                if (paginationParameters != null && !string.IsNullOrEmpty(paginationParameters.NextMaxId))
                {
                    data.Add("max_id", paginationParameters.NextMaxId);
                }

                var request =
                    httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaSuggestionUserContainerResponse>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaSuggestionUserContainerResponse>(json);
                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException,
                                   default(InstaSuggestionUserContainerResponse),
                                   ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaSuggestionUserContainerResponse>(exception);
            }
        }

        private async Task<IResult<InstaUserInfo>> GetUserInfoAsync(Uri userUri)
        {
            try
            {
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, userUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaUserInfo>(response, json);
                }

                var userInfo = JsonConvert.DeserializeObject<InstaUserInfoContainerResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetUserInfoConverter(userInfo);
                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUserInfo), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaUserInfo>(exception);
            }
        }

        private async Task<IResult<InstaUserShortList>> GetBesties(PaginationParameters paginationParameters,
                                                                   bool suggested = false)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var besties = new InstaUserShortList();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                var bestiesUri = InstaUriCreator.GetBestFriendsUri(paginationParameters.NextMaxId);
                if (suggested)
                {
                    bestiesUri = InstaUriCreator.GetBestiesSuggestionUri(paginationParameters.NextMaxId);
                }

                var bestiesResponse = await GetUserListByUriAsync(bestiesUri).ConfigureAwait(false);
                if (!bestiesResponse.Succeeded)
                {
                    return Result.Fail(bestiesResponse.Info, (InstaUserShortList)null);
                }

                besties.AddRange(
                    bestiesResponse.Value.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                paginationParameters.NextMaxId = besties.NextMaxId = bestiesResponse.Value.NextMaxId;

                var pagesLoaded = 1;
                while (!string.IsNullOrEmpty(bestiesResponse.Value.NextMaxId) &&
                    pagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    var nextBestiesUri = InstaUriCreator.GetBestFriendsUri(bestiesResponse.Value.NextMaxId);
                    if (suggested)
                    {
                        nextBestiesUri = InstaUriCreator.GetBestiesSuggestionUri(bestiesResponse.Value.NextMaxId);
                    }

                    bestiesResponse = await GetUserListByUriAsync(nextBestiesUri).ConfigureAwait(false);
                    if (!bestiesResponse.Succeeded)
                    {
                        return Result.Fail(bestiesResponse.Info, besties);
                    }

                    besties.AddRange(
                        bestiesResponse.Value.Items?.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                            .Select(converter => converter.Convert()));
                    pagesLoaded++;
                    paginationParameters.PagesLoaded = pagesLoaded;
                    paginationParameters.NextMaxId = besties.NextMaxId = bestiesResponse.Value.NextMaxId;
                }

                return Result.Success(besties);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, besties, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail(exception, besties);
            }
        }

        private async Task<IResult<InstaUserListShortResponse>> GetUserListByUriAsync(Uri uri)
        {
            try
            {
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaUserListShortResponse>(response, json);
                }

                var instaUserListResponse = JsonConvert.DeserializeObject<InstaUserListShortResponse>(json);
                if (instaUserListResponse.IsOk())
                {
                    return Result.Success(instaUserListResponse);
                }

                return Result.UnExpectedResponse<InstaUserListShortResponse>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaUserListShortResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return Result.Fail<InstaUserListShortResponse>(exception);
            }
        }

        private async Task<IResult<InstaMediaListResponse>> GetUserMedia(long userId, PaginationParameters paginationParameters)
        {
            try
            {
                var instaUri = InstaUriCreator.GetUserMediaListUri(userId, paginationParameters.NextMaxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaMediaListResponse>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaListResponse>(json, new InstaMediaListDataConverter());
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

        private async Task<IResult<bool>> FavoriteUnfavoriteUser(Uri instaUri, long userId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new JObject
                {
                    { "user_id", userId.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<bool>(response, obj.Message, null);
                }

                return obj.Status.ToLower() == "ok"
                    ? Result.Success(true)
                    : Result.UnExpectedResponse<bool>(response, obj.Message, null);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>(ex);
            }
        }

        private async Task<IResult<InstaStoryFriendshipStatus>> MuteUnMuteUserMedia(
            Uri instaUri,
            long userId,
            InstaMuteOption muteUnmuteOption)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                switch (muteUnmuteOption)
                {
                    case InstaMuteOption.All:
                        data.Add("target_reel_author_id", userId.ToString());
                        data.Add("target_posts_author_id", userId.ToString());
                        break;
                    case InstaMuteOption.Post:
                        data.Add("target_posts_author_id", userId.ToString());
                        break;
                    case InstaMuteOption.Story:
                        data.Add("target_reel_author_id", userId.ToString());
                        break;
                }

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaStoryFriendshipStatus>(response, obj.Message, null);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaStoryFriendshipStatusContainerResponse>(json);
                var converter =
                    InstaConvertersFabric.Instance.GetStoryFriendshipStatusConverter(friendshipStatus.FriendshipStatus);

                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaStoryFriendshipStatus), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<InstaStoryFriendshipStatus>(ex);
            }
        }

        private async Task<IResult<InstaStoryFriendshipStatus>> HideUnhideMyStoryFromUser(Uri instaUri)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new JObject
                {
                    { "source", "profile" },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaStoryFriendshipStatus>(response, obj.Message, null);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaStoryFriendshipStatusContainerResponse>(json);
                var converter =
                    InstaConvertersFabric.Instance.GetStoryFriendshipStatusConverter(friendshipStatus.FriendshipStatus);

                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaStoryFriendshipStatus), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<InstaStoryFriendshipStatus>(ex);
            }
        }

        private async Task<IResult<InstaStoryFriendshipStatus>> MuteUnMuteFriendStory(Uri instaUri)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return Result.UnExpectedResponse<InstaStoryFriendshipStatus>(response, obj.Message, null);
                }

                var friendshipStatus = JsonConvert.DeserializeObject<InstaStoryFriendshipStatusContainerResponse>(json);
                var converter =
                    InstaConvertersFabric.Instance.GetStoryFriendshipStatusConverter(friendshipStatus.FriendshipStatus);

                return Result.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return Result.Fail(httpException, default(InstaStoryFriendshipStatus), ResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return Result.Fail<InstaStoryFriendshipStatus>(ex);
            }
        }

        private async Task<IResult<InstaMediaListResponse>> GetUserTags(
            long userId,
            PaginationParameters paginationParameters)
        {
            try
            {
                var uri = InstaUriCreator.GetUserTagsUri(userId, user.RankToken, paginationParameters?.NextMaxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
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
    }
}