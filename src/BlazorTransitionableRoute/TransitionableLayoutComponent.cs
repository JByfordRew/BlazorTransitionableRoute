using Microsoft.AspNetCore.Components;
using System;

namespace BlazorTransitionableRoute
{
    public class TransitionableLayoutComponent : LayoutComponentBase, ITransitionableLayoutComponent
    {
        [CascadingParameter]
        public Transition Transition{ get; set; }

        protected (Type fromType, Type toType) TransitionType =>
            (Transition.IntoView ? Transition.SwitchedRouteData.PageType : Transition.RouteData.PageType,
             Transition.IntoView ? Transition.RouteData.PageType : Transition.SwitchedRouteData.PageType);
    }
}
