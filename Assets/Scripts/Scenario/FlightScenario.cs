using System;
using System.Collections;
using System.Collections.Generic;
using Overload;
using UnityEngine;
using Zenject;

public class FlightScenario : BaseScenario
{
    [SerializeField]
    private float _afterStartDelay;

    [SerializeField]
    private PlayVoice _instructionVoice;

    [SerializeField]
    protected Trigger _startFlyTrigger;

    [SerializeField]
    private Tumbler _tumbler;

    [SerializeField]
    private Animator _shipAnimator;
    [SerializeField]
    private Animator _roofAnimator;

    [SerializeField]
    private SustainedSound _roofSound;
    [SerializeField]
    private AudioSource _engineStartSound;
    [SerializeField]
    private AudioSource _engineBoomSound;
    [SerializeField]
    private GameObject _blackScreen;

    private SceneController _sceneController;

    [Inject]
    private void Construct(SceneController sceneController)
    {
        _sceneController = sceneController;
    }
    
    private void Start()
    {
        _tumbler.IsInteractionDisabled = true;
        Trigger.OnTriggerInvoke += OnTriggered;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= OnTriggered;
    }

    private void OnTriggered(Trigger trigger)
    {
        if (trigger == _startFlyTrigger)
        {
            Trigger.OnTriggerInvoke -= OnTriggered;
            StartCoroutine(Flying());
        }
    }

    public override void Run()
    {
        if (_isScenarioStarted) return;
        base.Run();
        StartCoroutine(StartScenario());
    }

    private IEnumerator StartScenario()
    {
        Debug.Log("FlightScenario start");
        yield return new WaitForSeconds(_afterStartDelay);
        yield return _instructionVoice.Play();
        _questController.AddQuest(QuestsDescriptionContainer.SHIP_FLY_ON);

        _tumbler.IsInteractionDisabled = false;
        _tumbler.IsAvailableNow = true;
    }

    private IEnumerator Flying()
    {
        _engineStartSound.Play();
        yield return new WaitForSeconds(1f);
        _shipAnimator.Play("RotateUp");
        _questController.CompleteQuest(QuestsDescriptionContainer.SHIP_FLY_ON);
        yield return new WaitForSeconds(3f);
        _roofSound.Play();
        _roofAnimator.Play("OpenRoof");
        yield return new WaitForSeconds(3f);
        _roofSound.Stop();
        _shipAnimator.Play("Flight");

        yield return new WaitForSeconds(3f);
        _engineBoomSound.Play();
        yield return new WaitForSeconds(0.5f);
        _engineStartSound.Stop();
        _blackScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        Finish();
        
        _sceneController.LoadNewScene();
    }
}
