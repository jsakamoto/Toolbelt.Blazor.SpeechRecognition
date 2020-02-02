# Blazor Speech Recognition [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.SpeechRecognition.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.SpeechRecognition/)

## Summary

This is a class library for Blazor app to provide Speech Recognition API access.

## Requirements

[Blazor](https://blazor.net/) v.3.1.0 Preview 4 or later.

Both "Blazor WebAssembly App" (a.k.a."Client-side Blazor") and "Blazor Server App" (a.k.a."Server-side Blazor") are supoorted.


## How to install and use?

### 1. Installation and Registration

**Step.1-1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.SpeechRecognition --version 0.0.4.6-alpha
```

**Step.1-2** Register "SpeechRecognition" service into the DI container.

If the project is a Blazor Server App or a Blazor WebAssembly App ver.3.1 Preview 4 or earlyer, add the code into the `ConfigureService` method in the `Startup` class of your Blazor application.

```csharp
// Startup.cs

using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this, and...
...
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddSpeechRecognition(); // <- Add this line.
    ...
```

If the project is a Blazor WebAssembly App ver.3.2 Preview 1 or later, add the code into the `Main` method in the `Program` class of your Blazor application.

```csharp
// Program.cs

using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this, and...
...
public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    ...
    builder.Services.AddSpeechRecognition(); // <- Add this line.
    ...
```

### 2. Usage in your Blazor component (.razor)

**Step.2-1**  Open the `Toolbelt.Blazor.SpeechRecognition` namespace, and inject the `SpeechRecognition` service into the component.

```csharp
@{/* This is your component .razor */}
@using Toolbelt.Blazor.SpeechRecognition @{/* Add these two lines. */}
@inject SpeechRecognition SpeechRecognition
...
```

**Step.2-2**  Subscribe `Result` event of the SpeechRecognition service to receive the results of speech recognition.

```csharp
protected override void OnInitialized()
{
  this.SpeechRecognition.Result += OnSpeechRecognized;
}

void OnSpeechRecognized(object sender, SpeechRecognitionEventArgs args)
{
  // DO SOMETHING...
}
```

**Step.2-3** Invoke `StartAsync()` method of the SpeechRecognition service when you want to start speech recognition.

```csharp
async Task OnClickStart()
{
  await this.SpeechRecognition.StartAsync();
}
```

**Step.2-4** Implement `IDisposable` interface on the component, and unsubscribe `Result` event when the component is disposing.

```csharp
...
@implements IDisposable
...
@code {
  ...
  public void Dispose()
  {
    this.SpeechRecognition.Result -= OnSpeechRecognized;
  }
}
```

See also [sample code on the GitHub repository](https://github.com/jsakamoto/Toolbelt.Blazor.SpeechRecognition/blob/master/SampleSites/SampleSite.Components/App.razor).

### Configuration

The calling of `services.AddSpeechRecognition()` injects the references of JavaScript file (.js) - which is bundled with this package - into your page automatically.

If you don't want this behavior, you can disable these automatic injection, please call `services.AddSpeechRecognition()` with configuration action like this:

```csharp
services.AddSpeechRecognition(options =>
{
  // If you don't want automatic injection of js file, add bellow;
  options.DisableClientScriptAutoInjection = true;
});
```

You can inject the helper JavaScript file manually. The URLs is bellow:

- `_content/Toolbelt.Blazor.SpeechRecognition/script.min.js`

## Release Note

- **v.0.0.4-alpha**
    - Upgrade Blazor to v.3.1.0 Preview 4.
    - Add support for Blazor Server App (Server-side Blazor).
- **v.0.0.3-alpha** - Upgrade Blazor to v.3.0.0 Preview 9.
- **v.0.0.2-alpha** - Upgrade Blazor to v.3.0.0 Preview 6.
- **v.0.0.1-alpha** - 1st release.

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.SpeechRecognition/blob/master/LICENSE)