using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.SpeechRecognition
{
    public class SpeechRecognition
    {
        private static readonly string Namespace = "Toolbelt.Blazor.SpeechRecognitionProxy";

        private readonly IJSRuntime JSRuntime;

        private DotNetObjectReference<SpeechRecognition> _ObjectRefOfThis;

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

        private DotNetObjectReference<SpeechRecognition> GetObjectRef()
        {
            if (_ObjectRefOfThis == null) _ObjectRefOfThis = DotNetObjectReference.Create(this);
            return _ObjectRefOfThis;
        }

        internal SpeechRecognition Attach()
        {
            // Console.WriteLine("M-1: Attach");
            this.Available = (this.JSRuntime as IJSInProcessRuntime).Invoke<bool>(Namespace + ".attach", this.GetObjectRef());
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
            // Console.WriteLine($"M-3: _OnResult: {JsonSerializer.ToString(args)}");
            this.Result?.Invoke(this, args);
        }
    }
}
