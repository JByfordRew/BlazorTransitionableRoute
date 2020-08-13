using BlazorTransitionableRoute;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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
                .And.SecondaryViewIsNotRendered()
                .And.NoRouteHasTransition();
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
                .When.UpdateRoute(firstRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(null)
                .And.NoRouteHasTransition()
                ;
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
                .When.UpdateRoute(secondRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.NoRouteHasTransition();
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
        private TransitionableRoutePrimary primaryTransitionableRoute;
        private TransitionableRouteSecondary secondaryTransitionableRoute;
        private RouteData routeData;

        internal TransitionableRouteFixture Given => this;
        internal TransitionableRouteFixture When => this;
        internal TransitionableRouteFixture Then => this;
        internal TransitionableRouteFixture And => this;

        internal TransitionableRouteFixture PrimaryTransitionableRoute()
        {
            this.primaryTransitionableRoute = new TransitionableRoutePrimary();
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
            return this;
        }

        internal TransitionableRouteFixture UpdateRoute(RouteData routeData)
        {
            this.routeData = routeData;
            this.primaryTransitionableRoute.RouteData = this.primaryTransitionableRoute.ViewRoutData(this.routeData);
            this.secondaryTransitionableRoute.RouteData = this.secondaryTransitionableRoute.ViewRoutData(this.routeData);
            return this;
        }

        internal TransitionableRouteFixture PrimaryRouteDataIs(RouteData routeData)
        {
            this.primaryTransitionableRoute.RouteData.Should().BeEquivalentTo(routeData);
            return this;
        }

        internal TransitionableRouteFixture SecondaryRouteDataIs(RouteData routeData)
        {
            this.secondaryTransitionableRoute.RouteData.Should().BeEquivalentTo(routeData);
            return this;
        }

        internal TransitionableRouteFixture SecondaryViewIsNotRendered()
        {
            this.secondaryTransitionableRoute.CanRender.Should().BeFalse();
            return this;
        }

        internal TransitionableRouteFixture PrimaryRouteHasTransition()
        {
            this.primaryTransitionableRoute.TransitioningIn.Should().BeTrue();
            this.secondaryTransitionableRoute.TransitioningIn.Should().BeFalse();
            return this;
        }

        internal TransitionableRouteFixture NoRouteHasTransition()
        {
            this.primaryTransitionableRoute.TransitioningIn.Should().BeFalse();
            this.secondaryTransitionableRoute.TransitioningIn.Should().BeFalse();
            return this;
        }



        internal TransitionableRouteFixture SecondaryRouteHasTransition()
        {
            this.primaryTransitionableRoute.TransitioningIn.Should().BeFalse();
            this.secondaryTransitionableRoute.TransitioningIn.Should().BeTrue();
            return this;
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
}
