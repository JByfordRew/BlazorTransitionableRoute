using Microsoft.JSInterop;

namespace BlazorTransitionableRoute
{
    public class JsInterop : IJsInterop, IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public JsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BlazorTransitionableRoute/jsInterop.js").AsTask());
        }

        public async Task Init<T>(DotNetObjectReference<T> instance, bool isActive) where T : class
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("init", instance, isActive);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}