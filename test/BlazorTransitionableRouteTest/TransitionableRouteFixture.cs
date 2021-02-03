using BlazorTransitionableRoute;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;

namespace BlazorTransitionableRouteTest
{
    internal class TransitionableRouteFixture
    {
        private bool doNotInvokeStateChangedDuringTestBool = false;
        private IJSRuntime jsRuntime = new StubJsRuntime();

        private StubRouteTransitionInvoker transitionInvoker1 = new StubRouteTransitionInvoker();
        private StubRouteTransitionInvoker transitionInvoker2 = new StubRouteTransitionInvoker();

        private TransitionableRoutePrimary primaryTransitionableRoute;
        private TransitionableRouteSecondary secondaryTransitionableRoute;
        private RouteData routeData;
        private bool isBackwards = false;
        private bool isRouteChanged = true;
        private bool isFirstRender = true;

        internal TransitionableRouteFixture Given => this;
        internal TransitionableRouteFixture When => this;
        internal TransitionableRouteFixture Then => this;
        internal TransitionableRouteFixture And => this;

        internal TransitionableRouteFixture PrimaryTransitionableRoute()
        {
            this.primaryTransitionableRoute = new TransitionableRoutePrimary();
            this.primaryTransitionableRoute.TransitionInvoker = this.transitionInvoker1;
            this.primaryTransitionableRoute.JSRuntime = jsRuntime;
            this.primaryTransitionableRoute.invokesStateChanged = doNotInvokeStateChangedDuringTestBool;
            return this;
        }

        internal static RouteData GenerateRouteData(Type type)
            => new RouteData(typeof(StubType1),
               new Dictionary<string, object> {
                   { "type", type.ToString() },
                   { "data", Guid.NewGuid() }
               });

        internal TransitionableRouteFixture SecondaryTransitionableRoute()
        {
            this.secondaryTransitionableRoute = new TransitionableRouteSecondary();
            this.secondaryTransitionableRoute.MakeSecondary();
            this.secondaryTransitionableRoute.TransitionInvoker = this.transitionInvoker2;
            this.secondaryTransitionableRoute.JSRuntime = jsRuntime;
            this.secondaryTransitionableRoute.invokesStateChanged = doNotInvokeStateChangedDuringTestBool;
            return this;
        }

        internal TransitionableRouteFixture OptionToForgetStateOnTransition()
        {
            this.primaryTransitionableRoute.ForgetStateOnTransition = true;
            this.secondaryTransitionableRoute.ForgetStateOnTransition = true;
            return this;
        }

        internal TransitionableRouteFixture NavigateBack()
        {
            this.isBackwards = true;
            return this;
        }

        internal TransitionableRouteFixture RouteHasNotChanged()
        {
            this.isRouteChanged = false;
            return this;
        }

        internal TransitionableRouteFixture UpdateRoute(RouteData routeData)
        {
            this.routeData = routeData;

            this.primaryTransitionableRoute.RouteData = this.routeData;
            this.secondaryTransitionableRoute.RouteData = this.routeData;

            if (isFirstRender)
            {
                this.primaryTransitionableRoute.HandleFirstRender().GetAwaiter().GetResult();
                this.secondaryTransitionableRoute.HandleFirstRender().GetAwaiter().GetResult();
            }
            else if (isRouteChanged)
            {
                this.primaryTransitionableRoute.Navigate(isBackwards, isFirstRender).GetAwaiter().GetResult();
                this.secondaryTransitionableRoute.Navigate(isBackwards, isFirstRender).GetAwaiter().GetResult();
            }

            isFirstRender = false;
            isRouteChanged = true;

            return this;
        }

        internal TransitionableRouteFixture PrimaryRouteDataIs(RouteData routeData)
        {
            this.primaryTransitionableRoute.Transition.RouteData
                .Should().BeEquivalentTo(routeData);
            return this;
        }

        internal TransitionableRouteFixture SecondaryRouteDataIs(RouteData routeData)
        {
            this.secondaryTransitionableRoute.Transition.RouteData
                .Should().BeEquivalentTo(routeData);
            return this;
        }

        internal TransitionableRouteFixture SecondaryViewIsNotRendered()
        {
            this.secondaryTransitionableRoute.Transition.RouteData
                .Should().BeNull();
            return this;
        }

        internal TransitionableRouteFixture PrimaryRouteHasTransition()
        {
            this.primaryTransitionableRoute.Transition.IntoView.Should().BeTrue();
            this.primaryTransitionableRoute.Transition.Backwards.Should().Be(isBackwards);
            CheckInvokedTransition(this.transitionInvoker1);

            this.secondaryTransitionableRoute.Transition.IntoView.Should().BeFalse();
            this.secondaryTransitionableRoute.Transition.Backwards.Should().Be(isBackwards);
            CheckInvokedTransition(this.transitionInvoker2);

            this.isBackwards = false;

            return this;
        }

        internal TransitionableRouteFixture SecondaryRouteHasTransition()
        {
            this.primaryTransitionableRoute.Transition.IntoView.Should().BeFalse();
            this.primaryTransitionableRoute.Transition.Backwards.Should().Be(isBackwards);
            CheckInvokedTransition(this.transitionInvoker1);

            this.secondaryTransitionableRoute.Transition.IntoView.Should().BeTrue();
            this.secondaryTransitionableRoute.Transition.Backwards.Should().Be(isBackwards);
            CheckInvokedTransition(this.transitionInvoker2);

            return this;
        }

        private void CheckInvokedTransition(StubRouteTransitionInvoker stubRouteTransitionInvoker)
        {
            stubRouteTransitionInvoker.invoked.Should().BeTrue();
            stubRouteTransitionInvoker.backwards.Should().Be(this.isBackwards);
            stubRouteTransitionInvoker.Reset();
        }
    }
}
