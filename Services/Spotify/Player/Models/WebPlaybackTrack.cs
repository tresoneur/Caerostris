using Newtonsoft.Json;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Converters;
using SpotifyAPI.Web.Enums;

namespace Caerostris.Services.Spotify.Player.Models
{
    public class WebPlaybackTrack
    {
        [JsonProperty("album")]
        public WebPlaybackAlbum? Album { get; set; }

        [JsonProperty("artists")]
        public List<WebPlaybackArtist> Artists { get; set; }

        [JsonProperty("duration_ms")]
        public int DurationMs { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TrackType Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("is_playable")]
        public bool? IsPlayable { get; set; }

        public FullTrack ApplyTo(FullTrack track)
        {
            /// Sometimes different metadata is supplied by the two different APIs for the exact same track
            if (!(track.Id.Equals(Id, StringComparison.InvariantCulture)))
            {
                Album?.ApplyTo(track.Album);

                track.Artists = Artists
                    .Select((artist) => 
                        new SimpleArtist() 
                        { 
                            Id = artist.Uri.Split(':').LastOrDefault(), 
                            Name = artist.Name, 
                            Uri = artist.Uri 
                        })
                    .ToList();
            }

            track.DurationMs = DurationMs;
            track.Id = Id;
            track.Name = Name;
            track.Type = SpotifyAPI.Web.Util.GetStringAttribute(Type);
            track.Uri = Uri;
            track.IsPlayable = IsPlayable;

            return track;
        }
    }
}
