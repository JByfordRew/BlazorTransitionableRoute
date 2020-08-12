using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BlazorTransitionableRouteTest")]

namespace BlazorTransitionableRoute
{
    public class NavigationState
    {
        internal string lastLocation;
        private readonly IRouteTransitionInvoker routeTransitionInvoker;
        private readonly IJSRuntime jsRuntime;

        public NavigationState(
            IRouteTransitionInvoker routeTransitionInvoker,
            IJSRuntime jsRuntime)
        {
            this.routeTransitionInvoker = routeTransitionInvoker;
            this.jsRuntime = jsRuntime;
        }

        internal async Task Initialise() =>
            await jsRuntime.InvokeVoidAsync("window.blazorTransitionableRoute.init", DotNetObjectReference.Create(this));

        /// <summary>
        ///     Navigation of internal app links are always fowards
        ///     If back buttons implemented they should call browser back i.e. window.history.back();
        ///     If browser navigation was performed route views will not be in a state to transition forwards
        ///     therefore navigating forwards will have no effect 
        ///     and the browser navigation back and forwards will fire later after render and transition correctly.
        /// </summary>
        /// <param name="locationChangedEventArgs"></param>
        /// <returns></returns>
        internal async Task LocationChanged(LocationChangedEventArgs locationChangedEventArgs)
        {
            await Navigate(isForwards: true, locationChangedEventArgs.Location);
        }

        [JSInvokable]
        public async Task Navigate(bool isForwards, string location)
        {
            if (!IsSameAtPrevious(location))
            {
                await AllowRenderingToComplete();
                lastLocation = location;
                var direction = isForwards ? BrowserNavigationDirection.Forward : BrowserNavigationDirection.Backward;
                await routeTransitionInvoker.InvokeRouteTransitionAsync(direction);
            }
        }

        private bool IsSameAtPrevious(string location)
            => lastLocation != null && lastLocation.Equals(location);

        private static async Task AllowRenderingToComplete()
        {
            await Task.Yield();
        }
    }
}
