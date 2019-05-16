namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public interface IInstaRecipientsResponse
    {
        long Expires { get; set; }

        bool Filtered { get; set; }

        InstaRankedRecipientResponse[] RankedRecipients { get; set; }

        string RankToken { get; set; }

        string RequestId { get; set; }
    }
}