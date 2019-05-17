using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Android.DeviceInfo;
using Wikiled.Instagram.Api.Classes.Models.Account;
using Wikiled.Instagram.Api.Classes.Models.Challenge;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Login;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Converters;
using Wikiled.Instagram.Api.Enums;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logger;
using Wikiled.Instagram.Api.Logic.Processors;
using Wikiled.Instagram.Api.Logic.Versions;

namespace Wikiled.Instagram.Api.Logic
{
    /// <summary>
    ///     Base of everything that you want.
    /// </summary>
    internal class InstaApi : IInstaApi
    {
        private readonly ILogger logger;

        private readonly InstaUserAuthValidate userAuthValidate;

        private InstaApiVersion apiVersion;

        private string facebookToken;

        private InstaChallengeLoginInfo challengeInfo;

        private bool isUserAuthenticated;

        private InstaAccountRegistrationPhoneNumber signUpPhoneNumberInfo;

        private TwoFactorLoginInfo twoFactorInfo;

        private UserSessionData userSession;

        private string waterfallIdReg = "";

        private string deviceIdReg = "";

        private string phoneIdReg = "";

        private string guidReg = "";

        private AndroidDevice deviceInfo;

        private bool isCustomDevice;

        private InstaApiVersionType apiVersionType;

        public InstaApi(ILogger logger, IHttpRequestProcessor httpRequestProcessor, UserSessionData user, InstaApiVersionType version, AndroidDevice deviceInfo)
        {
            userAuthValidate = new InstaUserAuthValidate();
            User = user;
            this.deviceInfo = deviceInfo;
            this.logger = logger;
            HttpRequestProcessor = httpRequestProcessor;
            apiVersionType = version;
            apiVersion = InstaApiVersionList.GetApiVersionList().GetApiVersion(version);
            HttpHelper = new InstaHttpHelper(apiVersion);
        }

        public IRequestDelay Delay { get; private set; } = RequestDelay.Empty();


        private InstaHttpHelper HttpHelper { get; set; }

        private UserSessionData User
        {
            get => userSession;
            set
            {
                userSession = value;
                userAuthValidate.User = value;
            }
        }

        /// <summary>
        ///     Helper processor for other processors
        /// </summary>
        internal InstaHelperProcessor HelperProcessor { get; private set; }

        public ISessionHandler SessionHandler { get; set; }

        /// <summary>
        ///     Indicates whether user authenticated or not
        /// </summary>
        public bool IsUserAuthenticated
        {
            get => isUserAuthenticated;
            private set
            {
                isUserAuthenticated = value;
                userAuthValidate.IsUserAuthenticated = value;
            }
        }

        /// <summary>
        ///     Current HttpClient
        /// </summary>
        public HttpClient HttpClient => HttpRequestProcessor.Client;

        /// <summary>
        ///     Current <see cref="IHttpRequestProcessor" />
        /// </summary>
        public IHttpRequestProcessor HttpRequestProcessor { get; }

        /// <summary>
        ///     Live api functions.
        /// </summary>
        public ILiveProcessor LiveProcessor { get; private set; }

        /// <summary>
        ///     Discover api functions.
        /// </summary>
        public IDiscoverProcessor DiscoverProcessor { get; private set; }

        /// <summary>
        ///     Account api functions.
        /// </summary>
        public IAccountProcessor AccountProcessor { get; private set; }

        /// <summary>
        ///     Comments api functions.
        /// </summary>
        public ICommentProcessor CommentProcessor { get; private set; }

        /// <summary>
        ///     Story api functions.
        /// </summary>
        public IStoryProcessor StoryProcessor { get; private set; }

        /// <summary>
        ///     Media api functions.
        /// </summary>
        public IMediaProcessor MediaProcessor { get; private set; }

        /// <summary>
        ///     Messaging (direct) api functions.
        /// </summary>
        public IMessagingProcessor MessagingProcessor { get; private set; }

        /// <summary>
        ///     Feed api functions.
        /// </summary>
        public IFeedProcessor FeedProcessor { get; private set; }

        /// <summary>
        ///     Collection api functions.
        /// </summary>
        public ICollectionProcessor CollectionProcessor { get; private set; }

        /// <summary>
        ///     Location api functions.
        /// </summary>
        public ILocationProcessor LocationProcessor { get; private set; }

        /// <summary>
        ///     Hashtag api functions.
        /// </summary>
        public IHashtagProcessor HashtagProcessor { get; private set; }

        /// <summary>
        ///     User api functions.
        /// </summary>
        public IUserProcessor UserProcessor { get; private set; }

        /// <summary>
        ///     Instagram TV api functions
        /// </summary>
        public ITvProcessor TvProcessor { get; private set; }

        /// <summary>
        ///     Business api functions
        ///     <para>Note: All functions of this interface only works with business accounts!</para>
        /// </summary>
        public IBusinessProcessor BusinessProcessor { get; private set; }

        /// <summary>
        ///     Shopping and commerce api functions
        /// </summary>
        public IShoppingProcessor ShoppingProcessor { get; private set; }

        /// <summary>
        ///     Instagram Web api functions.
        ///     <para>It's related to https://instagram.com/accounts/ </para>
        /// </summary>
        public IWebProcessor WebProcessor { get; private set; }

        /// <summary>
        ///     Check email availability
        /// </summary>
        /// <param name="email">Email to check</param>
        public async Task<IResult<InstaCheckEmailRegistration>> CheckEmailAsync(string email)
        {
            return await CheckEmail(email).ConfigureAwait(false);
        }

        /// <summary>
        ///     Check phone number availability
        /// </summary>
        /// <param name="phoneNumber">Phone number to check</param>
        public async Task<IResult<bool>> CheckPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                deviceIdReg = ApiRequestMessage.GenerateDeviceId();

                var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;

                var postData = new Dictionary<string, string>
                {
                    { "_csrftoken", csrftoken },
                    { "login_nonces", "[]" },
                    { "phone_number", phoneNumber },
                    { "device_id", deviceInfo.DeviceId }
                };

                var instaUri = InstaUriCreator.GetCheckPhoneNumberUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                return InstaResult.Success(true);
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
        ///     Check username availablity.
        /// </summary>
        /// <param name="username">Username</param>
        public async Task<IResult<InstaAccountCheck>> CheckUsernameAsync(string username)
        {
            try
            {
                var instaUri = InstaUriCreator.GetCheckUsernameUri();
                var data = new JObject { { "_csrftoken", User.CsrfToken }, { "username", username } };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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
        ///     Send sign up sms code
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        public async Task<IResult<bool>> SendSignUpSmsCodeAsync(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(waterfallIdReg))
                {
                    waterfallIdReg = Guid.NewGuid().ToString();
                }

                await CheckPhoneNumberAsync(phoneNumber).ConfigureAwait(false);

                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;
                var postData = new Dictionary<string, string>
                {
                    { "phone_id", deviceInfo.PhoneGuid.ToString() },
                    { "phone_number", phoneNumber },
                    { "_csrftoken", csrftoken },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId },
                    { "waterfall_id", waterfallIdReg }
                };
                var instaUri = InstaUriCreator.GetSignUpSmsCodeUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var o = JsonConvert.DeserializeObject<InstaAccountRegistrationPhoneNumber>(json);

                    return InstaResult.UnExpectedResponse<bool>(response, o.Message?.Errors?[0], json);
                }

                signUpPhoneNumberInfo = JsonConvert.DeserializeObject<InstaAccountRegistrationPhoneNumber>(json);
                return InstaResult.Success(true);
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
        ///     Verify sign up sms code
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="verificationCode">Verification code</param>
        public async Task<IResult<InstaPhoneNumberRegistration>> VerifySignUpSmsCodeAsync(
            string phoneNumber,
            string verificationCode)
        {
            try
            {
                if (string.IsNullOrEmpty(waterfallIdReg))
                {
                    throw new ArgumentException("You should call SendSignUpSmsCodeAsync function first.");
                }

                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;
                var postData = new Dictionary<string, string>
                {
                    { "verification_code", verificationCode },
                    { "phone_number", phoneNumber },
                    { "_csrftoken", csrftoken },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId },
                    { "waterfall_id", waterfallIdReg }
                };
                var instaUri = InstaUriCreator.GetValidateSignUpSmsCodeUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var o = JsonConvert.DeserializeObject<InstaAccountRegistrationPhoneNumberVerifySms>(json);

                    return InstaResult.Fail(o.Errors?.Nonce?[0], (InstaPhoneNumberRegistration)null);
                }

                var r = JsonConvert.DeserializeObject<InstaAccountRegistrationPhoneNumberVerifySms>(json);
                if (r.ErrorType == "invalid_nonce")
                {
                    return InstaResult.Fail(r.Errors?.Nonce?[0], (InstaPhoneNumberRegistration)null);
                }

                await GetRegistrationStepsAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaPhoneNumberRegistration>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaPhoneNumberRegistration), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaPhoneNumberRegistration>(exception);
            }
        }

        /// <summary>
        ///     Get username suggestions
        /// </summary>
        /// <param name="name">Name</param>
        public async Task<IResult<InstaRegistrationSuggestionResponse>> GetUsernameSuggestionsAsync(string name)
        {
            return await GetUsernameSuggestions(name).ConfigureAwait(false);
        }

        /// <summary>
        ///     Validate new account creation with phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="verificationCode">Verification code</param>
        /// <param name="username">Username to set</param>
        /// <param name="password">Password to set</param>
        /// <param name="firstName">First name to set</param>
        public async Task<IResult<InstaAccountCreation>> ValidateNewAccountWithPhoneNumberAsync(
            string phoneNumber,
            string verificationCode,
            string username,
            string password,
            string firstName)
        {
            try
            {
                if (string.IsNullOrEmpty(waterfallIdReg) || signUpPhoneNumberInfo == null)
                {
                    throw new ArgumentException("You should call SendSignUpSmsCodeAsync function first.");
                }

                if (signUpPhoneNumberInfo.GdprRequired)
                {
                    var acceptGdpr = await AcceptConsentRequiredAsync(null, phoneNumber).ConfigureAwait(false);
                    if (!acceptGdpr.Succeeded)
                    {
                        return InstaResult.Fail(acceptGdpr.Info.Message, (InstaAccountCreation)null);
                    }
                }

                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;

                var postData = new Dictionary<string, string>
                {
                    { "allow_contacts_sync", "true" },
                    { "verification_code", verificationCode },
                    { "sn_result", "API_ERROR:+null" },
                    { "phone_id", deviceInfo.PhoneGuid.ToString() },
                    { "phone_number", phoneNumber },
                    { "_csrftoken", csrftoken },
                    { "username", username },
                    { "first_name", firstName },
                    { "adid", Guid.NewGuid().ToString() },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId },
                    { "sn_nonce", "" },
                    { "force_sign_up_code", "" },
                    { "waterfall_id", waterfallIdReg },
                    { "qs_stamp", "" },
                    { "password", password },
                    { "has_sms_consent", "true" }
                };
                if (signUpPhoneNumberInfo.GdprRequired)
                {
                    postData.Add("gdpr_s", "[0,2,0,null]");
                }

                var instaUri = InstaUriCreator.GetCreateValidatedUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var o = JsonConvert.DeserializeObject<InstaAccountCreationResponse>(json);

                    return InstaResult.Fail(o.Errors?.Username?[0], (InstaAccountCreation)null);
                }

                var r = JsonConvert.DeserializeObject<InstaAccountCreationResponse>(json);
                if (r.ErrorType == "username_is_taken")
                {
                    return InstaResult.Fail(r.Errors?.Username?[0], (InstaAccountCreation)null);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountCreation>(json);
                if (obj.AccountCreated && obj.CreatedUser != null)
                {
                    ValidateUserAsync(obj.CreatedUser, csrftoken, true, password);
                }

                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaAccountCreation), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaAccountCreation>(exception);
            }
        }

        /// <summary>
        ///     Create a new instagram account
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="email">Email</param>
        /// <param name="firstName">First name (optional)</param>
        /// <returns></returns>
        public async Task<IResult<InstaAccountCreation>> CreateNewAccountAsync(
            string username,
            string password,
            string email,
            string firstName)
        {
            var createResponse = new InstaAccountCreation();
            try
            {
                var deviceIdReg = ApiRequestMessage.GenerateDeviceId();
                var phoneIdReg = Guid.NewGuid().ToString();
                var waterfallIdReg = Guid.NewGuid().ToString();
                var guidReg = Guid.NewGuid().ToString();
                var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                await firstResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;

                var postData = new Dictionary<string, string>
                {
                    { "allow_contacts_sync", "true" },
                    { "sn_result", "API_ERROR:+null" },
                    { "phone_id", phoneIdReg },
                    { "_csrftoken", csrftoken },
                    { "username", username },
                    { "first_name", firstName },
                    { "adid", Guid.NewGuid().ToString() },
                    { "guid", guidReg },
                    { "device_id", deviceIdReg },
                    { "email", email },
                    { "sn_nonce", "" },
                    { "force_sign_up_code", "" },
                    { "waterfall_id", waterfallIdReg },
                    { "qs_stamp", "" },
                    { "password", password }
                };
                var instaUri = InstaUriCreator.GetCreateAccountUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaAccountCreation>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountCreation>(json);
                if (obj.AccountCreated && obj.CreatedUser != null)
                {
                    ValidateUserAsync(obj.CreatedUser, csrftoken, true, password);
                }

                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaAccountCreation), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaAccountCreation>(exception);
            }
        }

        /// <summary>
        ///     Login using given credentials asynchronously
        /// </summary>
        /// <param name="isNewLogin"></param>
        /// <returns>
        ///     Success --> is succeed
        ///     TwoFactorRequired --> requires 2FA login.
        ///     BadPassword --> Password is wrong
        ///     InvalidUser --> User/phone number is wrong
        ///     Exception --> Something wrong happened
        ///     ChallengeRequired --> You need to pass Instagram challenge
        /// </returns>
        public async Task<IResult<InstaLoginResult>> LoginAsync(bool isNewLogin = true)
        {
            ValidateUser();
            ValidateRequestMessage();
            try
            {
                var result =  await LoginInternal(isNewLogin).ConfigureAwait(false);
                if (!result.Succeeded && result.Value == InstaLoginResult.CheckpointLoggedOut)
                {
                    logger.LogInformation("CheckpointLoggedOut detected, logging in again");
                    result = await LoginInternal(isNewLogin).ConfigureAwait(false);
                }

                return result;
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, InstaLoginResult.Exception, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return InstaResult.Fail(exception, InstaLoginResult.Exception);
            }
            finally
            {
                InvalidateProcessors();
            }
        }

        private async Task<IResult<InstaLoginResult>> LoginInternal(bool isNewLogin)
        {
            if (isNewLogin)
            {
                var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                await firstResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                logger?.LogResponse(firstResponse);
            }

            var cookies = HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(HttpRequestProcessor.Client.BaseAddress);
            var csrfToken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
            User.CsrfToken = csrfToken;
            var instaUri = InstaUriCreator.GetLoginUri();
            var signature = string.Empty;
            var devid = string.Empty;
            signature = isNewLogin
                ? $"{HttpRequestProcessor.RequestMessage.GenerateSignature(apiVersion, apiVersion.SignatureKey, out devid)}.{HttpRequestProcessor.RequestMessage.GetMessageString()}"
                : $"{HttpRequestProcessor.RequestMessage.GenerateChallengeSignature(apiVersion, apiVersion.SignatureKey, csrfToken, out devid)}.{HttpRequestProcessor.RequestMessage.GetChallengeMessageString(csrfToken)}";

            deviceInfo.DeviceId = devid;
            var fields = new Dictionary<string, string>
            {
                { InstaApiConstants.HeaderIgSignature, signature },
                { InstaApiConstants.HeaderIgSignatureKeyVersion, InstaApiConstants.IgSignatureKeyVersion }
            };

            var request = HttpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo, fields);
            request.Headers.Add("Host", "i.instagram.com");
            var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var loginFailReason = JsonConvert.DeserializeObject<InstaLoginBaseResponse>(json);

                if (loginFailReason.InvalidCredentials)
                {
                        return InstaResult.Fail(
                            "Invalid Credentials",
                            loginFailReason.ErrorType == "bad_password"
                                ? InstaLoginResult.BadPassword
                                : InstaLoginResult.InvalidUser);
                }

                if (loginFailReason.TwoFactorRequired)
                {
                    if (loginFailReason.TwoFactorLoginInfo != null)
                    {
                        HttpRequestProcessor.RequestMessage.Username = loginFailReason.TwoFactorLoginInfo.Username;
                    }

                    twoFactorInfo = loginFailReason.TwoFactorLoginInfo;
                     return InstaResult.Fail("Two Factor Authentication is required", InstaLoginResult.TwoFactorRequired);
                }

                if (loginFailReason.ErrorType == "checkpoint_challenge_required")
                {
                    challengeInfo = loginFailReason.Challenge;
                    return InstaResult.Fail("Challenge is required", InstaLoginResult.ChallengeRequired);
                }

                if (loginFailReason.ErrorType == "rate_limit_error")
                {
                    return InstaResult.Fail("Please wait a few minutes before you try again.",
                                            InstaLoginResult.LimitError);
                }

                if (loginFailReason.ErrorType == "inactive user" || loginFailReason.ErrorType == "inactive_user")
                {
                    return InstaResult.Fail($"{loginFailReason.Message}\r\nHelp url: {loginFailReason.HelpUrl}",
                                            InstaLoginResult.InactiveUser);
                }

                if (loginFailReason.ErrorType == "checkpoint_logged_out")
                {
                    return InstaResult.Fail($"{loginFailReason.ErrorType} {loginFailReason.CheckpointUrl}", InstaLoginResult.CheckpointLoggedOut);
                }

                return InstaResult.UnExpectedResponse<InstaLoginResult>(response, json);
            }

            var loginInfo = JsonConvert.DeserializeObject<LoginResponse>(json);
            User.UserName = loginInfo.User?.UserName;
            IsUserAuthenticated = loginInfo.User != null;
            if (loginInfo.User != null)
            {
                HttpRequestProcessor.RequestMessage.Username = loginInfo.User.UserName;
            }

            var converter = InstaConvertersFabric.Instance.GetUserShortConverter(loginInfo.User);
            User.LoggedInUser = converter.Convert();
            User.RankToken = $"{User.LoggedInUser.Pk}_{HttpRequestProcessor.RequestMessage.PhoneId}";
            if (string.IsNullOrEmpty(User.CsrfToken))
            {
                cookies = HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(HttpRequestProcessor.Client.BaseAddress);
                User.CsrfToken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
            }

            return InstaResult.Success(InstaLoginResult.Success);
        }

        /// <summary>
        ///     Login using cookies
        ///     <para>
        ///         Note: You won't be able to change password, if you use <see cref="IInstaApi.LoginWithCookiesAsync(string)" />
        ///         function for logging in!
        ///     </para>
        /// </summary>
        /// <param name="cookies">Cookies</param>
        public async Task<IResult<bool>> LoginWithCookiesAsync(string cookies)
        {
            try
            {
                if (cookies.Contains("Cookie:"))
                {
                    cookies = cookies.Substring(8);
                }

                var parts = cookies.Split(';')
                    .Where(xx => xx.Contains("="))
                    .Select(xx => xx.Trim().Split('='))
                    .Select(xx => new { Name = xx.First(), Value = xx.Last() });

                var user = parts.FirstOrDefault(u => u.Name.ToLower() == "ds_user")?.Value?.ToLower();
                var userId = parts.FirstOrDefault(u => u.Name.ToLower() == "ds_user_id")?.Value;
                var csrfToken = parts.FirstOrDefault(u => u.Name.ToLower() == "csrftoken")?.Value;

                if (string.IsNullOrEmpty(csrfToken))
                {
                    return InstaResult.Fail<bool>("Cannot find 'csrftoken' in cookies!");
                }

                if (string.IsNullOrEmpty(userId))
                {
                    return InstaResult.Fail<bool>("Cannot find 'ds_user_id' in cookies!");
                }

                var uri = new Uri(InstaApiConstants.InstagramUrl);
                cookies = cookies.Replace(';', ',');
                HttpRequestProcessor.HttpHandler.CookieContainer.SetCookies(uri, cookies);
                User = UserSessionData.Empty;
                user = user ?? "AlakiMasalan";
                User.UserName = HttpRequestProcessor.RequestMessage.Username = user;
                User.Password = "AlakiMasalan";
                User.LoggedInUser = new UserShortDescription { UserName = user };
                try
                {
                    User.LoggedInUser.Pk = long.Parse(userId);
                }
                catch
                {
                }

                User.CsrfToken = csrfToken;
                User.RankToken = $"{deviceInfo.RankToken}_{userId}";

                IsUserAuthenticated = true;
                InvalidateProcessors();

                var us = await UserProcessor.GetUserInfoByIdAsync(long.Parse(userId)).ConfigureAwait(false);
                if (!us.Succeeded)
                {
                    IsUserAuthenticated = false;
                    return InstaResult.Fail(us.Info, false);
                }

                User.UserName = HttpRequestProcessor.RequestMessage.Username =
                    User.LoggedInUser.UserName = us.Value.Username;
                User.LoggedInUser.FullName = us.Value.FullName;
                User.LoggedInUser.IsPrivate = us.Value.IsPrivate;
                User.LoggedInUser.IsVerified = us.Value.IsVerified;
                User.LoggedInUser.ProfilePicture = us.Value.ProfilePicUrl;
                User.LoggedInUser.ProfilePicUrl = us.Value.ProfilePicUrl;

                return InstaResult.Success(true);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     2-Factor Authentication Login using a verification code
        ///     Before call this method, please run LoginAsync first.
        /// </summary>
        /// <param name="verificationCode">Verification Code sent to your phone number</param>
        /// <returns>
        ///     Success --> is succeed
        ///     InvalidCode --> The code is invalid
        ///     CodeExpired --> The code is expired, please request a new one.
        ///     Exception --> Something wrong happened
        /// </returns>
        public async Task<IResult<InstaLoginTwoFactorResult>> TwoFactorLoginAsync(string verificationCode)
        {
            if (twoFactorInfo == null)
            {
                return InstaResult.Fail<InstaLoginTwoFactorResult>("Run LoginAsync first");
            }

            try
            {
                var twoFactorRequestMessage = new InstaApiTwoFactorRequestMessage(
                    verificationCode,
                    HttpRequestProcessor.RequestMessage.Username,
                    HttpRequestProcessor.RequestMessage.DeviceId,
                    twoFactorInfo.TwoFactorIdentifier);

                var instaUri = InstaUriCreator.GetTwoFactorLoginUri();
                var signature =
                    $"{twoFactorRequestMessage.GenerateSignature(apiVersion, apiVersion.SignatureKey)}.{twoFactorRequestMessage.GetMessageString()}";
                var fields = new Dictionary<string, string>
                {
                    { InstaApiConstants.HeaderIgSignature, signature },
                    {
                        InstaApiConstants.HeaderIgSignatureKeyVersion,
                        InstaApiConstants.IgSignatureKeyVersion
                    }
                };
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, deviceInfo);
                request.Content = new FormUrlEncodedContent(fields);
                request.Properties.Add(InstaApiConstants.HeaderIgSignature, signature);
                request.Properties.Add(
                    InstaApiConstants.HeaderIgSignatureKeyVersion,
                    InstaApiConstants.IgSignatureKeyVersion);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var loginInfo =
                        JsonConvert.DeserializeObject<LoginResponse>(json);
                    User.UserName = loginInfo.User?.UserName;
                    IsUserAuthenticated = loginInfo.User != null;
                    HttpRequestProcessor.RequestMessage.Username = loginInfo.User?.UserName;
                    var converter = InstaConvertersFabric.Instance.GetUserShortConverter(loginInfo.User);
                    User.LoggedInUser = converter.Convert();
                    User.RankToken = $"{User.LoggedInUser.Pk}_{HttpRequestProcessor.RequestMessage.PhoneId}";

                    return InstaResult.Success(InstaLoginTwoFactorResult.Success);
                }

                var loginFailReason = JsonConvert.DeserializeObject<LoginTwoFactorBaseResponse>(json);

                if (loginFailReason.ErrorType == "sms_code_validation_code_invalid")
                {
                    return InstaResult.Fail("Please check the security code.", InstaLoginTwoFactorResult.InvalidCode);
                }

                if (loginFailReason.Message.ToLower().Contains("challenge"))
                {
                    challengeInfo = loginFailReason.Challenge;

                    return InstaResult.Fail("Challenge is required", InstaLoginTwoFactorResult.ChallengeRequired);
                }

                return InstaResult.Fail(
                    "This code is no longer valid, please, call LoginAsync again to request a new one",
                    InstaLoginTwoFactorResult.CodeExpired);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaLoginTwoFactorResult), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return InstaResult.Fail(exception, InstaLoginTwoFactorResult.Exception);
            }
        }

        /// <summary>
        ///     Get Two Factor Authentication details
        /// </summary>
        /// <returns>
        ///     An instance of TwoFactorInfo if success.
        ///     A null reference if not success; in this case, do LoginAsync first and check if Two Factor Authentication is
        ///     required, if not, don't run this method
        /// </returns>
        public async Task<IResult<TwoFactorLoginInfo>> GetTwoFactorInfoAsync()
        {
            return await Task.Run(
                () =>
                    twoFactorInfo != null
                        ? InstaResult.Success(twoFactorInfo)
                        : InstaResult.Fail<TwoFactorLoginInfo>("No Two Factor info available.")).ConfigureAwait(false);
        }

        /// <summary>
        ///     Logout from instagram asynchronously
        /// </summary>
        /// <returns>
        ///     True if logged out without errors
        /// </returns>
        public async Task<IResult<bool>> LogoutAsync()
        {
            ValidateUser();
            ValidateLoggedIn();
            try
            {
                var instaUri = InstaUriCreator.GetLogoutUri();
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var logoutInfo = JsonConvert.DeserializeObject<InstaBaseStatusResponse>(json);
                if (logoutInfo.Status == "ok")
                {
                    IsUserAuthenticated = false;
                }

                return InstaResult.Success(!IsUserAuthenticated);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return InstaResult.Fail(exception, false);
            }
        }

        /// <summary>
        ///     Get user lookup for recovery options
        /// </summary>
        /// <param name="usernameOrEmailOrPhoneNumber">Username or email or phone number</param>
        public async Task<IResult<InstaUserLookup>> GetRecoveryOptionsAsync(string usernameOrEmailOrPhoneNumber)
        {
            try
            {
                var csrfToken = "";
                if (!string.IsNullOrEmpty(User.CsrfToken))
                {
                    csrfToken = User.CsrfToken;
                }
                else
                {
                    var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                    var cookies =
                        HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                            HttpRequestProcessor.Client
                                .BaseAddress);
                    logger?.LogResponse(firstResponse);
                    csrfToken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                }

                var data = new JObject
                {
                    { "_csrftoken", csrfToken },
                    { "q", usernameOrEmailOrPhoneNumber },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId },
                    { "directly_sign_in", "true" }
                };

                var instaUri = InstaUriCreator.GetUsersLookupUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);

                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<InstaUserLookupResponse>(json);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.Fail<InstaUserLookup>(obj.Message);
                }

                return InstaResult.Success(InstaConvertersFabric.Instance.GetUserLookupConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaUserLookup), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                return InstaResult.Fail<InstaUserLookup>(exception);
            }
        }

        /// <summary>
        ///     Send recovery code by Username
        /// </summary>
        /// <param name="username">Username</param>
        public async Task<IResult<InstaRecovery>> SendRecoveryByUsernameAsync(string username)
        {
            return await SendRecoveryByEmailAsync(username).ConfigureAwait(false);
        }

        /// <summary>
        ///     Send recovery code by Email
        /// </summary>
        /// <param name="email">Email Address</param>
        public async Task<IResult<InstaRecovery>> SendRecoveryByEmailAsync(string email)
        {
            try
            {
                var token = "";
                if (!string.IsNullOrEmpty(User.CsrfToken))
                {
                    token = User.CsrfToken;
                }
                else
                {
                    var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                    var cookies =
                        HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                            HttpRequestProcessor.Client
                                .BaseAddress);
                    logger?.LogResponse(firstResponse);
                    token = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                }

                var postData = new JObject
                {
                    { "query", email },
                    { "adid", deviceInfo.GoogleAdId },
                    { "device_id", ApiRequestMessage.GenerateDeviceId() },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "_csrftoken", token }
                };

                var instaUri = InstaUriCreator.GetAccountRecoveryEmailUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);

                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);

                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = JsonConvert.DeserializeObject<InstaMessageErrorsResponseRecoveryEmail>(result);
                    return InstaResult.Fail<InstaRecovery>(error.Message);
                }

                return InstaResult.Success(JsonConvert.DeserializeObject<InstaRecovery>(result));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaRecovery), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                return InstaResult.Fail<InstaRecovery>(exception);
            }
        }

        /// <summary>
        ///     Send recovery code by Phone
        /// </summary>
        /// <param name="phone">Phone Number</param>
        public async Task<IResult<InstaRecovery>> SendRecoveryByPhoneAsync(string phone)
        {
            try
            {
                var token = "";
                if (!string.IsNullOrEmpty(User.CsrfToken))
                {
                    token = User.CsrfToken;
                }
                else
                {
                    var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                    var cookies =
                        HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                            HttpRequestProcessor.Client
                                .BaseAddress);
                    logger?.LogResponse(firstResponse);
                    token = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                }

                var postData = new JObject { { "query", phone }, { "_csrftoken", User.CsrfToken } };

                var instaUri = InstaUriCreator.GetAccountRecoverPhoneUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);

                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = JsonConvert.DeserializeObject<InstaBadStatusErrorsResponse>(result);
                    var errors = "";
                    error.Message.Errors.ForEach(errorContent => errors += errorContent + "\n");
                    return InstaResult.Fail<InstaRecovery>(errors);
                }

                if (result.Contains("errors"))
                {
                    var error = JsonConvert.DeserializeObject<InstaBadStatusErrorsResponseRecovery>(result);
                    var errors = "";
                    error.PhoneNumber.Errors.ForEach(errorContent => errors += errorContent + "\n");

                    return InstaResult.Fail<InstaRecovery>(errors);
                }

                return InstaResult.Success(JsonConvert.DeserializeObject<InstaRecovery>(result));
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaRecovery), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                return InstaResult.Fail<InstaRecovery>(exception);
            }
        }

        /// <summary>
        ///     Send Two Factor Login SMS Again
        /// </summary>
        public async Task<IResult<TwoFactorLoginSms>> SendTwoFactorLoginSmsAsync()
        {
            try
            {
                if (twoFactorInfo == null)
                {
                    return InstaResult.Fail<TwoFactorLoginSms>("Run LoginAsync first");
                }

                var postData = new Dictionary<string, string>
                {
                    { "two_factor_identifier", twoFactorInfo.TwoFactorIdentifier },
                    { "username", HttpRequestProcessor.RequestMessage.Username },
                    { "device_id", HttpRequestProcessor.RequestMessage.DeviceId },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "_csrftoken", User.CsrfToken }
                };

                var instaUri = InstaUriCreator.GetAccount2FaLoginAgainUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var T = JsonConvert.DeserializeObject<TwoFactorLoginSms>(result);
                if (!string.IsNullOrEmpty(T.TwoFactorInfo.TwoFactorIdentifier))
                {
                    twoFactorInfo.TwoFactorIdentifier = T.TwoFactorInfo.TwoFactorIdentifier;
                }

                return InstaResult.Success(T);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(TwoFactorLoginSms), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<TwoFactorLoginSms>(exception);
            }
        }

        /// <summary>
        ///     Get challenge data for logged in user
        ///     <para>This will pop-on, if some suspecious login happend</para>
        /// </summary>
        public async Task<IResult<InstaLoggedInChallengeDataInfo>> GetLoggedInChallengeDataInfoAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);

            try
            {
                var instaUri =
                    InstaUriCreator.GetChallengeRequireFirstUri("/challenge/",
                                                           deviceInfo.DeviceGuid.ToString(),
                                                           deviceInfo.DeviceId);
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaLoggedInChallengeDataInfo>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaLoggedInChallengeDataInfoContainer>(json);
                return InstaResult.Success(obj?.StepData);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaLoggedInChallengeDataInfo), InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, (InstaLoggedInChallengeDataInfo)null);
            }
        }

        /// <summary>
        ///     Accept challlenge, it is THIS IS ME feature!!!!
        ///     <para>
        ///         You must call <see cref="IInstaApi.GetLoggedInChallengeDataInfoAsync" /> first,
        ///         if you across to <see cref="ResultInfo.ResponseType" /> equals to <see cref="InstaResponseType.ChallengeRequired" />
        ///         while you logged in!
        ///     </para>
        /// </summary>
        public async Task<IResult<bool>> AcceptChallengeAsync()
        {
            InstaUserAuthValidator.Validate(userAuthValidate);
            try
            {
                var instaUri = InstaUriCreator.GetChallengeUri();

                var data = new JObject
                {
                    { "choice", "0" },
                    { "_csrftoken", User.CsrfToken },
                    { "_uid", User.LoggedInUser.Pk },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() }
                };

                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaChallengeRequireVerifyCode>(json);
                return obj.Action.ToLower() == "close" ? InstaResult.Success(true) : InstaResult.Success(false);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, false, InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail<bool>(ex);
            }
        }

        /// <summary>
        ///     Get challenge require (checkpoint required) options
        /// </summary>
        public async Task<IResult<InstaChallengeRequireVerifyMethod>> GetChallengeRequireVerifyMethodAsync()
        {
            if (challengeInfo == null)
            {
                return InstaResult.Fail("challenge require info is empty.\r\ntry to call LoginAsync function first.",
                                   (InstaChallengeRequireVerifyMethod)null);
            }

            try
            {
                var instaUri =
                    InstaUriCreator.GetChallengeRequireFirstUri(challengeInfo.ApiPath,
                                                           deviceInfo.DeviceGuid.ToString(),
                                                           deviceInfo.DeviceId);
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, deviceInfo);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaChallengeRequireVerifyMethod>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaChallengeRequireVerifyMethod>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaChallengeRequireVerifyMethod), InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, (InstaChallengeRequireVerifyMethod)null);
            }
        }

        /// <summary>
        ///     Reset challenge require (checkpoint required) method
        /// </summary>
        public async Task<IResult<InstaChallengeRequireVerifyMethod>> ResetChallengeRequireVerifyMethodAsync()
        {
            if (challengeInfo == null)
            {
                return InstaResult.Fail("challenge require info is empty.\r\ntry to call LoginAsync function first.",
                                   (InstaChallengeRequireVerifyMethod)null);
            }

            try
            {
                var instaUri = InstaUriCreator.GetResetChallengeRequireUri(challengeInfo.ApiPath);
                var data = new JObject
                {
                    { "_csrftoken", User.CsrfToken },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId }
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var msg = "";
                    try
                    {
                        var j = JsonConvert.DeserializeObject<InstaChallengeRequireVerifyMethod>(json);
                        msg = j.Message;
                    }
                    catch
                    {
                    }

                    return InstaResult.UnExpectedResponse<InstaChallengeRequireVerifyMethod>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaChallengeRequireVerifyMethod>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaChallengeRequireVerifyMethod),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, (InstaChallengeRequireVerifyMethod)null);
            }
        }

        /// <summary>
        ///     Request verification code sms for challenge require (checkpoint required)
        /// </summary>
        /// <param name="replayChallenge">true if Instagram should resend verification code to you</param>
        public Task<IResult<InstaChallengeRequireSmsVerify>> RequestVerifyCodeToSmsForChallengeRequireAsync(
            bool replayChallenge)
        {
            return RequestVerifyCodeToSmsForChallengeRequire(replayChallenge);
        }

        /// <summary>
        ///     Submit phone number for challenge require (checkpoint required)
        ///     <para>
        ///         Note: This only needs , when you calling <see cref="IInstaApi.GetChallengeRequireVerifyMethodAsync" /> or
        ///         <see cref="IInstaApi.ResetChallengeRequireVerifyMethodAsync" /> and
        ///         <see cref="InstaChallengeRequireVerifyMethod.SubmitPhoneRequired" /> property is true.
        ///     </para>
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        public Task<IResult<InstaChallengeRequireSmsVerify>> SubmitPhoneNumberForChallengeRequireAsync(string phoneNumber, bool replayChallenge)
        {
            return RequestVerifyCodeToSmsForChallengeRequire(replayChallenge, phoneNumber);
        }

        /// <summary>
        ///     Request verification code email for challenge require (checkpoint required)
        /// </summary>
        /// <param name="replayChallenge">true if Instagram should resend verification code to you</param>
        public async Task<IResult<InstaChallengeRequireEmailVerify>> RequestVerifyCodeToEmailForChallengeRequireAsync(
            bool replayChallenge)
        {
            if (challengeInfo == null)
            {
                return InstaResult.Fail("challenge require info is empty.\r\ntry to call LoginAsync function first.",
                                   (InstaChallengeRequireEmailVerify)null);
            }

            try
            {
                Uri instaUri;

                if (replayChallenge)
                {
                    instaUri = InstaUriCreator.GetChallengeReplayUri(challengeInfo.ApiPath);
                }
                else
                {
                    instaUri = InstaUriCreator.GetChallengeRequireUri(challengeInfo.ApiPath);
                }

                var data = new JObject
                {
                    { "choice", "1" },
                    { "_csrftoken", User.CsrfToken },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId }
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var msg = "";
                    try
                    {
                        var j = JsonConvert.DeserializeObject<InstaChallengeRequireEmailVerify>(json);
                        msg = j.Message;
                    }
                    catch
                    {
                    }

                    return InstaResult.Fail(msg, (InstaChallengeRequireEmailVerify)null);
                }

                var obj = JsonConvert.DeserializeObject<InstaChallengeRequireEmailVerify>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaChallengeRequireEmailVerify),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, (InstaChallengeRequireEmailVerify)null);
            }
        }

        /// <summary>
        ///     Verify verification code for challenge require (checkpoint required)
        /// </summary>
        /// <param name="verifyCode">Verification code</param>
        public async Task<IResult<InstaLoginResult>> VerifyCodeForChallengeRequireAsync(string verifyCode)
        {
            if (challengeInfo == null)
            {
                return InstaResult.Fail("challenge require info is empty.\r\ntry to call LoginAsync function first.",
                                   InstaLoginResult.Exception);
            }

            if (verifyCode.Length != 6)
            {
                return InstaResult.Fail("Verify code must be an 6 digit number.", InstaLoginResult.Exception);
            }

            try
            {
                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;
                var instaUri = InstaUriCreator.GetChallengeRequireUri(challengeInfo.ApiPath);

                var data = new JObject
                {
                    { "security_code", verifyCode },
                    { "_csrftoken", User.CsrfToken },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId }
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var msg = "";
                    try
                    {
                        var j = JsonConvert.DeserializeObject<InstaChallengeRequireVerifyCode>(json);
                        msg = j.Message;
                    }
                    catch
                    {
                    }

                    return InstaResult.Fail(msg, InstaLoginResult.Exception);
                }

                var obj = JsonConvert.DeserializeObject<InstaChallengeRequireVerifyCode>(json);
                if (obj != null)
                {
                    if (obj.LoggedInUser != null)
                    {
                        ValidateUserAsync(obj.LoggedInUser, csrftoken);
                        await Task.Delay(3000).ConfigureAwait(false);
                        await MessagingProcessor.GetDirectInboxAsync(PaginationParameters.MaxPagesToLoad(1)).ConfigureAwait(false);
                        await FeedProcessor.GetRecentActivityFeedAsync(PaginationParameters.MaxPagesToLoad(1)).ConfigureAwait(false);

                        return InstaResult.Success(InstaLoginResult.Success);
                    }

                    if (!string.IsNullOrEmpty(obj.Action))
                    {
                        // we should wait at least 15 seconds and then trying to login again
                        await Task.Delay(15000).ConfigureAwait(false);
                        return await LoginAsync(false).ConfigureAwait(false);
                    }
                }

                return InstaResult.Fail(obj?.Message, InstaLoginResult.Exception);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaLoginResult), InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InstaResult.Fail(ex, InstaLoginResult.Exception);
            }
        }

        /// <summary>
        ///     Login with Facebook access token
        /// </summary>
        /// <param name="fbAccessToken">Facebook access token</param>
        /// <param name="cookiesContainer">Cookies</param>
        /// <returns>
        ///     Success --> is succeed
        ///     TwoFactorRequired --> requires 2FA login.
        ///     BadPassword --> Password is wrong
        ///     InvalidUser --> User/phone number is wrong
        ///     Exception --> Something wrong happened
        ///     ChallengeRequired --> You need to pass Instagram challenge
        /// </returns>
        public async Task<IResult<InstaLoginResult>> LoginWithFacebookAsync(
            string fbAccessToken,
            string cookiesContainer)
        {
            return await LoginWithFacebookAsync(fbAccessToken, cookiesContainer, true).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get current API version info (signature key, api version info, app id)
        /// </summary>
        public InstaApiVersion GetApiVersionInfo()
        {
            return apiVersion;
        }

        public void SetStateData(StateData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!isCustomDevice)
            {
                deviceInfo = data.DeviceInfo;
            }

            User = data.UserSession;

            HttpRequestProcessor.RequestMessage.Username = data.UserSession.UserName;
            HttpRequestProcessor.RequestMessage.Password = data.UserSession.Password;

            HttpRequestProcessor.RequestMessage.DeviceId = deviceInfo.DeviceId;
            HttpRequestProcessor.RequestMessage.PhoneId = deviceInfo.PhoneGuid.ToString();
            HttpRequestProcessor.RequestMessage.Guid = deviceInfo.DeviceGuid;
            HttpRequestProcessor.RequestMessage.AdId = deviceInfo.AdId.ToString();

            foreach (var cookie in data.RawCookies)
            {
                HttpRequestProcessor.HttpHandler.CookieContainer.Add(new Uri(InstaApiConstants.InstagramUrl), cookie);
            }

            apiVersion = InstaApiVersionList.GetApiVersionList().GetApiVersion(data.ApiVersion);
            HttpHelper = new InstaHttpHelper(apiVersion);

            IsUserAuthenticated = data.IsAuthenticated;
            InvalidateProcessors();
        }

        public StateData GetStateData()
        {
            var cookies = HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(new Uri(InstaApiConstants.InstagramUrl));
            var rawCookiesList = new List<Cookie>();
            foreach (Cookie cookie in cookies)
            {
                rawCookiesList.Add(cookie);
            }

            var state = new StateData
            {
                DeviceInfo = deviceInfo,
                IsAuthenticated = IsUserAuthenticated,
                UserSession = User,
                Cookies = HttpRequestProcessor.HttpHandler.CookieContainer,
                RawCookies = rawCookiesList,
                ApiVersion = apiVersionType
            };

            return state;
        }

        /// <summary>
        ///     Get user agent of current <see cref="IInstaApi" />
        /// </summary>
        public string GetUserAgent()
        {
            return deviceInfo.GenerateUserAgent(apiVersion);
        }

        /// <summary>
        ///     Set timeout to <see cref="System.Net.Http.HttpClient" />
        ///     <para>Note: Set timeout more than 100 seconds!</para>
        /// </summary>
        /// <param name="timeout">Timeout (set more than 100 seconds!)</param>
        public void SetTimeout(TimeSpan timeout)
        {
            if (timeout == null)
            {
                timeout = TimeSpan.FromSeconds(350);
            }

            HttpClient.Timeout = timeout;
        }

        /// <summary>
        ///     Set custom HttpClientHandler to be able to use certain features, e.g Proxy and so on
        /// </summary>
        /// <param name="handler">HttpClientHandler</param>
        public void UseHttpClientHandler(HttpClientHandler handler)
        {
            HttpRequestProcessor.SetHttpClientHandler(handler);
        }

        /// <summary>
        ///     Sets user credentials
        /// </summary>
        public void SetUser(string username, string password)
        {
            User.UserName = username;
            User.Password = password;

            HttpRequestProcessor.RequestMessage.Username = username;
            HttpRequestProcessor.RequestMessage.Password = password;
        }

        /// <summary>
        ///     Sets user credentials
        /// </summary>
        public void SetUser(UserSessionData user)
        {
            SetUser(user.UserName, user.Password);
        }

        /// <summary>
        ///     Gets current device
        /// </summary>
        public AndroidDevice GetCurrentDevice()
        {
            return deviceInfo;
        }

        /// <summary>
        ///     Gets logged in user
        /// </summary>
        public UserSessionData GetLoggedUser()
        {
            return User;
        }

        /// <summary>
        ///     Get currently logged in user info asynchronously
        /// </summary>
        /// <returns>
        ///     <see cref="T:InstagramApiSharp.Classes.Models.CurrentUser" />
        /// </returns>
        public async Task<IResult<CurrentUser>> GetCurrentUserAsync()
        {
            ValidateUser();
            ValidateLoggedIn();
            return await UserProcessor.GetCurrentUserAsync().ConfigureAwait(false);
        }

        /// <summary>
        ///     Get Accept Language
        /// </summary>
        public string GetAcceptLanguage()
        {
            try
            {
                return InstaApiConstants.AcceptLanguage;
            }
            catch (Exception exception)
            {
                return InstaResult.Fail<string>(exception).Value;
            }
        }

        /// <summary>
        ///     Get current time zone
        ///     <para>Returns something like: Asia/Tehran</para>
        /// </summary>
        /// <returns>Returns something like: Asia/Tehran</returns>
        public string GetTimezone()
        {
            return InstaApiConstants.Timezone;
        }

        /// <summary>
        ///     Get current time zone offset
        ///     <para>Returns something like this: 16200</para>
        /// </summary>
        /// <returns>Returns something like this: 16200</returns>
        public int GetTimezoneOffset()
        {
            return InstaApiConstants.TimezoneOffset;
        }

        /// <summary>
        ///     Set delay between requests. Useful when API supposed to be used for mass-bombing.
        /// </summary>
        /// <param name="delay">Timespan delay</param>
        public void SetRequestDelay(IRequestDelay delay)
        {
            if (delay == null)
            {
                delay = RequestDelay.Empty();
            }

            this.Delay = delay;
            HttpRequestProcessor.Delay = this.Delay;
        }

        /// <summary>
        ///     Set instagram api version (for user agent version)
        /// </summary>
        /// <param name="apiVersion">Api version</param>
        public void SetApiVersion(InstaApiVersionType apiVersion)
        {
            apiVersionType = apiVersion;
            this.apiVersion = InstaApiVersionList.GetApiVersionList().GetApiVersion(apiVersion);
            HttpHelper.ApiVersion = this.apiVersion;
        }

        /// <summary>
        ///     Set custom android device.
        ///     <para>
        ///         Note 1: If you want to use this method, you should call it before you calling
        ///         <seealso cref="IInstaApi.LoadStateDataFromStream(Stream)" /> or
        ///         <seealso cref="IInstaApi.LoadStateDataFromString(string)" />
        ///     </para>
        ///     <para>Note 2: this is optional, if you didn't set this, InstagramApiSharp will choose random device.</para>
        /// </summary>
        /// <param name="device">Android device</param>
        public void SetDevice(AndroidDevice device)
        {
            if (device == null)
            {
                return;
            }

            isCustomDevice = true;
            deviceInfo = device;
        }

        /// <summary>
        ///     Set Accept Language
        /// </summary>
        /// <param name="languageCodeAndCountryCode">
        ///     Language Code and Country Code. For example:
        ///     <para>en-US for united states</para>
        ///     <para>fa-IR for IRAN</para>
        /// </param>
        public bool SetAcceptLanguage(string languageCodeAndCountryCode)
        {
            try
            {
                InstaApiConstants.AcceptLanguage = languageCodeAndCountryCode;
                return true;
            }
            catch (Exception exception)
            {
                return InstaResult.Fail<bool>(exception).Value;
            }
        }

        /// <summary>
        ///     Set time zone
        ///     <para>I.e: Asia/Tehran for Iran</para>
        /// </summary>
        /// <param name="timezone">
        ///     time zone
        ///     <para>I.e: Asia/Tehran for Iran</para>
        /// </param>
        public void SetTimezone(string timezone)
        {
            if (string.IsNullOrEmpty(timezone))
            {
                return;
            }

            InstaApiConstants.Timezone = timezone;
        }

        /// <summary>
        ///     Set time zone offset
        ///     <para>I.e: 16200 for Iran/Tehran</para>
        /// </summary>
        /// <param name="timezoneOffset">
        ///     time zone offset
        ///     <para>I.e: 16200 for Iran/Tehran</para>
        /// </param>
        public void SetTimezoneOffset(int timezoneOffset)
        {
            InstaApiConstants.TimezoneOffset = timezoneOffset;
        }

        /// <summary>
        ///     Send get request
        /// </summary>
        /// <param name="uri">Desire uri (must include https://i.instagram.com/api/v...) </param>
        public async Task<IResult<string>> SendGetRequestAsync(Uri uri)
        {
            try
            {
                if (uri == null)
                {
                    return InstaResult.Fail("Uri cannot be null!", default(string));
                }

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, uri, deviceInfo);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<string>(response, json);
                }

                return InstaResult.Success(json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(string), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, default(string));
            }
        }

        /// <summary>
        ///     Send signed post request (include signed signature)
        /// </summary>
        /// <param name="uri">Desire uri (must include https://i.instagram.com/api/v...) </param>
        /// <param name="data">Data to post</param>
        public async Task<IResult<string>> SendSignedPostRequestAsync(Uri uri, Dictionary<string, string> data)
        {
            return await SendSignedPostRequest(uri, null, data).ConfigureAwait(false);
        }

        /// <summary>
        ///     Send signed post request (include signed signature)
        /// </summary>
        /// <param name="uri">Desire uri (must include https://i.instagram.com/api/v...) </param>
        /// <param name="data">Data to post</param>
        public async Task<IResult<string>> SendSignedPostRequestAsync(Uri uri, JObject data)
        {
            return await SendSignedPostRequest(uri, data, null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Send post request
        /// </summary>
        /// <param name="uri">Desire uri (must include https://i.instagram.com/api/v...) </param>
        /// <param name="data">Data to post</param>
        public async Task<IResult<string>> SendPostRequestAsync(Uri uri, Dictionary<string, string> data)
        {
            try
            {
                if (uri == null)
                {
                    return InstaResult.Fail("Uri cannot be null!", default(string));
                }

                data.Add("_uuid", deviceInfo.DeviceGuid.ToString());
                data.Add("_uid", User.LoggedInUser.Pk.ToString());
                data.Add("_csrftoken", User.CsrfToken);
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Post, uri, deviceInfo, data);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<string>(response, json);
                }

                return InstaResult.Success(json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(string), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, default(string));
            }
        }



        private async Task<IResult<InstaCheckEmailRegistration>> CheckEmail(string email, bool useNewWaterfall = true)
        {
            try
            {
                if (waterfallIdReg == null || useNewWaterfall)
                {
                    waterfallIdReg = Guid.NewGuid().ToString();
                }

                var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;

                var postData = new Dictionary<string, string>
                {
                    { "_csrftoken", csrftoken },
                    { "login_nonces", "[]" },
                    { "email", email },
                    { "qe_id", Guid.NewGuid().ToString() },
                    { "waterfall_id", waterfallIdReg }
                };
                var instaUri = InstaUriCreator.GetCheckEmailUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var obj = JsonConvert.DeserializeObject<InstaCheckEmailRegistration>(json);
                    if (obj.ErrorType == "fail")
                    {
                        return InstaResult.UnExpectedResponse<InstaCheckEmailRegistration>(response, json);
                    }

                    if (obj.ErrorType == "email_is_taken")
                    {
                        return InstaResult.Fail("Email is taken.", (InstaCheckEmailRegistration)null);
                    }

                    if (obj.ErrorType == "invalid_email")
                    {
                        return InstaResult.Fail("Please enter a valid email address.", (InstaCheckEmailRegistration)null);
                    }

                    return InstaResult.UnExpectedResponse<InstaCheckEmailRegistration>(response, json);
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<InstaCheckEmailRegistration>(json);
                    if (obj.ErrorType == "fail")
                    {
                        return InstaResult.UnExpectedResponse<InstaCheckEmailRegistration>(response, json);
                    }

                    if (obj.ErrorType == "email_is_taken")
                    {
                        return InstaResult.Fail("Email is taken.", (InstaCheckEmailRegistration)null);
                    }

                    if (obj.ErrorType == "invalid_email")
                    {
                        return InstaResult.Fail("Please enter a valid email address.", (InstaCheckEmailRegistration)null);
                    }

                    return InstaResult.Success(obj);
                }
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaCheckEmailRegistration), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaCheckEmailRegistration>(exception);
            }
        }

        public async Task<IResult<InstaRegistrationSuggestionResponse>> GetUsernameSuggestions(
            string name,
            bool useNewIds = true)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceIdReg))
                {
                    deviceIdReg = ApiRequestMessage.GenerateDeviceId();
                }

                if (useNewIds)
                {
                    phoneIdReg = Guid.NewGuid().ToString();
                    waterfallIdReg = Guid.NewGuid().ToString();
                    guidReg = Guid.NewGuid().ToString();
                }

                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;
                var postData = new Dictionary<string, string>
                {
                    { "name", name }, { "_csrftoken", csrftoken }, { "email", "" }
                };
                if (useNewIds)
                {
                    postData.Add("phone_id", phoneIdReg);
                    postData.Add("guid", guidReg);
                    postData.Add("device_id", deviceIdReg);
                    postData.Add("waterfall_id", waterfallIdReg);
                }
                else
                {
                    postData.Add("phone_id", deviceInfo.PhoneGuid.ToString());
                    postData.Add("guid", deviceInfo.DeviceGuid.ToString());
                    postData.Add("device_id", deviceInfo.DeviceId);
                    postData.Add("waterfall_id", waterfallIdReg ?? Guid.NewGuid().ToString());
                }

                var instaUri = InstaUriCreator.GetUsernameSuggestionsUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var o = JsonConvert.DeserializeObject<InstaAccountRegistrationPhoneNumber>(json);

                    return InstaResult.Fail(o.Message?.Errors?[0], (InstaRegistrationSuggestionResponse)null);
                }

                var obj = JsonConvert.DeserializeObject<InstaRegistrationSuggestionResponse>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaRegistrationSuggestionResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaRegistrationSuggestionResponse>(exception);
            }
        }

        private async Task<IResult<object>> GetRegistrationStepsAsync()
        {
            try
            {
                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;
                var postData = new Dictionary<string, string>
                {
                    { "fb_connected", "false" },
                    { "seen_steps", "[]" },
                    { "phone_id", phoneIdReg },
                    { "fb_installed", "false" },
                    { "locale", "en_US" },
                    { "timezone_offset", "16200" },
                    { "network_type", "WIFI-UNKNOWN" },
                    { "_csrftoken", csrftoken },
                    { "guid", guidReg },
                    { "is_ci", "false" },
                    { "android_id", deviceIdReg },
                    { "reg_flow_taken", "phone" },
                    { "tos_accepted", "false" }
                };
                var instaUri = InstaUriCreator.GetOnboardingStepsUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var o = JsonConvert.DeserializeObject<InstaAccountRegistrationPhoneNumber>(json);

                    return InstaResult.Fail(o.Message?.Errors?[0], (InstaRegistrationSuggestionResponse)null);
                }

                var obj = JsonConvert.DeserializeObject<InstaRegistrationSuggestionResponse>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaRegistrationSuggestionResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaRegistrationSuggestionResponse>(exception);
            }
        }

        /// <summary>
        ///     Create a new instagram account [NEW FUNCTION, BUT NOT WORKING?!!!!!!!!!!]
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="email">Email</param>
        /// <param name="firstName">First name (optional)</param>
        /// <param name="delay">Delay between requests. null = 2.5 seconds</param>
        private async Task<IResult<InstaAccountCreation>> CreateNewAccountAsync(
            string username,
            string password,
            string email,
            string firstName = "",
            TimeSpan? delay = null)
        {
            var createResponse = new InstaAccountCreation();
            try
            {
                if (delay == null)
                {
                    delay = TimeSpan.FromSeconds(2.5);
                }

                var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                await firstResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                var checkEmail = await CheckEmail(email, false).ConfigureAwait(false);
                if (!checkEmail.Succeeded)
                {
                    return InstaResult.Fail(checkEmail.Info.Message, (InstaAccountCreation)null);
                }

                await Task.Delay((int)delay.Value.TotalMilliseconds).ConfigureAwait(false);
                if (checkEmail.Value.GdprRequired)
                {
                    var acceptGdpr = await AcceptConsentRequiredAsync(email).ConfigureAwait(false);
                    if (!acceptGdpr.Succeeded)
                    {
                        return InstaResult.Fail(acceptGdpr.Info.Message, (InstaAccountCreation)null);
                    }
                }

                await Task.Delay((int)delay.Value.TotalMilliseconds).ConfigureAwait(false);
                if (username.Length > 6)
                {
                    await GetUsernameSuggestions(username.Substring(0, 4), false).ConfigureAwait(false);
                    await Task.Delay(1000).ConfigureAwait(false);
                    await GetUsernameSuggestions(username.Substring(0, 5), false).ConfigureAwait(false);
                }
                else
                {
                    await GetUsernameSuggestions(username, false).ConfigureAwait(false);
                    await Task.Delay(1000).ConfigureAwait(false);
                    await GetUsernameSuggestions(username, false).ConfigureAwait(false);
                }

                await Task.Delay((int)delay.Value.TotalMilliseconds).ConfigureAwait(false);
                var postData = new Dictionary<string, string>
                {
                    { "allow_contacts_sync", "true" },
                    { "sn_result", "API_ERROR:+null" },
                    { "phone_id", deviceInfo.PhoneGuid.ToString() },
                    { "_csrftoken", csrftoken },
                    { "username", username },
                    { "first_name", firstName },
                    { "adid", Guid.NewGuid().ToString() },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId },
                    { "email", email },
                    { "sn_nonce", "" },
                    { "force_sign_up_code", "" },
                    { "waterfall_id", waterfallIdReg ?? Guid.NewGuid().ToString() },
                    { "qs_stamp", "" },
                    { "password", password }
                };
                if (checkEmail.Value.GdprRequired)
                {
                    postData.Add("gdpr_s", "[0,2,0,null]");
                }

                var instaUri = InstaUriCreator.GetCreateAccountUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<InstaAccountCreation>(response, json);
                }

                var obj = JsonConvert.DeserializeObject<InstaAccountCreation>(json);

                //{"account_created": false, "errors": {"email": ["Another account is using iranramtin73jokar@live.com."], "username": ["This username isn't available. Please try another."]}, "allow_contacts_sync": true, "status": "ok", "error_type": "email_is_taken, username_is_taken"}
                //{"message": "feedback_required", "spam": true, "feedback_title": "Signup Error", "feedback_message": "Sorry! There\u2019s a problem signing you up right now. Please try again later. We restrict certain content and actions to protect our community. Tell us if you think we made a mistake.", "feedback_url": "repute/report_problem/instagram_signup/", "feedback_appeal_label": "Report problem", "feedback_ignore_label": "OK", "feedback_action": "report_problem", "status": "fail", "error_type": "signup_block"}

                if (obj.AccountCreated && obj.CreatedUser != null)
                {
                    ValidateUserAsync(obj.CreatedUser, csrftoken, true, password);
                }

                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaAccountCreation), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaAccountCreation>(exception);
            }
        }

        /// <summary>
        ///     Accept consent require (for GDPR countries)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        private async Task<IResult<bool>> AcceptConsentRequiredAsync(string email, string phone = null)
        {
            try
            {
                var delay = TimeSpan.FromSeconds(2);

                //{"message": "consent_required", "consent_data": {"headline": "Updates to Our Terms and Data Policy", "content": "We've updated our Terms and made some changes to our Data Policy. Please take a moment to review these changes and let us know that you agree to them.\n\nYou need to finish reviewing this information before you can use Instagram.", "button_text": "Review Now"}, "status": "fail"}
                await Task.Delay((int)delay.TotalMilliseconds).ConfigureAwait(false);
                var instaUri = InstaUriCreator.GetConsentNewUserFlowBeginsUri();
                var data = new JObject { { "phone_id", deviceInfo.PhoneGuid }, { "_csrftoken", User.CsrfToken } };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                await Task.Delay((int)delay.TotalMilliseconds).ConfigureAwait(false);

                instaUri = InstaUriCreator.GetConsentNewUserFlowUri();
                data = new JObject
                {
                    { "phone_id", deviceInfo.PhoneGuid },
                    { "gdpr_s", "" },
                    { "_csrftoken", User.CsrfToken },
                    { "guid", deviceInfo.DeviceGuid },
                    { "device_id", deviceInfo.DeviceId }
                };
                if (email != null)
                {
                    data.Add("email", email);
                }
                else
                {
                    if (phone != null && !phone.StartsWith("+"))
                    {
                        phone = $"+{phone}";
                    }

                    if (phone == null)
                    {
                        phone = string.Empty;
                    }

                    data.Add("phone", phone);
                }

                request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                await Task.Delay((int)delay.TotalMilliseconds).ConfigureAwait(false);

                data = new JObject
                {
                    { "current_screen_key", "age_consent_two_button" },
                    { "phone_id", deviceInfo.PhoneGuid },
                    { "gdpr_s", "[0,0,0,null]" },
                    { "_csrftoken", User.CsrfToken },
                    { "updates", "{\"age_consent_state\":\"2\"}" },
                    { "guid", deviceInfo.DeviceGuid },
                    { "device_id", deviceInfo.DeviceId }
                };
                request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                return InstaResult.Success(true);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, false);
            }
        }

        private async Task<IResult<InstaChallengeRequireSmsVerify>> RequestVerifyCodeToSmsForChallengeRequire(
            bool replayChallenge,
            string phoneNumber = null)
        {
            if (challengeInfo == null)
            {
                return InstaResult.Fail("challenge require info is empty.\r\ntry to call LoginAsync function first.",
                                   (InstaChallengeRequireSmsVerify)null);
            }

            try
            {
                Uri instaUri;

                if (replayChallenge)
                {
                    instaUri = InstaUriCreator.GetChallengeReplayUri(challengeInfo.ApiPath);
                }
                else
                {
                    instaUri = InstaUriCreator.GetChallengeRequireUri(challengeInfo.ApiPath);
                }

                var data = new JObject
                {
                    { "_csrftoken", User.CsrfToken },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId }
                };
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    data.Add("phone_number", phoneNumber);
                }
                else
                {
                    data.Add("choice", "0");
                }

                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var msg = "";
                    try
                    {
                        var j = JsonConvert.DeserializeObject<InstaChallengeRequireSmsVerify>(json);
                        msg = j.Message;
                    }
                    catch
                    {
                    }

                    return InstaResult.Fail(msg, (InstaChallengeRequireSmsVerify)null);
                }

                var obj = JsonConvert.DeserializeObject<InstaChallengeRequireSmsVerify>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(InstaChallengeRequireSmsVerify), InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, (InstaChallengeRequireSmsVerify)null);
            }
        }

        public async Task<IResult<InstaLoginResult>> LoginWithFacebookAsync(
            string fbAccessToken,
            string cookiesContainer,
            bool dryrun = true,
            string username = null,
            string waterfallId = null,
            string adId = null,
            bool newToken = true)
        {
            try
            {
                facebookToken = null;
                if (newToken)
                {
                    var firstResponse = await HttpRequestProcessor.GetAsync(HttpRequestProcessor.Client.BaseAddress).ConfigureAwait(false);
                    await firstResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                else
                {
                    Debug.WriteLine("--------------------RELOGIN-------------------------");
                }

                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                var uri = new Uri(InstaApiConstants.InstagramUrl);

                cookiesContainer = cookiesContainer.Replace(';', ',');
                HttpRequestProcessor.HttpHandler.CookieContainer.SetCookies(uri, cookiesContainer);

                if (adId.IsEmpty())
                {
                    adId = Guid.NewGuid().ToString();
                }

                if (waterfallId.IsEmpty())
                {
                    waterfallId = Guid.NewGuid().ToString();
                }

                var instaUri = InstaUriCreator.GetFacebookSignUpUri();

                var data = new JObject
                {
                    { "dryrun", dryrun.ToString().ToLower() },
                    { "phone_id", deviceInfo.PhoneGuid.ToString() },
                    { "_csrftoken", csrftoken },
                    { "adid", adId },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "device_id", deviceInfo.DeviceId },
                    { "waterfall_id", waterfallId },
                    { "fb_access_token", fbAccessToken }
                };
                if (username.IsNotEmpty())
                {
                    data.Add("username", username);
                }

                facebookToken = fbAccessToken;
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var loginFailReason = JsonConvert.DeserializeObject<InstaLoginBaseResponse>(json);

                    if (loginFailReason.InvalidCredentials)
                    {
                        return InstaResult.Fail(
                            "Invalid Credentials",
                            loginFailReason.ErrorType == "bad_password"
                                ? InstaLoginResult.BadPassword
                                : InstaLoginResult.InvalidUser);
                    }

                    if (loginFailReason.TwoFactorRequired)
                    {
                        twoFactorInfo = loginFailReason.TwoFactorLoginInfo;
                        HttpRequestProcessor.RequestMessage.Username = twoFactorInfo.Username;
                        HttpRequestProcessor.RequestMessage.DeviceId = deviceInfo.DeviceId;
                        return InstaResult.Fail("Two Factor Authentication is required", InstaLoginResult.TwoFactorRequired);
                    }

                    if (loginFailReason.ErrorType == "checkpoint_challenge_required")
                    {
                        challengeInfo = loginFailReason.Challenge;

                        return InstaResult.Fail("Challenge is required", InstaLoginResult.ChallengeRequired);
                    }

                    if (loginFailReason.ErrorType == "rate_limit_error")
                    {
                        return InstaResult.Fail("Please wait a few minutes before you try again.",
                                           InstaLoginResult.LimitError);
                    }

                    if (loginFailReason.ErrorType == "inactive user" || loginFailReason.ErrorType == "inactive_user")
                    {
                        return InstaResult.Fail($"{loginFailReason.Message}\r\nHelp url: {loginFailReason.HelpUrl}",
                                           InstaLoginResult.InactiveUser);
                    }

                    if (loginFailReason.ErrorType == "checkpoint_logged_out")
                    {
                        return InstaResult.Fail($"{loginFailReason.ErrorType} {loginFailReason.CheckpointUrl}", InstaLoginResult.CheckpointLoggedOut);
                    }

                    return InstaResult.UnExpectedResponse<InstaLoginResult>(response, json);
                }

                var fbUserId = string.Empty;
                InstaUserShortResponse loginInfoUser = null;
                if (json.Contains("\"account_created\""))
                {
                    var rmt = JsonConvert.DeserializeObject<FacebookRegistrationResponse>(json);
                    if (rmt?.AccountCreated != null)
                    {
                        fbUserId = rmt?.FbUserId;
                        if (rmt.AccountCreated.Value)
                        {
                            loginInfoUser = JsonConvert.DeserializeObject<FacebookLoginResponse>(json)
                                ?.CreatedUser;
                        }
                        else
                        {
                            var desireUsername = rmt?.UsernameSuggestionsWithMetadata?.Suggestions?.LastOrDefault()
                                ?.Username;
                            await Task.Delay(4500).ConfigureAwait(false);
                            await GetFacebookOnboardingStepsAsync().ConfigureAwait(false);
                            await Task.Delay(12000).ConfigureAwait(false);

                            return await LoginWithFacebookAsync(fbAccessToken,
                                                                cookiesContainer,
                                                                false,
                                                                desireUsername,
                                                                waterfallId,
                                                                adId,
                                                                false).ConfigureAwait(false);
                        }
                    }
                }

                if (loginInfoUser == null)
                {
                    var obj = JsonConvert.DeserializeObject<FacebookLoginResponse>(json);
                    fbUserId = obj?.FbUserId;
                    loginInfoUser = obj?.LoggedInUser;
                }

                IsUserAuthenticated = true;
                var converter = InstaConvertersFabric.Instance.GetUserShortConverter(loginInfoUser);
                User.LoggedInUser = converter.Convert();
                User.RankToken = $"{User.LoggedInUser.Pk}_{HttpRequestProcessor.RequestMessage.PhoneId}";
                User.CsrfToken = csrftoken;
                User.FacebookUserId = fbUserId;
                User.UserName = User.LoggedInUser.UserName;
                User.FacebookAccessToken = fbAccessToken;
                User.Password = "ALAKIMASALAN";

                InvalidateProcessors();

                User.RankToken = $"{User.LoggedInUser.Pk}_{HttpRequestProcessor.RequestMessage.PhoneId}";
                if (string.IsNullOrEmpty(User.CsrfToken))
                {
                    cookies =
                        HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                            HttpRequestProcessor.Client
                                .BaseAddress);
                    User.CsrfToken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                }

                return InstaResult.Success(InstaLoginResult.Success);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, InstaLoginResult.Exception, InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return InstaResult.Fail(exception, InstaLoginResult.Exception);
            }
        }

        private async Task<IResult<object>> GetFacebookOnboardingStepsAsync()
        {
            try
            {
                var cookies =
                    HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                        HttpRequestProcessor.Client
                            .BaseAddress);
                var csrftoken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                User.CsrfToken = csrftoken;

                //{
                //  "fb_connected": "true",
                //  "seen_steps": "[]",
                //  "phone_id": "d46328c2-01af-4457-9da2-bc60637abde6",
                //  "fb_installed": "false",
                //  "locale": "en_US",
                //  "timezone_offset": "12600",
                //  "_csrftoken": "2YmsoSkHtIknBA8maAqb1QSk92nrM6xo",
                //  "network_type": "WIFI-UNKNOWN",
                //  "_uid": "9013775990",
                //  "guid": "6324ecb2-e663-4dc8-a3a1-289c699cc876",
                //  "_uuid": "6324ecb2-e663-4dc8-a3a1-289c699cc876",
                //  "is_ci": "false",
                //  "android_id": "android-21c311d494a974fe",
                //  "reg_flow_taken": "facebook",
                //  "tos_accepted": "false"
                //}

                var postData = new Dictionary<string, string>
                {
                    { "fb_connected", "true" },
                    { "seen_steps", "[]" },
                    { "phone_id", deviceInfo.PhoneGuid.ToString() },
                    { "fb_installed", "false" },
                    { "locale", InstaApiConstants.AcceptLanguage.Replace("-", "_") },
                    { "timezone_offset", InstaApiConstants.TimezoneOffset.ToString() },
                    { "_csrftoken", csrftoken },
                    { "network_type", "WIFI-UNKNOWN" },
                    { "guid", deviceInfo.DeviceGuid.ToString() },
                    { "_uuid", deviceInfo.DeviceGuid.ToString() },
                    { "is_ci", "false" },
                    { "android_id", deviceInfo.DeviceId },
                    { "reg_flow_taken", "facebook" },
                    { "tos_accepted", "false" }
                };

                var instaUri = InstaUriCreator.GetOnboardingStepsUri();
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, postData);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var o = JsonConvert.DeserializeObject<InstaAccountRegistrationPhoneNumber>(json);

                    return InstaResult.Fail(o.Message?.Errors?[0], (InstaRegistrationSuggestionResponse)null);
                }

                var obj = JsonConvert.DeserializeObject<InstaRegistrationSuggestionResponse>(json);
                return InstaResult.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException,
                                   default(InstaRegistrationSuggestionResponse),
                                   InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail<InstaRegistrationSuggestionResponse>(exception);
            }
        }

        private async Task<IResult<bool>> AcceptFacebookConsentRequiredAsync(string email, string phone = null)
        {
            try
            {
                var delay = TimeSpan.FromSeconds(2);

                //{"message": "consent_required", "consent_data": {"headline": "Updates to Our Terms and Data Policy", "content": "We've updated our Terms and made some changes to our Data Policy. Please take a moment to review these changes and let us know that you agree to them.\n\nYou need to finish reviewing this information before you can use Instagram.", "button_text": "Review Now"}, "status": "fail"}
                await Task.Delay((int)delay.TotalMilliseconds).ConfigureAwait(false);
                var instaUri = InstaUriCreator.GetConsentNewUserFlowBeginsUri();
                var data = new JObject { { "phone_id", deviceInfo.PhoneGuid }, { "_csrftoken", User.CsrfToken } };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                await Task.Delay((int)delay.TotalMilliseconds).ConfigureAwait(false);

                instaUri = InstaUriCreator.GetConsentNewUserFlowUri();
                data = new JObject
                {
                    { "phone_id", deviceInfo.PhoneGuid },
                    { "gdpr_s", "" },
                    { "_csrftoken", User.CsrfToken },
                    { "guid", deviceInfo.DeviceGuid },
                    { "device_id", deviceInfo.DeviceId }
                };
                if (email != null)
                {
                    data.Add("email", email);
                }
                else
                {
                    if (phone != null && !phone.StartsWith("+"))
                    {
                        phone = $"+{phone}";
                    }

                    if (phone == null)
                    {
                        phone = string.Empty;
                    }

                    data.Add("phone", phone);
                }

                request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                await Task.Delay((int)delay.TotalMilliseconds).ConfigureAwait(false);

                data = new JObject
                {
                    { "current_screen_key", "age_consent_two_button" },
                    { "phone_id", deviceInfo.PhoneGuid },
                    { "gdpr_s", "[0,0,0,null]" },
                    { "_csrftoken", User.CsrfToken },
                    { "updates", "{\"age_consent_state\":\"2\"}" },
                    { "guid", deviceInfo.DeviceGuid },
                    { "device_id", deviceInfo.DeviceId }
                };
                request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, deviceInfo, data);
                response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<bool>(response, json);
                }

                return InstaResult.Success(true);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(bool), InstaResponseType.NetworkProblem);
            }
            catch (Exception ex)
            {
                return InstaResult.Fail(ex, false);
            }
        }

        internal IRequestDelay GetRequestDelay()
        {
            return Delay;
        }

        private async Task<IResult<string>> SendSignedPostRequest(Uri uri,
                                                                  JObject jData,
                                                                  Dictionary<string, string> dicData)
        {
            try
            {
                if (uri == null)
                {
                    return InstaResult.Fail("Uri cannot be null!", default(string));
                }

                HttpRequestMessage request;
                if (jData != null)
                {
                    jData.Add("_uuid", deviceInfo.DeviceGuid.ToString());
                    jData.Add("_uid", User.LoggedInUser.Pk.ToString());
                    jData.Add("_csrftoken", User.CsrfToken);
                    request = HttpHelper.GetSignedRequest(HttpMethod.Post, uri, deviceInfo, jData);
                }
                else
                {
                    dicData.Add("_uuid", deviceInfo.DeviceGuid.ToString());
                    dicData.Add("_uid", User.LoggedInUser.Pk.ToString());
                    dicData.Add("_csrftoken", User.CsrfToken);
                    request = HttpHelper.GetSignedRequest(HttpMethod.Post, uri, deviceInfo, dicData);
                }

                var response = await HttpRequestProcessor.SendAsync(request).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return InstaResult.UnExpectedResponse<string>(response, json);
                }

                return InstaResult.Success(json);
            }
            catch (HttpRequestException httpException)
            {
                logger?.LogError(httpException, "Error");
                return InstaResult.Fail(httpException, default(string), InstaResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                logger?.LogError(exception, "Error");
                return InstaResult.Fail(exception, default(string));
            }
        }

        private void InvalidateProcessors()
        {
            HashtagProcessor = new InstaHashtagProcessor(deviceInfo,
                                                    User,
                                                    HttpRequestProcessor,
                                                    logger,
                                                    userAuthValidate,
                                                    this,
                                                    HttpHelper);
            LocationProcessor = new InstaLocationProcessor(deviceInfo,
                                                      User,
                                                      HttpRequestProcessor,
                                                      logger,
                                                      userAuthValidate,
                                                      this,
                                                      HttpHelper);
            CollectionProcessor = new InstaCollectionProcessor(deviceInfo,
                                                          User,
                                                          HttpRequestProcessor,
                                                          logger,
                                                          userAuthValidate,
                                                          this,
                                                          HttpHelper);
            MediaProcessor = new InstaMediaProcessor(deviceInfo,
                                                User,
                                                HttpRequestProcessor,
                                                logger,
                                                userAuthValidate,
                                                this,
                                                HttpHelper);
            UserProcessor = new InstaUserProcessor(deviceInfo,
                                              User,
                                              HttpRequestProcessor,
                                              logger,
                                              userAuthValidate,
                                              this,
                                              HttpHelper);
            StoryProcessor = new InstaStoryProcessor(deviceInfo,
                                                User,
                                                HttpRequestProcessor,
                                                logger,
                                                userAuthValidate,
                                                this,
                                                HttpHelper);
            CommentProcessor = new InstaCommentProcessor(deviceInfo,
                                                    User,
                                                    HttpRequestProcessor,
                                                    logger,
                                                    userAuthValidate,
                                                    this,
                                                    HttpHelper);
            MessagingProcessor = new InstaMessagingProcessor(deviceInfo,
                                                        User,
                                                        HttpRequestProcessor,
                                                        logger,
                                                        userAuthValidate,
                                                        this,
                                                        HttpHelper);
            FeedProcessor = new InstaFeedProcessor(deviceInfo,
                                              User,
                                              HttpRequestProcessor,
                                              logger,
                                              userAuthValidate,
                                              this,
                                              HttpHelper);

            LiveProcessor = new InstaLiveProcessor(deviceInfo,
                                              User,
                                              HttpRequestProcessor,
                                              logger,
                                              userAuthValidate,
                                              this,
                                              HttpHelper);
            DiscoverProcessor = new InstaDiscoverProcessor(deviceInfo,
                                                      User,
                                                      HttpRequestProcessor,
                                                      logger,
                                                      userAuthValidate,
                                                      this,
                                                      HttpHelper);
            AccountProcessor = new InstaAccountProcessor(deviceInfo,
                                                    User,
                                                    HttpRequestProcessor,
                                                    logger,
                                                    userAuthValidate,
                                                    this,
                                                    HttpHelper);
            HelperProcessor = new InstaHelperProcessor(deviceInfo,
                                                  User,
                                                  HttpRequestProcessor,
                                                  logger,
                                                  userAuthValidate,
                                                  this,
                                                  HttpHelper);
            TvProcessor = new InstaTvProcessor(deviceInfo,
                                          User,
                                          HttpRequestProcessor,
                                          logger,
                                          userAuthValidate,
                                          this,
                                          HttpHelper);
            BusinessProcessor = new InstaBusinessProcessor(deviceInfo,
                                                      User,
                                                      HttpRequestProcessor,
                                                      logger,
                                                      userAuthValidate,
                                                      this,
                                                      HttpHelper);
            ShoppingProcessor = new InstaShoppingProcessor(deviceInfo,
                                                      User,
                                                      HttpRequestProcessor,
                                                      logger,
                                                      userAuthValidate,
                                                      this,
                                                      HttpHelper);
            WebProcessor = new InstaWebProcessor(deviceInfo,
                                            User,
                                            HttpRequestProcessor,
                                            logger,
                                            userAuthValidate,
                                            this,
                                            HttpHelper);
        }

        private void ValidateUserAsync(InstaUserShortResponse user,
                                       string csrfToken,
                                       bool validateExtra = true,
                                       string password = null)
        {
            try
            {
                var converter = InstaConvertersFabric.Instance.GetUserShortConverter(user);
                User.LoggedInUser = converter.Convert();
                if (password != null)
                {
                    User.Password = password;
                }

                User.UserName = User.UserName;
                if (validateExtra)
                {
                    User.RankToken = $"{User.LoggedInUser.Pk}_{HttpRequestProcessor.RequestMessage.PhoneId}";
                    User.CsrfToken = csrfToken;
                    if (string.IsNullOrEmpty(User.CsrfToken))
                    {
                        var cookies =
                            HttpRequestProcessor.HttpHandler.CookieContainer.GetCookies(
                                HttpRequestProcessor.Client
                                    .BaseAddress);
                        User.CsrfToken = cookies[InstaApiConstants.Csrftoken]?.Value ?? string.Empty;
                    }

                    IsUserAuthenticated = true;
                    InvalidateProcessors();
                }
            }
            catch
            {
            }
        }

        private void ValidateUser()
        {
            if (string.IsNullOrEmpty(User.UserName) || string.IsNullOrEmpty(User.Password))
            {
                throw new ArgumentException("user name and password must be specified");
            }
        }

        private void ValidateLoggedIn()
        {
            if (!IsUserAuthenticated)
            {
                throw new ArgumentException("user must be authenticated");
            }
        }

        private void ValidateRequestMessage()
        {
            if (HttpRequestProcessor.RequestMessage == null || HttpRequestProcessor.RequestMessage.IsEmpty())
            {
                throw new ArgumentException("API request message null or empty");
            }
        }

        private void LogError(Exception exception)
        {
            logger?.LogError(exception, "Error");
        }
    }
}