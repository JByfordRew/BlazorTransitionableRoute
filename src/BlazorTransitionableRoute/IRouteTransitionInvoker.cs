using System.Threading.Tasks;

namespace BlazorTransitionableRoute
{
    /// <summary>
    /// <para>
    /// Provides the mechanism to implement the transition behaviour
    /// when a route transition occurs. 
    /// </para>
    /// <para>
    /// An implementation commonly implements a JavaScript interop
    /// but could also use Blazor code.
    /// </para>
    /// <para>
    /// Note: if not using interop, consider
    /// using the <see cref="DefaultRouteTransitionInvoker"/> and implement
    /// the Blazor code in you view or route container component.
    /// </para>
    /// <para>
    /// It is important to inherit the <see cref="TransitionableLayoutComponent"/>
    /// or implement the <see cref="ITransitionableLayoutComponent"/> for your
    /// route container component.  This is to access the <see cref="Transition"/> 
    /// that is provided via CascadingParameter which will allow your markup to changed
    /// for transition css classes or styles etc.
    /// </para>
    /// </summary>
    public interface IRouteTransitionInvoker
    {
        Task InvokeRouteTransitionAsync(bool backwards);
    }
}
