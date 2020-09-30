using Caerostris.Services.Spotify.Web.SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;

namespace Caerostris.Shared.Data.TrackGrid.ViewModel
{
    public class Track
    {
        public string Id { get; set; } = default!;
        
        public string Uri { get; set; } = default!;

        public string? LinkedFromId { get; set; }

        public string Title { get; set; } = default!;

        public bool Explicit { get; set; } = default!;

        public string AlbumTitle { get; set; } = default!;

        public string AlbumId { get; set; } = default!;

        public Dictionary<string, string> AlbumExternalUrls { get; set; } = default!;

        public int AlbumTrackNumber { get; set; }

        public List<SimpleArtist> Artists { get; set; } = default!;

        public int Popularity { get; set; }

        public int DurationMs { get; set; }

        public DateTime AddedAt { get; set; }

        // TODO: pl. PublicProfile AddedBy, etc.
    }
}
