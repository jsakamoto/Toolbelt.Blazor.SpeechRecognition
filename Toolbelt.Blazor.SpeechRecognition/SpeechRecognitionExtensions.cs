using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding SpeechRecognition service.
    /// </summary>
    public static class SpeechRecognitionExtensions
    {
        /// <summary>
        ///  Adds a SpeechRecognition service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        public static IServiceCollection AddSpeechRecognition(this IServiceCollection services)
        {
            services.AddScoped(serviceProvider => new global::Toolbelt.Blazor.SpeechRecognition.SpeechRecognition(serviceProvider.GetService<IJSRuntime>()).Attach());
            return services;
        }
    }
}