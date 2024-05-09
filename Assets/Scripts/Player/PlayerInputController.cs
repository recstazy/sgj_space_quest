using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInputController
{
    private bool _isPlayerActive = default;
    public bool IsPlayerActive => _isPlayerActive;

    [Inject]
    private void Init()
    {
        _isPlayerActive = true;
    }

    public void SetActivateInteraction()
    {
        _isPlayerActive = true;
    }

    public void SetDeactivateInteraction()
    {
        _isPlayerActive = false;
    }
}
