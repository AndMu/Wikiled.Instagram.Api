namespace Wikiled.Instagram.Api.Classes
{
    /// <summary>
    ///     IResult - common return type for library public methods, can contain some additional info like: Exception details,
    ///     Instagram response type etc.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    public interface IResult<out T>
    {
        InstaResultInfo Info { get; }

        bool Succeeded { get; }

        T Value { get; }
    }
}