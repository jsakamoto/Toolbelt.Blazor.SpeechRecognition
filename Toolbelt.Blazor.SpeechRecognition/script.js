"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var SpeechRecognitionProxy;
        (function (SpeechRecognitionProxy) {
            var speechRecognition = null;
            var dotnetObjRef = null;
            function attach(objRef) {
                dotnetObjRef = objRef;
                var TSpeechRecognition = window.webkitSpeechRecognition || window.SpeechRecognition;
                if (speechRecognition === null && typeof (TSpeechRecognition) !== 'undefined') {
                    speechRecognition = new TSpeechRecognition();
                    speechRecognition.onresult = function (ev) {
                        var results = [];
                        for (var i = 0; i < ev.results.length; i++) {
                            var recognitionResult = ev.results[i];
                            var a = { isFinal: recognitionResult.isFinal, items: [] };
                            for (var j = 0; j < recognitionResult.length; j++) {
                                a.items.push({ confidence: recognitionResult[j].confidence, transcript: recognitionResult[j].transcript });
                            }
                            results.push(a);
                        }
                        var args = {
                            resultIndex: ev.resultIndex,
                            results: results
                        };
                        dotnetObjRef.invokeMethodAsync('_OnResult', args);
                    };
                    speechRecognition.onend = function () {
                        dotnetObjRef.invokeMethodAsync('_OnEnd');
                    };
                }
                return speechRecognition !== null;
            }
            SpeechRecognitionProxy.attach = attach;
            function lang(lang) {
                if (speechRecognition !== null) {
                    speechRecognition.lang = lang;
                }
            }
            SpeechRecognitionProxy.lang = lang;
            function continuous(continuous) {
                if (speechRecognition !== null) {
                    speechRecognition.continuous = continuous;
                }
            }
            SpeechRecognitionProxy.continuous = continuous;
            function interimResults(interimResults) {
                if (speechRecognition !== null) {
                    speechRecognition.interimResults = interimResults;
                }
            }
            SpeechRecognitionProxy.interimResults = interimResults;
            function start() {
                if (speechRecognition !== null) {
                    speechRecognition.start();
                }
            }
            SpeechRecognitionProxy.start = start;
            function stop() {
                if (speechRecognition !== null) {
                    speechRecognition.stop();
                }
            }
            SpeechRecognitionProxy.stop = stop;
        })(SpeechRecognitionProxy = Blazor.SpeechRecognitionProxy || (Blazor.SpeechRecognitionProxy = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
