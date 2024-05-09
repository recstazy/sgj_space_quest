using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInputController
{
    private bool _isPlayerActive = default;
    public bool IsPlayerActive => _isPlayerActive;

    [Inject]
    private void Inti()
    {
        _isPlayerActive = true;
    }

    public void SetActivateIntraction()
    {
        _isPlayerActive = true;
    }

    public void SetDeactivaterInvterraction()
    {
        _isPlayerActive = false;
    }
}
