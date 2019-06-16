# Blazor Speech Recognition [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.SpeechRecognition.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.SpeechRecognition/)

## Summary

This is a class library for Blazor app to provide Speech Recognition API access.


## How to install and use?

### 1. Installation and Registration

**Step.1-1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.SpeechRecognition --version 0.0.2.1-alpha
```

**Step.1-2** Register "SpeechRecognition" service into the DI container, at `ConfigureService` method in the `Startup` class of your Blazor application.

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
protected override void OnInit()
{
  this.SpeechRecognition.Result += OnSpeechRecognized;
}

void OnSpeechRecognized(object sender, SpeechRecognitionEventArgs args)
{
  // DO SOMETHING...
}
```

**Step.2-3** Invoke `start()` method of the SpeechRecognition service when you want to start speech recognition.

```csharp
void OnClickStart()
{
  this.SpeechRecognition.Start();
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

See also [sample code on the GitHub repository](https://github.com/jsakamoto/Toolbelt.Blazor.SpeechRecognition/blob/master/SampleSites/ClientSideBlazorSampleSite/App.razor).

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.SpeechRecognition/blob/master/LICENSE)