using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WireGameController : GameController, IInteractable
{
    [SerializeField]
    private List<Wire> _wires;

    [SerializeField]
    private CinemachineVirtualCamera _camera;

    [SerializeField]
    private Transform _door;

    [SerializeField]
    private PlayerTrigger _playerTrigger;

    [SerializeField]
    private Animator _tumblerAnimator;

    private Vector3 _doorStartPosition;

    private bool _gameFinished = default(bool);

    public string InteractionHint => _hint;

    public bool IsInteractionDisabled => _isInteractionDisabled;

    public bool IsAvailableNow { get; set; }

    private bool _isInteractionDisabled = false;
    private string _hint;

    private void Start()
    {
        _doorStartPosition = _door.rotation.eulerAngles;
        //Trigger.OnTriggerInvoke += StartGameOnTriggerInvoke;
        _playerTrigger.OnPlayerTriggered += OnPlayerTrigger;
    }

    private void OnPlayerTrigger(bool isPlayer)
    {
        if (_gameFinished)
        {
            _isInteractionDisabled = true;
            return;
        }
        _isInteractionDisabled = !isPlayer;
    }

    private void OnDestroy()
    {
        //Trigger.OnTriggerInvoke -= StartGameOnTriggerInvoke;
        foreach (var wire in _wires)
        {
            wire.OnPlaced -= CheckWinCondition;
        }
    }

    public async UniTask Interact(CancellationToken cancellation)
    {
        if (IsAvailableNow)
        {
            foreach (var wire in _wires)
            {
                wire.OnPlaced += CheckWinCondition;
            }

            _playerInputController.SetDeactivateInteraction();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _camera.Priority = 100;
            _isInteractionDisabled = true;
            await _door.DORotate(new Vector3(0, 250f, 0), 0.3f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetOutOfGame();
        }
    }

    private void GetOutOfGame()
    {
        _playerInputController.SetActivateInteraction();
        _camera.Priority = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        _gameFinished = true;
        GetOutOfGame();
        _door.DORotate(_doorStartPosition, 0.3f);
        _isInteractionDisabled = true;
        _tumblerAnimator.Play("SwitchOn");
    }

    protected override void StartGame()
    {
        
    }
}
