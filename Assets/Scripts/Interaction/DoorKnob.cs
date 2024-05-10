using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DoorKnob : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Door _door;

    public string InteractionHint => _door.IsOpened ? "Закрыть" : "Открыть";
    public bool IsInteractionDisabled { get; }
    public bool IsAvailableNow { get; set; }

    private void Awake()
    {
        if (_door == null)
        {
            _door = GetComponentInParent<Door>();
        }
    }

    public UniTask Interact(CancellationToken cancellation)
    {
        if (_door == null) return UniTask.CompletedTask;
        
        if (_door.IsOpened) _door.Close();
        else _door.Open();
        return UniTask.CompletedTask;
    }
}
