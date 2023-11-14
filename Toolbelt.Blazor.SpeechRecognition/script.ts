namespace Toolbelt.Blazor.SpeechRecognition {
    const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
    export var ready = import('./script.module.min.js?' + searchParam).then(m => {
        (SpeechRecognition as any).createInstance = m.createInstance;
    });
}
