let speechRecognition = null;
let dotnetObjRef = null;
let w = window;
export const attach = (objRef) => {
    dotnetObjRef = objRef;
    const TSpeechRecognition = w.webkitSpeechRecognition || w.SpeechRecognition;
    if (!speechRecognition && TSpeechRecognition) {
        speechRecognition = new TSpeechRecognition();
        speechRecognition.onresult = (ev) => {
            dotnetObjRef?.invokeMethodAsync('_OnResult', {
                resultIndex: ev.resultIndex,
                results: Array.from(ev.results).map(result => ({
                    isFinal: result.isFinal,
                    items: Array.from(result).map(item => ({
                        confidence: item.confidence,
                        transcript: item.transcript
                    }))
                }))
            });
        };
        speechRecognition.onend = () => {
            dotnetObjRef.invokeMethodAsync('_OnEnd');
        };
    }
    return speechRecognition !== null;
};
export const start = (options) => {
    if (speechRecognition) {
        Object.assign(speechRecognition, options);
        speechRecognition.start();
    }
};
export const stop = () => {
    if (speechRecognition) {
        speechRecognition.stop();
    }
};
