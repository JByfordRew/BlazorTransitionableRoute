using Microsoft.AspNetCore.Components;

namespace BlazorTransitionableRoute
{
    public partial class TransitionableRoutePrimary : ComponentBase
    {
        private bool isActiveRoute = true;
        private bool isFirstRender = true;
        private RouteData lastUsedRouteData;
        private RouteData lastRouteData;

        public RouteData ViewRoutData(RouteData routeData)
        {
            TransitioningIn = false;

            if (IsSameAsPrevious(routeData))
            {
                if (isActiveRoute)
                {
                    routeData = lastUsedRouteData;
                }
            }
            else
            {
                lastRouteData = routeData;

                if (isActiveRoute)
                {
                    lastUsedRouteData = routeData;
                    TransitioningIn = !isFirstRender && true;
                }
                else
                {
                    routeData = lastUsedRouteData;
                }

                isActiveRoute = !isActiveRoute;
            }

            isFirstRender = false;

            return routeData;
        }

        private bool IsSameAsPrevious(RouteData routeData)
            => lastRouteData != default
            && lastRouteData.PageType.Equals(routeData.PageType)
            && lastRouteData.RouteValues.Equals(routeData.RouteValues);

        internal void MakeSecondary() => isActiveRoute = false;
    }
}
