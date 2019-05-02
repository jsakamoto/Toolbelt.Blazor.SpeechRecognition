interface Window {
    webkitSpeechRecognition: typeof SpeechRecognition | undefined,
    SpeechRecognition: typeof SpeechRecognition | undefined
}

interface DotNetObjectRef {
    invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
}

namespace Toolbelt.Blazor.SpeechRecognitionProxy {

    let r: SpeechRecognition | null = null;
    let o: DotNetObjectRef | null = null;

    export function attach(obj: DotNetObjectRef): boolean {
        console.log('A-1: called attach.');
        o = obj;
        const T = window.webkitSpeechRecognition || window.SpeechRecognition;
        console.log('A-2: T is:', T);
        if (r === null && typeof (T) !== 'undefined') {
            r = new T();

            r.onresult = (ev) => {
                const rss = [] as any[];
                const il = ev.results.length;
                for (let i = 0; i < il; i++) {
                    const rs = ev.results[i];
                    const a = { isFinal: rs.isFinal, items: [] as SpeechRecognitionAlternative[] };
                    const jl = rs.length;
                    for (let j = 0; j < jl; j++) {
                        a.items.push({ confidence: rs[j].confidence, transcript: rs[j].transcript });
                    }
                    rss.push(a);
                }
                const evv = {
                    resultIndex: ev.resultIndex,
                    results: rss
                };
                console.log('A-4: onresult:', evv);
                console.log('A-5:', JSON.stringify(evv));
                o!.invokeMethodAsync('_OnResult', evv);
            }
        }
        console.log('A-3: r is:', r);
        return r !== null;
    }

    export function lang(l: string): void {
        if (r !== null) {
            console.log('A-8: lang:', l);
            r.lang = l;
        }
    }

    export function continuous(f: boolean): void {
        if (r !== null) {
            console.log('A-9: continuous:', f);
            r.continuous = f;
        }
    }

    export function interimResults(f: boolean): void {
        if (r !== null) {
            console.log('A-A: interimResults:', f);
            r.interimResults = f;
        }
    }

    export function start() {
        if (r !== null) {
            console.log('A-6: start');
            r.start();
        }
    }

    export function stop() {
        if (r !== null) {
            console.log('A-7: stop');
            r.stop();
        }
    }
}