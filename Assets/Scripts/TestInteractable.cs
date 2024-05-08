using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public UniTask Interact(CancellationToken cancellation = default)
    {
        Debug.Log($"{gameObject.name} was interacted");
        return UniTask.CompletedTask;
    }
}
