using System;
using System.Collections.Generic;
using UnityEngine;

public class FuelGameController : GameController
{
    [SerializeField]
    private List<ValveItem> _valveItems;

    private void Start()
    {
        _valveItems.ForEach(item =>
        {
            item.Valve.QuestedValveCondition = item.ValveWinCondition;
            item.Valve.IsValvePlaced = item.IsValvePlaced;
            item.Valve.OnFinishInterract += CheckWinCondition;
            item.Valve.Init();

        });
    }

    private void OnDestroy()
    {
        _valveItems.ForEach(item => item.Valve.OnFinishInterract -= CheckWinCondition);
    }

    protected override bool GetWinCondition()
    {
        foreach (var item in _valveItems)
        {
            if (!item.Valve.IsCorrectValveCondition())
            {
                return false;

            }
        }
        return true;
    }
    protected override void FinishGame()
    {
        base.FinishGame();
        _valveItems.ForEach(item =>
        {
            item.Valve.SetWinCondition();
        });
    }


    protected override void StartGame()
    {
        

    }
}

[Serializable]
public class ValveItem
{
    public Valve Valve;
    public bool ValveWinCondition;
    public bool IsValvePlaced;
}
