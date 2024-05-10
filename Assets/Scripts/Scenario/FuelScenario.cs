using System.Collections;
using UnityEngine;

public class FuelScenario : BaseScenario
{
	[SerializeField]
	private float _afterStartDelay;

    [SerializeField]
    private PlayVoice _getFuelCollegueVoice;

    [SerializeField]
    private Trigger _finishFindValveTrigger;

    [SerializeField]
    private Trigger _fuelGameFinalTrigger;

    private bool _isGetValve = default(bool);
	private bool _isFuelGameComplete = default(bool);

    public override void Run()
	{
		if (_isScenarioStarted) return;
        base.Run();
		StartCoroutine(GetFuelStart());
		Trigger.OnTriggerInvoke += GetValveTrigger;
		Trigger.OnTriggerInvoke += GetFuelGameTrigger;
    }

	private void OnDestroy()
	{
        Trigger.OnTriggerInvoke -= GetValveTrigger;
        Trigger.OnTriggerInvoke -= GetFuelGameTrigger;
    }

	private void GetValveTrigger(Trigger trigger)
	{
		if(_finishFindValveTrigger == trigger)
		{
            _isGetValve = true;
        }
	}

    private void GetFuelGameTrigger(Trigger trigger)
    {
        if (_fuelGameFinalTrigger == trigger)
        {
            _isFuelGameComplete = true;
        }
    }

    private IEnumerator GetFuelStart()
	{
        Debug.Log("GetFuelStart start");
        yield return new WaitForSeconds(_afterStartDelay);
        yield return _getFuelCollegueVoice.Play();
        _questController.AddQuest(QuestsDescriptionContainer.GET_FUEL);
        yield return new WaitUntil(() => _isGetValve);
        yield return new WaitUntil(() => _isFuelGameComplete);
        _questController.CompleteQuest(QuestsDescriptionContainer.GET_FUEL);
        Debug.Log("GetFuelStart finished");
        Finish();
	}
}
