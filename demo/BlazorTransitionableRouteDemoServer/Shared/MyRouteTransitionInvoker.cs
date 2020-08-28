using BlazorTransitionableRoute;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteDemoServer.Shared
{
    public class MyRouteTransitionInvoker : IRouteTransitionInvoker
    {
        private readonly IJSRuntime jsRuntime;

        public MyRouteTransitionInvoker(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task InvokeRouteTransitionAsync(bool backwards)
        {
            await jsRuntime.InvokeVoidAsync("window.yourJsInterop.transitionFunction", backwards);
        }
    }
}
