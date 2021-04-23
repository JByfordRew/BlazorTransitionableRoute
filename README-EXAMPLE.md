# Demo Documentation

### A change to routing approach

Modify the App.razor file to take advantage of the transitionable route layouts and view.  This means moving the `MainLayout` to be more explicit in the app router and providing a more container like `MyViewLayout` as the default layouts. You can see below the simple use of primary and secondary route views. The `TransitionableRoutePrimary / Secondary` modify the `RouteData` passed to each inner `TransitionableRouteView` based on the active state, which is swapped after each navigation to preserve component instances.
```html
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <LayoutView Layout="@typeof(MainLayout)">
            <TransitionableRoutePrimary RouteData="@routeData" ForgetStateOnTransition="false">
                <TransitionableRouteView DefaultLayout="@typeof(MyViewLayout)" />
            </TransitionableRoutePrimary>
            <TransitionableRouteSecondary RouteData="@routeData" ForgetStateOnTransition="false">
                <TransitionableRouteView DefaultLayout="@typeof(MyViewLayout)" />
            </TransitionableRouteSecondary>
        </LayoutView>
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>
```

### Example usage

You will need to create your own transitiong view, for example (`Transition` parameter is provided by the inherited `TransitionableLayoutComponent`)
```html
@inherits TransitionableLayoutComponent

<div class="@transitioningClass">
    @Body
</div>

@code {
    private string transitioningDirection => Transition.Backwards ? "Up" : "Down";

    private string transitioningClass => Transition.FirstRender ? "" : Transition.IntoView
        ? $"animate__fadeIn{transitioningDirection} animate__faster animate__animated"
         : $"animate__fadeOut{transitioningDirection} animate__faster animate__animated";
}
```
(alternatively you can use the default one provided by the component called `TransitionableLayoutComponent` but you will need to handle the `Transition' cascading parameter and probably wrap each page in it's own containing component.  You are free to implement how you like but the cascading parameter is your starting point to prepare for transitioning.)

### Optional example JavaScript Interop usage
You can optionally create an implementation of `IRouteTransitionInvoker` and save it where you like, perhaps in `Shared` folder and make sure it is registered with DI. 
```C#
using BlazorTransitionableRoute;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteDemoWasm.Client.Shared
{
    public class RouteTransitionInvoker : IRouteTransitionInvoker
    {
        private readonly IJSRuntime jsRuntime;

        public RouteTransitionInvoker(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task InvokeRouteTransitionAsync(bool backwards)
        {
            await jsRuntime.InvokeVoidAsync("window.yourJsInterop.transitionFunction", backwards);
        }
    }
}
```

For client-side and server-side Blazor - add script section to index.html or _Host.cshtml (head section), for example
```html
<script src="yourJsInteropForAnimatedTransitions.js"></script>
... any other supporting animation library scripts you are using, for example using animate.css
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css" />
```

Add your js interop implementation, for example (this is from the demos using animate.css)
```Javascript
window.yourJsInterop = {
    transitionFunction: function (back) {

        let transitionIn = document.getElementsByClassName('transition-in')[0];
        let transitionOut = document.getElementsByClassName('transition-out')[0];

        let direction = back ? "Up" : "Down";

        if (transitionIn && transitionOut) {

            transitionOut.classList.remove('transition-out');
            transitionOut.classList.add(
                "animate__fadeOut" + direction,
                "animate__faster",
                "animate__animated"
            );

            transitionIn.classList.remove('transition-in');
            transitionIn.classList.add(
                "animate__fadeIn" + direction,
                "animate__faster",
                "animate__animated"
            );
        }
    }
}
```
If you experience timing issues when transition animations are not performing you may need to wrap you inner function code in a zero timeout, for example
```Javascript
window.yourJsInterop = {
    transitionFunction: function (back) {
        setTimout(() => {
           ...
        }, 0);
    }
}
```
