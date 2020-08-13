# BlazorTransitionableRoute
Allows current and previous route to exist enabling transition animations of UI/UX design systems.

## What it does?
Sometimes you need to show transitions between route but Blazor by default only allows one.  This provides an ability to remember the last route and allow you to perform transitions out and in on the old and new route views rendered.

![Demo](demo/BlazorTransitionableRouteDemo.gif)

## How it works?
Having two view layouts means we can remember the previous route but we cannot simply overwrite them, they must be preserved so we do not lose browser state like scroll position etc. This solution handles the route views and knows when to call the transition implementation you provide to it.  It also works with the browser navigation buttons.
If you need to had in app back buttons then use the jsInterop to call the native back i.e. `window.history.back();`
This is as simple a solution I could arrive at.  If there is another simpler one please let me know.

See the demos for how to implement.

## Installation

Latest version in here:  [![NuGet](https://img.shields.io/nuget/v/BlazorTransitionableRoute.svg)](https://www.nuget.org/packages/BlazorTransitionableRoute/)

```
Install-Package BlazorTransitionableRoute
```
or 
```
dotnet add package BlazorTransitionableRoute
```

## Usage

### Common component steps

For client-side and server-side Blazor - add script section to index.html or _Host.cshtml (head section) 

```html
<script src="_content/BlazorTransitionalRoute/jsInterop.js"></script>
```

Add reference to _Imports.razor file
```C#
@using BlazorTransitionableRoute
```

For client-side and server-side Blazor - add registrations for the dependency injection framework in `Program.cs` or `Startup.cs`, for example
```C#
builder.Services.AddScoped<BlazorTransitionableRoute.NavigationState>();
builder.Services.AddScoped<BlazorTransitionableRoute.NavigationStateHandler>();
builder.Services.AddScoped<BlazorTransitionableRoute.IRouteTransitionInvoker, RouteTransitionInvoker>();
```

### A change to routing approach

Modify the App.razor file to take advantage of the transitionable route layouts and view.  This means moving the `MainLayout` to be more explicit in the app router and providing a more container like `MyViewLayout` as the default layouts. You can see below the simple use of primary and secondary route views. The `TransitionableRoutePrimary / Secondary` modify the `RouteData` passed to each inner `TransitionableRouteView` based on the active state, which is swapped after each navigation to preserve component instances.
```html
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <LayoutView Layout="@typeof(MainLayout)">
            <TransitionableRoutePrimary RouteData="@routeData">
                <TransitionableRouteView DefaultLayout="@typeof(MyViewLayout)" />
            </TransitionableRoutePrimary>
            <TransitionableRouteSecondary RouteData="@routeData">
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

You will need to create your own transitiong view, for example (`TransitioningIn` is provided by the inherited `TransitionableLayoutComponent`)
```html
@inherits TransitionableLayoutComponent

<div class="transition @transitioningClass">
    @Body
</div>

<style>
    .transition {
        position: absolute;
    }

    .transition-in {
        opacity: 0;
    }
</style>

@code {
    private string transitioningClass => TransitioningIn ? "transition-in" : "transition-out";
}
```
(alternatively you can use the default one provided by the component called `TransitionableLayoutComponent` but you will need to handle the `TransitioningIn' cascading parameter and probably wrap each page in it's own containing component.  You are free to implement how you like but the cascading parameter is your starting point to prepare for transitioning.)

You will need to create an implementation of `IRouteTransitionInvoker` and save it where you like, perhaps in `Shared` folder. 
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

        public async Task InvokeRouteTransitionAsync(BrowserNavigationDirection navigationDirection)
        {
            var isNavigatingBack = navigationDirection == BrowserNavigationDirection.Backward;
            await jsRuntime.InvokeVoidAsync("window.yourJsInterop.transitionFunction", isNavigatingBack);
        }
    }
}
```

For client-side and server-side Blazor - add script section to index.html or _Host.cshtml (head section), for example
```html
<script src="yourJsInteropForAnimatedTransitions.js"></script>
... any other supporting animation library scripts you are using
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
