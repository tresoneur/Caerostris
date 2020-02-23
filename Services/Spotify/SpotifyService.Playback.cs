using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify
{
    public sealed partial class SpotifyService
    {
        public async Task<PlaybackContext> GetPlayback()
        {
            var playback = await dispatcher.GetPlayback();
            FirePlaybackContextChanged(playback);
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
