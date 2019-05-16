using System;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors;

namespace Wikiled.Instagram.Api.Helpers
{
    internal static class InstaErrorHandlingHelper
    {
        internal static InstaBadStatusResponse GetBadStatusFromJsonString(string json)
        {
            var badStatus = new InstaBadStatusResponse();
            try
            {
                if (json == "Oops, an error occurred\n")
                {
                    badStatus.Message = json;
                }
                else
                {
                    badStatus = JsonConvert.DeserializeObject<InstaBadStatusResponse>(json);
                }
            }
            catch (Exception ex)
            {
                badStatus.Message = ex.Message;
            }

            return badStatus;
        }
    }
}