using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorTransitionableRoute
{
    public class Transition
    {
        private Transition(RouteData routeData, bool intoView, bool backwards, bool firstRender)
        {
            RouteData = routeData;
            this.IntoView = intoView;
            this.Backwards = backwards;
            this.FirstRender = firstRender;
        }

        public static Transition Create(RouteData routeData, bool intoView, bool backwards, bool firstRender)
            => new Transition(routeData, intoView, backwards, firstRender);

        public RouteData RouteData { get; }
        public bool IntoView { get; }
        public bool Backwards { get; }
        public bool FirstRender { get; }
    }
}
