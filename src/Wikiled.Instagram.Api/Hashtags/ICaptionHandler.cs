using Wikiled.Instagram.Api.Hashtags.Data;

namespace Wikiled.Instagram.Api.Hashtags
{
    public interface ICaptionHandler
    {
        SmartCaption Extract(string caption);
    }
}