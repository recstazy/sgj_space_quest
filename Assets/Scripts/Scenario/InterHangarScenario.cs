using System.Collections;
using UnityEngine;

public class InterHangarScenario : BaseScenario
{
	[SerializeField]
	private float _afterStartDelay;

	[SerializeField]
	private PlayVoice _collegueHangarVoice;

	[SerializeField]
	private DoorTrigger _hangarDoor;

	private void Start()
	{
		_hangarDoor.IsAvailableNow = false;

    }

	public override void Run()
	{
		if (_isScenarioStarted) return;
        base.Run();
		StartCoroutine(StartScenario());
	}

	private IEnumerator StartScenario()
	{
        Debug.Log("Hangar start");

        yield return _collegueHangarVoice.Play();
		_questController.AddQuest(QuestsDescriptionContainer.HANGAR_INTER);
        _hangarDoor.IsAvailableNow = true;
	}

    public void EnterInHangar()
    {
        _questController.CompleteQuest(QuestsDescriptionContainer.HANGAR_INTER);
        Finish();
        Debug.Log("Hangar finished");
    }
}
