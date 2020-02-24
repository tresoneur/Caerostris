using Caerostris.Services.Spotify.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caerostris.Services.Spotify
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method to register a SpotifyService and all its dependecies.
        /// </summary>
        public static IServiceCollection AddSpotify(this IServiceCollection services)
        {
            // The dependency injection module will take care of the Dispose() call
            services.AddSingleton<SpotifyService>();

            // Injected SpotifyService dependencies
            services.AddSingleton<ImplicitGrantAuthManager>();

            return services;
        }
    }
}
