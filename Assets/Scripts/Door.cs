using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private bool _startOpened;

    public bool IsOpened { get; private set; } = false;

    private void Start()
    {
        if (_startOpened)
        {
            Open();
        }
    }

    public void Open()
    {
        IsOpened = true;
        _animator.Play("OpenDoor");
    }

    public void Close()
    {
        IsOpened = false;
        _animator.Play("CloseDoor");
    }
}
