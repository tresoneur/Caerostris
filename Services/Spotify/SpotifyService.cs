using Caerostris.Services.Spotify.Auth;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify
{
    /// <remarks>
    /// Choose the 'singleton' instantiation mode when using Dependency Injection. The internal state of the class was devised with a per session instantiation policy in mind.
    /// </remarks>
    public sealed partial class SpotifyService : IDisposable
    {
        private SpotifyWebAPI api;
        private SpotifyWebAPIProxy dispatcher;


        #pragma warning disable CS8618 // Partial constructors aren't a thing, so the initalization of these attributes happens in the Initialize...() methods.
        public SpotifyService(ImplicitGrantAuthManager injectedAuthManager)
        #pragma warning restore CS8618
        {
            api = new SpotifyWebAPI();
            dispatcher = new SpotifyWebAPIProxy(api);

            InitializeAuth(injectedAuthManager);
            InitializePlayback();
        }

        public async Task<string> GetUsername()
        {
            PrivateProfile profile = await dispatcher.GetPrivateProfile();
            return string.IsNullOrEmpty(profile.DisplayName) 
                ? profile.Id 
                : profile.DisplayName;
        }

        public void Dispose()
        {
            playbackContextPollingTimer.Dispose();
            authPollingTimer.Dispose();
            api.Dispose();
        }
    }
}
