using BlazorTransitionableRoute;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteTest
{
    internal class StubJsInterop : IJsInterop
    {
        public async Task Init<T>(DotNetObjectReference<T> instance, bool isActive) where T : class
        {
            await Task.CompletedTask;
        }
    }
}
