using BlazorTransitionableRoute;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlazorTransitionableRouteTest
{
    public class TransitionableRouteTest
    {
        [Fact]
        public void InitialRouteData()
        {
            var routeData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .When.UpdateRoute(routeData)
                .Then.PrimaryRouteDataIs(routeData)
                .And.SecondaryRouteDataIs(null)
                .And.PrimaryRouteHasTransition()
                .And.SecondaryViewIsNotRendered();
        }

        [Fact]
        public void SecondRouteData()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));
            var secondRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType2));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(firstRouteData)
                .When.UpdateRoute(secondRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteHasTransition();
        }

        [Fact]
        public void ThirdRouteData()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));
            var secondRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType2));
            var thirdRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType3));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .When.UpdateRoute(thirdRouteData)
                .Then.PrimaryRouteDataIs(thirdRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.PrimaryRouteHasTransition();
        }

        [Fact]
        public void MoveBackToPreviousRoute()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));
            var secondRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType2));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.NavigateBack()
                .And.UpdateRoute(firstRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.PrimaryRouteHasTransition();
        }

        [Fact]
        public void MoveBackTwiceToPreviousRoute()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));
            var secondRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType2));
            var thirdRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType3));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.UpdateRoute(thirdRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.UpdateRoute(firstRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.PrimaryRouteHasTransition();
        }

        [Fact]
        public void MoveBackThenForwardOverPreviousRoutes()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));
            var secondRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType2));
            var thirdRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType3));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.UpdateRoute(thirdRouteData)
                .And.UpdateRoute(secondRouteData)
                .When.UpdateRoute(thirdRouteData)
                .Then.PrimaryRouteDataIs(thirdRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.PrimaryRouteHasTransition();
        }

        [Fact]
        public void InitialRouteDataSubsequentRouteIsSameAsInitial()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(firstRouteData)
                .And.RouteHasNotChanged()
                .When.UpdateRoute(firstRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(null)
                .And.PrimaryRouteHasTransition()
                .And.SecondaryViewIsNotRendered();
        }

        [Fact]
        public void RouteDataSubsequentRouteIsSame()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));
            var secondRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType2));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.RouteHasNotChanged()
                .When.UpdateRoute(secondRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteHasTransition();
        }

        [Fact]
        public void SameRouteWithDifferentRouteValues()
        {
            var routeDataVanilla = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));
            var routeDataCustom = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.UpdateRoute(routeDataVanilla)
                .When.UpdateRoute(routeDataCustom)
                .Then.PrimaryRouteDataIs(routeDataVanilla)
                .And.SecondaryRouteDataIs(routeDataCustom)
                .And.SecondaryRouteHasTransition();
        }
    }

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

    internal class StubType1 : IComponent
    {
        public void Attach(RenderHandle renderHandle)
        {
            throw new NotImplementedException();
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubType2 : StubType1 { }
    internal class StubType3 : StubType2 { }

    internal class StubJsRuntime : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            return default;
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubRouteTransitionInvoker : IRouteTransitionInvoker
    {
        internal bool invoked;
        internal bool backwards;
        internal void Reset()
        {
            invoked = false;
        }

        public Task InvokeRouteTransitionAsync(bool backwards)
        {
            this.invoked = true;
            this.backwards = backwards;
            return Task.CompletedTask;
        }
    }
}
