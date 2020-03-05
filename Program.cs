using Blazored.Modal;
using Caerostris.Services.Spotify;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Caerostris
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var services = builder.Services;
            services.AddBlazoredModal();
            services.AddDevExpressBlazor();
            services.AddSpotify();

            builder.RootComponents.Add<App>("app");
            await builder.Build().RunAsync();
        }
    }
}
