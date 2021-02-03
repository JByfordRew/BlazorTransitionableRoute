using BlazorTransitionableRoute;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteTest
{
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
