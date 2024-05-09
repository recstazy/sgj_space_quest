using UnityEngine;

public class BaseScenario : MonoBehaviour
{
	[SerializeField]
	protected Trigger _startTrigger;

	[SerializeField]
	protected Trigger _finishTrigger;

	public Trigger StartTrigger => _startTrigger;

	public virtual void Run()
	{

	}

	protected virtual void Finish()
	{
		if (_finishTrigger != null)
			_finishTrigger?.Invoke();
	}
}
