using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelGameController : GameController
{
    [SerializeField]
    private bool[] _valveWinPositions;
    
    protected override bool GetWinCondition()
    {
        return false;
    }

    protected override void StartGame()
    {
    }
}
