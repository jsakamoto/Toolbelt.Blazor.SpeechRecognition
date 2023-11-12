namespace Toolbelt.Blazor.SpeechRecognition;

public class SpeechRecognitionEventArgs
{
    public int ResultIndex { get; set; }

    public SpeechRecognitionResult[]? Results { get; set; }
}
