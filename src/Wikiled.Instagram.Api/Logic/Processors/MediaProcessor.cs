using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Extensions;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Media api functions.
    /// </summary>
    internal class InstaMediaProcessor : IMediaProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly InstaUserAuthValidate userAuthValidate;

        public InstaMediaProcessor(
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
        ///     Add an post to archive list (this will show the post only for you!)
        /// </summary>
        /// <param name="mediaId">Media id (<see cref="InstaMedia.Identifier" />)</param>
        /// <returns>Return true if the media is archived</returns>
        public async Task<IResult<bool>> ArchiveMediaAsync(string mediaId)
        {
            return await LikeUnlikeArchiveUnArchiveMediaInternal(mediaId, InstaUriCreator.GetArchiveMediaUri(mediaId)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Delete a media (photo, video or album)
        /// </summary>
        /// <param name="mediaId">Media id (<see cref="InstaMedia.Identifier" />)</param>
        /// <param name="mediaType">The type of the media</param>
        /// <returns>Return true if the media is deleted</returns>
        public async Task<IResult<bool>> DeleteMediaAsync(string mediaId, InstaMediaType mediaType)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var deleteMediaUri = InstaUriCreator.GetDeleteMediaUri(mediaId, mediaType);

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "media_id", mediaId }
                };

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Get, deleteMediaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var deletedResponse = JsonConvert.DeserializeObject<InstaDeleteResponse>(json);
                return InstaResult.Success(deletedResponse.IsDeleted);
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
        ///     Edit the caption/location of the media (photo/video/album)
        /// </summary>
        /// <param name="mediaId">The media ID</param>
        /// <param name="caption">The new caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        /// <param name="userTags">User tags => Optional</param>
        /// <returns>Return true if everything is ok</returns>
        public async Task<IResult<InstaMedia>> EditMediaAsync(string mediaId,
                                                              string caption,
                                                              InstaLocationShort location = null,
                                                              InstaUserTagUpload[] userTags = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var editMediaUri = InstaUriCreator.GetEditMediaUri(mediaId);

                var currentMedia = await GetMediaByIdAsync(mediaId).ConfigureAwait(false);

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "caption_text", caption ?? string.Empty }
                };
                if (location != null)
                {
                    data.Add("location", location.GetJson());
                }

                var removeArr = new JArray();
                if (currentMedia.Succeeded)
                {
                    if (currentMedia.Value?.UserTags?.Count > 0)
                    {
                        foreach (var user in currentMedia.Value.UserTags)
                        {
                            removeArr.Add(user.User.Pk.ToString());
                        }
                    }
                }

                if (userTags?.Length > 0)
                {
                    var currentDelay = instaApi.GetRequestDelay();
                    instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));

                    var tagArr = new JArray();

                    foreach (var tag in userTags)
                    {
                        var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                        if (instaUser != null)
                        {
                            var position = new JArray(tag.X, tag.Y);
                            var singleTag = new JObject { { "user_id", instaUser.Pk }, { "position", position } };
                            tagArr.Add(singleTag);
                        }
                    }

                    instaApi.SetRequestDelay(currentDelay);
                    var root = new JObject { { "in", tagArr } };
                    if (removeArr.Any())
                    {
                        root.Add("removed", removeArr);
                    }

                    data.Add("usertags", root.ToString(Formatting.None));
                }
                else
                {
                    if (removeArr.Any())
                    {
                        var root = new JObject { { "removed", removeArr } };
                        data.Add("usertags", root.ToString(Formatting.None));
                    }
                }

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, editMediaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var mediaResponse = JsonConvert.DeserializeObject<InstaMediaItemResponse>(
                        json,
                        new InstaMediaDataConverter());
                    var converter = InstaConvertersFabric.Instance.GetSingleMediaConverter(mediaResponse);
                    return InstaResult.Success(converter.Convert());
                }

                var error = JsonConvert.DeserializeObject<InstaBadStatusResponse>(json);
                return InstaResult.Fail(error.Message, (InstaMedia)null);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        public async Task<IResult<InstaMediaList>> GetArchivedMediaAsync(PaginationParameters paginationParameters)
        {
            var mediaList = new InstaMediaList();
            try
            {
                if (paginationParameters == null)
                {
                    paginationParameters = PaginationParameters.MaxPagesToLoad(1);
                }

                InstaMediaList Convert(InstaMediaListResponse instaMediaListResponse)
                {
                    return InstaConvertersFabric.Instance.GetMediaListConverter(instaMediaListResponse).Convert();
                }

                var archivedPostsResult = await GetArchivedMedia(paginationParameters?.NextMaxId).ConfigureAwait(false);
                if (!archivedPostsResult.Succeeded)
                {
                    return InstaResult.Fail(archivedPostsResult.Info, mediaList);
                }

                var archivedResponse = archivedPostsResult.Value;

                mediaList = Convert(archivedResponse);
                mediaList.NextMaxId = paginationParameters.NextMaxId = archivedResponse.NextMaxId;

                paginationParameters.PagesLoaded++;
                while (archivedResponse.MoreAvailable &&
                    !string.IsNullOrEmpty(paginationParameters.NextMaxId) &&
                    paginationParameters.PagesLoaded < paginationParameters.MaximumPagesToLoad)
                {
                    paginationParameters.PagesLoaded++;
                    var nextMedia = await GetArchivedMedia(paginationParameters.NextMaxId).ConfigureAwait(false);
                    if (!nextMedia.Succeeded)
                    {
                        return InstaResult.Fail(nextMedia.Info, mediaList);
                    }

                    mediaList.NextMaxId = paginationParameters.NextMaxId = nextMedia.Value.NextMaxId;
                    archivedResponse.MoreAvailable = nextMedia.Value.MoreAvailable;
                    archivedResponse.ResultsCount += nextMedia.Value.ResultsCount;
                    mediaList.AddRange(Convert(nextMedia.Value));
                    paginationParameters.PagesLoaded++;
                }

                mediaList.Pages = paginationParameters.PagesLoaded;
                mediaList.PageSize = archivedResponse.ResultsCount;
                return InstaResult.Success(mediaList);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, mediaList, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, mediaList);
            }
        }

        /// <summary>
        ///     Get blocked medias
        ///     <para>Note: returns media ids!</para>
        /// </summary>
        public async Task<IResult<InstaMediaIdList>> GetBlockedMediasAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var mediaIds = new InstaMediaIdList();
            try
            {
                var mediaUri = InstaUriCreator.GetBlockedMediaUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, mediaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaMediaIdList>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaMediaIdsResponse>(json);

                return InstaResult.Success(obj.MediaIds);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, mediaIds, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, mediaIds);
            }
        }

        /// <summary>
        ///     Get media by its id asynchronously
        /// </summary>
        /// <param name="mediaId">Media id (<see cref="InstaMedia.Identifier" />)</param>
        /// <returns>
        ///     <see cref="InstaMedia" />
        /// </returns>
        public async Task<IResult<InstaMedia>> GetMediaByIdAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var mediaUri = InstaUriCreator.GetMediaUri(mediaId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, mediaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaMedia>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaListResponse>(
                    json,
                    new InstaMediaListDataConverter());
                if (mediaResponse.Medias?.Count > 1)
                {
                    var errorMessage = $"Got wrong media count for request with media id={mediaId}";
                    logger?.LogInformation(errorMessage);
                    return InstaResult.Fail<InstaMedia>(errorMessage);
                }

                var converter =
                    InstaConvertersFabric.Instance.GetSingleMediaConverter(mediaResponse.Medias.FirstOrDefault());
                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        /// <summary>
        ///     Get multiple media by its multiple ids asynchronously
        /// </summary>
        /// <param name="mediaIds">Media ids</param>
        /// <returns>
        ///     <see cref="InstaMediaList" />
        /// </returns>
        public async Task<IResult<InstaMediaList>> GetMediaByIdsAsync(params string[] mediaIds)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var mediaList = new InstaMediaList();
            try
            {
                if (mediaIds?.Length == 0)
                {
                    throw new ArgumentNullException("At least one media id is required");
                }

                var instaUri =
                    InstaUriCreator.GetMediaInfoByMultipleMediaIdsUri(mediaIds,
                                                                 deviceInfo.DeviceGuid.ToString(),
                                                                 user.CsrfToken);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaMediaList>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaListResponse>(
                    json,
                    new InstaMediaListDataConverter());
                mediaList = InstaConvertersFabric.Instance.GetMediaListConverter(mediaResponse).Convert();

                return InstaResult.Success(mediaList);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, mediaList, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, mediaList);
            }
        }

        /// <summary>
        ///     Get media ID from an url (got from "share link")
        /// </summary>
        /// <param name="uri">Uri to get media ID</param>
        /// <returns>Media ID</returns>
        public async Task<IResult<string>> GetMediaIdFromUrlAsync(Uri uri)
        {
            try
            {
                var collectionUri = InstaUriCreator.GetMediaIdFromUrlUri(uri);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, collectionUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<string>(response, json);
                }

                var data = JsonConvert.DeserializeObject<InstaOembedUrlResponse>(json);
                return InstaResult.Success(data.MediaId);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(string), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<string>(exception);
            }
        }

        /// <summary>
        ///     Get users (short) who liked certain media. Normaly it return around 1000 last users.
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<InstaLikersList>> GetMediaLikersAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var likers = new InstaLikersList();
                var likersUri = InstaUriCreator.GetMediaLikersUri(mediaId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, likersUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaLikersList>(response, json);
                }

                var mediaLikersResponse = JsonConvert.DeserializeObject<InstaMediaLikersResponse>(json);
                likers.UsersCount = mediaLikersResponse.UsersCount;
                if (mediaLikersResponse.UsersCount < 1)
                {
                    return InstaResult.Success(likers);
                }

                likers.AddRange(
                    mediaLikersResponse.Users.Select(InstaConvertersFabric.Instance.GetUserShortConverter)
                        .Select(converter => converter.Convert()));
                return InstaResult.Success(likers);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaLikersList), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaLikersList>(exception);
            }
        }

        /// <summary>
        ///     Get share link from media Id
        /// </summary>
        /// <param name="mediaId">media ID</param>
        /// <returns>Share link as Uri</returns>
        public async Task<IResult<Uri>> GetShareLinkFromMediaIdAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var collectionUri = InstaUriCreator.GetShareLinkFromMediaId(mediaId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, collectionUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<Uri>(response, json);
                }

                var data = JsonConvert.DeserializeObject<InstaPermalinkResponse>(json);
                return InstaResult.Success(new Uri(data.Permalink));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(Uri), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<Uri>(exception);
            }
        }

        /// <summary>
        ///     Like media (photo or video)
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<bool>> LikeMediaAsync(string mediaId)
        {
            return await LikeUnlikeArchiveUnArchiveMediaInternal(mediaId, InstaUriCreator.GetLikeMediaUri(mediaId)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Report media
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<bool>> ReportMediaAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetReportMediaUri(mediaId);
                var fields = new Dictionary<string, string>
                {
                    { "media_id", mediaId },
                    { "reason", "1" },
                    { "source_name", "photo_view_profile" },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Save media
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<bool>> SaveMediaAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSaveMediaUri(mediaId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Remove an post from archive list (this will show the post for everyone!)
        /// </summary>
        /// <param name="mediaId">Media id (<see cref="InstaMedia.Identifier" />)</param>
        /// <returns>Return true if the media is unarchived</returns>
        public async Task<IResult<bool>> UnArchiveMediaAsync(string mediaId)
        {
            return await LikeUnlikeArchiveUnArchiveMediaInternal(mediaId, InstaUriCreator.GetUnArchiveMediaUri(mediaId)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Remove like from media (photo or video)
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<bool>> UnLikeMediaAsync(string mediaId)
        {
            return await LikeUnlikeArchiveUnArchiveMediaInternal(mediaId, InstaUriCreator.GetUnLikeMediaUri(mediaId)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Unsave media
        /// </summary>
        /// <param name="mediaId">Media id</param>
        public async Task<IResult<bool>> UnSaveMediaAsync(string mediaId)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUnSaveMediaUri(mediaId);
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Upload album (videos and photos)
        /// </summary>
        /// <param name="images">Array of photos to upload</param>
        /// <param name="videos">Array of videos to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadAlbumAsync(InstaImageUpload[] images,
                                                                InstaVideoUpload[] videos,
                                                                string caption,
                                                                InstaLocationShort location = null)
        {
            return await UploadAlbumAsync(null, images, videos, caption, location).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload album (videos and photos)
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="images">Array of photos to upload</param>
        /// <param name="videos">Array of videos to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadAlbumAsync(
            Action<InstaUploaderProgress> progress,
            InstaImageUpload[] images,
            InstaVideoUpload[] videos,
            string caption,
            InstaLocationShort location = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var upProgress = new InstaUploaderProgress
            {
                Caption = caption ?? string.Empty,
                UploadState = InstaUploadState.Preparing
            };
            try
            {
                upProgress.Name = "Album upload";
                progress?.Invoke(upProgress);
                var imagesUploadIds = new Dictionary<string, InstaImageUpload>();
                var index = 1;
                if (images?.Length > 0)
                {
                    foreach (var image in images)
                    {
                        if (image.UserTags?.Count > 0)
                        {
                            var currentDelay = instaApi.GetRequestDelay();
                            instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));
                            foreach (var tag in image.UserTags)
                            {
                                var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                                if (instaUser != null)
                                {
                                    tag.Pk = instaUser.Pk;
                                }
                            }

                            instaApi.SetRequestDelay(currentDelay);
                        }
                    }

                    foreach (var image in images)
                    {
                        upProgress.Name = $"[Album] Photo uploading {index}/{images.Length}";
                        upProgress.UploadState = InstaUploadState.Uploading;
                        progress?.Invoke(upProgress);
                        upProgress.UploadState = InstaUploadState.Uploading;
                        progress?.Invoke(upProgress);
                        var uploadId = await UploadSinglePhoto(progress, image, upProgress).ConfigureAwait(false);
                        if (uploadId.Succeeded)
                        {
                            upProgress.UploadState = InstaUploadState.Uploaded;
                            progress?.Invoke(upProgress);
                            imagesUploadIds.Add(uploadId.Value, image);
                        }
                        else
                        {
                            upProgress.UploadState = InstaUploadState.Error;
                            progress?.Invoke(upProgress);
                            return InstaResult.Fail<InstaMedia>(uploadId.Info.Message);
                        }
                    }
                }

                var videosDic = new Dictionary<string, InstaVideoUpload>();
                var vidIndex = 1;
                if (videos?.Length > 0)
                {
                    foreach (var video in videos)
                    {
                        foreach (var tag in video.UserTags)
                        {
                            var currentDelay = instaApi.GetRequestDelay();
                            instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));
                            if (tag.Pk <= 0)
                            {
                                var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                                if (instaUser != null)
                                {
                                    tag.Pk = instaUser.Pk;
                                }
                            }

                            instaApi.SetRequestDelay(currentDelay);
                        }
                    }

                    foreach (var video in videos)
                    {
                        upProgress.Name = $"[Album] Video uploading {vidIndex}/{videos.Length}";
                        upProgress.UploadState = InstaUploadState.Uploading;
                        progress?.Invoke(upProgress);
                        var uploadId = await UploadSingleVideo(progress, video, upProgress).ConfigureAwait(false);
                        var thumb = await UploadSinglePhoto(progress,
                                                            video.VideoThumbnail.ConvertToImageUpload(),
                                                            upProgress,
                                                            uploadId.Value).ConfigureAwait(false);
                        videosDic.Add(uploadId.Value, video);

                        upProgress.UploadState = InstaUploadState.Uploaded;
                        progress?.Invoke(upProgress);
                        vidIndex++;
                    }
                }

                var config =
                    await ConfigureAlbumAsync(progress, upProgress, imagesUploadIds, videosDic, caption, location).ConfigureAwait(false);
                return config;
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        /// <summary>
        ///     Upload album (videos and photos)
        /// </summary>
        /// <param name="album">Array of photos or videos to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadAlbumAsync(InstaAlbumUpload[] album,
                                                                string caption,
                                                                InstaLocationShort location = null)
        {
            return await UploadAlbumAsync(null, album, caption, location).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload album (videos and photos) with progress
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="album">Array of photos or videos to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadAlbumAsync(Action<InstaUploaderProgress> progress,
                                                                InstaAlbumUpload[] album,
                                                                string caption,
                                                                InstaLocationShort location = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var upProgress = new InstaUploaderProgress
            {
                Caption = caption ?? string.Empty,
                UploadState = InstaUploadState.Preparing
            };
            try
            {
                upProgress.Name = "Album upload";
                progress?.Invoke(upProgress);
                var uploadIds = new Dictionary<string, InstaAlbumUpload>();
                var index = 1;

                foreach (var al in album)
                {
                    if (al.IsImage)
                    {
                        var image = al.ImageToUpload;
                        if (image.UserTags?.Count > 0)
                        {
                            var currentDelay = instaApi.GetRequestDelay();
                            instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));
                            foreach (var tag in image.UserTags)
                            {
                                if (tag.Pk <= 0)
                                {
                                    var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                                    if (instaUser != null)
                                    {
                                        tag.Pk = instaUser.Pk;
                                    }
                                }
                            }

                            instaApi.SetRequestDelay(currentDelay);
                        }
                    }
                    else if (al.IsVideo)
                    {
                        var video = al.VideoToUpload;
                        if (video.UserTags?.Count > 0)
                        {
                            var currentDelay = instaApi.GetRequestDelay();
                            instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));
                            foreach (var tag in video.UserTags)
                            {
                                if (tag.Pk > 0)
                                {
                                    continue;
                                }

                                var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                                if (instaUser != null)
                                {
                                    tag.Pk = instaUser.Pk;
                                }
                            }

                            instaApi.SetRequestDelay(currentDelay);
                        }
                    }
                }

                foreach (var al in album)
                {
                    if (al.IsImage)
                    {
                        upProgress.Name = $"[Album] uploading {index}/{album.Length}";
                        upProgress.UploadState = InstaUploadState.Uploading;
                        progress?.Invoke(upProgress);
                        var image = await UploadSinglePhoto(progress, al.ImageToUpload, upProgress).ConfigureAwait(false);
                        if (image.Succeeded)
                        {
                            uploadIds.Add(image.Value, al);
                        }
                    }
                    else if (al.IsVideo)
                    {
                        upProgress.Name = $"[Album] uploading {index}/{album.Length}";
                        upProgress.UploadState = InstaUploadState.Uploading;
                        progress?.Invoke(upProgress);
                        var video = await UploadSingleVideo(progress, al.VideoToUpload, upProgress).ConfigureAwait(false);
                        if (video.Succeeded)
                        {
                            var image = await UploadSinglePhoto(progress,
                                                                al.VideoToUpload.VideoThumbnail.ConvertToImageUpload(),
                                                                upProgress,
                                                                video.Value).ConfigureAwait(false);
                            uploadIds.Add(video.Value, al);
                        }
                    }

                    index++;
                }

                var config = await ConfigureAlbumAsync(progress, upProgress, uploadIds, caption, location).ConfigureAwait(false);
                return config;
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        /// <summary>
        ///     Upload photo [Supports user tags]
        /// </summary>
        /// <param name="image">Photo to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadPhotoAsync(InstaImageUpload image,
                                                                string caption,
                                                                InstaLocationShort location = null)
        {
            return await UploadPhotoAsync(null, image, caption, location).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload photo with progress [Supports user tags]
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="image">Photo to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadPhotoAsync(
            Action<InstaUploaderProgress> progress,
            InstaImageUpload image,
            string caption,
            InstaLocationShort location = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendMediaPhotoAsync(progress, image, caption, location).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload video [Supports user tags]
        /// </summary>
        /// <param name="video">Video and thumbnail to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadVideoAsync(InstaVideoUpload video,
                                                                string caption,
                                                                InstaLocationShort location = null)
        {
            return await UploadVideoAsync(null, video, caption, location).ConfigureAwait(false);
        }

        /// <summary>
        ///     Upload video with progress [Supports user tags]
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="video">Video and thumbnail to upload</param>
        /// <param name="caption">Caption</param>
        /// <param name="location">
        ///     Location => Optional (get it from <seealso cref="InstaLocationProcessor.SearchLocationAsync" />
        /// </param>
        public async Task<IResult<InstaMedia>> UploadVideoAsync(Action<InstaUploaderProgress> progress,
                                                                InstaVideoUpload video,
                                                                string caption,
                                                                InstaLocationShort location = null)
        {
            var upProgress = new InstaUploaderProgress
            {
                Caption = caption ?? string.Empty,
                UploadState = InstaUploadState.Preparing
            };
            try
            {
                if (video?.UserTags?.Count > 0)
                {
                    var currentDelay = instaApi.GetRequestDelay();
                    instaApi.SetRequestDelay(RequestDelay.FromSeconds(1, 2));
                    foreach (var tag in video.UserTags)
                    {
                        if (tag.Pk > 0)
                        {
                            continue;
                        }

                        var instaUser = await instaApi.UserProcessor.GetUserSafe(tag.Username, logger).ConfigureAwait(false);
                        if (instaUser != null)
                        {
                            tag.Pk = instaUser.Pk;
                        }
                    }

                    instaApi.SetRequestDelay(currentDelay);
                }

                upProgress.UploadState = InstaUploadState.Uploading;
                progress?.Invoke(upProgress);
                var uploadVideo = await UploadSingleVideo(progress, video, upProgress, false).ConfigureAwait(false);

                if (!uploadVideo.Succeeded)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.Fail<InstaMedia>(uploadVideo.Info.Message);
                }

                upProgress.UploadState = InstaUploadState.Uploaded;
                progress?.Invoke(upProgress);

                upProgress.UploadState = InstaUploadState.UploadingThumbnail;
                progress?.Invoke(upProgress);

                var uploadPhoto = await UploadSinglePhoto(progress,
                                                          video.VideoThumbnail.ConvertToImageUpload(),
                                                          upProgress,
                                                          uploadVideo.Value,
                                                          false).ConfigureAwait(false);

                if (uploadPhoto.Succeeded)
                {
                    //upProgress = progressContent?.UploaderProgress;
                    upProgress.UploadState = InstaUploadState.ThumbnailUploaded;
                    progress?.Invoke(upProgress);
                    return await ConfigureVideoAsync(progress, upProgress, video, uploadVideo.Value, caption, location).ConfigureAwait(false);
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.Fail<InstaMedia>(uploadPhoto.Value);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        private async Task<IResult<InstaMedia>> ConfigureAlbumAsync(
            Action<InstaUploaderProgress> progress,
            InstaUploaderProgress upProgress,
            Dictionary<string, InstaAlbumUpload> album,
            string caption,
            InstaLocationShort location)
        {
            try
            {
                upProgress.Name = "Album upload";
                upProgress.UploadState = InstaUploadState.Configuring;
                progress?.Invoke(upProgress);
                var instaUri = InstaUriCreator.GetMediaAlbumConfigureUri();
                var clientSidecarId = ApiRequestMessage.GenerateUploadId();
                var childrenArray = new JArray();

                foreach (var al in album)
                {
                    if (al.Value.IsImage)
                    {
                        childrenArray.Add(GetImageConfigure(al.Key, al.Value.ImageToUpload));
                    }
                    else if (al.Value.IsVideo)
                    {
                        childrenArray.Add(GetVideoConfigure(al.Key, al.Value.VideoToUpload));
                    }
                }

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "caption", caption },
                    { "client_sidecar_id", clientSidecarId },
                    { "upload_id", clientSidecarId },
                    { "timezone_offset", InstaApiConstants.TimezoneOffset.ToString() },
                    { "source_type", "4" },
                    { "device_id", deviceInfo.DeviceId },
                    { "creation_logger_session_id", Guid.NewGuid().ToString() },
                    {
                        "device",
                        new JObject
                        {
                            { "manufacturer", deviceInfo.HardwareManufacturer },
                            { "model", deviceInfo.DeviceModelIdentifier },
                            { "android_release", deviceInfo.AndroidVer.VersionNumber },
                            { "android_version", deviceInfo.AndroidVer.ApiLevel }
                        }
                    },
                    { "children_metadata", childrenArray }
                };
                if (location != null)
                {
                    data.Add("location", location.GetJson());
                    data.Add("date_time_digitalized", DateTime.Now.ToString("yyyy:dd:MM+h:mm:ss"));
                }

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaMedia>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaAlbumResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetSingleMediaFromAlbumConverter(mediaResponse);
                var obj = converter.Convert();
                if (obj.Caption == null && !string.IsNullOrEmpty(caption))
                {
                    var editedMedia =
                        await instaApi.MediaProcessor.EditMediaAsync(obj.Identifier, caption, location).ConfigureAwait(false);
                    if (editedMedia.Succeeded)
                    {
                        upProgress.UploadState = InstaUploadState.Configured;
                        progress?.Invoke(upProgress);
                        upProgress.UploadState = InstaUploadState.Completed;
                        progress?.Invoke(upProgress);
                        return InstaResult.Success(editedMedia.Value);
                    }
                }

                upProgress.UploadState = InstaUploadState.Configured;
                progress?.Invoke(upProgress);
                upProgress.UploadState = InstaUploadState.Completed;
                progress?.Invoke(upProgress);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        private async Task<IResult<InstaMedia>> ConfigureAlbumAsync(
            Action<InstaUploaderProgress> progress,
            InstaUploaderProgress upProgress,
            Dictionary<string, InstaImageUpload> imagesUploadIds,
            Dictionary<string, InstaVideoUpload> videos,
            string caption,
            InstaLocationShort location)
        {
            try
            {
                upProgress.Name = "Album upload";
                upProgress.UploadState = InstaUploadState.Configuring;
                progress?.Invoke(upProgress);
                var instaUri = InstaUriCreator.GetMediaAlbumConfigureUri();
                var clientSidecarId = ApiRequestMessage.GenerateUploadId();
                var childrenArray = new JArray();
                if (imagesUploadIds != null && imagesUploadIds.Any())
                {
                    foreach (var img in imagesUploadIds)
                    {
                        childrenArray.Add(GetImageConfigure(img.Key, img.Value));
                    }
                }

                if (videos != null && videos.Any())
                {
                    foreach (var id in videos)
                    {
                        childrenArray.Add(GetVideoConfigure(id.Key, id.Value));
                    }
                }

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "caption", caption },
                    { "client_sidecar_id", clientSidecarId },
                    { "upload_id", clientSidecarId },
                    {
                        "device",
                        new JObject
                        {
                            { "manufacturer", deviceInfo.HardwareManufacturer },
                            { "model", deviceInfo.DeviceModelIdentifier },
                            { "android_release", deviceInfo.AndroidVer.VersionNumber },
                            { "android_version", deviceInfo.AndroidVer.ApiLevel }
                        }
                    },
                    { "children_metadata", childrenArray }
                };
                if (location != null)
                {
                    data.Add("location", location.GetJson());
                    data.Add("date_time_digitalized", DateTime.Now.ToString("yyyy:dd:MM+h:mm:ss"));
                }

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaMedia>(response, json);
                }

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaAlbumResponse>(json);
                var converter = InstaConvertersFabric.Instance.GetSingleMediaFromAlbumConverter(mediaResponse);
                var obj = converter.Convert();
                if (obj.Caption == null && !string.IsNullOrEmpty(caption))
                {
                    var editedMedia =
                        await instaApi.MediaProcessor.EditMediaAsync(obj.Identifier, caption, location).ConfigureAwait(false);
                    if (editedMedia.Succeeded)
                    {
                        upProgress.UploadState = InstaUploadState.Configured;
                        progress?.Invoke(upProgress);
                        upProgress.UploadState = InstaUploadState.Completed;
                        progress?.Invoke(upProgress);
                        return InstaResult.Success(editedMedia.Value);
                    }
                }

                upProgress.UploadState = InstaUploadState.Configured;
                progress?.Invoke(upProgress);
                upProgress.UploadState = InstaUploadState.Completed;
                progress?.Invoke(upProgress);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        private async Task<IResult<InstaMedia>> ConfigureVideoAsync(
            Action<InstaUploaderProgress> progress,
            InstaUploaderProgress upProgress,
            InstaVideoUpload video,
            string uploadId,
            string caption,
            InstaLocationShort location)
        {
            try
            {
                upProgress.UploadState = InstaUploadState.Configuring;
                progress?.Invoke(upProgress);
                var instaUri = InstaUriCreator.GetMediaConfigureUri(true);
                var data = new JObject
                {
                    { "caption", caption ?? string.Empty },
                    { "upload_id", uploadId },
                    { "source_type", "4" },
                    { "camera_position", "unknown" },
                    { "creation_logger_session_id", Guid.NewGuid().ToString() },
                    { "timezone_offset", InstaApiConstants.TimezoneOffset.ToString() },
                    { "date_time_original", DateTime.Now.ToString("yyyy-dd-MMTh:mm:ss-0fffZ") },
                    { "extra", new JObject { { "source_width", 0 }, { "source_height", 0 } } },
                    {
                        "clips",
                        new JArray
                        {
                            new JObject
                            {
                                { "length", 0 },
                                { "creation_date", DateTime.Now.ToString("yyyy-dd-MMTh:mm:ss-0fff") },
                                { "source_type", "3" },
                                { "camera_position", "back" }
                            }
                        }
                    },
                    { "poster_frame_index", 0 },
                    { "audio_muted", false },
                    { "filter_type", "0" },
                    { "video_result", "" },
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.UserName }
                };
                if (location != null)
                {
                    data.Add("location", location.GetJson());
                    data.Add("date_time_digitalized", DateTime.Now.ToString("yyyy:dd:MM+h:mm:ss"));
                }

                if (video.UserTags?.Count > 0)
                {
                    var tagArr = new JArray();
                    foreach (var tag in video.UserTags)
                    {
                        if (tag.Pk != -1)
                        {
                            var position = new JArray(0.0, 0.0);
                            var singleTag = new JObject { { "user_id", tag.Pk }, { "position", position } };
                            tagArr.Add(singleTag);
                        }
                    }

                    var root = new JObject { { "in", tagArr } };
                    data.Add("usertags", root.ToString(Formatting.None));
                }

                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post,
                                                 InstaUriCreator.GetMediaUploadFinishUri(),
                                                 deviceInfo,
                                                 data);
                request.Headers.Host = "i.instagram.com";
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaMedia>(response, json);
                }

                upProgress.UploadState = InstaUploadState.Configured;
                progress?.Invoke(upProgress);

                var mediaResponse = JsonConvert.DeserializeObject<InstaMediaItemResponse>(
                    json,
                    new InstaMediaDataConverter());
                var converter = InstaConvertersFabric.Instance.GetSingleMediaConverter(mediaResponse);
                var obj = converter.Convert();
                if (obj.Caption == null && !string.IsNullOrEmpty(caption))
                {
                    var editedMedia =
                        await instaApi.MediaProcessor.EditMediaAsync(obj.Identifier, caption, location).ConfigureAwait(false);
                    if (editedMedia.Succeeded)
                    {
                        return InstaResult.Success(editedMedia.Value);
                    }
                }

                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMedia), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaMedia>(exception);
            }
        }

        private async Task<IResult<InstaMediaListResponse>> GetArchivedMedia(string nextMaxId)
        {
            var mediaList = new InstaMediaList();
            try
            {
                var instaUri = InstaUriCreator.GetArchivedMediaFeedsListUri(nextMaxId);
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaMediaListResponse>(response, json);
                }

                var archivedResponse = JsonConvert.DeserializeObject<InstaMediaListResponse>(json);
                return InstaResult.Success(archivedResponse);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaMediaListResponse), InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail<InstaMediaListResponse>(ex);
            }
        }

        private JObject GetImageConfigure(string uploadId, InstaImageUpload image)
        {
            var imgData = new JObject
            {
                { "timezone_offset", InstaApiConstants.TimezoneOffset.ToString() },
                { "source_type", "4" },
                { "upload_id", uploadId },
                { "caption", "" },
                {
                    "extra", JsonConvert.SerializeObject(
                        new JObject { { "source_width", 0 }, { "source_height", 0 } })
                },
                {
                    "device", JsonConvert.SerializeObject(
                        new JObject
                        {
                            { "manufacturer", deviceInfo.HardwareManufacturer },
                            { "model", deviceInfo.DeviceModelIdentifier },
                            { "android_release", deviceInfo.AndroidVer.VersionNumber },
                            { "android_version", deviceInfo.AndroidVer.ApiLevel }
                        })
                }
            };
            if (image.UserTags?.Count > 0)
            {
                var tagArr = new JArray();
                foreach (var tag in image.UserTags)
                {
                    if (tag.Pk != -1)
                    {
                        var position = new JArray(tag.X, tag.Y);
                        var singleTag = new JObject { { "user_id", tag.Pk }, { "position", position } };
                        tagArr.Add(singleTag);
                    }
                }

                var root = new JObject { { "in", tagArr } };
                imgData.Add("usertags", root.ToString(Formatting.None));
            }

            return imgData;
        }

        private JObject GetVideoConfigure(string uploadId, InstaVideoUpload video)
        {
            var vidData = new JObject
            {
                { "timezone_offset", InstaApiConstants.TimezoneOffset.ToString() },
                { "caption", "" },
                { "upload_id", uploadId },
                { "date_time_original", DateTime.Now.ToString("yyyy-dd-MMTh:mm:ss-0fffZ") },
                { "source_type", "4" },
                {
                    "extra", JsonConvert.SerializeObject(
                        new JObject { { "source_width", 0 }, { "source_height", 0 } })
                },
                {
                    "clips", JsonConvert.SerializeObject(
                        new JArray { new JObject { { "length", video.Video.Length }, { "source_type", "4" } } })
                },
                {
                    "device", JsonConvert.SerializeObject(
                        new JObject
                        {
                            { "manufacturer", deviceInfo.HardwareManufacturer },
                            { "model", deviceInfo.DeviceModelIdentifier },
                            { "android_release", deviceInfo.AndroidVer.VersionNumber },
                            { "android_version", deviceInfo.AndroidVer.ApiLevel }
                        })
                },
                { "length", video.Video.Length.ToString() },
                { "poster_frame_index", "0" },
                { "audio_muted", "false" },
                { "filter_type", "0" },
                { "video_result", "" }
            };
            if (video.UserTags?.Count > 0)
            {
                var tagArr = new JArray();
                foreach (var tag in video.UserTags)
                {
                    if (tag.Pk != -1)
                    {
                        var position = new JArray(0.0, 0.0);
                        var singleTag = new JObject { { "user_id", tag.Pk }, { "position", position } };
                        tagArr.Add(singleTag);
                    }
                }

                var root = new JObject { { "in", tagArr } };
                vidData.Add("usertags", root.ToString(Formatting.None));
            }

            return vidData;
        }

        private async Task<IResult<bool>> LikeUnlikeArchiveUnArchiveMediaInternal(string mediaId, Uri instaUri)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "media_id", mediaId },
                    { "radio_type", "wifi-none" }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK
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
                return InstaResult.Fail<bool>(exception);
            }
        }

        private async Task<IResult<string>> UploadSinglePhoto(
            Action<InstaUploaderProgress> progress,
            InstaImageUpload image,
            InstaUploaderProgress upProgress,
            string uploadId = null,
            bool album = true)
        {
            if (string.IsNullOrEmpty(uploadId))
            {
                uploadId = ApiRequestMessage.GenerateUploadId();
            }

            var photoHashCode = Path.GetFileName(image.Uri ?? $"C:\\{13.GenerateRandomString()}.jpg").GetHashCode();
            var photoEntityName = $"{uploadId}_0_{photoHashCode}";
            var photoUri = InstaUriCreator.GetStoryUploadPhotoUri(uploadId, photoHashCode);
            var photoUploadParamsObj = new JObject
            {
                { "upload_id", uploadId },
                { "media_type", "1" },
                { "retry_context", InstaHelperProcessor.GetRetryContext() },
                { "image_compression", "{\"lib_name\":\"moz\",\"lib_version\":\"3.1.m\",\"quality\":\"95\"}" },
                { "xsharing_user_ids", "[]" }
            };
            if (album)
            {
                photoUploadParamsObj.Add("is_sidecar", "1");
            }

            upProgress.UploadState = InstaUploadState.UploadingThumbnail;
            progress?.Invoke(upProgress);
            var photoUploadParams = JsonConvert.SerializeObject(photoUploadParamsObj);
            var imageBytes = image.ImageBytes ?? File.ReadAllBytes(image.Uri);
            var imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.Add("Content-Transfer-Encoding", "binary");
            imageContent.Headers.Add("Content-Type", "application/octet-stream");
            var request = httpHelper.GetDefaultRequest(HttpMethod.Post, photoUri, deviceInfo);
            request.Content = imageContent;
            request.Headers.Add("X-Entity-Type", "image/jpeg");
            request.Headers.Add("Offset", "0");
            request.Headers.Add("X-Instagram-Rupload-Params", photoUploadParams);
            request.Headers.Add("X-Entity-Name", photoEntityName);
            request.Headers.Add("X-Entity-Length", imageBytes.Length.ToString());
            request.Headers.Add("X_FB_PHOTO_WATERFALL_ID", Guid.NewGuid().ToString());
            var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                //upProgress = progressContent?.UploaderProgress;
                upProgress.UploadState = InstaUploadState.Uploaded;
                progress?.Invoke(upProgress);
                return InstaResult.Success(uploadId);
            }

            upProgress.UploadState = InstaUploadState.Error;
            progress?.Invoke(upProgress);
            return InstaResult.Fail<string>("NO UPLOAD ID");
        }

        private async Task<IResult<string>> UploadSingleVideo(Action<InstaUploaderProgress> progress,
                                                              InstaVideoUpload video,
                                                              InstaUploaderProgress upProgress,
                                                              bool album = true)
        {
            var uploadId = ApiRequestMessage.GenerateRandomUploadId();
            var videoHashCode = Path.GetFileName(video.Video.Uri ?? $"C:\\{13.GenerateRandomString()}.mp4")
                .GetHashCode();
            var waterfallId = Guid.NewGuid().ToString();
            var videoEntityName = $"{uploadId}_0_{videoHashCode}";
            var videoUri = InstaUriCreator.GetStoryUploadVideoUri(uploadId, videoHashCode);
            var retryContext = InstaHelperProcessor.GetRetryContext();
            HttpRequestMessage request = null;
            HttpResponseMessage response = null;
            string videoUploadParams = null;
            string json = null;

            var videoUploadParamsObj = new JObject
            {
                { "upload_media_height", "0" },
                { "upload_media_width", "0" },
                { "upload_media_duration_ms", "0" },
                { "upload_id", uploadId },
                { "retry_context", retryContext },
                { "media_type", "2" },
                { "xsharing_user_ids", "[]" }
            };
            if (album)
            {
                videoUploadParamsObj.Add("is_sidecar", "1");
            }

            videoUploadParams = JsonConvert.SerializeObject(videoUploadParamsObj);
            request = httpHelper.GetDefaultRequest(HttpMethod.Get, videoUri, deviceInfo);
            request.Headers.Add("X_FB_VIDEO_WATERFALL_ID", waterfallId);
            request.Headers.Add("X-Instagram-Rupload-Params", videoUploadParams);
            response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
            json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<string>(response, json);
            }

            var videoBytes = video.Video.VideoBytes ?? File.ReadAllBytes(video.Video.Uri);

            var videoContent = new ByteArrayContent(videoBytes);

            //var progressContent = new ProgressableStreamContent(videoContent, 4096, progress)
            //{
            //    UploaderProgress = upProgress
            //};
            request = httpHelper.GetDefaultRequest(HttpMethod.Post, videoUri, deviceInfo);
            request.Content = videoContent;
            upProgress.UploadState = InstaUploadState.Uploading;
            progress?.Invoke(upProgress);
            var vidExt = Path.GetExtension(video.Video.Uri ?? $"C:\\{13.GenerateRandomString()}.mp4")
                .Replace(".", "")
                .ToLower();
            if (vidExt == "mov")
            {
                request.Headers.Add("X-Entity-Type", "video/quicktime");
            }
            else
            {
                request.Headers.Add("X-Entity-Type", "video/mp4");
            }

            request.Headers.Add("Offset", "0");
            request.Headers.Add("X-Instagram-Rupload-Params", videoUploadParams);
            request.Headers.Add("X-Entity-Name", videoEntityName);
            request.Headers.Add("X-Entity-Length", videoBytes.Length.ToString());
            request.Headers.Add("X_FB_VIDEO_WATERFALL_ID", waterfallId);
            response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
            json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.UnExpectedResponse<string>(response, json);
            }

            return InstaResult.Success(uploadId);
        }

        private async Task<IResult<bool>> UploadVideoThumbnailAsync(Action<InstaUploaderProgress> progress,
                                                                    InstaUploaderProgress upProgress,
                                                                    InstaImage image,
                                                                    string uploadId)
        {
            try
            {
                var instaUri = InstaUriCreator.GetUploadPhotoUri();
                upProgress.UploadState = InstaUploadState.UploadingThumbnail;
                progress?.Invoke(upProgress);
                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    { new StringContent(uploadId), "\"upload_id\"" },
                    { new StringContent(deviceInfo.DeviceGuid.ToString()), "\"_uuid\"" },
                    { new StringContent(user.CsrfToken), "\"_csrftoken\"" },
                    {
                        new StringContent("{\"lib_name\":\"jt\",\"lib_version\":\"1.3.0\",\"quality\":\"87\"}"),
                        "\"image_compression\""
                    }
                };
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
                requestContent.Add(imageContent, "photo", $"pending_media_{uploadId}.jpg");
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = requestContent;
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var imgResp = JsonConvert.DeserializeObject<InstaImageThumbnailResponse>(json);
                if (imgResp.Status.ToLower() == "ok")
                {
                    upProgress.UploadState = InstaUploadState.ThumbnailUploaded;
                    progress?.Invoke(upProgress);
                    return InstaResult.Success(true);
                }

                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                return InstaResult.Fail<bool>("Could not upload thumbnail");
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
    }
}