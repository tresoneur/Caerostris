using Caerostris.Services.Spotify.Auth;
using SpotifyAPI.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify
{
    /// <remarks>
    /// Methods within the Auth section do not use the SpotifyWebAPIProxy, because the SpotifyWebAPI does not contain any OAuth functionality.
    /// </remarks>
    public sealed partial class SpotifyService
    {
        private const string clientID = "87b0c14e92bc4958b1b6fe15259d2577";

        private ImplicitGrantAuthManager authManager;

        /// <summary>
        /// Fires when the auth state changes: either the current token expires or a new token is acquired.
        /// Also fires when a valid token is found in cache on startup. The SpotifyService instance won't be able to fetch any user-specific data before this happens, so it is best if any component in need of e.g. the username subscribes to this event.
        /// </summary>
        public event Action<bool>? AuthStateChanged;
        private System.Threading.Timer authPollingTimer;
        private bool authGrantedWhenLastChecked = false;


        private void InitializeAuth(ImplicitGrantAuthManager injectedAuthManager)
        {
            authManager = injectedAuthManager;

            api.TokenType = "Bearer";

            authPollingTimer = new System.Threading.Timer(
                callback: async _ => { await CheckAuth(); },
                state: null,
                dueTime: 0,
                period: 1000
            );
        }

        /// <summary>
        /// Gets an access token through the OAuth2 Implicit Grant process. Should be used before the use of any other API functionality is attempted.
        /// The token gets saved to LocalStorage.
        /// Will reload the page (!)
        /// </summary>
        /// <remarks>
        /// The callback URI has to be added to the whitelist on the Spotify Developer Dashboard.
        /// Any permissions needed will have to be passed to <see cref="AuthManager.StartProcess(string, string, Scope)"/>.
        /// </remarks>
        public async Task StartAuthWithImplicitGrant(string callbackURI)
        {
            await authManager.StartProcess(
                clientID,
                callbackURI,
                Scope.UserReadPrivate
                    | Scope.UserReadEmail
                    | Scope.UserReadPlaybackState
                    | Scope.UserModifyPlaybackState
                    | Scope.Streaming
            );
        }

        /// <summary>
        /// The OAuth2 Implicit Grant process involves a callback to an address provided by the caller of <see cref="StartAuthWithImplicitGrant(string)"/> to redirect to after authentication success or failure.
        /// Invoke this function at the given address.
        /// </summary>
        /// <returns>Whether the process was successful</returns>
        public async Task<bool> ContinueAuthWithImplicitGrantOnCallback()
        {
            // Internal redirection inside (forceReload: false)
            string? token = await authManager.StoreTokenOnCallback();

            if (token is null)
                return false;
            
            api.AccessToken = token;

            // Fire event(s)
            await CheckAuth();

            return true;
        }

        /// <summary>
        /// Checks whether there is a valid token to use.
        /// </summary>
        public async Task<bool> AuthGranted()
        {
            return (!((await GetAuthToken()) is null));
        }

        private async Task CheckAuth()
        {
            string? token = await GetAuthToken();
            if (!(token is null))
                api.AccessToken = token;

            bool authGranted = (await AuthGranted());
            if(authGranted != authGrantedWhenLastChecked)
            {
                authGrantedWhenLastChecked = authGranted;
                AuthStateChanged?.Invoke(authGranted);
            }
        }

        private async Task<string?> GetAuthToken() =>
            await authManager.GetToken();
    }
}
