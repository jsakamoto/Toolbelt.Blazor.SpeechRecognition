interface Window {
    webkitSpeechRecognition: typeof SpeechRecognition | undefined,
    SpeechRecognition: typeof SpeechRecognition | undefined
}

interface DotNetObjectRef {
    invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
}

namespace Toolbelt.Blazor.SpeechRecognitionProxy {

    let speechRecognition: SpeechRecognition | null = null;
    let dotnetObjRef: DotNetObjectRef | null = null;

    export function attach(objRef: DotNetObjectRef): boolean {
        dotnetObjRef = objRef;
        const TSpeechRecognition = window.webkitSpeechRecognition || window.SpeechRecognition;
        if (speechRecognition === null && typeof (TSpeechRecognition) !== 'undefined') {
            speechRecognition = new TSpeechRecognition();

            speechRecognition.onresult = (ev) => {
                const results = [] as any[];
                for (let i = 0; i < ev.results.length; i++) {
                    const recognitionResult = ev.results[i];
                    const a = { isFinal: recognitionResult.isFinal, items: [] as SpeechRecognitionAlternative[] };
                    for (let j = 0; j < recognitionResult.length; j++) {
                        a.items.push({ confidence: recognitionResult[j].confidence, transcript: recognitionResult[j].transcript });
                    }
                    results.push(a);
                }
                const args = {
                    resultIndex: ev.resultIndex,
                    results: results
                };
                dotnetObjRef!.invokeMethodAsync('_OnResult', args);
            }
            speechRecognition.onend = () => {
                dotnetObjRef!.invokeMethodAsync('_OnEnd');
            }
        }
        return speechRecognition !== null;
    }

    export function lang(lang: string): void {
        if (speechRecognition !== null) {
            speechRecognition.lang = lang;
        }
    }

    export function continuous(continuous: boolean): void {
        if (speechRecognition !== null) {
            speechRecognition.continuous = continuous;
        }
    }

    export function interimResults(interimResults: boolean): void {
        if (speechRecognition !== null) {
            speechRecognition.interimResults = interimResults;
        }
    }

    export function start() {
        if (speechRecognition !== null) {
            speechRecognition.start();
        }
    }

    export function stop() {
        if (speechRecognition !== null) {
            speechRecognition.stop();
        }
    }
}