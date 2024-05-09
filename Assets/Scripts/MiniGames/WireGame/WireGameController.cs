using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGameController : GameController
{
    [SerializeField]
    private List<Wire> _wires;

    private void Start()
    {
        foreach (var wire in _wires)
        {
            wire.OnPlaced += CheckWinCondition;
        } 
    }

    protected override bool GetWinCondition()
    {
        foreach (var wire in _wires)
        {
            if (!wire.IsPlaced)
            {
                return false;
            }
        }
        return true;
    }

    protected override void StartGame()
    {
        
    }
}
