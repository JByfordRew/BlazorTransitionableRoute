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
                .And.SwitchedPrimaryRouteDataIs(routeData)
                .And.SecondaryRouteDataIs(null)
                .And.SwitchedSecondaryRouteDataIs(routeData)
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
                .And.SwitchedPrimaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SwitchedSecondaryRouteDataIs(firstRouteData)
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
                .And.SwitchedPrimaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SwitchedSecondaryRouteDataIs(thirdRouteData)
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
                .And.SwitchedPrimaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SwitchedSecondaryRouteDataIs(firstRouteData)
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
                .And.SwitchedPrimaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SwitchedSecondaryRouteDataIs(firstRouteData)
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
                .And.SwitchedPrimaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SwitchedSecondaryRouteDataIs(thirdRouteData)
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
                .And.SwitchedPrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(null)
                .And.SwitchedSecondaryRouteDataIs(firstRouteData)
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
                .And.SwitchedPrimaryRouteDataIs(secondRouteData)
                .And.SecondaryRouteDataIs(secondRouteData)
                .And.SwitchedSecondaryRouteDataIs(firstRouteData)
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
                .And.SwitchedPrimaryRouteDataIs(routeDataCustom)
                .And.SecondaryRouteDataIs(routeDataCustom)
                .And.SwitchedSecondaryRouteDataIs(routeDataVanilla)
                .And.SecondaryRouteHasTransition();
        }
    }
}
