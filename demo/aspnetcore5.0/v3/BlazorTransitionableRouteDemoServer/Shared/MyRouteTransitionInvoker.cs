using BlazorTransitionableRoute;
using BlazorTransitionableRouteDemoServer.Pages;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
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

        public async Task InvokeRouteTransitionAsync(Transition transition)
        {
            var effectOut = transition.Backwards ? "fadeOutUp" : "fadeOutDown";
            var effectIn = transition.Backwards ? "fadeInUp" : "fadeInDown";

            if (customTransitions.TryGetValue((transition.SwitchedRouteData.PageType, transition.RouteData.PageType), out var custom))
            {
                effectOut = custom.effectOut;
                effectIn = custom.effectIn;
            }

            await jsRuntime.InvokeVoidAsync("window.yourJsInterop.transitionFunction", effectOut, effectIn);
        }

        private Dictionary<(Type from, Type to), (string effectOut, string effectIn)> customTransitions =
            new Dictionary<(Type from, Type to), (string effectOut, string effectIn)>
            {
                { (typeof(FetchData), typeof(WeatherDetail)), ( "fadeOutLeft", "fadeInRight" ) },
                { (typeof(WeatherDetail), typeof(FetchData)), ( "fadeOutRight", "fadeInLeft" ) }
            };
    }
}
