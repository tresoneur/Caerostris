using System.Threading.Tasks;
using Blazored.Modal;
using Caerostris.Resources;
using Caerostris.Services.Spotify;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Caerostris.Services.Spotify.Web.SpotifyAPI.Web.Enums;

namespace Caerostris
{
    public class Program
    {
        private const string clientId = "87b0c14e92bc4958b1b6fe15259d2577";

        private const Scope permissions = Scope.UserReadPrivate
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
            builder.RootComponents.Add<App>("app");

            var services = builder.Services;

            services.AddLocalization(/*options => options.ResourcesPath = "Resources"*/);
            services.AddScoped<Text>();

            services.AddBlazoredModal();
            services.AddDevExpressBlazor();
            services.AddSpotify("https://caerostrisauthserver.azurewebsites.net/auth");

            var host = builder.Build();

            await host.Services.InitializeSpotify(nameof(Caerostris), clientId, permissions);

            await host.RunAsync();
        }
    }
}
