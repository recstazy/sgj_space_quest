using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class TuneRadioScenario : BaseScenario
{
	[SerializeField]
	private float _afterStartDelay;

	[SerializeField]
	private PlayVoice _instructionVoice;
    [SerializeField]
    private TuneRadioGame _tuneRadioGame;

    [SerializeField]
    private InteractableWithTrigger _radioInteractableTrigger;

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
        Debug.Log("TuneRadioScenario finished");
        Finish();
	}
}
