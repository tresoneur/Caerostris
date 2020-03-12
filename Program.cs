using System.Threading.Tasks;
using Blazored.Modal;
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
            builder.RootComponents.Add<App>("app");

            var services = builder.Services;
            services.AddBaseAddressHttpClient();
            services.AddBlazoredModal();
            services.AddDevExpressBlazor();
            services.AddSpotify();

            var host = builder.Build();

            await host.Services.InitializeSpotify();

            await host.RunAsync();
        }
    }
}
