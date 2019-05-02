"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var SpeechRecognitionProxy;
        (function (SpeechRecognitionProxy) {
            var r = null;
            var o = null;
            function attach(obj) {
                console.log('A-1: called attach.');
                o = obj;
                var T = window.webkitSpeechRecognition || window.SpeechRecognition;
                console.log('A-2: T is:', T);
                if (r === null && typeof (T) !== 'undefined') {
                    r = new T();
                    r.onresult = function (ev) {
                        var rss = [];
                        var il = ev.results.length;
                        for (var i = 0; i < il; i++) {
                            var rs = ev.results[i];
                            var a = { isFinal: rs.isFinal, items: [] };
                            var jl = rs.length;
                            for (var j = 0; j < jl; j++) {
                                a.items.push({ confidence: rs[j].confidence, transcript: rs[j].transcript });
                            }
                            rss.push(a);
                        }
                        var evv = {
                            resultIndex: ev.resultIndex,
                            results: rss
                        };
                        console.log('A-4: onresult:', evv);
                        console.log('A-5:', JSON.stringify(evv));
                        o.invokeMethodAsync('_OnResult', evv);
                    };
                }
                console.log('A-3: r is:', r);
                return r !== null;
            }
            SpeechRecognitionProxy.attach = attach;
            function lang(l) {
                if (r !== null) {
                    console.log('A-8: lang:', l);
                    r.lang = l;
                }
            }
            SpeechRecognitionProxy.lang = lang;
            function continuous(f) {
                if (r !== null) {
                    console.log('A-9: continuous:', f);
                    r.continuous = f;
                }
            }
            SpeechRecognitionProxy.continuous = continuous;
            function interimResults(f) {
                if (r !== null) {
                    console.log('A-A: interimResults:', f);
                    r.interimResults = f;
                }
            }
            SpeechRecognitionProxy.interimResults = interimResults;
            function start() {
                if (r !== null) {
                    console.log('A-6: start');
                    r.start();
                }
            }
            SpeechRecognitionProxy.start = start;
            function stop() {
                if (r !== null) {
                    console.log('A-7: stop');
                    r.stop();
                }
            }
            SpeechRecognitionProxy.stop = stop;
        })(SpeechRecognitionProxy = Blazor.SpeechRecognitionProxy || (Blazor.SpeechRecognitionProxy = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
