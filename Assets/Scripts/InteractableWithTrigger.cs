using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

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

	[SerializeField]
	private List<GameObject> _disabledObjects;

	[SerializeField]
	private UnityEvent _onInteracted;

	public string InteractionHint => _hint;

	public bool IsInteractionDisabled { get; set; }

	private bool _isAvailableNow = default(bool);


    public bool IsAvailableNow 
	{
		get 
		{
			return _isAvailableNow;
        }
		set
		{
			_isAvailableNow = value;
		}
	} 

	public async UniTask Interact(CancellationToken cancellation)
	{
		if (_isAvailableNow)
		{
            Debug.Log($"{gameObject.name} interaction start");
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: cancellation);
            _trigger.Invoke();
            Debug.Log($"{gameObject.name} interaction end");
            if (_interactOneTime)
                IsInteractionDisabled = true;
            if (_disableAfterInteract)
			{
				if (_disabledObjects.Count > 0)
				{
                    _disabledObjects.ForEach(obj => obj.SetActive(false));
                }
                gameObject.SetActive(false);
            }
        }
	}

}

