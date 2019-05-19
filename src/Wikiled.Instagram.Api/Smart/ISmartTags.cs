using System.Threading.Tasks;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public interface ISmartTags
    {
        Task<HashTagData[]> Get(HashTagData tag);
    }
}
