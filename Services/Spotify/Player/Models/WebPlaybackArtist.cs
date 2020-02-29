using Newtonsoft.Json;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify.Player.Models
{
    public class WebPlaybackArtist
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public SimpleArtist ApplyTo(SimpleArtist artist)
        {
            artist.Name = Name;
            artist.Uri = Uri;

            return artist;
        }
    }
}
