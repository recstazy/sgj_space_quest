using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FindValveScenario : BaseScenario
{
	[SerializeField]
	private AudioSource _pickUpValveSound;

    [SerializeField]
    private AudioSource _placeValveSound;

    [SerializeField]
	private float _pickUpTime;

	[SerializeField]
	private Trigger _getValveTrigger;

    [SerializeField]
    private Trigger _valvePlacedTrigger;

	private bool _playerInBox;
	private bool _isPlayerGetValve;

	private void Start()
	{
		Trigger.OnTriggerInvoke += GetValveTrigger;

    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= GetValveTrigger;
    }

    public override void Run()
	{
		base.Run();
		StartCoroutine(FindValve());
	}

	private void GetValveTrigger(Trigger trigger)
	{
		if (_getValveTrigger == trigger)
		{
			if(_pickUpValveSound != null)
			{
                _pickUpValveSound.Play();
            }
			_isPlayerGetValve = true;
        }

		if (_valvePlacedTrigger == trigger)
		{
            if (_pickUpValveSound != null)
            {
                _placeValveSound.Play();
            }
			_playerInBox = true;
        }
	}

	private IEnumerator FindValve()
	{
		_questController.AddQuest(QuestsDescriptionContainer.FIND_VALVE);
		while (!_playerInBox || !_isPlayerGetValve)
			yield return null;
        _questController.CompleteQuest(QuestsDescriptionContainer.FIND_VALVE);
        Finish();
	}
}
