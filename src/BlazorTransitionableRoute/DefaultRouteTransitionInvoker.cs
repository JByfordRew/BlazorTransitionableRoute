using System.Threading.Tasks;

namespace BlazorTransitionableRoute
{
    /// <summary>
    /// <para>
    /// Use this when you handle transition behaviour 
    /// using Blazor code and not via JavaScript interop.
    /// </para>
    /// <para>
    /// Register with dependency injection for example via
    /// Microsoft.Extensions.DependencyInjection.IServiceCollection
    /// as AddSingleton with this default implementation.
    /// e.g. builder.Services.AddScoped<IRouteTransitionInvoker, DefaultRouteTransitionInvoker>();
    /// </para>
    /// </summary>
    public class DefaultRouteTransitionInvoker : IRouteTransitionInvoker
    {
        public Task InvokeRouteTransitionAsync(Transition transition)
        {
            return Task.CompletedTask;
        }
    }
}
