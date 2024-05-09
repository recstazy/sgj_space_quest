using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class InteractableWithTrigger : MonoBehaviour, IInteractable
{
	[SerializeField]
	private float _delay;
	[SerializeField]
	private Trigger _trigger;
	[SerializeField]
	private string _hint;

	[SerializeField]
	private bool _interactOneTime;
	[SerializeField]
	private bool _disableAfterInteract;

	private bool _isInteractionDisabled;

	public string InteractionHint => _hint;

	public bool IsInteractionDisabled => _isInteractionDisabled;

	public bool IsAvailableNow { get; set; } = false;
	public async UniTask Interact(CancellationToken cancellation)
	{
		if (IsAvailableNow)
		{
            Debug.Log($"{gameObject.name} interaction start");
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: cancellation);
            _trigger.Invoke();
            Debug.Log($"{gameObject.name} interaction end");
            if (_interactOneTime)
                _isInteractionDisabled = true;
            if (_disableAfterInteract)
                gameObject.SetActive(false);
        }
	}
}

