using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UniRx;
using UnityEngine;
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
        UpdateCurrentInteractable();
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;
        
        _interactables.Add(interactable);
        UpdateCurrentInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;
        
        _interactables.Remove(interactable);
        UpdateCurrentInteractable();
    }

    private void UpdateCurrentInteractable()
    {
        var mostAlignedWithViewDirection = _interactables
            .Where(x => !x.IsInteractionDisabled)
            .OrderByDescending(x => GetViewAlignFactor(_camera, x.transform))
            .FirstOrDefault();

        if (mostAlignedWithViewDirection == CurrentInteractable.Value) return;
        CurrentInteractable.Value = mostAlignedWithViewDirection;

        float GetViewAlignFactor(Camera viewCamera, Transform target)
        {
            var distanceVector = viewCamera.transform.position - target.position;
            var cameraViewDirection = viewCamera.transform.forward;
            return Vector3.Dot(distanceVector.normalized, cameraViewDirection.normalized);
        }
    }
}
