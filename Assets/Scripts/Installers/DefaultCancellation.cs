using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class DefaultCancellation : IDisposable
{
    private static CancellationTokenSource _cancellation;
    public static CancellationToken Token => _cancellation.Token;

    [Inject]
    private void Construct(DiContainer container)
    {
        Debug.Log($"{nameof(DefaultCancellation)}: Constructed");
        _cancellation = new();
    }
    
    public void Dispose()
    {
        Debug.Log($"{nameof(DefaultCancellation)}: Cancelling");
        _cancellation.Cancel();
        _cancellation.Dispose();
    }
}
