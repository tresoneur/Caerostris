using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify.Auth.Models
{
    public class ImplicitGrantToken
    {
        public string Timestamp { get; set; } = "";

        public int ExpiresInSec { get; set; }

        public string AccessToken { get; set; } = "";
    }
}
