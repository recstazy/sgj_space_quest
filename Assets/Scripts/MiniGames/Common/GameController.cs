using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameController : MonoBehaviour
{
    /*[SerializeField]
    private Trigger _trigger*/

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
        Debug.Log("Finished");
    }
}
