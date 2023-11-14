"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var SpeechRecognition;
        (function (SpeechRecognition) {
            const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
            SpeechRecognition.ready = import('./script.module.min.js?' + searchParam).then(m => {
                SpeechRecognition.createInstance = m.createInstance;
            });
        })(SpeechRecognition = Blazor.SpeechRecognition || (Blazor.SpeechRecognition = {}));
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
