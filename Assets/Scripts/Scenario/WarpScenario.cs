using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using Zenject;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WarpScenario : BaseScenario
{
    [Inject]
    private PlayerInputController _playerInputController;

    [SerializeField]
	private float _afterStartDelay;

	[SerializeField]
	private PlayVoice _collegueStartVoice;

	[SerializeField]
	private float _aftepStartVoiceDelay = 5f;

    [SerializeField]
    private PlayVoice _collegueAfterDelayVoice;

    [SerializeField]
    private PlayVoice _colleguePreWarpVoice;

    [SerializeField]
    private PlayVoice _colleguePizdecWarpVoice;

    [SerializeField]
    private Trigger _startWarpTrigger;

    [SerializeField]
    private InteractableWithTrigger _warpOnInteractItem;

    [SerializeField]
    private WarpEffect _warpEffect;

    private bool _isWarpOn = default(bool);

    private void Start()
    {
        //_playerInputController.SetDeactivateInteraction();
        _warpOnInteractItem.IsAvailableNow = false;
        Trigger.OnTriggerInvoke += OnTrigger;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= OnTrigger;
    }

    private void OnTrigger(Trigger trigger)
    {
        if(_startWarpTrigger == trigger)
        {
            _isWarpOn = true;
        }
    }

    public override void Run()
	{
		if (_isScenarioStarted) return;
        base.Run();
		StartCoroutine(StartWarpScenario());
	}

	private IEnumerator StartWarpScenario()
	{
        Debug.Log("StartGameScenario start");

        yield return new WaitForSeconds(_afterStartDelay);
        Debug.Log("_collegueStartVoice start");
        yield return _collegueStartVoice.Play();
		yield return new WaitForSeconds(_aftepStartVoiceDelay);
        Debug.Log("_collegueAfterDelayVoice start");
        yield return _collegueAfterDelayVoice.Play();
        _questController.AddQuest(QuestsDescriptionContainer.SHIP_WARP_ON);
        _warpOnInteractItem.IsAvailableNow = true;
        yield return new WaitUntil(() => _isWarpOn);
        Debug.Log("_isWarpOn start");
        _questController.CompleteQuest(QuestsDescriptionContainer.SHIP_WARP_ON);
        _warpEffect.PlayAnimation();
        yield return new WaitUntil(() => _warpEffect.IsPrewarpStateComplete);
        Debug.Log("IsPrewarpStateComplete start");
        yield return _colleguePreWarpVoice.Play();
        yield return new WaitUntil(() => _warpEffect.IsPizdecStateComplete);
        Debug.Log("IsPizdecStateComplete start");
        yield return _colleguePizdecWarpVoice.Play();
        Debug.Log("StartGameScenario finished");
        Finish();
	}
}
