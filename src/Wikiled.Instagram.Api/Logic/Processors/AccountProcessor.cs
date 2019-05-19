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
using Wikiled.Instagram.Api.Classes.Models.Account;
using Wikiled.Instagram.Api.Classes.Models.Business;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Business;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Login;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Converters.Json;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    /// <summary>
    ///     Account api functions.
    ///     <para>Note: this is for self account.</para>
    /// </summary>
    internal class InstaAccountProcessor : IAccountProcessor
    {
        private readonly AndroidDevice deviceInfo;

        private readonly InstaHttpHelper httpHelper;

        private readonly IHttpRequestProcessor httpRequestProcessor;

        private readonly InstaApi instaApi;

        private readonly ILogger logger;

        private readonly UserSessionData user;

        private readonly UserAuthValidate userAuthValidate;

        public InstaAccountProcessor(
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
        ///     Set current account private
        /// </summary>
        public async Task<IResult<UserShortDescription>> SetAccountPrivateAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUriSetAccountPrivate();
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var hash = InstaCryptoHelper.CalculateHash(
                    httpHelper.ApiVersion.SignatureKey,
                    JsonConvert.SerializeObject(fields));
                var payload = JsonConvert.SerializeObject(fields);
                var signature = $"{hash}.{Uri.EscapeDataString(payload)}";
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = new FormUrlEncodedContent(fields);
                request.Properties.Add(InstaApiConstants.HeaderIgSignature, signature);
                request.Properties.Add(
                    InstaApiConstants.HeaderIgSignatureKeyVersion,
                    InstaApiConstants.IgSignatureKeyVersion);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<UserShortDescription>(response, json);
                }

                var userInfoUpdated =
                    JsonConvert.DeserializeObject<InstaUserShortResponse>(json, new InstaUserShortDataConverter());
                if (userInfoUpdated.Pk < 1)
                {
                    return InstaResult.Fail<UserShortDescription>("Pk is null or empty");
                }

                var converter = InstaConvertersFabric.Instance.GetUserShortConverter(userInfoUpdated);
                return InstaResult.Success(converter.Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(UserShortDescription), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<UserShortDescription>(exception);
            }
        }

        /// <summary>
        ///     Set current account public
        /// </summary>
        public async Task<IResult<UserShortDescription>> SetAccountPublicAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetUriSetAccountPublic();
                var fields = new Dictionary<string, string>
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var hash = InstaCryptoHelper.CalculateHash(
                    httpHelper.ApiVersion.SignatureKey,
                    JsonConvert.SerializeObject(fields));
                var payload = JsonConvert.SerializeObject(fields);
                var signature = $"{hash}.{Uri.EscapeDataString(payload)}";
                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = new FormUrlEncodedContent(fields);
                request.Properties.Add(InstaApiConstants.HeaderIgSignature, signature);
                request.Properties.Add(
                    InstaApiConstants.HeaderIgSignatureKeyVersion,
                    InstaApiConstants.IgSignatureKeyVersion);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userInfoUpdated =
                        JsonConvert.DeserializeObject<InstaUserShortResponse>(json, new InstaUserShortDataConverter());
                    if (userInfoUpdated.Pk < 1)
                    {
                        return InstaResult.Fail<UserShortDescription>("Pk is incorrect");
                    }

                    var converter = InstaConvertersFabric.Instance.GetUserShortConverter(userInfoUpdated);
                    return InstaResult.Success(converter.Convert());
                }

                return InstaResult.UnExpectedResponse<UserShortDescription>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(UserShortDescription), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<UserShortDescription>(exception);
            }
        }

        /// <summary>
        ///     Change password
        /// </summary>
        /// <param name="oldPassword">The old password</param>
        /// <param name="newPassword">
        ///     The new password (shouldn't be the same old password, and should be a password you never used
        ///     here)
        /// </param>
        /// <returns>Return true if the password is changed</returns>
        public async Task<IResult<bool>> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            if (oldPassword == newPassword)
            {
                return InstaResult.Fail("The old password should not the same of the new password", false);
            }

            try
            {
                var changePasswordUri = InstaUriCreator.GetChangePasswordUri();

                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk },
                    { "_csrftoken", user.CsrfToken },
                    { "old_password", oldPassword },
                    { "new_password1", newPassword },
                    { "new_password2", newPassword }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Get, changePasswordUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return InstaResult.Success(true);
                }

                var error = JsonConvert.DeserializeObject<InstaBadStatusErrorsResponse>(json);
                var errors = "";
                error.Message.Errors.ForEach(errorContent => errors += errorContent + "\n");
                return InstaResult.Fail(errors, false);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Edit profile
        /// </summary>
        /// <param name="name">Name (leave null if you don't want to change it)</param>
        /// <param name="biography">Biography (leave null if you don't want to change it)</param>
        /// <param name="url">Url (leave null if you don't want to change it)</param>
        /// <param name="email">Email (leave null if you don't want to change it)</param>
        /// <param name="phone">Phone number (leave null if you don't want to change it)</param>
        /// <param name="gender">Gender type (leave null if you don't want to change it)</param>
        /// <param name="newUsername">New username (optional) (leave null if you don't want to change it)</param>
        public async Task<IResult<InstaUserEdit>> EditProfileAsync(string name,
                                                                   string biography,
                                                                   string url,
                                                                   string email,
                                                                   string phone,
                                                                   GenderType? gender,
                                                                   string newUsername = null)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var editRequest = await GetRequestForEditProfileAsync().ConfigureAwait(false);
                if (!editRequest.Succeeded)
                {
                    return InstaResult.Fail("Edit request returns badrequest", (InstaUserEdit)null);
                }

                var user = editRequest.Value.Username;

                if (string.IsNullOrEmpty(newUsername))
                {
                    newUsername = user;
                }

                if (name == null)
                {
                    name = editRequest.Value.FullName;
                }

                if (biography == null)
                {
                    biography = editRequest.Value.Biography;
                }

                if (url == null)
                {
                    url = editRequest.Value.ExternalUrl;
                }

                if (email == null)
                {
                    email = editRequest.Value.Email;
                }

                if (phone == null)
                {
                    phone = editRequest.Value.PhoneNumber;
                }

                if (gender == null)
                {
                    gender = editRequest.Value.Gender;
                }

                var instaUri = InstaUriCreator.GetEditProfileUri();

                var data = new JObject
                {
                    { "external_url", url },
                    { "gender", ((int)gender).ToString() },
                    { "phone_number", phone },
                    { "_csrftoken", this.user.CsrfToken },
                    { "username", newUsername },
                    { "first_name", name },
                    { "_uid", this.user.LoggedInUser.Pk.ToString() },
                    { "biography", biography },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "email", email }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserEdit>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserEditContainer>(json);

                return InstaResult.Success(obj.User);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserEdit), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUserEdit>(exception);
            }
        }

        /// <summary>
        ///     Set biography (support hashtags and user mentions)
        /// </summary>
        /// <param name="bio">Biography text, hashtags or user mentions</param>
        public async Task<IResult<InstaBiography>> SetBiographyAsync(string bio)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var editRequest = await GetRequestForEditProfileAsync().ConfigureAwait(false);
                if (!editRequest.Succeeded)
                {
                    return InstaResult.Fail("Edit request returns badrequest.\r\nPlease try again.", (InstaBiography)null);
                }

                var instaUri = InstaUriCreator.GetSetBiographyUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "raw_text", bio }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBiography>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBiography>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBiography), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBiography>(exception);
            }
        }

        /// <summary>
        ///     Get request for edit profile.
        /// </summary>
        public async Task<IResult<InstaUserEdit>> GetRequestForEditProfileAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetRequestForEditProfileUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserEdit>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserEditContainer>(json);
                return InstaResult.Success(obj.User);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserEdit), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUserEdit>(exception);
            }
        }

        /// <summary>
        ///     Set name and phone number.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="phoneNumber">Phone number</param>
        public async Task<IResult<bool>> SetNameAndPhoneNumberAsync(string name, string phoneNumber = "")
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetProfileSetPhoneAndNameUri();
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken },
                    { "first_name", name },
                    { "phone_number", phoneNumber }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefaultResponse>(json);
                if (obj.Status.ToLower() == "ok")
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<bool>(exception);
            }
        }

        /// <summary>
        ///     Remove profile picture.
        /// </summary>
        public async Task<IResult<InstaUserEdit>> RemoveProfilePictureAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetRemoveProfilePictureUri();
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUserEdit>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserEditContainer>(json);

                return InstaResult.Success(obj.User);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserEdit), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUserEdit>(exception);
            }
        }

        /// <summary>
        ///     Change profile picture(only jpg and jpeg formats).
        /// </summary>
        /// <param name="pictureBytes">Picture(JPG,JPEG) bytes</param>
        public Task<IResult<InstaUserEdit>> ChangeProfilePictureAsync(byte[] pictureBytes)
        {
            return ChangeProfilePictureAsync(null, pictureBytes);
        }

        /// <summary>
        ///     Change profile picture(only jpg and jpeg formats).
        /// </summary>
        /// <param name="progress">Progress action</param>
        /// <param name="pictureBytes">Picture(JPG,JPEG) bytes</param>
        public async Task<IResult<InstaUserEdit>> ChangeProfilePictureAsync(
            Action<UploaderProgress> progress,
            byte[] pictureBytes)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            var upProgress = new UploaderProgress
            {
                Caption = string.Empty, UploadState = InstaUploadState.Preparing
            };
            try
            {
                var instaUri = InstaUriCreator.GetChangeProfilePictureUri();
                var uploadId = ApiRequestMessage.GenerateUploadId();
                upProgress.UploadId = uploadId;
                progress?.Invoke(upProgress);
                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    { new StringContent(deviceInfo.DeviceGuid.ToString()), "\"_uuid\"" },
                    { new StringContent(user.LoggedInUser.Pk.ToString()), "\"_uid\"" },
                    { new StringContent(user.CsrfToken), "\"_csrftoken\"" }
                };
                var imageContent = new ByteArrayContent(pictureBytes);
                requestContent.Add(imageContent, "profile_pic", $"r{ApiRequestMessage.GenerateUploadId()}.jpg");

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

                //if (progressContent.UploaderProgress != null)
                //    upProgress = progressContent.UploaderProgress;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    upProgress.UploadState = InstaUploadState.Error;
                    progress?.Invoke(upProgress);
                    return InstaResult.UnExpectedResponse<InstaUserEdit>(response, json);
                }

                upProgress.UploadState = InstaUploadState.Uploaded;
                progress?.Invoke(upProgress);

                var obj = JsonConvert.DeserializeObject<InstaUserEditContainer>(json);
                upProgress.UploadState = InstaUploadState.Completed;
                progress?.Invoke(upProgress);
                return InstaResult.Success(obj.User);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserEdit), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                upProgress.UploadState = InstaUploadState.Error;
                progress?.Invoke(upProgress);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUserEdit>(exception);
            }
        }

        /// <summary>
        ///     Get request for download backup account data.
        /// </summary>
        /// <param name="email">Email</param>
        public Task<IResult<InstaRequestDownloadData>> GetRequestForDownloadAccountDataAsync(string email)
        {
            return GetRequestForDownloadAccountDataAsync(email, null);
        }

        /// <summary>
        ///     Get request for download backup account data.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password (only for facebook logins)</param>
        public async Task<IResult<InstaRequestDownloadData>> GetRequestForDownloadAccountDataAsync(
            string email,
            string password)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    password = user.Password;
                }

                var instaUri = InstaUriCreator.GetRequestForDownloadDataUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "email", email },
                    { "password", password }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaRequestDownloadData>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaRequestDownloadData), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaRequestDownloadData>(exception);
            }
        }

        /// <summary>
        ///     Upload nametag image
        /// </summary>
        /// <param name="nametagImage">Nametag image</param>
        public async Task<IResult<InstaMedia>> UploadNametagAsync(InstaImage nametagImage)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            return await instaApi.HelperProcessor.SendMediaPhotoAsync(null,
                                                                      nametagImage.ConvertToImageUpload(),
                                                                      null,
                                                                      null,
                                                                      true).ConfigureAwait(false);
        }

        // Story settings
        /// <summary>
        ///     Get story settings.
        /// </summary>
        public async Task<IResult<InstaStorySettings>> GetStorySettingsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetStorySettingsUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaStorySettings>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaStorySettings>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaStorySettings), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaStorySettings>(exception);
            }
        }

        /// <summary>
        ///     Enable Save story to gallery.
        /// </summary>
        public async Task<IResult<bool>> EnableSaveStoryToGalleryAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSetReelSettingsUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "save_to_camera_roll", 1.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                if (obj.Status.ToLower() == "ok")
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
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
        ///     Disable Save story to gallery.
        /// </summary>
        public async Task<IResult<bool>> DisableSaveStoryToGalleryAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSetReelSettingsUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "save_to_camera_roll", 0.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                if (obj.Status.ToLower() == "ok")
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
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
        ///     Enable Save story to archive.
        /// </summary>
        public async Task<IResult<bool>> EnableSaveStoryToArchiveAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSetReelSettingsUri();

                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "reel_auto_archive", "on" }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountArchiveStory>(json);
                if (obj.ReelAutoArchive.ToLower() == "on")
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
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
        ///     Disable Save story to archive.
        /// </summary>
        public async Task<IResult<bool>> DisableSaveStoryToArchiveAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSetReelSettingsUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "check_pending_archive", "1" },
                    { "reel_auto_archive", "off" }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountArchiveStory>(json);
                if (obj.ReelAutoArchive.ToLower() == "off")
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
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
        ///     Allow story sharing.
        /// </summary>
        /// <param name="allow">Allow or disallow story sharing</param>
        public async Task<IResult<bool>> AllowStorySharingAsync(bool allow = true)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSetReelSettingsUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };

                data.Add("allow_story_reshare", allow ? "1" : "0");

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountArchiveStory>(json);
                if (obj.Status.ToLower() == "off")
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
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
        ///     Allow story message replies.
        /// </summary>
        /// <param name="repliesType">Reply typo</param>
        public async Task<IResult<bool>> AllowStoryMessageRepliesAsync(InstaMessageRepliesType repliesType)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSetReelSettingsUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "message_prefs", repliesType.ToString().ToLower() }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountArchiveStory>(json);
                if (obj.MessagePrefs.ToLower() == "anyone" && repliesType == InstaMessageRepliesType.Everyone)
                {
                    return InstaResult.Success(true);
                }

                if (obj.MessagePrefs.ToLower() == "following" && repliesType == InstaMessageRepliesType.Following)
                {
                    return InstaResult.Success(true);
                }

                if (obj.MessagePrefs.ToLower() == "off" && repliesType == InstaMessageRepliesType.Off)
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
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
        ///     Check username availablity. (for logged in user)
        /// </summary>
        /// <param name="desiredUsername">Desired username</param>
        public async Task<IResult<InstaAccountCheck>> CheckUsernameAsync(string desiredUsername)
        {
            try
            {
                var instaUri = InstaUriCreator.GetCheckUsernameUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "username", desiredUsername }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaAccountCheck>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountCheck>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaAccountCheck), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaAccountCheck>(exception);
            }
        }

        /// <summary>
        ///     Get Security settings (two factor authentication and backup codes).
        /// </summary>
        public async Task<IResult<InstaAccountSecuritySettings>> GetSecuritySettingsInfoAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAccountSecurityInfoUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaAccountSecuritySettings>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountSecuritySettings>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaAccountSecuritySettings), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaAccountSecuritySettings>(exception);
            }
        }

        /// <summary>
        ///     Disable two factor authentication.
        /// </summary>
        public async Task<IResult<bool>> DisableTwoFactorAuthenticationAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetDisableSmsTwoFactorUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaDefault>(json);
                if (obj.Status.ToLower() == "ok")
                {
                    return InstaResult.Success(true);
                }

                return InstaResult.Success(false);
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
        ///     Send two factor enable sms.
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        public async Task<IResult<AccountTwoFactorSms>> SendTwoFactorEnableSmsAsync(string phoneNumber)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetSendTwoFactorEnableSmsUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "phone_number", phoneNumber }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<AccountTwoFactorSms>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<AccountTwoFactorSms>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(AccountTwoFactorSms), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<AccountTwoFactorSms>(exception);
            }
        }

        /// <summary>
        ///     Verify enable two factor.
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="verificationCode">Verification code</param>
        public async Task<IResult<AccountTwoFactor>> TwoFactorEnableAsync(
            string phoneNumber,
            string verificationCode)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetEnableSmsTwoFactorUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "phone_number", phoneNumber },
                    { "verification_code", verificationCode }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<AccountTwoFactor>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<AccountTwoFactor>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(AccountTwoFactor), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<AccountTwoFactor>(exception);
            }
        }

        /// <summary>
        ///     Send confirm email.
        /// </summary>
        public async Task<IResult<AccountConfirmEmail>> SendConfirmEmailAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAccountSendConfirmEmailUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "send_source", "edit_profile" }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<AccountConfirmEmail>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<AccountConfirmEmail>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(AccountConfirmEmail), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<AccountConfirmEmail>(exception);
            }
        }

        /// <summary>
        ///     Send sms code.
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        public async Task<IResult<AccountSendSms>> SendSmsCodeAsync(string phoneNumber)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAccountSendSmsCodeUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "phone_number", phoneNumber }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<AccountSendSms>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<AccountSendSms>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(AccountSendSms), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<AccountSendSms>(exception);
            }
        }

        /// <summary>
        ///     Verify email by verification url
        /// </summary>
        /// <param name="verificationUri">Verification url</param>
        public async Task<IResult<bool>> VerifyEmailByVerificationUriAsync(Uri verificationUri)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                if (verificationUri == null)
                {
                    throw new ArgumentNullException("Verification uri cannot be null");
                }

                var instaUri = InstaUriCreator.GetVerifyEmailUri(verificationUri);
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var obj = JsonConvert.DeserializeObject<AccountConfirmEmail>(json);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, obj.Body, null);
                }

                return obj.Title.ToLower() == "thanks" ? InstaResult.Success(true) : InstaResult.Fail(obj.Body, false);
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
        ///     Verify sms code.
        /// </summary>
        /// <param name="phoneNumber">Phone number (ex: +9891234...)</param>
        /// <param name="verificationCode">Verification code</param>
        public async Task<IResult<AccountVerifySms>> VerifySmsCodeAsync(
            string phoneNumber,
            string verificationCode)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAccountVerifySmsCodeUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "phone_number", phoneNumber },
                    { "verification_code", verificationCode }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<AccountVerifySms>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<AccountVerifySms>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(AccountVerifySms), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<AccountVerifySms>(exception);
            }
        }

        /// <summary>
        ///     Regenerate two factor backup codes
        /// </summary>
        public async Task<IResult<InstaTwoFactorRegenBackupCodes>> RegenerateTwoFactorBackupCodesAsync()
        {
            try
            {
                var instaUri = InstaUriCreator.GetRegenerateTwoFactorBackUpCodeUri();
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_csrftoken", user.CsrfToken }
                };

                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaTwoFactorRegenBackupCodes>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaTwoFactorRegenBackupCodes>(json);
                return obj.Status.ToLower() == "ok"
                    ? InstaResult.Success(obj)
                    : InstaResult.UnExpectedResponse<InstaTwoFactorRegenBackupCodes>(response, json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaTwoFactorRegenBackupCodes), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaTwoFactorRegenBackupCodes>(exception);
            }
        }

        /// <summary>
        ///     Enable presence (people can track your activities and you can see their activies too)
        /// </summary>
        public Task<IResult<bool>> EnablePresenceAsync()
        {
            return EnableDisablePresenceAsync(true);
        }

        /// <summary>
        ///     Disable presence (people can't track your activities and you can't see their activies too)
        /// </summary>
        public Task<IResult<bool>> DisablePresenceAsync()
        {
            return EnableDisablePresenceAsync(false);
        }

        /// <summary>
        ///     Get presence options (see your presence is disable or not)
        /// </summary>
        public async Task<IResult<InstaPresence>> GetPresenceOptionsAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetPresenceUri(httpHelper.ApiVersion.SignatureKey);

                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaPresence>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaPresenceResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetPresenceConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaPresence), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaPresence>(exception);
            }
        }

        /// <summary>
        ///     Switch to personal account
        /// </summary>
        public async Task<IResult<InstaUser>> SwitchToPersonalAccountAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetConvertToPersonalAccountUri();
                var data = new JObject
                {
                    { "_csrftoken", user.CsrfToken },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaUser>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaUserContainerResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetUserConverter(obj.User).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUser), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaUser>(exception);
            }
        }

        /// <summary>
        ///     Switch to business account
        /// </summary>
        public async Task<IResult<InstaBusinessUser>> SwitchToBusinessAccountAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetConvertToBusinessAccountUri();
                var request = httpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBusinessUser>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaBusinessUserContainerResponse>(json);
                return InstaResult.Success(InstaConvertersFabric.Instance.GetBusinessUserConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBusinessUser), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBusinessUser>(exception);
            }
        }

        //NOT COMPLETE
        private async Task<IResult<object>> GetCommentFilterAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = new Uri(InstaApiConstants.BaseInstagramApiUrl + "accounts/get_comment_filter/");
                Debug.WriteLine(instaUri.ToString());

                var request = httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Debug.WriteLine(response.StatusCode);
                Debug.WriteLine(json);

                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Result.UnExpectedResponse<object>(response, json);
                //{"config_value": 0, "status": "ok"}
                return null;
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(object), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<object>(exception);
            }
        }

        /// <summary>
        ///     [NOT WORKING] Set contact information for business account
        /// </summary>
        /// <param name="categoryId">Category id (Use <see cref="IBusinessProcessor.GetCategoriesAsync" /> to get category id)</param>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="email">Email address</param>
        public async Task<IResult<InstaBusinessUser>> SetBusinessInfoAsync(
            string categoryId,
            string phoneNumber,
            string email)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                //[NOT WORKING]
                var instaUri = InstaUriCreator.GetCreateBusinessInfoUri();

                var publicPhoneContact = new JObject
                {
                    { "public_phone_number", phoneNumber }, { "business_contact_method", "CALL" }
                };
                var edit = await GetRequestForEditProfileAsync().ConfigureAwait(false);

                //{
                //  "set_public": "false",
                //  "entry_point": "setting",
                //  "_csrftoken": "UBPgM6BG1Qr95lO4ofLYpgJXtbVvVnvs",
                //  "public_phone_contact": "{\"public_phone_number\":\"+989174314006\",\"business_contact_method\":\"CALL\"}",
                //  "_uid": "7405924766",
                //  "_uuid": "6324ecb2-e663-4dc8-a3a1-289c699cc876",
                //  "public_email": "ramtinjokar@yahoo.com",
                //  "category_id": "2700"
                //}
                var pub = edit.Value.IsPrivate;

                var data = new JObject
                {
                    { "set_public", pub.ToString().ToLower() },
                    { "entry_point", "setting" },
                    { "_csrftoken", user.CsrfToken },
                    { "public_phone_contact", publicPhoneContact.ToString(Formatting.None) },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "public_email", email ?? string.Empty },
                    { "category_id", categoryId }
                };
                var request =
                    httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await httpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaBusinessUser>(response, json);
                }

                //{"message": "Business details are malformed", "error_identifier": "BUSINESS_ID", "status": "fail"}
                //{"message": "Can not convert to business, Try again later", "error_identifier": "CANNOT_CONVERT", "status": "fail"}
                var obj = JsonConvert.DeserializeObject<InstaBusinessUserContainerResponse>(json);

                return InstaResult.Success(InstaConvertersFabric.Instance.GetBusinessUserConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaBusinessUser), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaBusinessUser>(exception);
            }
        }

        private async Task<IResult<bool>> EnableDisablePresenceAsync(bool enable)
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetAccountSetPresenseDisabledUri();
                var data = new JObject
                {
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "_uid", user.LoggedInUser.Pk.ToString() },
                    { "disabled", enable ? "0" : "1" },
                    { "_csrftoken", user.CsrfToken }
                };
                var request = httpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
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