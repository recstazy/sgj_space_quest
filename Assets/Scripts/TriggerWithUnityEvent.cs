using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerWithUnityEvent : MonoBehaviour
{
	[SerializeField]
	private string _targetTag = "Player";
	[SerializeField]
	private UnityEvent _event;

	[SerializeField]
	private Trigger _trigger;

    private bool _isTriggered;

	private void OnTriggerEnter(Collider other)
	{
		if (_isTriggered)
			return;

		if (other.CompareTag(_targetTag))
		{
			_event?.Invoke();
			_trigger?.Invoke();
			_isTriggered = true;
		}
	}
}
