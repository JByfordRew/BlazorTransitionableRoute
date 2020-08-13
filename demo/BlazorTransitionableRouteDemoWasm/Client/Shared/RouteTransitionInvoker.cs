using BlazorTransitionableRoute;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteDemoWasm.Client.Shared
{
    public class RouteTransitionInvoker : IRouteTransitionInvoker
    {
        private readonly IJSRuntime jsRuntime;

        public RouteTransitionInvoker(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task InvokeRouteTransitionAsync(BrowserNavigationDirection navigationDirection)
        {
            var isNavigatingBack = navigationDirection == BrowserNavigationDirection.Backward;
            await jsRuntime.InvokeVoidAsync("window.yourJsInterop.transitionFunction", isNavigatingBack);
        }
    }
}
