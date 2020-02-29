using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify.Auth.Models
{
    public class ImplicitGrantToken
    {
        public ImplicitGrantToken() { }

        public ImplicitGrantToken(int expiresInSec, string accessToken)
        {
            Timestamp = DateTime.UtcNow.ToString("o");
            ExpiresInSec = expiresInSec;
            AccessToken = accessToken;
        }

        public string Timestamp { get; set; } = "";

        public int ExpiresInSec { get; set; }

        public string AccessToken { get; set; } = "";


        public bool IsExpired()
        {
            var expiryDate = DateTime.Parse(Timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind);
            expiryDate = expiryDate.AddSeconds(ExpiresInSec);
            return (expiryDate < DateTime.UtcNow);
        }
    }
}
