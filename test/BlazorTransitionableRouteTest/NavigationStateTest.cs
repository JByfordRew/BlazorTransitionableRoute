using BlazorTransitionableRoute;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Routing;
using System.Threading.Tasks;
using Xunit;

namespace BlazorTransitionableRouteTest
{
    public class NavigationStateTest
    {
        [Fact]
        public void NavigationRoute()
        {
            new NavigationStateFixture()
                .Given.RouteTransitionInvoker()
                .When.NavigatedViaApp("first uri")
                .Then.TransitionWasInvoked(BrowserNavigationDirection.Forward);
        }

        [Fact]
        public void DifferentNavigationRoute()
        {
            new NavigationStateFixture()
                .Given.RouteTransitionInvoker()
                .And.NavigatedViaApp("first uri")
                .When.NavigatedViaApp("subsequent uri")
                .Then.TransitionWasInvoked(BrowserNavigationDirection.Forward);
        }

        [Fact]
        public void SameNavigationRoute()
        {
            new NavigationStateFixture()
                .Given.RouteTransitionInvoker()
                .And.NavigatedViaApp("app uri")
                .When.NavigatedViaApp("app uri")
                .Then.TransitionWasNotInvoked();
        }
    }

    internal class NavigationStateFixture
    {
        private StubRouteTransitionInvoker stubRouteTransitionInvoker;
        private LocationChangedEventArgs locationChangeEventArgs;
        private NavigationState sut;

        internal NavigationStateFixture Given => this;
        internal NavigationStateFixture When => this;
        internal NavigationStateFixture Then => this;
        internal NavigationStateFixture And => this;

        private void LocationChanged()
        {
            this.sut.LocationChanged(this.locationChangeEventArgs).GetAwaiter().GetResult();
        }

        internal NavigationStateFixture NavigatedViaApp(string uri)
        {
            this.stubRouteTransitionInvoker.BrowserNavigationDirection = null;
            this.locationChangeEventArgs = new LocationChangedEventArgs(uri, true);
            LocationChanged();
            return this;
        }

        internal NavigationStateFixture RouteTransitionInvoker()
        {
            this.stubRouteTransitionInvoker = new StubRouteTransitionInvoker();
            this.sut = new NavigationState(this.stubRouteTransitionInvoker, null);
            return this;
        }

        internal NavigationStateFixture TransitionWasInvoked(BrowserNavigationDirection expectNavigationDirection)
        {
            this.stubRouteTransitionInvoker.BrowserNavigationDirection.Should().Be(expectNavigationDirection);
            this.stubRouteTransitionInvoker.BrowserNavigationDirection = null;
            return this;
        }

        internal NavigationStateFixture TransitionWasNotInvoked()
        {
            this.stubRouteTransitionInvoker.BrowserNavigationDirection.Should().BeNull();
            return this;
        }
    }

    internal class StubRouteTransitionInvoker : IRouteTransitionInvoker
    {
        internal BrowserNavigationDirection? BrowserNavigationDirection { get; set; }

        public async Task InvokeRouteTransitionAsync(BrowserNavigationDirection browserNavigationDirection)
        {
            BrowserNavigationDirection = browserNavigationDirection;
            await Task.CompletedTask;
        }
    }
}
