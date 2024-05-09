using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class PlayerInteraction : MonoBehaviour
{
    private readonly List<IInteractable> _interactables = new();
    private Camera _camera;

    public ReactiveProperty<IInteractable> CurrentInteractable { get; private set; } = new();

    private readonly IntReactiveProperty _interactionDisableCounter = new();
    public readonly BoolReactiveProperty CanInteract = new();

    [Inject]
    private void Construct([Inject(Id = GameSceneInstaller.MainCameraId)] Camera mainCamera)
    {
        _camera = mainCamera;
        _interactionDisableCounter.Subscribe(x => CanInteract.Value = x == 0).AddTo(this);
    }
    
    private void Update()
    {
        UpdateCurrent();
        if (!Input.GetButtonDown("Interaction")) return;
        if (!CanInteract.Value) return;
        InteractWith(CurrentInteractable.Value);
    }

    public IDisposable DisableInteractionsTemporary()
    {
        _interactionDisableCounter.Value++;
        var disposer = new CallbackDisposable(() => _interactionDisableCounter.Value--);
        return disposer;
    }

    private async void InteractWith(IInteractable interactable)
    {
        if (CurrentInteractable.Value == null) return;
        using var disableInteractions = DisableInteractionsTemporary();
        await interactable.Interact(DefaultCancellation.Token);
        UpdateCurrent();
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;
        
        _interactables.Add(interactable);
        UpdateCurrent();
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;
        
        _interactables.Remove(interactable);
        UpdateCurrent();
    }

    //private void UpdateCurrentInteractable()
    //{
    //    var mostAlignedWithViewDirection = _interactables
    //        .Where(x => !x.IsInteractionDisabled)
    //        .Select(x => (interactable: x, align: GetViewAlignFactor(_camera, x.transform)))
    //        .Where(x => x.align >= 0f)
    //        .OrderByDescending(x => x.align)
    //        .FirstOrDefault().interactable;

    //    if (mostAlignedWithViewDirection == CurrentInteractable.Value) return;
    //    CurrentInteractable.Value = mostAlignedWithViewDirection;
    //}

    private void UpdateCurrent()
    {
        IInteractable mostAlignedInteractable = null;
        var bestAlign = -1f;

        foreach (var interactable in _interactables)
        {
            if (interactable.IsInteractionDisabled) continue;

            var alignment = GetViewAlignFactor(_camera, interactable.transform);
            if (alignment <= 0.3f) continue;

            if (alignment > bestAlign)
            {
                mostAlignedInteractable = interactable;
                bestAlign = alignment;
            }
        }

        if (mostAlignedInteractable == CurrentInteractable.Value) return;
        CurrentInteractable.Value = mostAlignedInteractable;
    }

    private float GetViewAlignFactor(Camera viewCamera, Transform target)
    {
        var distanceVector = target.position - viewCamera.transform.position;
        var cameraViewDirection = viewCamera.transform.forward;
        return Vector3.Dot(distanceVector.normalized, cameraViewDirection.normalized);
    }
}
