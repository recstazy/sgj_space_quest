using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private bool _startOpened;

    private void Start()
    {
        if (_startOpened)
        {
            Open();
        }
    }

    public void Open()
    {
        _animator.Play("OpenDoor");
    }

    public void Close()
    {
        _animator.Play("CloseDoor");
    }
}
