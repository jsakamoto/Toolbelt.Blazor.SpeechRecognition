using System.ComponentModel;
using System.Reflection;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.SpeechRecognition;

public class SpeechRecognition : IAsyncDisposable
{
    private static readonly string Prefix = "Toolbelt.Blazor.SpeechRecognition.";

    internal readonly SpeechRecognitionOptions Options = new();

    private readonly IJSRuntime _JSRuntime;

    private IJSObjectReference? _JSModule = null;

    private DotNetObjectReference<SpeechRecognition>? _objectRefOfThis;

    public event EventHandler<SpeechRecognitionEventArgs>? Result;

    public event EventHandler? End;

    private ValueTask<bool> AvailableTask;

    public string? Lang { get; set; }

    public bool Continuous { get; set; }

    public bool InterimResults { get; set; }

    public SpeechRecognition(IJSRuntime jsRuntime)
    {
        this._JSRuntime = jsRuntime;
    }

    private bool _scriptLoaded = false;

    private SemaphoreSlim _syncer = new SemaphoreSlim(1, 1);

    private async ValueTask InvokeJSVoidAsync(string identifier, params object[] args)
    {
        await this.InvokeJSAsync<object>(identifier, args);
    }

    private async ValueTask<T> InvokeJSAsync<T>(string identifier, params object[] args)
    {
        if (!this._scriptLoaded)
        {
            await this._syncer.WaitAsync();
            try
            {
                if (!this._scriptLoaded)
                {
                    if (!this.Options.DisableClientScriptAutoInjection)
                    {
                        var isOnLine = await this._JSRuntime.InvokeAsync<bool>("Toolbelt.Blazor.getProperty", "navigator.onLine");
                        var scriptPath = "./_content/Toolbelt.Blazor.SpeechRecognition/script.module.min.js";
                        if (isOnLine) scriptPath += $"?v={this.GetVersionText()}";

                        this._JSModule = await this._JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath);
                    }
                    else
                    {
                        try { await this._JSRuntime.InvokeVoidAsync("eval", "Toolbelt.Blazor.SpeechRecognition.ready"); } catch { }
                    }
                    this._scriptLoaded = true;
                }
            }
            finally { this._syncer.Release(); }
        }

        if (this._JSModule != null)
            return await this._JSModule.InvokeAsync<T>(identifier, args);
        else
            return await this._JSRuntime.InvokeAsync<T>(Prefix + identifier, args);
    }

    private string GetVersionText()
    {
        var assembly = this.GetType().Assembly;
        var version = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "0.0.0";
        return version;
    }

    private DotNetObjectReference<SpeechRecognition> GetObjectRef()
    {
        this._objectRefOfThis ??= DotNetObjectReference.Create(this);
        return this._objectRefOfThis;
    }

    internal SpeechRecognition Attach()
    {
        this.AvailableTask = this.InvokeJSAsync<bool>("attach", this.GetObjectRef());
        return this;
    }

    public ValueTask<bool> IsAvailableAsync()
    {
        return this.AvailableTask;
    }

    public ValueTask StartAsync()
    {
        return this.InvokeJSVoidAsync("start", new
        {
            lang = this.Lang,
            continuous = this.Continuous,
            interimResults = this.InterimResults,
        });
    }

    public ValueTask StopAsync()
    {
        return this.InvokeJSVoidAsync("stop");
    }

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
        if (this._JSModule != null)
        {
            try { await this._JSModule.DisposeAsync(); }
            catch (JSDisconnectedException) { }
        }
    }
}
