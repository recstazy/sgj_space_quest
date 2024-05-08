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

    [Inject]
    private void Construct([Inject(Id = GameSceneInstaller.MainCameraId)] Camera mainCamera)
    {
        _camera = mainCamera;
    }
    
    private void Update()
    {
        if (!Input.GetButtonDown("Interaction")) return;
        InteractWith(CurrentInteractable.Value);
    }

    private async void InteractWith(IInteractable interactable)
    {
        if (CurrentInteractable.Value == null) return;
        await interactable.Interact(CancellationToken.None);
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;
        
        _interactables.Add(interactable);
        InteractableListChanged();
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;
        
        _interactables.Remove(interactable);
        InteractableListChanged();
    }

    private void InteractableListChanged()
    {
        Debug.Log($"List changed: {String.Join(", ", _interactables.Select(x => x.transform.gameObject.name))}");
        
        var mostAlignedWithViewDirection = _interactables
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
