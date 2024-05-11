using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightScenario : BaseScenario
{
    [SerializeField]
    private float _afterStartDelay;

    [SerializeField]
    private PlayVoice _instructionVoice;

    [SerializeField]
    protected Trigger _startFlyTrigger;

    [SerializeField]
    private Tumbler _tumbler;

    [SerializeField]
    private Animator _shipAnimator;
    [SerializeField]
    private Animator _roofAnimator;

    private void Start()
    {
        _tumbler.IsInteractionDisabled = true;
        Trigger.OnTriggerInvoke += OnTriggered;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= OnTriggered;
    }

    private void OnTriggered(Trigger trigger)
    {
        if (trigger == _startFlyTrigger)
        {
            Trigger.OnTriggerInvoke -= OnTriggered;
            StartCoroutine(Flying());
        }
    }

    public override void Run()
    {
        if (_isScenarioStarted) return;
        base.Run();
        StartCoroutine(StartScenario());
    }

    private IEnumerator StartScenario()
    {
        Debug.Log("FlightScenario start");
        yield return new WaitForSeconds(_afterStartDelay);
        yield return _instructionVoice.Play();
        _questController.AddQuest(QuestsDescriptionContainer.SHIP_FLY_ON);

        _tumbler.IsInteractionDisabled = false;
        _tumbler.IsAvailableNow = true;
    }

    private IEnumerator Flying()
    {
        yield return new WaitForSeconds(1f);
        _shipAnimator.Play("RotateUp");
        _questController.CompleteQuest(QuestsDescriptionContainer.SHIP_FLY_ON);
        yield return new WaitForSeconds(3f);
        _roofAnimator.Play("OpenRoof");
        yield return new WaitForSeconds(3f);
        _shipAnimator.Play("Flight");

        Finish();
    }
}
