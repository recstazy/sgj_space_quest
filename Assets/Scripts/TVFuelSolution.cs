using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class TVFuelSolution : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string _hint;

    [SerializeField]
    private CanvasGroup _solutionImage;

    [SerializeField]
    private float _animationTime = 0.3f;

    [SerializeField]
    private PlayerTrigger _playerTrigger;

    [SerializeField]
    private UnityEvent _onInteracted;

    public string InteractionHint => _hint;

    public bool IsInteractionDisabled => _isInteractionDisabled;

    public bool IsAvailableNow { get; set ; }

    private bool _isInteractionDisabled = default(bool);
    private bool _isActive = default(bool);

    private void Start()
    {
        _playerTrigger.OnPlayerTriggered += SetInterractTrigger;
        IsAvailableNow = true;
        _solutionImage.DOFade(0, 0);
    }

    private void OnDestroy()
    {
        _playerTrigger.OnPlayerTriggered -= SetInterractTrigger;
    }

    private void SetInterractTrigger(bool isActive)
    {
        _isInteractionDisabled = !isActive;
    }

    public async UniTask Interact(CancellationToken cancellation)
    {
        if (IsAvailableNow)
        {
            IsAvailableNow = false;
            _isActive = !_isActive;
            _onInteracted.Invoke();
            var fade = _isActive ? 1 : 0;
            await _solutionImage.DOFade(fade, _animationTime).WithCancellation(cancellation);
            IsAvailableNow = true;
        }
    }
}
