using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify.Player.Models
{
    public class WebPlaybackTrackWindow
    {
        [JsonProperty("current_track")]
        public WebPlaybackTrack? CurrentTrack { get; set; }

        [JsonProperty("previous_tracks")]
        public List<WebPlaybackTrack> PreviousTracks { get; set; }

        [JsonProperty("next_tracks")]
        public List<WebPlaybackTrack> NextTracks { get; set; }
    }
}