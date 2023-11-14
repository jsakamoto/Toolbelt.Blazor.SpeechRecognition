using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Toolbelt.Blazor.SpeechRecognition;

namespace Toolbelt.Blazor.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for adding SpeechRecognition service.
/// </summary>
public static class SpeechRecognitionExtensions
{
    /// <summary>
    ///  Adds a SpeechRecognition service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
    public static IServiceCollection AddSpeechRecognition(this IServiceCollection services) => services.AddSpeechRecognition(configure: null);

    /// <summary>
    ///  Adds a SpeechRecognition service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
    /// <param name="configure">A delegate that is used to configure the SpeechRecognitionOptions.</param>
    public static IServiceCollection AddSpeechRecognition(this IServiceCollection services, Action<SpeechRecognitionOptions>? configure)
    {
        services.AddTransient(serviceProvider =>
        {
            var jsRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
            var speechRecognitionService = new global::Toolbelt.Blazor.SpeechRecognition.SpeechRecognition(jsRuntime);
            configure?.Invoke(speechRecognitionService.Options);
            return speechRecognitionService;
        });
        return services;
    }
}