using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSystem : MonoBehaviour
{
	[SerializeField]
	private List<BaseScenario> _scenarios;

	private BaseScenario _currentScenario;

	private void Start()
	{
		Trigger.OnTriggerInvoke += OnTriggered;
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
