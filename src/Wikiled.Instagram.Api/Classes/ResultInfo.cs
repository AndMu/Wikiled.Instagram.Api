using System;
using System.Text.RegularExpressions;
using Wikiled.Instagram.Api.Classes.Models.Challenge;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors;

namespace Wikiled.Instagram.Api.Classes
{
    public class ResultInfo
    {
        public ResultInfo(string message)
        {
            Message = message;
            HandleMessages(message);
        }

        public ResultInfo(Exception exception)
        {
            Exception = exception;
            Message = exception?.Message;
            ResponseType = InstaResponseType.InternalException;
            HandleMessages(Message);
        }

        public ResultInfo(Exception exception, InstaResponseType responseType)
        {
            Exception = exception;
            Message = exception?.Message;
            ResponseType = responseType;
            HandleMessages(Message);
        }

        public ResultInfo(InstaResponseType responseType, string errorMessage)
        {
            ResponseType = responseType;
            Message = errorMessage;
            HandleMessages(errorMessage);
        }

        public ResultInfo(InstaResponseType responseType, InstaBadStatusResponse status)
        {
            Message = status?.Message;
            Challenge = status?.Challenge;
            ResponseType = responseType;
            HandleMessages(Message);
            switch (ResponseType)
            {
                case InstaResponseType.ActionBlocked:
                case InstaResponseType.Spam:
                    if (status != null &&
                        !string.IsNullOrEmpty(status.FeedbackMessage) &&
                        status.FeedbackMessage.ToLower().Contains("this block will expire on"))
                    {
                        var dateRegex = new Regex(@"(\d+)[-.\/](\d+)[-.\/](\d+)");
                        var dateMatch = dateRegex.Match(status.FeedbackMessage);
                        if (DateTime.TryParse(dateMatch.ToString(), out var parsedDate))
                        {
                            ActionBlockEnd = parsedDate;
                        }
                    }
                    else
                    {
                        ActionBlockEnd = null;
                    }

                    break;
                default:
                    ActionBlockEnd = null;
                    break;
            }
        }

        public DateTime? ActionBlockEnd { get; internal set; }

        public ChallengeLoginInfo Challenge { get; internal set; }

        public Exception Exception { get; }

        public string Message { get; }

        public bool NeedsChallenge { get; internal set; }

        public InstaResponseType ResponseType { get; }

        public bool Timeout { get; internal set; }

        public override string ToString()
        {
            return $"{ResponseType.ToString()}: {Message}.";
        }

        public void HandleMessages(string errorMessage)
        {
            if (errorMessage.Contains("task was canceled"))
            {
                Timeout = true;
            }

            if (errorMessage.ToLower().Contains("challenge"))
            {
                NeedsChallenge = true;
            }
        }
    }
}