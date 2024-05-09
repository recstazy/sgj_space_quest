using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    string InteractionHint { get; }
    bool IsInteractionDisabled { get; }
    UniTask Interact(CancellationToken cancellation);
}
