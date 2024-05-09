using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public string InteractionHint { get; private set; }

    public async UniTask Interact(CancellationToken cancellation)
    {
        Debug.Log($"{gameObject.name} interaction start");
        await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: cancellation);
        Debug.Log($"{gameObject.name} interaction end");
    }
}
