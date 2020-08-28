using Microsoft.AspNetCore.Components;

namespace BlazorTransitionableRoute
{
    /// <summary>
    /// <para>
    /// Optionally implement this interface to get access
    /// to the <see cref="Transition"/> object so you can adjust
    /// markup to invoke transition behaviour.
    /// </para>
    /// <para>
    /// Alternatively inherit the <see cref="TransitionableLayoutComponent"/>.
    /// </para>
    /// </summary>
    public interface ITransitionableLayoutComponent
    {
        /// <summary>
        /// Contains information on the transition behaviour adjust the view.
        /// </summary>
        [CascadingParameter]
        Transition Transition { get; set; }
    }
}
