type SpeechRecognitionOptions = {
    lang: string;
    continuous: boolean;
    interimResults: boolean;
}

type SpeechRecognition = {
    start: () => void;
    stop: () => void;
    onresult: (ev: {
        resultIndex: number;
        results: SpeechRecognitionResultList;
    }) => any;
    onend: (ev: Event) => any;
} & SpeechRecognitionOptions;

declare global {
    interface Window {
        SpeechRecognition: (new () => SpeechRecognition) | undefined,
        webkitSpeechRecognition: (new () => SpeechRecognition) | undefined
    }
}

interface DotNetObjectRef {
    invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
}

export const createInstance = (dotnetObjRef: DotNetObjectRef) => {
    const w = window;
    const TSpeechRecognition = w.webkitSpeechRecognition || w.SpeechRecognition;
    const speechRecognition = TSpeechRecognition ? new TSpeechRecognition() : null;
    const falseFunc = () => false;
    const invokeMethodAsync = dotnetObjRef.invokeMethodAsync.bind(dotnetObjRef);

    if (!speechRecognition) return ({ available: falseFunc, start: falseFunc, stop: falseFunc });

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
    }

    speechRecognition.onend = () => invokeMethodAsync('_OnEnd');

    return ({
        available: () => true,
        start: (options: SpeechRecognitionOptions) => Object.assign(speechRecognition, options).start(),
        stop: () => speechRecognition.stop()
    });
}