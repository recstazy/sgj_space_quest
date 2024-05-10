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

	[SerializeField]
	private PlayerTrigger _playerTrigger;

    private bool _playerInBox;
	private bool _isPlayerGetValve;

	private void Start()
	{
		Trigger.OnTriggerInvoke += GetValveTrigger;
		_playerTrigger.OnPlayerTriggered += PlayerInBox;
    }

    private void OnDestroy()
    {
        Trigger.OnTriggerInvoke -= GetValveTrigger;
        _playerTrigger.OnPlayerTriggered -= PlayerInBox;
    }

    public override void Run()
	{
        if (_isScenarioStarted) return;

        base.Run();
		StartCoroutine(FindValve());
	}

	private void PlayerInBox(bool isPlayerInBox)
	{
        if (_pickUpValveSound != null && isPlayerInBox && _isPlayerGetValve)
        {
            _placeValveSound.Play();
        }
        _playerInBox = isPlayerInBox;
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
