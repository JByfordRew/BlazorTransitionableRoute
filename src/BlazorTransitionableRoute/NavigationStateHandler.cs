using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Threading.Tasks;

namespace BlazorTransitionableRoute
{
    public class NavigationStateHandler
    {
        private readonly NavigationState navigationState;
        private readonly NavigationManager navigationManager;

        public NavigationStateHandler(
            NavigationState navigationState,
            NavigationManager navigationManager)
        {
            this.navigationState = navigationState;
            this.navigationManager = navigationManager;
            this.navigationManager.LocationChanged += HandleLocationChanged;
        }

        internal void HandleLocationChanged(object sender, LocationChangedEventArgs locationChangedEventArgs)
        {
            navigationState.LocationChanged(locationChangedEventArgs);
        }

        internal async Task Initialise() => await navigationState.Initialise();
    }
}
