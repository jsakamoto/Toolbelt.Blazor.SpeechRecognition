namespace Toolbelt.Blazor.SpeechRecognition;

internal class SpeechRecognitionStartOptions
{
    public string? Lang { get; set; }
    public bool Continuous { get; set; }
    public bool InterimResults { get; set; }
}
