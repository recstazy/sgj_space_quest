using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WireGameController : GameController
{
    [SerializeField]
    private List<Wire> _wires;

    [SerializeField]
    private CinemachineVirtualCamera _camera;

    [SerializeField]
    private Trigger _startWireGameTrigger;

    [SerializeField]
    private InteractableWithTrigger _gameTrigger;

    [SerializeField]
    private Transform _door;

    private void Start()
    {
        _camera.gameObject.SetActive(false);
        _gameTrigger.IsAvailableNow = true;
        Trigger.OnTriggerInvoke += StartGameOnTriggerInvoke;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= StartGameOnTriggerInvoke;
    }

    private void StartGameOnTriggerInvoke(Trigger trigger)
    {
        _playerInputController.SetDeactivateInteraction();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _camera.gameObject.SetActive(true);
        _door.DORotate(new Vector3(0, 160f, 0), 0.3f);
        if (_startWireGameTrigger == trigger)
        {
            foreach (var wire in _wires)
            {
                wire.OnPlaced += CheckWinCondition;
            }
        }
    }

    protected override bool GetWinCondition()
    {
        foreach (var wire in _wires)
        {
            if (!wire.IsPlaced)
            {
                return false;
            }
        }
        return true;
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        _playerInputController.SetActivateInteraction();
        _camera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _door.DORotate(new Vector3(0, 0, 0), 0.3f);
        _gameTrigger.IsInteractionDisabled = true;
    }

    protected override void StartGame()
    {
        
    }
}
