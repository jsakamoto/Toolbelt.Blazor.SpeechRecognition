using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.SpeechRecognition
{
    public class SpeechRecognition
    {
        private static readonly string Namespace = "Toolbelt.Blazor.SpeechRecognitionProxy";

        private readonly IJSRuntime JSRuntime;

        private DotNetObjectRef _ObjectRefOfThis;

        public event EventHandler<SpeechRecognitionEventArgs> Result;

        public bool Available { get; private set; }

        private string _Lang = "";

        public string Lang
        {
            get => _Lang;
            set { if (_Lang != value) { _Lang = value; JSRuntime.InvokeAsync<object>(Namespace + ".lang", _Lang); } }
        }

        private bool _Continuous;

        public bool Continuous
        {
            get => _Continuous;
            set { if (_Continuous != value) { _Continuous = value; JSRuntime.InvokeAsync<object>(Namespace + ".continuous", _Continuous); } }
        }


        private bool _InterimResults;

        public bool InterimResults
        {
            get => _InterimResults;
            set { if (_InterimResults != value) { _InterimResults = value; JSRuntime.InvokeAsync<object>(Namespace + ".interimResults", _InterimResults); } }
        }

        public SpeechRecognition(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        private DotNetObjectRef GetObjectRef()
        {
            if (_ObjectRefOfThis == null) _ObjectRefOfThis = new DotNetObjectRef(this);
            return _ObjectRefOfThis;
        }

        internal SpeechRecognition Attach()
        {
            Console.WriteLine("M-1: Attach");
            this.JSRuntime.InvokeAsync<bool>(Namespace + ".attach", this.GetObjectRef())
                .ContinueWith(t =>
                {
                    Console.WriteLine($"M-2: {t.Result}");
                    if (t.IsCompleted)
                    {
                        this.Available = t.Result;
                    }
                });
            return this;
        }

        public void Start()
        {
            this.JSRuntime.InvokeAsync<bool>(Namespace + ".start");
        }

        public void Stop()
        {
            this.JSRuntime.InvokeAsync<bool>(Namespace + ".stop");
        }

        [JSInvokable(nameof(_OnResult)), EditorBrowsable(EditorBrowsableState.Never)]
        public void _OnResult(SpeechRecognitionEventArgs args)
        {
            Console.WriteLine($"M-3: _OnResult: {Json.Serialize(args)}");
            this.Result?.Invoke(this, args);
        }
    }
}
