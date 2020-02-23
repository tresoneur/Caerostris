using Caerostris.Services.Spotify;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Caerostris
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // The dependency injection module will take care of the Dispose() call
            services.AddSingleton<SpotifyService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
