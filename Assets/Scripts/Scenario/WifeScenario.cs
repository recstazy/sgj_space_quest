using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WifeScenario : BaseScenario
{
    [SerializeField]
    private QuestView _callBackQuestPrefab;
    
    [SerializeField]
    private PlayVoice _notificationSystemVoice;
    
    [SerializeField]
    private PlayVoice _wifeVoice;

    [SerializeField]
    private float _delayBetweenVoices = 0.5f;

    [SerializeField]
    private float _delayAfterWife = 2f;

    [SerializeField]
    private bool _addQuestBadge = true;
    
    public override async void Run()
    {
        base.Run();
        
        try
        {
            await _notificationSystemVoice.Play().ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenVoices), cancellationToken: this.GetCancellationTokenOnDestroy());
            await _wifeVoice.Play().ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());

            if (_addQuestBadge)
            {
                _questController.AddQuest(QuestsDescriptionContainer.CALL_BACK, siblingIndexOverride: 0, _callBackQuestPrefab);
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_delayAfterWife), cancellationToken: this.GetCancellationTokenOnDestroy());
            Finish();
        }
        catch (OperationCanceledException) {}
    }
}
