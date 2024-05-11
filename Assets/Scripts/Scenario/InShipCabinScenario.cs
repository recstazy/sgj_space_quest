using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InShipCabinScenario : BaseScenario
{
    [SerializeField]
    private float _afterStartDelay;

    [SerializeField]
    private PlayVoice _instructionVoice;

    [SerializeField]
    private PlayerMovement _playerMovement;

    [SerializeField]
    protected Trigger _enablePowerTrigger;
    [SerializeField]
    protected Trigger _enableFuelTrigger;
    [SerializeField]
    protected Trigger _enableRadioTrigger;

    [SerializeField]
    private List<Tumbler> _interactables;

    private List<Trigger> _triggersToWait;

    private void Start()
    {
        Trigger.OnTriggerInvoke += OnTriggered;
        _triggersToWait = new List<Trigger>
        {
            _enableRadioTrigger,
            _enableFuelTrigger,
            _enablePowerTrigger
        };
        _interactables.ForEach(x => { x.IsInteractionDisabled = true; });
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= OnTriggered;
    }

    private void OnTriggered(Trigger trigger)
    {
        if (trigger == _enablePowerTrigger)
            _questController.CompleteQuest(QuestsDescriptionContainer.SHIP_ENGINE_ON);
        if (trigger == _enableFuelTrigger)
            _questController.CompleteQuest(QuestsDescriptionContainer.GET_FUEL);
        if (trigger == _enableRadioTrigger)
            _questController.CompleteQuest(QuestsDescriptionContainer.SHIP_SPICE_ON);

        _triggersToWait.Remove(trigger);
        if (_triggersToWait.Count == 0)
        {
            Trigger.OnTriggerInvoke -= OnTriggered;
            Debug.Log("InShipCabinScenario finished");
            _interactables.ForEach(x => { x.IsInteractionDisabled = true; });
            Finish();
        }
    }

    public override void Run()
    {
        if (_isScenarioStarted) return;
        base.Run();
        _playerMovement.DisableMovement();
        StartCoroutine(StartScenario());
    }

    private IEnumerator StartScenario()
    {
        Debug.Log("InShipCabinScenario start");
        yield return new WaitForSeconds(_afterStartDelay);
        yield return _instructionVoice.Play();
        _questController.AddQuest(QuestsDescriptionContainer.SHIP_ENGINE_ON);
        yield return new WaitForSeconds(0.25f);
        _questController.AddQuest(QuestsDescriptionContainer.GET_FUEL);
        yield return new WaitForSeconds(0.25f);
        _questController.AddQuest(QuestsDescriptionContainer.SHIP_SPICE_ON);
        _interactables.ForEach(x => { x.IsInteractionDisabled = false; x.IsAvailableNow = true; });
    }
}
