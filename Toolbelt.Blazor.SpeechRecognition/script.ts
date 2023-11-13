namespace Toolbelt.Blazor.SpeechRecognition {
    const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
    export var ready = import('./script.module.min.js?' + searchParam).then(m => {
        Object.assign(SpeechRecognition, {
            attach: m.attach,
            start: m.start,
            stop: m.stop,
            onresult: m.onresult,
            onend: m.onend,
        });
    });
}
