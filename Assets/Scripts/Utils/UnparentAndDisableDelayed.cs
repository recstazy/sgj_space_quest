using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UnparentAndDisableDelayed : MonoBehaviour
{
    [SerializeField]
    private Transform _newParent;

    [SerializeField]
    private float _delayBeforeDisable;
    
    public async void Execute()
    {
        transform.SetParent(_newParent);
        await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeDisable),
            cancellationToken: this.GetCancellationTokenOnDestroy());
    }
}
