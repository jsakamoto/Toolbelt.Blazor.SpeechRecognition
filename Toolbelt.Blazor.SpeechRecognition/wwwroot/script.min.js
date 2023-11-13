"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var SpeechRecognition;
        (function (SpeechRecognition) {
            const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
            SpeechRecognition.ready = import('./script.module.min.js?' + searchParam).then(m => {
                Object.assign(SpeechRecognition, {
                    attach: m.attach,
                    start: m.start,
                    stop: m.stop,
                    onresult: m.onresult,
                    onend: m.onend,
                });
            });
        })(SpeechRecognition = Blazor.SpeechRecognition || (Blazor.SpeechRecognition = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
