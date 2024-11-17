using AvaloniaVisualBasic.IDE;

namespace AvaloniaVisualBasic.Events;

public class TextInputForPropertyEvent(string? text) : IEvent
{
    public string? Text { get; } = text;
}