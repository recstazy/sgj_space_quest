using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSystem : MonoBehaviour
{
	[SerializeField]
	private List<BaseScenario> _scenarios;
	[SerializeField]
	private bool _startFirstOnStart; 

	private BaseScenario _currentScenario;

	private void Start()
	{
		Trigger.OnTriggerInvoke += OnTriggered;
		if (_startFirstOnStart)
		{
			_scenarios[0].Run();
		}
	}

	private void OnTriggered(Trigger trigger)
	{
		foreach (var scenario in _scenarios)
		{
			if (scenario.StartTrigger == trigger)
			{
				scenario.Run();
				break;
			}
		}
	}

	private void OnDestroy()
	{
		Trigger.OnTriggerInvoke -= OnTriggered;
	}
}
