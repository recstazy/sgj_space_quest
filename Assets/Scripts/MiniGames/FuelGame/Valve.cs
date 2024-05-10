using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Valve : MonoBehaviour, IInteractable
{
    public event Action OnFinishInterract;

    [field: SerializeField]
    public string InteractionHint { get; private set; }

    [SerializeField]
    private Trigger _valveFindStartTrigger;

    [SerializeField]
    private Trigger _valveFindEndTrigger;

    [SerializeField]
    private GameObject _valveVisual;

    [SerializeField]
    private Image _visualInfo;

    [SerializeField]
    private Image _visualSolution;

    [SerializeField]
    private Color _activateColor;

    [SerializeField]
    private Color _deactivateColor;

    [SerializeField]
    private float _activationTime;

    [SerializeField]
    private bool _isWinPosition;

    public bool IsInteractionDisabled { get; private set; }

    public bool IsValvePlaced { get; set; }

    public bool QuestedValveCondition { get; set; }

    private bool _a = false;

    private bool _currentValveCondition
    {
        get
        {
            return _a;
        }
        set
        {
            _a = value;
        }
    }

    public bool IsAvailableNow { get; set; } = true;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
  

    public void Init()
    {
        _visualSolution.color = QuestedValveCondition ? _activateColor : _deactivateColor;
        _startPosition = _valveVisual.transform.localRotation.eulerAngles;
        _endPosition = _valveVisual.transform.localRotation.eulerAngles +  new Vector3(170f, 0f, 0f);
        SetVisualValvePart();
        SetHitString();
        Trigger.OnTriggerInvoke += SetValveConditionByTrigger;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= SetValveConditionByTrigger;
    }

    public async UniTask Interact(CancellationToken cancellation)
    {
        if (IsAvailableNow)
        {
            if (IsValvePlaced)
            {
                _currentValveCondition = !_currentValveCondition;
                var rotate = _currentValveCondition ? _endPosition : _startPosition;
                Debug.Log($"{gameObject.name} interaction start");
                await _valveVisual.transform.DOLocalRotate(rotate, _activationTime).WithCancellation(cancellation);
                Debug.Log($"{gameObject.name} interaction end");
                SetVisualValvePart();
                OnFinishInterract?.Invoke();
            }
            else
            {
                _valveFindStartTrigger.Invoke();
            }
        }
    }

    private void SetValveConditionByTrigger(Trigger trigger)
    {
        if(trigger == _valveFindEndTrigger)
        {
            IsValvePlaced = true;
            SetVisualValvePart();
            SetHitString();
        }
    }

    private void SetHitString()
    {
        if (IsValvePlaced)
        {
            InteractionHint = "Крути";
        }
        else
        {
            InteractionHint = "Не хватает крутилки";
        }
    }

    public bool IsCorrectValveCondition()
    {
        _isWinPosition = QuestedValveCondition.Equals(_currentValveCondition);
        return _isWinPosition;
    }

    public void SetWinCondition()
    {
        IsInteractionDisabled = true;
    }

    private void SetVisualValvePart()
    {
        _valveVisual.SetActive(IsValvePlaced);
        if (IsValvePlaced)
        {
            _visualInfo.color = _currentValveCondition ? _activateColor : _deactivateColor;
        }
    }
}
