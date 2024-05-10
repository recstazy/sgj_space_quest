using UnityEngine;
using Zenject;

public class BaseScenario : MonoBehaviour
{
	[SerializeField]
	protected Trigger _startTrigger;

	[SerializeField]
	protected Trigger _finishTrigger;

    [Inject]
    protected QuestController _questController;

    public Trigger StartTrigger => _startTrigger;
    protected bool _isScenarioStarted = default(bool);

    public virtual void Run()
	{
        if (_isScenarioStarted) return;
        _isScenarioStarted = true;
    }

    protected virtual void Finish()
	{
		if (_finishTrigger != null)
			_finishTrigger?.Invoke();
	}
}
