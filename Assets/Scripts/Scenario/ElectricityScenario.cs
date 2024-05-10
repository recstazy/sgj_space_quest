using System.Collections;
using UnityEngine;

public class ElectricityScenario : BaseScenario
{
	[SerializeField]
	private float _afterStartDelay;

    [SerializeField]
    private PlayVoice _electricityCheckCollegueVoice;

	[SerializeField]
	private Trigger _getCardTrigger;

    [SerializeField]
    private Trigger _wireGameFinalTrigger;

    [SerializeField]
    private WireGameController _electricalPanel;

    [SerializeField]
    private InteractableWithTrigger _cardTrigger;

    private bool _isGetCard = default(bool);
	private bool _isWireGameComplete = default(bool);

    private void Start()
    {
        _electricalPanel.IsAvailableNow = false;
        _cardTrigger.IsAvailableNow = false;
    }

    public override void Run()
	{
		if (_isScenarioStarted) return;
        base.Run();
		StartCoroutine(ElectricityCheckStart());
		Trigger.OnTriggerInvoke += GetCardTrigger;
		Trigger.OnTriggerInvoke += GetWireGameTrigger;
    }

	private void OnDestroy()
	{
        Trigger.OnTriggerInvoke -= GetCardTrigger;
        Trigger.OnTriggerInvoke -= GetWireGameTrigger;
    }

	private void GetCardTrigger(Trigger trigger)
	{
		if(_getCardTrigger == trigger)
		{
            _electricalPanel.IsAvailableNow = true;
            _isGetCard = true;
        }
	}

    private void GetWireGameTrigger(Trigger trigger)
    {
        if (_wireGameFinalTrigger == trigger)
        {
            _isWireGameComplete = true;
        }
    }

    private IEnumerator ElectricityCheckStart()
	{
		Debug.Log("ElectricityCheckStart start");
		yield return new WaitForSeconds(_afterStartDelay);
        Debug.Log("Collega popizdel start");
        yield return _electricityCheckCollegueVoice.Play();
        Debug.Log("Collega popizdel finish");
        _questController.AddQuest(QuestsDescriptionContainer.FIND_CARD);
        Debug.Log("find card start");
        _cardTrigger.IsAvailableNow = true;
        yield return new WaitUntil(() => _isGetCard);
        _questController.CompleteQuest(QuestsDescriptionContainer.FIND_CARD);
        Debug.Log("find card finish");
        Debug.Log("wire game start");
        yield return new WaitUntil(() => _isWireGameComplete);
        _questController.CompleteQuest(QuestsDescriptionContainer.ELECTRICITY_CHECK);
        Debug.Log("wire game final");

        Debug.Log("ElectricityCheckStart finished");
        Finish();
	}
}
