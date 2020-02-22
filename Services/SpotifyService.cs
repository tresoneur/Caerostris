using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Caerostris.Services
{
    public class SpotifyService
    {
        public async Task<string> GetUsername()
        {
            SpotifyWebAPI api = new SpotifyWebAPI
            {
                AccessToken = "BQAT6DDFdz5sJB-KIQT_GoMh1uj6e8-swv7IVE3i916itMeVTS2by-Lt-cAzuSAQDqnbOJXV1EMLvpTqKkzeGSsc91NlFl_B5zZo0H0n1TRHBMVvAsacwRbxqW4Pt_3h326k_1XxtWG1BcP0M-Vrlmvu6gfkdDU",
                TokenType = "Bearer"
            };

            PrivateProfile profile = await api.GetPrivateProfileAsync();
            return string.IsNullOrEmpty(profile.DisplayName) ? profile.Id : profile.DisplayName;
        }
    }
}
