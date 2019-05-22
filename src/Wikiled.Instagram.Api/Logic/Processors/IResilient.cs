using Wikiled.Common.Net.Resilience;

namespace Wikiled.Instagram.Api.Logic.Processors
{
    public interface IResilient
    {
        IResilience Resilience { get; }
    }
}
