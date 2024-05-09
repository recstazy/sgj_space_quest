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

        Observable.EveryUpdate().Subscribe(_ =>
        {
            var uiPosition = _camera.WorldToScreenPoint(interactable.transform.position);
            _hintOrigin.transform.position = uiPosition;
        }).AddTo(_currentHintDisposable);
    }
}
