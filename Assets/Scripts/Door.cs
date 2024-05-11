using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private bool _startOpened;
    [SerializeField]
    private float _doorOpenCloseTime = 1f;
    
    [field: SerializeField]
    public UnityEvent OnOpened { get; private set; }
    
    [field: SerializeField]
    public UnityEvent OnClosed { get; private set; }

    public bool IsOpened { get; private set; } = false;
    public ReactiveProperty<bool> IsInProgress { get; } = new(false);

    private bool? _nextStateAfterAnimation; // open or closed

    private void Start()
    {
        if (_startOpened)
        {
            OpenSilent();
        }
    }

    public void Open()
    {
        OpenInternal(true);
    }

    public void OpenSilent()
    {
        OpenInternal(false);
    }

    public void Close()
    {
        CloseInternal(true);
    }

    public void CloseSilent()
    {
        CloseInternal(false);
    }

    private void OpenInternal(bool sound)
    {
        if (IsOpened) return;
        if (IsInProgress.Value)
        {
            _nextStateAfterAnimation = true;
            return;
        }
        
        IsOpened = true;
        _animator.Play("OpenDoor");

        if (sound)
        {
            OnOpened.Invoke();
        }
        LockInProgress(_doorOpenCloseTime, sound);
    }

    private void CloseInternal(bool sound)
    {
        if (!IsOpened) return;
        if (IsInProgress.Value)
        {
            _nextStateAfterAnimation = false;
            return;
        }
        
        IsOpened = false;
        _animator.Play("CloseDoor");

        if (sound)
        {
            OnClosed.Invoke();
        }
        LockInProgress(_doorOpenCloseTime, sound);
    }

    private async void LockInProgress(float time, bool sound)
    {
        IsInProgress.Value = true;
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: this.GetCancellationTokenOnDestroy());
        IsInProgress.Value = false;

        if (_nextStateAfterAnimation.HasValue)
        {
            if (_nextStateAfterAnimation.Value is true) OpenInternal(sound);
            else CloseInternal(sound);
        }

        _nextStateAfterAnimation = default;
    }
}
