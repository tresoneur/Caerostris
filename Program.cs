using System.Threading.Tasks;
using Blazored.Modal;
using Caerostris.Resources;
using Caerostris.Services.Spotify;
using Caerostris.Services.Spotify.Web.SpotifyAPI.Web.Enums;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Caerostris
{
    public class Program
    {
        private const string ClientId = "87b0c14e92bc4958b1b6fe15259d2577";

        private const Scope Permissions = Scope.UserReadPrivate
                                          | Scope.UserReadEmail
                                          | Scope.UserReadPlaybackState
                                          | Scope.UserModifyPlaybackState
                                          | Scope.UserLibraryRead
                                          | Scope.UserReadCurrentlyPlaying
                                          | Scope.PlaylistReadPrivate
                                          | Scope.PlaylistReadCollaborative
                                          | Scope.PlaylistModifyPrivate
                                          | Scope.PlaylistModifyPublic
                                          | Scope.UserLibraryModify
                                          | Scope.Streaming;

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var services = builder.Services;

            services.AddScoped<Text>();
            services.AddLocalization();
            services.AddBlazoredModal();
            services.AddDevExpressBlazor();
            services.AddSpotify("https://caerostrisauthserver.azurewebsites.net/auth");

            var host = builder.Build();

            await host.Services.InitializeSpotify(nameof(Caerostris), ClientId, Permissions);

            await host.RunAsync();
        }
    }
}
