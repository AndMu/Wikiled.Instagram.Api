using System.Threading.Tasks;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Tags
{
    public interface ISmartTags
    {
        Task<HashTagData[]> Get(HashTagData tag);
    }
}
