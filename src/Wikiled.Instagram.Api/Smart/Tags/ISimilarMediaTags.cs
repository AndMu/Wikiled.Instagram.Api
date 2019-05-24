using System.Threading.Tasks;
using Wikiled.Instagram.Api.Smart.Caption;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Tags
{
    public interface ISimilarMediaTags
    {
        Task<HashTagData[]> Get(SmartCaption caption);
    }
}