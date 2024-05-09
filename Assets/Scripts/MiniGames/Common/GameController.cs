using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class GameController : MonoBehaviour
{
    public BoolReactiveProperty IsFinished { get; private set; } = new();

    protected abstract bool GetWinCondition();
    protected abstract void StartGame();

    protected virtual void CheckWinCondition()
    {
        if (GetWinCondition())
        {
            FinishGame();
        }
    }

    protected virtual void FinishGame()
    {
        IsFinished.Value = true;
        Debug.Log("Finished");
    }
}
