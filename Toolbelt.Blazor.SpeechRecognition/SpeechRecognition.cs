using System.ComponentModel;
using System.Reflection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.SpeechRecognition;

/// <summary>
/// The service allows us to access the Speech Recognition API of Web browsers.
/// </summary>
public class SpeechRecognition : IAsyncDisposable
{
    private static readonly string Prefix = "Toolbelt.Blazor.SpeechRecognition.";

    internal readonly SpeechRecognitionOptions Options = new();

    private readonly IJSRuntime _JSRuntime;

    private IJSObjectReference? _recognizer = null;

    private DotNetObjectReference<SpeechRecognition>? _objectRefOfThis;

    /// <summary>
    /// Occurs when the speech recognition service returns a result of recognition of a word or phrase.
    /// </summary>
    public event EventHandler<SpeechRecognitionEventArgs>? Result;

    /// <summary>
    /// Occurs when the speech recognition service has disconnected.
    /// </summary>
    public event EventHandler? End;

    /// <summary>
    /// Gets or sets the language of the current SpeechRecognition object.
    /// </summary>
    public string? Lang { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the speech recognition service should return for each recognition or only a single result.
    /// </summary>
    public bool Continuous { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the speech recognition service should return interim results.
    /// </summary>
    public bool InterimResults { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeechRecognition"/> class.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
    public SpeechRecognition(IJSRuntime jsRuntime)
    {
        this._JSRuntime = jsRuntime;
    }

    private bool _scriptLoaded = false;

    private SemaphoreSlim _syncer = new SemaphoreSlim(1, 1);

    private async ValueTask<IJSObjectReference?> GetRecognizerAsync()
    {
        if (this._scriptLoaded) return this._recognizer;
        await this._syncer.WaitAsync();
        try
        {
            if (this._scriptLoaded) return this._recognizer;

            this._objectRefOfThis ??= DotNetObjectReference.Create(this);

            if (!this.Options.DisableClientScriptAutoInjection)
            {
                var isOnLine = await this._JSRuntime.InvokeAsync<bool>("Toolbelt.Blazor.getProperty", "navigator.onLine");
                var scriptPath = "./_content/Toolbelt.Blazor.SpeechRecognition/script.module.min.js";
                if (isOnLine) scriptPath += $"?v={this.GetVersionText()}";

                await using var module = await this._JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
                this._recognizer = await module.InvokeAsync<IJSObjectReference>("createInstance", this._objectRefOfThis);
            }
            else
            {
                await this._JSRuntime.InvokeVoidAsync("eval", "Toolbelt.Blazor.SpeechRecognition.ready");
                this._recognizer = await this._JSRuntime.InvokeAsync<IJSObjectReference>(Prefix + "createInstance", this._objectRefOfThis);
            }
        }
        catch (InvalidOperationException) { }
        finally
        {
            this._scriptLoaded = true;
            this._syncer.Release();
        }

        return this._recognizer;
    }

    private async ValueTask InvokeRecognizerAsync(Func<IJSObjectReference, ValueTask> action)
    {
        var recognizer = await this.GetRecognizerAsync();
        if (recognizer == null) return;
        await action.Invoke(recognizer);
    }

    private string GetVersionText()
    {
        var assembly = this.GetType().Assembly;
        var version = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "0.0.0";
        return version;
    }

    /// <summary>
    /// Gets a value that indicates whether the speech recognition service is available on the current device.
    /// </summary>
    public async ValueTask<bool> IsAvailableAsync()
    {
        var recognizer = await this.GetRecognizerAsync();
        if (recognizer == null) return false;
        return await recognizer.InvokeAsync<bool>("available");
    }

    /// <summary>
    /// Starts the speech recognition service listening to incoming audio with intent to recognize grammars associated with the current SpeechRecognition object.
    /// </summary>
    /// <returns></returns>
    public async ValueTask StartAsync()
    {
        await this.InvokeRecognizerAsync(r => r.InvokeVoidAsync("start", new
        {
            lang = this.Lang,
            continuous = this.Continuous,
            interimResults = this.InterimResults,
        }));
    }

    /// <summary>
    /// Stops the speech recognition service from listening to incoming audio, and doesn't attempt to return a SpeechRecognitionResult any longer.
    /// </summary>
    public ValueTask StopAsync() => this.InvokeRecognizerAsync(r => r.InvokeVoidAsync("stop"));

    [JSInvokable(nameof(_OnResult)), EditorBrowsable(EditorBrowsableState.Never)]
    public void _OnResult(SpeechRecognitionEventArgs args)
    {
        this.Result?.Invoke(this, args);
    }

    [JSInvokable(nameof(_OnEnd)), EditorBrowsable(EditorBrowsableState.Never)]
    public void _OnEnd()
    {
        this.End?.Invoke(this, EventArgs.Empty);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (this._recognizer != null)
        {
            try { await this._recognizer.DisposeAsync(); }
            catch (JSDisconnectedException) { }
        }
    }
}
