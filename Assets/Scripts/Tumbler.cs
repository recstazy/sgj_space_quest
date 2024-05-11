using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Tumbler : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float _delay;

    [SerializeField]
    private Trigger _trigger;

    [SerializeField]
    private string _hint;

    [SerializeField]
    private bool _interactOneTime;

    [SerializeField]
    private AudioSource _useAudio;
    [SerializeField]
    private UnityEvent _unityEvent;
    [SerializeField]
    private Animator _animator;

    public string InteractionHint => _hint;

    public bool IsInteractionDisabled { get; set; }

    private bool _isAvailableNow = default(bool);

    public bool IsAvailableNow
    {
        get
        {
            return _isAvailableNow;
        }
        set
        {
            _isAvailableNow = value;
        }
    }

    public async UniTask Interact(CancellationToken cancellation)
    {
        if (_isAvailableNow)
        {
            if (_useAudio!=null)
            {
                _useAudio.Play();
            }
            _unityEvent.Invoke();
            _animator.Play("SwitchOn");

            Debug.Log($"{gameObject.name} interaction start");
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: cancellation);
            _trigger?.Invoke();
            Debug.Log($"{gameObject.name} interaction end");
            if (_interactOneTime)
                IsInteractionDisabled = true;
        }
    }
}
