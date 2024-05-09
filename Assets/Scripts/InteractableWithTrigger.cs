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

	public async UniTask Interact(CancellationToken cancellation)
	{
		Debug.Log($"{gameObject.name} interaction start");
		await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: cancellation);
		_trigger.Invoke();
		Debug.Log($"{gameObject.name} interaction end");
	}
}

