using System.Collections;
using UnityEngine;

public class FinalPreparationsScenario : BaseScenario
{
	[SerializeField]
	private QuestView _finalPreparationsQuestViewPrefab;
	
	[SerializeField]
	private float _afterStartDelay;

	[SerializeField]
	private PlayVoice _finalPreparationSystemVoice;

    [SerializeField]
    private PlayVoice _finalPreparationCollegueVoice;

    public override void Run()
	{
		if (_isScenarioStarted) return;
        base.Run();
		StartCoroutine(FinalPreparationsStart());
	}

	private IEnumerator FinalPreparationsStart()
	{
		Debug.Log("FinalPreparationsScenario start");
		yield return new WaitForSeconds(_afterStartDelay);
		yield return _finalPreparationSystemVoice.Play();
        
		_questController.AddQuest(QuestsDescriptionContainer.FINAL_PREPORATIONS, prefabOverride: _finalPreparationsQuestViewPrefab);
        
		yield return new WaitForSeconds(1f);
		yield return _finalPreparationCollegueVoice.Play();

        _questController.AddQuest(QuestsDescriptionContainer.ELECTRICITY_CHECK);

        Debug.Log("FinalPreparationsScenario finished");
        Finish();
	}
}
