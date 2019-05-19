using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public interface ICaptionHandler
    {
        SmartCaption Extract(string caption);
    }
}