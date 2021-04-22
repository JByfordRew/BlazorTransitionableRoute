using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorTransitionableRoute
{
    public class Transition
    {
        private Transition(RouteData routeData, RouteData switchedRouteData, bool intoView, bool backwards, bool firstRender)
        {
            this.RouteData = routeData;
            this.SwitchedRouteData = switchedRouteData;
            this.IntoView = intoView;
            this.Backwards = backwards;
            this.FirstRender = firstRender;
        }

        public static Transition Create(RouteData routeData, RouteData switchedRouteData, bool intoView, bool backwards, bool firstRender)
            => new Transition(routeData, switchedRouteData, intoView, backwards, firstRender);

        public RouteData RouteData { get; }
        public RouteData SwitchedRouteData { get; }
        public bool IntoView { get; }
        public bool Backwards { get; }
        public bool FirstRender { get; }
    }
}
