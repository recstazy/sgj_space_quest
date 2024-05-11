using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class InteractionView : MonoBehaviour
{
    [SerializeField]
    private string _defaultHintText = "Взаимодействовать";
    
    [SerializeField]
    private RectTransform _hintOrigin;

    [SerializeField]
    private TMP_Text _hintText;

    [SerializeField]
    private GameObject _activeTriggerImage;

    [SerializeField]
    private GameObject _inactiveTriggerImage;

    [SerializeField]
    private CanvasGroup _hintCanvasGroup;

    [SerializeField]
    private float _notInteractiveAlpha = 0.5f;

    private PlayerInteraction _interaction;
    private CompositeDisposable _currentHintDisposable;
    private Camera _camera;

    [Inject]
    private void Construct(Player player, [Inject(Id = GameSceneInstaller.MainCameraId)] Camera mainCamera)
    {
        _interaction = player.GetComponentInChildren<PlayerInteraction>();
        _camera = mainCamera;
    }

    private void Awake()
    {
        _interaction.CurrentInteractable.Subscribe(InteractableChanged).AddTo(this);
        _interaction.CanInteract.Subscribe(canInteract =>
        {
            _hintCanvasGroup.alpha = canInteract ? 1f : _notInteractiveAlpha;
        }).AddTo(this);
    }

    private void Update()
    {
        if (_interaction != null && _interaction.CurrentInteractable.Value != null)
        {
            var interactable = _interaction.CurrentInteractable.Value;
            _activeTriggerImage.SetActive(interactable.IsAvailableNow);
            _inactiveTriggerImage.SetActive(!interactable.IsAvailableNow);
            _hintCanvasGroup.alpha = interactable.IsAvailableNow ? 1f : _notInteractiveAlpha;
        }
    }

    private void OnDestroy()
    {
        _currentHintDisposable?.Dispose();
        _currentHintDisposable = null;
    }

    private void InteractableChanged(IInteractable interactable)
    {
        _currentHintDisposable?.Dispose();
        _hintOrigin.gameObject.SetActive(interactable != null);
        if (interactable == null) return;
        
        _currentHintDisposable = new();
        _hintText.text = string.IsNullOrEmpty(interactable.InteractionHint) ? _defaultHintText : interactable.InteractionHint;

        UpdateScreenPosition();
        Observable.EveryUpdate().Subscribe(_ => UpdateScreenPosition()).AddTo(_currentHintDisposable);

        void UpdateScreenPosition()
        {
            var uiPosition = _camera.WorldToScreenPoint(interactable.transform.position);
            _hintOrigin.transform.position = uiPosition;
            _hintText.text = string.IsNullOrEmpty(interactable.InteractionHint) ? _defaultHintText : interactable.InteractionHint;
        }
    }
}
