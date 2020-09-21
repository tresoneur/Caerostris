using Caerostris.Services.Spotify.Web.SpotifyAPI.Web.Models;
using Caerostris.Shared.Data.TrackGrid.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Shared.Data.TrackGrid.Extensions
{
    public static class TrackExtensions
    {
        public static Track AsTrack(this PlaylistTrack playlistTrack) // TODO: szebb mapping
        {
            var track = playlistTrack.Track.AsTrack();
            track.AddedAt = playlistTrack.AddedAt;
            return track;
        }

        public static Track AsTrack(this SavedTrack savedTrack)
        {
            var track = savedTrack.Track.AsTrack();
            track.AddedAt = savedTrack.AddedAt;
            return track;
        }

        public static Track AsTrack(this FullTrack fullTrack)
        {
            return new Track()
            {
                Uri = fullTrack.Uri,
                Id = fullTrack.Id,
                LinkedFromId = fullTrack.LinkedFrom?.Id,
                Title = fullTrack.Name,
                Explicit = fullTrack.Explicit,
                AlbumTitle = fullTrack.Album.Name,
                AlbumExternalUrls = fullTrack.Album.ExternalUrls,
                AlbumId = fullTrack.Album.Id,
                AlbumTrackNumber = fullTrack.TrackNumber,
                Artists = fullTrack.Artists,
                Popularity = fullTrack.Popularity,
                DurationMs = fullTrack.DurationMs
            };
        }

        public static Track AsTrack(this SimpleTrack simpleTrack) // TODO: közös ős v. interface?
        {
            return new Track()
            {
                Uri = simpleTrack.Uri,
                Id = simpleTrack.Id,
                Title = simpleTrack.Name,
                Explicit = simpleTrack.Explicit,
                AlbumTrackNumber = simpleTrack.TrackNumber,
                Artists = simpleTrack.Artists,
                DurationMs = simpleTrack.DurationMs
            };
        }
    }
}
