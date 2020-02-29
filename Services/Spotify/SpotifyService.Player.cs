using Caerostris.Services.Spotify.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caerostris.Services.Spotify.Player.Models;
using SpotifyAPI.Web.Models;

namespace Caerostris.Services.Spotify
{
    public sealed partial class SpotifyService
    {
        private WebPlaybackSDKManager player;

        private string localDeviceID = "";
        private bool isPlaybackLocal = false;


        private void InitializePlayer(WebPlaybackSDKManager injectedPlayer)
        {
            player = injectedPlayer;
            _ = player.Initialize(
                GetAuthToken,
                OnError,
                OnPlaybackContextChanged,
                OnLocalPlayerReady);

            PlaybackContextChanged += (PlaybackContext playback) =>
            {
                if (!(playback?.Device?.Id is null))
                {
                    bool playbackContextIndicatesLocalPlayback = 
                        playback.Device.Id.Equals(localDeviceID, StringComparison.InvariantCulture);

                    if (isPlaybackLocal && !playbackContextIndicatesLocalPlayback)
                    {
                        isPlaybackLocal = false;
                        Log("Playback transferred to remote device.");
                    }
                    else if(!isPlaybackLocal && playbackContextIndicatesLocalPlayback)
                    {
                        isPlaybackLocal = true;
                        Log("Playback transferred back to local device.");
                    }
                }
            };
        }

        private void OnPlaybackContextChanged(WebPlaybackState? state)
        {
            if (!(lastKnownPlayback is null) && !(state is null))
            {
                state.ApplyTo(lastKnownPlayback);
                FirePlaybackContextChanged(lastKnownPlayback);
            }
        }

        private async Task OnLocalPlayerReady(string deviceID)
        {
            localDeviceID = deviceID;
            await TransferPlayback(deviceID);
            isPlaybackLocal = true;
        }

        private async Task LocalPlay() => 
            await player.Play();

        private async Task LocalPause() =>
            await player.Pause();
       
        private async Task LocalNext() =>
            await player.Next();
        
        private async Task LocalPrevious() =>
            await player.Previous();

        private async Task LocalSeek(int positionMs) =>
            await player.Seek(positionMs);
    }
}
