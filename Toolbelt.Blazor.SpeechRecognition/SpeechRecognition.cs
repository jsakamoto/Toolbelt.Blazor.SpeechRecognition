using System.ComponentModel;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.SpeechRecognition;

public class SpeechRecognition
{
    private static readonly string Prefix = "Toolbelt.Blazor.SpeechRecognitionProxy.";

    internal readonly SpeechRecognitionOptions Options = new SpeechRecognitionOptions();

    private readonly IJSRuntime _JSRuntime;

    private DotNetObjectReference<SpeechRecognition>? _ObjectRefOfThis;

    public event EventHandler<SpeechRecognitionEventArgs>? Result;

    public event EventHandler? End;

    private ValueTask<bool> AvailableTask;

    private string _Lang = "";

    public string Lang
    {
        get => this._Lang;
        set { if (this._Lang != value) { this._Lang = value; this.InvokeJSVoidAsync("lang", this._Lang); } }
    }

    private bool _Continuous;

    public bool Continuous
    {
        get => this._Continuous;
        set { if (this._Continuous != value) { this._Continuous = value; this.InvokeJSVoidAsync("continuous", this._Continuous); } }
    }


    private bool _InterimResults;

    public bool InterimResults
    {
        get => this._InterimResults;
        set { if (this._InterimResults != value) { this._InterimResults = value; this.InvokeJSVoidAsync("interimResults", this._InterimResults); } }
    }

    public SpeechRecognition(IJSRuntime jsRuntime)
    {
        this._JSRuntime = jsRuntime;
    }

    private bool ScriptLoaded = false;

    private SemaphoreSlim Syncer = new SemaphoreSlim(1, 1);

    private async ValueTask InvokeJSVoidAsync(string identifier, params object[] args)
    {
        await this.InvokeJSAsync<object>(identifier, args);
    }

    private async ValueTask<T> InvokeJSAsync<T>(string identifier, params object[] args)
    {
        if (!this.ScriptLoaded && !this.Options.DisableClientScriptAutoInjection)
        {
            await this.Syncer.WaitAsync();
            try
            {
                if (!this.ScriptLoaded)
                {
                    const string scriptPath = "_content/Toolbelt.Blazor.SpeechRecognition/script.min.js";
                    await this._JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s)=>(h=>h.querySelector(t+`[src=\"${s}\"]`)?r():(e=>(e.src=s,e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "'))");
                    this.ScriptLoaded = true;
                }
            }
            finally { this.Syncer.Release(); }
        }
        return await this._JSRuntime.InvokeAsync<T>(Prefix + identifier, args);
    }

    private DotNetObjectReference<SpeechRecognition> GetObjectRef()
    {
        if (this._ObjectRefOfThis == null) this._ObjectRefOfThis = DotNetObjectReference.Create(this);
        return this._ObjectRefOfThis;
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
        return this.InvokeJSVoidAsync("start");
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
}
