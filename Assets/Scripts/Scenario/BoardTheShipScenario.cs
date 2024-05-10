using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BoardTheShipScenario : BaseScenario
{
    [SerializeField]
    private float _afterStartDelay;

    [SerializeField]
    private PlayVoice _instructionVoice;

    [SerializeField]
    protected Trigger _interactShipTrigger;

    [SerializeField]
    private GameObject _exteriorShip;
    [SerializeField]
    private Ship _ship;
    [SerializeField]
    private InteractableWithTrigger _shipInteractableTrigger;

    [Inject]
    private SceneController _sceneController;
    [Inject]
    private Player _player;

    private Fader _fader => _sceneController.Fader;

    private void Start()
    {
        Trigger.OnTriggerInvoke += OnTriggered;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= OnTriggered;
    }

    private void OnTriggered(Trigger trigger)
    {
        if (trigger == _interactShipTrigger)
        {
            StartCoroutine(FinishQuest());
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
        Debug.Log("TuneRadioScenario start");
        yield return new WaitForSeconds(_afterStartDelay);
        yield return _instructionVoice.Play();
        _questController.AddQuest(QuestsDescriptionContainer.GO_TO_SHIP);
        _shipInteractableTrigger.IsAvailableNow = true;
    }

    private IEnumerator FinishQuest()
    {
        yield return _fader.DoFade(FadeType.EndScene);
        PutPlayerInShip();
        yield return _fader.DoFade(FadeType.StartScene);
        _questController.CompleteQuest(QuestsDescriptionContainer.GO_TO_SHIP);
        Debug.Log("TuneRadioScenario finished");
        Finish();
    }

    private void PutPlayerInShip()
    {
        _exteriorShip.SetActive(false);

        _ship.gameObject.SetActive(true);
        _player.GetComponent<CharacterController>().enabled = false;
        _player.transform.parent = _ship.PlayerPoint;
        _player.transform.localPosition = Vector3.zero;
    }
}
