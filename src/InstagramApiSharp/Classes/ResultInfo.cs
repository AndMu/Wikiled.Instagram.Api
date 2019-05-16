using System;
using System.Text.RegularExpressions;

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
            System.Exception = exception;
            Message = exception?.Message;
            ResponseType = ResponseType.InternalException;
            HandleMessages(Message);
        }

        public ResultInfo(Exception exception, ResponseType responseType)
        {
            System.Exception = exception;
            Message = exception?.Message;
            ResponseType = responseType;
            HandleMessages(Message);
        }

        public ResultInfo(ResponseType responseType, string errorMessage)
        {
            ResponseType = responseType;
            Message = errorMessage;
            HandleMessages(errorMessage);
        }

        public ResultInfo(ResponseType responseType, BadStatusResponse status)
        {
            Message = status?.Message;
            Challenge = status?.Challenge;
            ResponseType = responseType;
            HandleMessages(Message);
            switch (ResponseType)
            {
                case ResponseType.ActionBlocked:
                case ResponseType.Spam:
                    if (status != null && !string.IsNullOrEmpty(status.FeedbackMessage) && status.FeedbackMessage.ToLower().Contains("this block will expire on"))
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

        public InstaChallengeLoginInfo Challenge { get; internal set; }

        public Exception Exception { get; }

        public string Message { get; }

        public bool NeedsChallenge { get; internal set; }

        public ResponseType ResponseType { get; }

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
