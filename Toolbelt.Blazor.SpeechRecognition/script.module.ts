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

let speechRecognition: SpeechRecognition | null = null;
let dotnetObjRef: DotNetObjectRef | null = null;
let w = window;

export const attach = (objRef: DotNetObjectRef): boolean => {
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
        }

        speechRecognition.onend = () => {
            dotnetObjRef!.invokeMethodAsync('_OnEnd');
        }
    }
    return speechRecognition !== null;
}

export const start = (options: SpeechRecognitionOptions) => {
    if (speechRecognition) {
        Object.assign(speechRecognition, options);
        speechRecognition.start();
    }
}

export const stop = () => {
    if (speechRecognition) {
        speechRecognition.stop();
    }
}
