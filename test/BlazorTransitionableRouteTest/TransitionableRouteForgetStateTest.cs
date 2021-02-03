using Xunit;

namespace BlazorTransitionableRouteTest
{
    public class TransitionableRouteForgetStateTest
    {
        [Fact]
        public void InitialRouteData()
        {
            var routeData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.OptionToForgetStateOnTransition()
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
                .And.OptionToForgetStateOnTransition()
                .And.UpdateRoute(firstRouteData)
                .When.UpdateRoute(secondRouteData)
                .Then.PrimaryRouteDataIs(null)
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
                .And.OptionToForgetStateOnTransition()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .When.UpdateRoute(thirdRouteData)
                .Then.PrimaryRouteDataIs(thirdRouteData)
                .And.SecondaryRouteDataIs(null)
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
                .And.OptionToForgetStateOnTransition()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.NavigateBack()
                .And.UpdateRoute(firstRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(null)
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
                .And.OptionToForgetStateOnTransition()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.UpdateRoute(thirdRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.UpdateRoute(firstRouteData)
                .Then.PrimaryRouteDataIs(firstRouteData)
                .And.SecondaryRouteDataIs(null)
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
                .And.OptionToForgetStateOnTransition()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.UpdateRoute(thirdRouteData)
                .And.UpdateRoute(secondRouteData)
                .When.UpdateRoute(thirdRouteData)
                .Then.PrimaryRouteDataIs(thirdRouteData)
                .And.SecondaryRouteDataIs(null)
                .And.PrimaryRouteHasTransition();
        }

        [Fact]
        public void InitialRouteDataSubsequentRouteIsSameAsInitial()
        {
            var firstRouteData = TransitionableRouteFixture.GenerateRouteData(typeof(StubType1));

            new TransitionableRouteFixture()
                .Given.PrimaryTransitionableRoute()
                .And.SecondaryTransitionableRoute()
                .And.OptionToForgetStateOnTransition()
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
                .And.OptionToForgetStateOnTransition()
                .And.UpdateRoute(firstRouteData)
                .And.UpdateRoute(secondRouteData)
                .And.RouteHasNotChanged()
                .When.UpdateRoute(secondRouteData)
                .Then.PrimaryRouteDataIs(null)
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
                .And.OptionToForgetStateOnTransition()
                .And.UpdateRoute(routeDataVanilla)
                .When.UpdateRoute(routeDataCustom)
                .Then.PrimaryRouteDataIs(null)
                .And.SecondaryRouteDataIs(routeDataCustom)
                .And.SecondaryRouteHasTransition();
        }
    }
}
