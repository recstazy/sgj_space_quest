using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class TuneGameStarter : GameController
{
    [SerializeField]
    private CinemachineVirtualCamera _camera;

    [SerializeField]
    private Trigger _startGameTrigger;

    [SerializeField]
    private InteractableWithTrigger _gameTrigger;

    [SerializeField]
    private PlayerTrigger _playerTrigger;
    [SerializeField]
    private TuneRadioGame _tuneGame;

    private bool _gameFinished = default(bool);

    private void Start()
    {
        _gameTrigger.IsAvailableNow = true;
        Trigger.OnTriggerInvoke += StartGameOnTriggerInvoke;
        _playerTrigger.OnPlayerTriggered += OnPlayerTrigger;
        _tuneGame.OnWin += OnWin;
    }
    
    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= StartGameOnTriggerInvoke;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetOutOfGame();
        }
    }

    private void OnWin()
    {
        FinishGame();
    }

    private void OnPlayerTrigger(bool isPlayer)
    {
        if (_gameFinished)
        {
            _gameTrigger.IsInteractionDisabled = true;
            return;
        }
        _gameTrigger.IsInteractionDisabled = !isPlayer;
    }

    private void GetOutOfGame()
    {
        _playerInputController.SetActivateInteraction();
        _camera.Priority = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _tuneGame.StopGame();
    }

    private void StartGameOnTriggerInvoke(Trigger trigger)
    {
        if (_startGameTrigger == trigger)
        {
            StartGame();

            _playerInputController.SetDeactivateInteraction();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _camera.Priority = 100;
            _gameTrigger.IsInteractionDisabled = true;
        }
    }

    protected override bool GetWinCondition()
    {
        return false;
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _gameFinished = true;
        GetOutOfGame();
        _tuneGame.StopGame();
        _gameTrigger.IsInteractionDisabled = true;
    }

    protected override void StartGame()
    {
        StartCoroutine(DelayBeforeStartGame());
    }

    private IEnumerator DelayBeforeStartGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        _tuneGame.StartGame();
    }
}
