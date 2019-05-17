using System;
using System.Net.Http;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Classes
{
    public class InstaResult<T> : IResult<T>
    {
        public InstaResult(bool succeeded, T value, ResultInfo info)
        {
            Succeeded = succeeded;
            Value = value;
            Info = info;
        }

        public InstaResult(bool succeeded, ResultInfo info)
        {
            Succeeded = succeeded;
            Info = info;
        }

        public InstaResult(bool succeeded, T value)
        {
            Succeeded = succeeded;
            Value = value;
        }

        public ResultInfo Info { get; } = new ResultInfo("");

        public bool Succeeded { get; }

        public T Value { get; }
    }

    public static class InstaResult
    {
        public static IResult<T> Fail<T>(Exception exception)
        {
            return new InstaResult<T>(false, default, new ResultInfo(exception));
        }

        public static IResult<T> Fail<T>(string errMsg)
        {
            return new InstaResult<T>(false, default, new ResultInfo(errMsg));
        }

        public static IResult<T> Fail<T>(string errMsg, T resValue)
        {
            return new InstaResult<T>(false, resValue, new ResultInfo(errMsg));
        }

        public static IResult<T> Fail<T>(Exception exception, T resValue)
        {
            return new InstaResult<T>(false, resValue, new ResultInfo(exception));
        }

        public static IResult<T> Fail<T>(Exception exception, T resValue, InstaResponseType responseType)
        {
            return new InstaResult<T>(false, resValue, new ResultInfo(exception, responseType));
        }

        public static IResult<T> Fail<T>(ResultInfo info, T resValue)
        {
            return new InstaResult<T>(false, resValue, info);
        }

        public static IResult<T> Fail<T>(string errMsg, InstaResponseType responseType, T resValue)
        {
            return new InstaResult<T>(false, resValue, new ResultInfo(responseType, errMsg));
        }

        public static IResult<T> Success<T>(T resValue)
        {
            return new InstaResult<T>(true, resValue, new ResultInfo(InstaResponseType.Ok, "No errors detected"));
        }

        public static IResult<T> Success<T>(string successMsg, T resValue)
        {
            return new InstaResult<T>(true, resValue, new ResultInfo(InstaResponseType.Ok, successMsg));
        }

        public static IResult<T> UnExpectedResponse<T>(HttpResponseMessage response, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                var resultInfo = new ResultInfo(
                    InstaResponseType.UnExpectedResponse,
                    $"Unexpected response status: {response.StatusCode}");
                return new InstaResult<T>(false, default, resultInfo);
            }
            else
            {
                var status = InstaErrorHandlingHelper.GetBadStatusFromJsonString(json);
                var responseType = GetResponseType(status);

                var resultInfo = new ResultInfo(responseType, status) { Challenge = status.Challenge };
                return new InstaResult<T>(false, default, resultInfo);
            }
        }

        public static IResult<T> UnExpectedResponse<T>(HttpResponseMessage response, string message, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                var resultInfo = new ResultInfo(
                    InstaResponseType.UnExpectedResponse,
                    $"{message}\r\nUnexpected response status: {response.StatusCode}");
                return new InstaResult<T>(false, default, resultInfo);
            }
            else
            {
                var status = InstaErrorHandlingHelper.GetBadStatusFromJsonString(json);
                var responseType = GetResponseType(status);

                var resultInfo = new ResultInfo(responseType, message) { Challenge = status.Challenge };

                return new InstaResult<T>(false, default, resultInfo);
            }
        }

        private static InstaResponseType GetResponseType(InstaBadStatusResponse status)
        {
            var responseType = InstaResponseType.UnExpectedResponse;
            if (!string.IsNullOrWhiteSpace(status.ErrorType))
            {
                switch (status.ErrorType)
                {
                    case "checkpoint_logged_out":
                        responseType = InstaResponseType.CheckPointRequired;
                        break;
                    case "login_required":
                        responseType = InstaResponseType.LoginRequired;
                        break;
                    case "Sorry, too many requests.Please try again later":
                        responseType = InstaResponseType.RequestsLimit;
                        break;
                    case "sentry_block":
                        responseType = InstaResponseType.SentryBlock;
                        break;
                    case "inactive user":
                    case "inactive_user":
                        responseType = InstaResponseType.InactiveUser;
                        break;
                    case "checkpoint_challenge_required":
                        responseType = InstaResponseType.ChallengeRequired;
                        break;
                }
            }

            if (!status.IsOk() && status.Message.Contains("wait a few minutes"))
            {
                responseType = InstaResponseType.RequestsLimit;
            }

            if (!string.IsNullOrEmpty(status.Message) && status.Message.Contains("consent_required"))
            {
                responseType = InstaResponseType.ConsentRequired;
            }

            if (!string.IsNullOrEmpty(status.FeedbackTitle) &&
                status.FeedbackTitle.ToLower().Contains("action blocked"))
            {
                responseType = InstaResponseType.ActionBlocked;
            }

            if (!string.IsNullOrEmpty(status.Message) && status.Message.Contains("login_required"))
            {
                responseType = InstaResponseType.LoginRequired;
            }

            if (!string.IsNullOrEmpty(status.Message) &&
                status.Message.ToLower().Contains("media not found or unavailable"))
            {
                responseType = InstaResponseType.MediaNotFound;
            }

            if (!string.IsNullOrEmpty(status.FeedbackTitle) &&
                status.FeedbackTitle.ToLower().Contains("commenting is Off"))
            {
                responseType = InstaResponseType.CommentingIsDisabled;
            }

            if (!string.IsNullOrEmpty(status.Message) && status.Message.ToLower().Contains("already liked"))
            {
                responseType = InstaResponseType.AlreadyLiked;
            }

            if (!string.IsNullOrEmpty(status.FeedbackMessage) &&
                status.FeedbackMessage.ToLower().Contains("post you were viewing has been deleted"))
            {
                responseType = InstaResponseType.DeletedPost;
            }

            if (!string.IsNullOrEmpty(status.Message) && status.Message.ToLower().Contains("you cannot like this"))
            {
                responseType = InstaResponseType.CantLike;
            }

            if (status.Payload != null)
            {
                if (!string.IsNullOrEmpty(status.Payload.Message) &&
                    status.Payload.Message.ToLower().Contains("media is not accessible"))
                {
                    responseType = InstaResponseType.DeletedPost;
                }
            }

            if (status.Spam)
            {
                responseType = InstaResponseType.Spam;
            }

            if (status?.Message?.IndexOf("challenge_required") != -1)
            {
                responseType = InstaResponseType.ChallengeRequired;
            }

            return responseType;
        }
    }
}