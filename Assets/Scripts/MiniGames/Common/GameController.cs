using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public abstract class GameController : MonoBehaviour
{
    [SerializeField]
    protected Trigger _completeTrigger;

    [Inject]
    protected PlayerInputController _playerInputController;

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
        _completeTrigger?.Invoke();
    }
}
