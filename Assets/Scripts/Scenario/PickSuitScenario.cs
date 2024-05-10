using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSuitScenario : BaseScenario
{
	[SerializeField]
	private AudioSource _instructionSound;
	[SerializeField]
	private float _delayBeforeInstruction;
	[SerializeField]
	private Trigger _suitTrigger;

	public override void Run()
	{
        if (_isScenarioStarted) return;

        base.Run();
		StartCoroutine(Scenario());
		Trigger.OnTriggerInvoke += OnTriggered;
	}

	private void OnTriggered(Trigger trigger)
	{
		if (trigger == _suitTrigger)
		{
			Trigger.OnTriggerInvoke -= OnTriggered;
            //TODO: suit UI enable
            _questController.CompleteQuest(QuestsDescriptionContainer.SUIT_UP);
            Finish();
		}
	}

	private IEnumerator Scenario()
	{
		yield return new WaitForSeconds(_delayBeforeInstruction);
		if (_instructionSound)
			_instructionSound.Play();
	}
}
