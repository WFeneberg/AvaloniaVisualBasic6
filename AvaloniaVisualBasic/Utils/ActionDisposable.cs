using System;

namespace AvaloniaVisualBasic.Utils;

public class ActionDisposable : IDisposable
{
    private readonly Action onDispose;

    public ActionDisposable(Action onDispose)
    {
        this.onDispose = onDispose;
    }

    public void Dispose()
    {
        onDispose?.Invoke();
    }
}