/// Spotify Web Playback SDK wrapper for SpotifyService
class WebPlaybackSDKWrapper {

    constructor() {
        this.Player = null;
        this.GetOAuthToken = null;
        this.OnError = null;
        this.DeviceID = null;
        this.Name = "Caerostris";
        this.LoggingPrefix = "SpotifyService Local Playback Device: ";
    }

    Initialize = (dotNetWebPlaybackSDKManager) => {
        const dotNetMethods =
        {
            GetAuthToken: 'GetAuthToken',
            OnError: 'OnError',
            OnPlayerStateChanged: 'OnPlaybackContextChanged',
            OnReady: 'OnDeviceReady'
        }

        this.OnError = (message) => {
            dotNetWebPlaybackSDKManager
                .invokeMethodAsync(dotNetMethods.OnError, message.message);
        }

        // Initialize the Playback SDK Player provided by Spotify.
        this.Player = new Spotify.Player({
            name: this.Name,
            getOAuthToken: (callback) => {
                dotNetWebPlaybackSDKManager
                    .invokeMethodAsync(dotNetMethods.GetAuthToken)
                    .then(token => {
                        if (token == null || token === "")
                            this.OnError({ message: "Empty or null token passed to Spotify Web Playback SDK wrapper" });
                        else
                            callback(token);
                    });
            },
            volume: 0.75
        });

        // Errors
        this.Player.addListener('initialization_error', this.OnError);
        this.Player.addListener('authentication_error', this.OnError);
        this.Player.addListener('account_error', this.OnError);
        this.Player.addListener('playback_error', this.OnError);

        // Controls
        this.Play = () => this.Player.resume();
        this.Pause = () => this.Player.pause();
        this.Next = () => this.Player.nextTrack();
        this.Previous = () => this.Player.previousTrack();
        this.Seek = (position) => this.Player.seek(position);

        // Info
        this.Player.addListener('player_state_changed', state => {
            /// The Playback SDK returns garbage position data on every other player_state_changed event (thousands separator in position field)
            if (state && state.hasOwnProperty('position') && Number.isInteger(state.position))
                /// The built-in JSInterop deserializer does not support Newtonsoft.JSON annotations/attributes
                dotNetWebPlaybackSDKManager
                    .invokeMethodAsync(dotNetMethods.OnPlayerStateChanged, JSON.stringify(state));
        });

        this.Player.addListener('ready', ({ device_id }) => {
            this.DeviceID = device_id;
            console.log(this.LoggingPrefix + 'Ready with the following ID: ' + device_id);
            dotNetWebPlaybackSDKManager
                .invokeMethodAsync(dotNetMethods.OnReady, device_id);
        });

        this.Player.addListener('not_ready', () => {
            // TODO: event handler 
            console.log(this.LoggingPrefix + 'Device ID has gone offline.');
        });

        this.Player.connect();
    }
}

window.SpotifyService = { WebPlaybackSDKWrapper: new WebPlaybackSDKWrapper() };
window.onSpotifyWebPlaybackSDKReady = () => { /* Pointless requirement of the SDK */ }