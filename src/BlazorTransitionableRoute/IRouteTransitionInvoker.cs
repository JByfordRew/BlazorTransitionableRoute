using System.Threading.Tasks;

namespace BlazorTransitionableRoute
{
    public interface IRouteTransitionInvoker
    {
        Task InvokeRouteTransitionAsync(BrowserNavigationDirection browserNavigationDirection);
    }
}
