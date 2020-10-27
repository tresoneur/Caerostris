using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services
{
    public class ClipboardService
    {
        private readonly IJSRuntime JsRuntime;

        public ClipboardService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async Task Write(string text) =>
            await JsRuntime.InvokeVoidAsync($"{NativeResources.JsUtils}.{NativeResources.WriteClipboardFunction}", text);
    }
}
