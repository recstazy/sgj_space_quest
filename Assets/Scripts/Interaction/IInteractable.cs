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
    bool IsAvailableNow { get; set; }
    UniTask Interact(CancellationToken cancellation);
}
