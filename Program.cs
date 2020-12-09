using System.Threading.Tasks;
using Blazored.Modal;
using Caerostris.Resources;
using Caerostris.Services;
using Caerostris.Services.Spotify;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Caerostris
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddScoped<ClipboardService>()
                .AddScoped<Text>()
                .AddLocalization()
                .AddSpotify(new()
                {
                    AuthServerApiBase = "https://caerostrisauthserver.azurewebsites.net/auth",
                    PlayerDeviceName = "Cærostris",
                    ClientId = "87b0c14e92bc4958b1b6fe15259d2577",
                    PermissionScopes = new[]
                    {
                        "user-read-private",
                        "user-read-email",
                        "user-read-playback-state",
                        "user-modify-playback-state",
                        "user-library-read",
                        "user-library-modify",
                        "user-read-currently-playing",
                        "playlist-read-private",
                        "playlist-read-collaborative",
                        "playlist-modify-private",
                        "playlist-modify-public",
                        "streaming"
                    }
                })
                .AddBlazoredModal()
                .AddDevExpressBlazor();

            var host = builder.Build();

            await host.Services.InitializeSpotify();
        
            await host.RunAsync();
        }
    }
}
