using Microsoft.JSInterop;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caerostris.Services.Spotify.Player.Models;
using Newtonsoft.Json;

namespace Caerostris.Services.Spotify.Player
{
    /// <remarks>
    /// Do not attempt to create more than one instance of this class in a single window.
    /// </remarks>
    public class WebPlaybackSDKManager : IDisposable
    {
        private const string JSWrapper = "SpotifyService.WebPlaybackSDKWrapper";

        private readonly IJSRuntime JSRuntime;
        private DotNetObjectReference<WebPlaybackSDKManager> selfReference;


        private Func<Task<string?>> authTokenCallback = 
            async () => await Task.FromResult<string?>(null);

        private Func<string, Task> errorCallback = 
            async (_) => { await Task.CompletedTask; };

        private Action<WebPlaybackState?> playbackContextCallback =
            (_) => { };

        private Func<string, Task> onDeviceReady =
            async (_) => { await Task.CompletedTask; };

        public WebPlaybackSDKManager(IJSRuntime injectedJSRuntime)
        {
            JSRuntime = injectedJSRuntime;
            selfReference = DotNetObjectReference.Create(this);
        }

        /// <summary>
        /// Call this method before attempting to interact with this class in any way.
        /// This method may be called several times. This resets the inner state of the instance and all JS entities associated with it.
        /// </summary>
        /// <param name="authTokenCallback">Function to call to acquire a valid OAuth token for streaming</param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task Initialize(
            Func<Task<string?>> authTokenCallback, 
            Func<string, Task> errorCallback, 
            Action<WebPlaybackState?> playbackContextCallback,
            Func<string, Task> onDeviceReady)
        {
            this.authTokenCallback = authTokenCallback;
            this.errorCallback = errorCallback;
            this.playbackContextCallback = playbackContextCallback;
            this.onDeviceReady = onDeviceReady;

            await JSRuntime.InvokeVoidAsync($"{JSWrapper}.Initialize", selfReference);
        }

        [JSInvokable]
        public async Task<string?> GetAuthToken() =>
            await authTokenCallback();

        [JSInvokable]
        public async Task OnError(string? message) =>
            await errorCallback(message ?? string.Empty);

        [JSInvokable]
        public void OnPlaybackContextChanged(string? state)
        {
            if (!string.IsNullOrEmpty(state))
                playbackContextCallback(JsonConvert.DeserializeObject<WebPlaybackState?>(state));
        }

        [JSInvokable]
        public async Task OnDeviceReady(string? deviceID)
        {
            if (!string.IsNullOrEmpty(deviceID))
                await onDeviceReady(deviceID);
        }

        public async Task Play() =>
            await JSRuntime.InvokeVoidAsync($"{JSWrapper}.Play");

        public async Task Pause() =>
            await JSRuntime.InvokeVoidAsync($"{JSWrapper}.Pause");

        public async Task Next() =>
            await JSRuntime.InvokeVoidAsync($"{JSWrapper}.Next");

        public async Task Previous() =>
            await JSRuntime.InvokeVoidAsync($"{JSWrapper}.Previous");

        public async Task Seek(int positionMs) =>
            await JSRuntime.InvokeVoidAsync($"{JSWrapper}.Seek", positionMs);

        public void Dispose()
        {
            selfReference?.Dispose();
        }
    }
}
