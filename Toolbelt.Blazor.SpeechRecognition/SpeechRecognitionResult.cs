//using System.ComponentModel;

namespace Toolbelt.Blazor.SpeechRecognition;

public class SpeechRecognitionResult
{
    public bool IsFinal { get; set; }

    //[EditorBrowsable(EditorBrowsableState.Never)]
    public SpeechRecognitionAlternative[] Items { get; set; }

    // public SpeechRecognitionAlternative this[int index] { get => this.Items[index]; }
}
