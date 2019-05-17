namespace Wikiled.Instagram.Api.Enums
{
    public enum LoginResult
    {
        Success,

        BadPassword,

        InvalidUser,

        TwoFactorRequired,

        Exception,

        ChallengeRequired,

        LimitError,

        InactiveUser,

        CheckpointLoggedOut
    }
}