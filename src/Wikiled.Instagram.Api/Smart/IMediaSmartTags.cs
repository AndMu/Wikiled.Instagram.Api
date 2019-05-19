using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public interface IMediaSmartTags
    {
        Task<HashTagData[]> Get(SectionMedia media);
    }
}
