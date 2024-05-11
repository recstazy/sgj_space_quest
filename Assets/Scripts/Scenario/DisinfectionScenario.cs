using System.Collections;
using UnityEngine;

public class DisinfectionScenario : BaseScenario
{
	[SerializeField]
	private AudioSource _scannerSound;
	[SerializeField]
	private AudioSource _disinfectionSound;
    [SerializeField]
    private PlayVoice _systemInformationVoiceBeforeDesinfection;
    [SerializeField]
    private PlayVoice _colleagueVoiceAfterDesinfection;

	[SerializeField]
	private ParticleSystem _particleSystem;

	[SerializeField]
	private Door _door1;

    [SerializeField]
    private Door _door2;

    [SerializeField]
	private float _disinfectionTime;

	public bool PlayerInBox { get; set; }

	public override void Run()
	{
        if (_isScenarioStarted) return;

        base.Run();
		StartCoroutine(Disinfection());
	}

	private IEnumerator Disinfection()
	{
        _scannerSound.Play();
        yield return new WaitForSeconds(_scannerSound.clip.length);
        _questController.CompleteQuest(QuestsDescriptionContainer.SCAN_FACE);
        Debug.Log("Disinfection");
		yield return _systemInformationVoiceBeforeDesinfection.Play();
        
        _questController.AddQuest(QuestsDescriptionContainer.DESINFECTION);
		yield return new WaitForSeconds(1f);
        _door1.Open();
		yield return new WaitForSeconds(1f);

		while (!PlayerInBox)
			yield return null;

		yield return new WaitForSeconds(0.5f);

        _door1.Close();
		yield return new WaitForSeconds(1f);

		_particleSystem.Play();
		_disinfectionSound.Play();

		yield return new WaitForSeconds(_disinfectionTime);
		_particleSystem.Stop();
        _door1.Open();
        _door2.Open();
        _questController.CompleteQuest(QuestsDescriptionContainer.DESINFECTION);

		yield return _colleagueVoiceAfterDesinfection.Play();
        _questController.AddQuest(QuestsDescriptionContainer.SUIT_UP);
        Finish();
	}


}
