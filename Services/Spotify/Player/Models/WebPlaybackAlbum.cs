using Newtonsoft.Json;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify.Player.Models
{
    public class WebPlaybackAlbum
    {
        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public SimpleAlbum ApplyTo(SimpleAlbum album)
        {
            album.Images = Images;
            album.Name = Name;
            album.Uri = Uri;

            return album;
        }
    }
}
