using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify
{
    public sealed partial class SpotifyService
    {
        private PlaybackContext? lastKnownPlayback;
        private DateTime? lastKnownPlaybackTimestamp;

        private Timer playbackContextPollingTimer;
        
        /// <summary>
        /// Fires when a new PlaybackContext is received from the Spotify API. A <seealso cref="PlaybackDisplayUpdate"/> event is also fired afterwards.
        /// </summary>
        public event Action<PlaybackContext>? PlaybackContextChanged;

        private Timer playbackUpdateTimer;

        /// <summary>
        /// Fires when the display of the PlaybackContext needs to be updated with the supplied arguments.
        /// This does not necessarily mean that a new PlaybackContext was acquired. 
        /// Suggested use: periodically refresh the UI.
        /// </summary>
        public event Action<int>? PlaybackDisplayUpdate;

        private void InitializePlayback()
        {
            playbackContextPollingTimer = new System.Threading.Timer(
                callback: async _ => { FirePlaybackContextChanged(await GetPlayback()); },
                state: null,
                dueTime: 0,
                period: 2000
            );

            playbackUpdateTimer = new System.Threading.Timer(
                callback: _ => { PlaybackDisplayUpdate?.Invoke(GetProgressMs()); },
                state: null,
                dueTime: 0,
                period: 33
            );
        }

        /// <summary>
        /// Retrieves the current state of playback. 
        /// This method will never return a cached value, so each call results in its own corresponding fetch request. 
        /// Use with care: for regular updates, subscribe to <seealso cref="PlaybackContextChanged"/> instead.
        /// </summary>
        /// <returns>The newly acquired PlaybackContext</returns>
        public async Task<PlaybackContext> GetPlayback()
        {
            return await dispatcher.GetPlayback();
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
            FirePlaybackContextChanged(await dispatcher.GetPlayback());
        }

        public int GetProgressMs()
        {
            if (lastKnownPlayback is null || lastKnownPlayback.Item is null || lastKnownPlaybackTimestamp is null)
                return 0;

            var extraProgressIfPlaying = DateTime.UtcNow - lastKnownPlaybackTimestamp;
            long totalProgressIfPlaying = Convert.ToInt64(extraProgressIfPlaying.Value.TotalMilliseconds) + lastKnownPlayback.ProgressMs;
            try 
            {
                int progressIfPlayingSane = Convert.ToInt32(totalProgressIfPlaying);
                var bestGuess = ((lastKnownPlayback.IsPlaying) ? progressIfPlayingSane : lastKnownPlayback.ProgressMs);
                var totalDuractionMs = lastKnownPlayback.Item.DurationMs;

                return ((bestGuess > totalDuractionMs) ? totalDuractionMs : bestGuess);
            }
            catch (OverflowException) 
            {
                return 0;
            }
        }

        private void FirePlaybackContextChanged(PlaybackContext playback)
        {
            lastKnownPlayback = playback;
            lastKnownPlaybackTimestamp = DateTime.UtcNow;
            PlaybackContextChanged?.Invoke(playback);
            PlaybackDisplayUpdate?.Invoke(GetProgressMs());
        }
    }
}
