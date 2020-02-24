using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify
{
    public sealed partial class SpotifyService
    {
        private System.Threading.Timer playbackContextPollingTimer;
        public event Action<PlaybackContext>? PlaybackContextChanged;

        private void InitializePlayback()
        {
            playbackContextPollingTimer = new System.Threading.Timer(
                callback: async _ => { await GetPlayback(); },
                state: null,
                dueTime: 0,
                period: 10000
            );
        }

        public async Task<PlaybackContext> GetPlayback()
        {
            var playback = await dispatcher.GetPlayback();
            PlaybackContextChanged?.Invoke(playback);
            return playback;
        }

        public async Task Play() =>
            await DoPlaybackOperation(dispatcher.ResumePlayback);

        public async Task Pause() =>
            await DoPlaybackOperation(dispatcher.PausePlayback);

        public async Task Next() =>
            await DoPlaybackOperation(dispatcher.SkipPlaybackToNext);

        public async Task Previous() => 
            await DoPlaybackOperation(dispatcher.SkipPlaybackToPrevious);

        private async Task DoPlaybackOperation(Func<Task> action)
        {
            await action();
            await dispatcher.GetPlayback();
        }            
    }
}
