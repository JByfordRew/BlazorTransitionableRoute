using Microsoft.JSInterop;

namespace BlazorTransitionableRoute
{
    public interface IJsInterop
    {
        Task Init<T>(DotNetObjectReference<T> instance, bool isActive) where T : class;
    }
}