export const createInstance = (dotnetObjRef) => {
    const w = window;
    const TSpeechRecognition = w.webkitSpeechRecognition || w.SpeechRecognition;
    const speechRecognition = TSpeechRecognition ? new TSpeechRecognition() : null;
    const falseFunc = () => false;
    const invokeMethodAsync = dotnetObjRef.invokeMethodAsync.bind(dotnetObjRef);
    if (!speechRecognition)
        return ({ available: falseFunc, start: falseFunc, stop: falseFunc });
    speechRecognition.onresult = (ev) => {
        invokeMethodAsync('_OnResult', {
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
    speechRecognition.onend = () => invokeMethodAsync('_OnEnd');
    return ({
        available: () => true,
        start: (options) => Object.assign(speechRecognition, options).start(),
        stop: () => speechRecognition.stop()
    });
};
