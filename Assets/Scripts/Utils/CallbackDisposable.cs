using System;

public class CallbackDisposable : IDisposable
{
    private Action _onDispose;
    private bool _disposed;
        
    public CallbackDisposable(Action onDispose)
    {
        _onDispose = onDispose;
    }

    public void Dispose()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(CallbackDisposable));
            
        _disposed = true;
        _onDispose?.Invoke();
        _onDispose = null;
    }
}
