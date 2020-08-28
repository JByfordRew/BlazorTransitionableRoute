using BlazorTransitionableRoute;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteDemoWasm.Client.Shared
{
    public class MyRouteTransitionInvoker : IRouteTransitionInvoker
    {
        private readonly IJSRuntime jsRuntime;

        public MyRouteTransitionInvoker(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task InvokeRouteTransitionAsync(bool backwards) 
            => await jsRuntime.InvokeVoidAsync("window.yourJsInterop.transitionFunction", backwards);
    }
}
