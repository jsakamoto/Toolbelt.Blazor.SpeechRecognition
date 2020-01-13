using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.SpeechRecognition
{
    public class SpeechRecognition
    {
        private static readonly string Prefix = "Toolbelt.Blazor.SpeechRecognitionProxy.";

        internal readonly SpeechRecognitionOptions Options = new SpeechRecognitionOptions();

        private readonly IJSRuntime _JSRuntime;

        private DotNetObjectReference<SpeechRecognition> _ObjectRefOfThis;

        public event EventHandler<SpeechRecognitionEventArgs> Result;

        public event EventHandler End;

        private ValueTask<bool> AvailableTask;

        private string _Lang = "";

        public string Lang
        {
            get => _Lang;
            set { if (_Lang != value) { _Lang = value; InvokeJSVoidAsync("lang", _Lang); } }
        }

        private bool _Continuous;

        public bool Continuous
        {
            get => _Continuous;
            set { if (_Continuous != value) { _Continuous = value; InvokeJSVoidAsync("continuous", _Continuous); } }
        }


        private bool _InterimResults;

        public bool InterimResults
        {
            get => _InterimResults;
            set { if (_InterimResults != value) { _InterimResults = value; InvokeJSVoidAsync("interimResults", _InterimResults); } }
        }

        public SpeechRecognition(IJSRuntime jsRuntime)
        {
            _JSRuntime = jsRuntime;
        }

        private bool ScriptLoaded = false;

        private SemaphoreSlim Syncer = new SemaphoreSlim(1, 1);

        private async ValueTask InvokeJSVoidAsync(string identifier, params object[] args)
        {
            await InvokeJSAsync<object>(identifier, args);
        }

        private async ValueTask<T> InvokeJSAsync<T>(string identifier, params object[] args)
        {
            if (!ScriptLoaded && !this.Options.DisableClientScriptAutoInjection)
            {
                await Syncer.WaitAsync();
                try
                {
                    if (!ScriptLoaded)
                    {
                        const string scriptPath = "_content/Toolbelt.Blazor.SpeechRecognition/script.min.js";
                        await _JSRuntime.InvokeVoidAsync("eval", "new Promise(r=>((d,t,s)=>(h=>h.querySelector(t+`[src=\"${s}\"]`)?r():(e=>(e.src=s,e.onload=r,h.appendChild(e)))(d.createElement(t)))(d.head))(document,'script','" + scriptPath + "'))");
                        ScriptLoaded = true;
                    }
                }
                finally { Syncer.Release(); }
            }
            return await _JSRuntime.InvokeAsync<T>(Prefix + identifier, args);
        }

        private DotNetObjectReference<SpeechRecognition> GetObjectRef()
        {
            if (_ObjectRefOfThis == null) _ObjectRefOfThis = DotNetObjectReference.Create(this);
            return _ObjectRefOfThis;
        }

        internal SpeechRecognition Attach()
        {
            this.AvailableTask = InvokeJSAsync<bool>("attach", this.GetObjectRef());
            return this;
        }

        public ValueTask<bool> IsAvailableAsync()
        {
            return this.AvailableTask;
        }

        public ValueTask StartAsync()
        {
            return InvokeJSVoidAsync("start");
        }

        public ValueTask StopAsync()
        {
            return InvokeJSVoidAsync("stop");
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
}
