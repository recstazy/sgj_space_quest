using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class TuneRadioScenario : BaseScenario
{
    [SerializeField]
    private float _afterStartDelay;
    [SerializeField]
    protected Trigger _completeMiniGameTrigger;

    [SerializeField]
    private PlayVoice _instructionVoice;
    [SerializeField]
    private TuneRadioGame _tuneRadioGame;

    [SerializeField]
    private InteractableWithTrigger _radioInteractableTrigger;

    private void Start()
    {
        Trigger.OnTriggerInvoke += OnTriggered;
        _radioInteractableTrigger.IsAvailableNow = false;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= OnTriggered;
    }

    private void OnTriggered(Trigger trigger)
    {
        if (trigger == _completeMiniGameTrigger)
        {
            _questController.CompleteQuest(QuestsDescriptionContainer.RADIO_CHECK);
            Debug.Log("TuneRadioScenario finished");
            Finish();
            Trigger.OnTriggerInvoke -= OnTriggered;
        }
    }

    public override void Run()
    {
        if (_isScenarioStarted) return;
        base.Run();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Debug.Log("TuneRadioScenario start");
        yield return new WaitForSeconds(_afterStartDelay);
        yield return _instructionVoice.Play();

        _tuneRadioGame.EnableNoise();

        _questController.AddQuest(QuestsDescriptionContainer.RADIO_CHECK);
        _radioInteractableTrigger.IsAvailableNow = true;
    }
}
