namespace Wikiled.Instagram.Api.Converters
{
    internal interface IObjectConverter<out T, TT>
    {
        TT SourceObject { get; set; }

        T Convert();
    }
}