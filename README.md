# BlazorTransitionableRoute
Allows current and previous route to exist enabling page transition animations.

## What it does?
Sometimes you need to show transitions between page routes but Blazor, by default, only allows one.  This Razor component library provides the ability to remember the last route and allows you to perform transitions out and in, on the old and new route views.  You can also use this information to contextually perform different transitions based on the route page types being transitioned.

![Demo](demo/BlazorTransitionableRouteDemo.gif)

## Feature Summary
* .NET 6 (and prior .net standard 2.1 using v3.1.0 or before)
* Two transition implementation options; Blazor coded or jsInterop
* Transition data provided allowing different transitions based on context
* Optionally remember previous page state
* Works with browser navigation history, forward and back 
* Very simple to use, only a couple of file changes required

---

## Installation

Latest version in here:  [![NuGet](https://img.shields.io/nuget/v/BlazorTransitionableRoute.svg)](https://www.nuget.org/packages/BlazorTransitionableRoute/)

```
Install-Package BlazorTransitionableRoute
```
or 
```
dotnet add package BlazorTransitionableRoute
```

---

## Usage

### Common component steps

Add reference to _Imports.razor file
```C#
@using BlazorTransitionableRoute
```

Add registrations for the dependency injection framework in `Program.cs`, for example
```C#
builder.Services.AddScoped<BlazorTransitionableRoute.IRouteTransitionInvoker, MyRouteTransitionInvoker>();
```
*Or `BlazorTransitionableRoute.DefaultRouteTransitionInvoker` if you implement transitions via Blazor code and not via jsInterop.*

### A change to routing approach
You will need to change `App.razor` to use the newly provided `TransitionableRoutePrimary/Secondary` components

<details>
<summary>Modify the App.razor file</summary>

Modify the App.razor file to take advantage of the transitionable route layouts and view.  This means moving the `MainLayout` to be more explicit in the app router and providing a more container like `MyViewLayout` as the default layouts. You can see below the simple use of primary and secondary route views. The `TransitionableRoutePrimary / Secondary` modify the `RouteData` passed to each inner `TransitionableRouteView` based on the active state, which is swapped after each navigation to preserve component instances.

```html
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <LayoutView Layout="@typeof(MainLayout)">
            <TransitionableRoutePrimary RouteData="@routeData" ForgetStateOnTransition="true">
                <TransitionableRouteView DefaultLayout="@typeof(MyViewLayout)" />
            </TransitionableRoutePrimary>
            <TransitionableRouteSecondary RouteData="@routeData" ForgetStateOnTransition="true">
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
</details>

<details>
<summary>Create your own transitiong view</summary>

This example code shows the Blazor coded implementation.  For jsInterop see the example usage section below.
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
*`Transition` parameter is provided by the inherited `TransitionableLayoutComponent`*

</details>

<details>
<summary>Notes on usage</summary>

* This library does not provide animation styles, it simply provides the means to hook into how and when to trigger them.
* If you need to remember page state, to keep track of scroll position for example, you will need to set `ForgetStateOnTransition` to `false`
  * If so, depending on your transition library used, you will need to use z-order, pointer-events, offset position, or other means, of the layout views to cope with interacting with the current route where the switched route is still present but hidden.
* If you need to handle in-app back buttons then use the jsInterop to call the native back i.e. `window.history.back();`
* If you use `AuthorizeRouteView` then you will have to create you own razor component similar to `TransitionableRouteView` and consume in the `App.razor` file.  See the 'Example Usage' section below for details.

</details>

---

## The API

The library provides a simple set of properties to aid you in adding transitions to your projects.

The `TransitionableLayoutComponent`, which you may inherit, providing 
* `Transition` properties
  * `IntoView` - true, if the route should be made visible
  * `Backwards` - true, if you want to reverse animations, for instance
  * `FirstRender` - true, if you want to stop transitioning because of first load
  * `RouteData` reference data you may like to use
  * `SwitchedRouteData` reference data you may like to use
* `TransitionType` a tuple property to allow custom transitions based on what is being transitioned
  * `fromType` is the previous route `PageType`
  * `toType` is the active route `PageType`

There is also `IRouteTransitionInvoker` that you implement when using jsInterop for transitions
* `InvokeRouteTransitionAsync` - provides the `Transition` parameter and where you invoke JS interop.

---

## Example usage
You can find *[detailed documentation on example usage here](README-EXAMPLE.md)*

These demos show examples of the Blazor coded transition option (for the Javascript interop transition option, see the detailed documentation mentioned above).
* The `BlazorTransitionableRouteDemoWasm` demo for WebAssembly Apps. *[see commit for implementation details](https://github.com/JByfordRew/BlazorTransitionableRoute/commit/396c18e625a0e036f28424c223b744406fd0ebf0)*
  * For `AuthorizeRouteView` changes see *[this commit](https://github.com/JByfordRew/BlazorTransitionableRoute/commit/2a71df654cbe0bfd9804b5ff29bb207bb08ba06a)*
* The `BlazorTransitionableRouteDemoServer` demo for Server Apps. *[see commit for implementation details](https://github.com/JByfordRew/BlazorTransitionableRoute/commit/d10a5621f5d4c4523510055093f430279f8c9208)*
  * For `AuthorizeRouteView` changes see *[this commit](https://github.com/JByfordRew/BlazorTransitionableRoute/commit/d110ec2e009ed11729d8e17dde83f8a9cc45a730)*

---

## Version History
* Version 4.0.0 .NET 6 component, previously .NET Standard 2.1.
* Version 3.1.0 - *[previous documentation](README-V3.md)*
  * Addition of `TransitionDurationMilliseconds` to clear switched route view after transition.  Only when `ForgetStateOnTransition` is set to true.
* Version 3.0.0 - *[previous documentation](README-V3.md)*
  * Implemented new `Transition.SwitchedRouteData` along with `Transition.RouteData` to provide means to perform custom transitions. i.e. parent/child views.
* Version 2.1.0 - *[previous documentation and v3 breaking changes](README-V2.md)*
  * Addition of `ForgetStateOnTransition` option to reset page state when returning to previous route via navigation.
* Version 2.0.0 - *[previous documentation](README-V2.md)*
  * Simplified implemention and usage
  * Transitions can now also be done via Blazor code as well as jsInterop
  * Adjust transition behaviour on first render via `Transition.FirstRender`
  * Get the transition direction from `Transition.Backwards`
* Version 1.0.0 - *[documentation and v2 breaking changes](README-V1.md)*

## Roadmap
* Check behaviour for slow loaded pages and handle transition when in ready state or suggest best practice for pages that exhibit this.
* Make `ForgetStateOnTransition` configurable to specific page types
* Potentially move increasing numbers of parameter settings to configuration options
* Remember scroll positions and set when transition.
