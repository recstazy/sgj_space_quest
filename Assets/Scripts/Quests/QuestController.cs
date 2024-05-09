using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class QuestController : MonoBehaviour
{
    [SerializeField]
    private RectTransform _questsRoot;

    [SerializeField]
    private QuestView _questPrefab;

    private DiContainer _container;
    private readonly List<QuestView> _quests = new();

    [Inject]
    private void Construct(DiContainer container)
    {
        _container = container;
    }
    
    public async void AddQuest(string questText)
    {
        var questView = _container.InstantiatePrefabForComponent<QuestView>(_questPrefab, _questsRoot);
        _quests.Add(questView);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_questsRoot);
        questView.Setup(questText, false);
        await questView.Show(questView.GetCancellationTokenOnDestroy());
    }

    public async void CompleteQuest(string questText)
    {
        var questView = _quests.FirstOrDefault(x => x.Text == questText);
        if (questView == null)
        {
            Debug.LogError($"Unexpected, no quest found with text '{questText}'");
            return;
        }

        await questView.CompleteAndHide(questView.GetCancellationTokenOnDestroy());
        _quests.Remove(questView);
        Destroy(questView.gameObject);
    }
    
    private void Update()
    {
        if (!Application.isEditor) return;
        const string testQuestText = "Тестовый квест";
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddQuest(testQuestText);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CompleteQuest(testQuestText);
        }
    }
}
