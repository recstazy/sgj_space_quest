using System.Collections;
using UnityEngine;

public class InterHangarScenario : BaseScenario
{
	[SerializeField]
	private float _afterStartDelay;

	[SerializeField]
	private PlayVoice _collegueHangarVoice;

    [SerializeField]
    private InteractableWithTrigger _scanerTrigger;

	[SerializeField]
	private Door _door;


    public override void Run()
	{
		if (_isScenarioStarted) return;
        base.Run();
		StartCoroutine(StartScenario());
	}

	private IEnumerator StartScenario()
	{
		yield return _collegueHangarVoice.Play();
		_questController.AddQuest(QuestsDescriptionContainer.HANGAR_INTER);
        _scanerTrigger.IsAvailableNow = true;
		yield return new WaitUntil(() => _scanerTrigger.IsInteractionDisabled);
        _questController.CompleteQuest(QuestsDescriptionContainer.HANGAR_INTER);
        _door.Open();
        Finish();
	}
}
