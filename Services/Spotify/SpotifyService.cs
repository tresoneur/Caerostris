using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify
{
    public sealed partial class SpotifyService : IDisposable
    {
        private SpotifyWebAPI api;
        private Proxy dispatcher;

        private System.Threading.Timer playbackContextPollingTimer;
        public event Action<PlaybackContext>? PlaybackContextChanged;

        public SpotifyService()
        {
            api = new SpotifyWebAPI
            {
                AccessToken = "BQBRAeKxrTaL0I6I9t2PLXzhjHJW07EdK_4E8QEu3iifw1sRiZ4sJDWIvKwN0MxkKdb7vfvLsMD-EJfTnfOkmlRHUpHx6D8XXNIlNfTSd1BFb302CsbW5rYmcSh1JujPUzuYLo4QdstZUuHIZrxd5y6e87oPsKZqpbUts2Sne6LoZbJOYDnlOkN20wD4hzxpwOGp5kXT8vqHl1plJGW-FCZ15FfISe-mRpZATnb_Gb0MGH6ipZAiBwq1NuOt6PDKM1pAoHYkR3rd",
                TokenType = "Bearer"
            };

            dispatcher = new Proxy(api);

            playbackContextPollingTimer = new System.Threading.Timer(
                callback: async _ => { await GetPlayback(); },
                state: null,
                dueTime: 0,
                period: 10000
            );
        }

        public async Task<string> GetUsername()
        {
            PrivateProfile profile = await dispatcher.GetPrivateProfile();
            return string.IsNullOrEmpty(profile.DisplayName) 
                ? profile.Id 
                : profile.DisplayName;
        }

        private void FirePlaybackContextChanged(PlaybackContext playback) => 
            PlaybackContextChanged?.Invoke(playback);

        public void Dispose()
        {
            playbackContextPollingTimer.Dispose();
            api.Dispose();
        }
    }
}
