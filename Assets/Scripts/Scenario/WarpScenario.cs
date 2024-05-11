using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

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
    private Tumbler _warpOnInteractItem;

    [SerializeField]
    private WarpEffect _warpEffect;

    [SerializeField]
    private PlayerMovement _playerMovement;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _beforeWaitForPizedecScream;

    [SerializeField]
    private float _waitForPizedecScream;

    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;

    [SerializeField]
    private CinemachineVirtualCamera _warpCamera;

    [SerializeField]
    private float _noiseTimeCoeficient;

    [SerializeField]
    private RotateAfterWarp _rotator;

    [SerializeField]
    private GameObject _earth;

    [SerializeField]
    private Animator _listWithDraw;

    private bool _isWarpOn = default(bool);

    private void Start()
    {
        _rotator.IsRotate = false;
        _earth.SetActive(false);
        _playerCamera.Priority = 10;
        _warpCamera.Priority = 0;
        _playerMovement.LockLookingAngle();
        _characterController.enabled = false;
        _warpOnInteractItem.IsInteractionDisabled = true;
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
        yield return new WaitForSeconds(1f);
        _warpOnInteractItem.IsInteractionDisabled = false;
        _warpOnInteractItem.IsAvailableNow = true;
        yield return new WaitUntil(() => _isWarpOn);
        Debug.Log("_isWarpOn start");
        _questController.CompleteQuest(QuestsDescriptionContainer.SHIP_WARP_ON);
        _warpEffect.PlayAnimation();
        yield return new WaitUntil(() => _warpEffect.IsSystemVoiceStateComplete);
        StartCoroutine(CameraAnimation());
        Debug.Log("IsPrewarpStateComplete start");
        yield return new WaitForSeconds(_beforeWaitForPizedecScream);
        yield return _colleguePreWarpVoice.Play();
        yield return new WaitForSeconds(_waitForPizedecScream);
        yield return _colleguePizdecWarpVoice.Play();
        yield return new WaitUntil(() => _warpEffect.IsPizdecStateComplete);
        Debug.Log("IsPizdecStateComplete start");
        Debug.Log("StartGameScenario finished");
        _rotator.IsRotate = true;
        _earth.SetActive(true);
        //ZEMLYA EBASHIT I PAPER VILEZAET
        yield return new WaitForSeconds(5F);
        _listWithDraw.enabled = true;
        Finish();
	}
    

    private IEnumerator CameraAnimation()
    {
        _playerInputController.SetDeactivateInteraction();
        _warpCamera.Priority = 11;

        var noise = _warpCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        var noiseAmp = 0f;
        //var freq = 0f;

        while (noiseAmp < 0.07)
        {
            noiseAmp += (Time.deltaTime /_warpEffect.PreWarpTime);
            //freq += (Time.deltaTime / _warpEffect.PreWarpTime);
            noise.m_AmplitudeGain = noiseAmp;
            //noise.m_FrequencyGain = freq;
            yield return null;
        }
        yield return new WaitUntil(() => _warpEffect.IsPrewarpStateComplete);
        while (noiseAmp < 0.25)
        {
            noiseAmp += Time.deltaTime / _warpEffect.WarpTime;
            //freq += Time.deltaTime / _warpEffect.WarpTime;
            noise.m_AmplitudeGain = noiseAmp;
            //noise.m_FrequencyGain = Mathf.Clamp(freq, 0, 2);
            yield return null;
        }
        yield return new WaitUntil(() => _warpEffect.IsWarpOver);

        while (noiseAmp > 0)
        {
            noiseAmp -= Time.deltaTime / 2;
            //freq += Time.deltaTime / _warpEffect.WarpTime;
            noise.m_AmplitudeGain = noiseAmp;
            //noise.m_FrequencyGain = Mathf.Clamp(freq, 0, 2);
            yield return null;
        }
        //_playerInputController.SetActivateInteraction();
        //_warpCamera.Priority = 0;
    }
}
