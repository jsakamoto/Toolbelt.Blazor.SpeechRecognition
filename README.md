# Blazor Speech Recognition [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.SpeechRecognition.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.SpeechRecognition/)

## Summary

This is a class library for Blazor app to provide Speech Recognition API access.

## Requirements

[Blazor](https://blazor.net/) v.6.0, 7.0, 8.0 or later.

Both "Blazor WebAssembly" and "Blazor Server" are supoorted.


## Quick Start

### 1. Installation and Registration

**Step.1-1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.SpeechRecognition
```

**Step.1-2** Register `SpeechRecognition` service into the DI container.

If the project is a Blazor Server App or a Blazor WebAssembly App ver.3.1 Preview 4 or earlyer, add the code into the `ConfigureService` method in the `Startup` class of your Blazor application.

```csharp
// Program.cs

using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this, and...
...
var builder = ...
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

private void OnSpeechRecognized(object sender, SpeechRecognitionEventArgs args)
{
  // DO SOMETHING...
}
```

**Step.2-3** Invoke `StartAsync()` method of the SpeechRecognition service when you want to start speech recognition.

```csharp
private async Task OnClickStart()
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

Release notes is [here](https://github.com/jsakamoto/Toolbelt.Blazor.SpeechRecognition/blob/master/RELEASE-NOTES.txt).

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.SpeechRecognition/blob/master/LICENSE)