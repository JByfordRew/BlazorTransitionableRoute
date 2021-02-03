using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorTransitionableRouteTest
{
    internal class StubType1 : IComponent
    {
        public void Attach(RenderHandle renderHandle)
        {
            throw new NotImplementedException();
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            throw new NotImplementedException();
        }
    }
}
