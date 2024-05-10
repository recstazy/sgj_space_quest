using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSuitScenario : BaseScenario
{
	[SerializeField]
	private PlayVoice _suitInstructionVoice;
	[SerializeField]
	private float _delayBeforeInstruction;
	[SerializeField]
	private Trigger _suitTrigger;

	[SerializeField]
	private InteractableWithTrigger _suitPickUpTrigger;

	private void Start()
	{
        _suitPickUpTrigger.IsInteractionDisabled = true;
    }

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
            _suitPickUpTrigger.IsInteractionDisabled = true;
            _questController.CompleteQuest(QuestsDescriptionContainer.SUIT_UP);
            Finish();
		}
	}

	private IEnumerator Scenario()
	{
		yield return new WaitForSeconds(_delayBeforeInstruction);
		yield return _suitInstructionVoice.Play();
		_suitPickUpTrigger.IsInteractionDisabled = false;
    }
}
