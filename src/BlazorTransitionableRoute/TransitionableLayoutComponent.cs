using Microsoft.AspNetCore.Components;

namespace BlazorTransitionableRoute
{
    public class TransitionableLayoutComponent : LayoutComponentBase, ITransitionableLayoutComponent
    {
        [CascadingParameter]
        public Transition Transition{ get; set; }
    }
}
