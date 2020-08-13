using Microsoft.AspNetCore.Components;

namespace BlazorTransitionableRoute
{
    public interface ITransitionableLayoutComponent
    {
        [CascadingParameter]
        bool TransitioningIn { get; set; }
    }
}
