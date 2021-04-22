using BlazorTransitionableRoute;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteTest
{
    internal class StubRouteTransitionInvoker : IRouteTransitionInvoker
    {
        internal bool invoked;
        internal Transition transition;

        internal void Reset()
        {
            invoked = false;
            transition = null;
        }

        public Task InvokeRouteTransitionAsync(Transition transition)
        {
            this.invoked = true;
            this.transition = transition;
            return Task.CompletedTask;
        }
    }
}
