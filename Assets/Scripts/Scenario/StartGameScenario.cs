using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class StartGameScenario : BaseScenario
{
	[SerializeField]
	private float _afterStartDelay;

	[SerializeField]
	private AudioSource _systemVoice;

    [SerializeField]
    private InteractableWithTrigger _scanerTrigger;

    public override void Run()
	{
		if (_isScenarioStarted) return;
        base.Run();
		StartCoroutine(StartGame());
	}

	private IEnumerator StartGame()
	{
		yield return new WaitForSeconds(_afterStartDelay);
        _systemVoice.Play();
        yield return new WaitForSeconds(1f);
		_questController.AddQuest(QuestsDescriptionContainer.SCAN_FACE);
        _scanerTrigger.IsAvailableNow = true;
        Finish();
	}
}
