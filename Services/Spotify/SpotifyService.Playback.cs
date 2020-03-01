using SpotifyAPI.Web.Enums;
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
                period: 1000
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
        /// This method will never return a cached value, so each call results in its own corresponding fetch request, even if the playback is local.
        /// Use with care: for regular updates, subscribe to <seealso cref="PlaybackContextChanged"/> instead.
        /// </summary>
        /// <returns>The newly acquired PlaybackContext</returns>
        public async Task<PlaybackContext> GetPlayback() =>
            await dispatcher.GetPlayback();

        public async Task<AvailabeDevices> GetDevices() =>
            await dispatcher.GetDevices();

        public async Task TransferPlayback(string deviceID) =>
            await dispatcher.TransferPlayback(deviceID, play: lastKnownPlayback?.IsPlaying ?? false);

        public async Task Play() =>
            await DoPlaybackOperation(player.Play, dispatcher.ResumePlayback);

        public async Task Pause() =>
            await DoPlaybackOperation(player.Pause, dispatcher.PausePlayback);

        public async Task Next() =>
            await DoPlaybackOperation(player.Next, dispatcher.SkipPlaybackToNext);

        public async Task Previous() => 
            await DoPlaybackOperation(player.Previous, dispatcher.SkipPlaybackToPrevious);

        public async Task Seek(int positionMs) =>
            await DoPlaybackOperation(
                async () => await player.Seek(positionMs),
                async () => await dispatcher.SeekPlayback(positionMs));

        public async Task SetShuffle(bool shuffle) =>
            await DoRemotePlaybackOperation(async () => await dispatcher.SetShuffle(shuffle));

        public async Task SetRepeat(RepeatState state) =>
            await DoRemotePlaybackOperation(async () => await dispatcher.SetRepeatMode(state));

        public async Task SetVolume(int volumePercent) =>
            await DoPlaybackOperation(
                async () => await player.SetVolume(volumePercent), 
                async () => await dispatcher.SetVolume(volumePercent));

        private async Task DoPlaybackOperation(Func<Task> local, Func<Task> remote)
        {
            if (isPlaybackLocal)
                await DoLocalPlaybackOperation(local);
            else
                await DoRemotePlaybackOperation(remote);
        }

        private async Task DoLocalPlaybackOperation(Func<Task> action)
        {
            await action();
            // The Spotify Web Playback SDK will issue a state update event. The PlaybackContextChanged event is fired from our callback function.
        }

        private async Task DoRemotePlaybackOperation(Func<Task> action)
        {
            await action();
            await Task.Delay(100); /// The Spotify Web API sends incorrect results when queried too soon after a playback operation.
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
