using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITuner : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private float _value = 0f;

	[SerializeField]
	private Transform _tunerModel;
	[SerializeField]
	private TuneRadioGame _tunerGame;
	[SerializeField]
	private int _index;
	[SerializeField]
	private UITuner _secondTuner;

	public float _normalizedValue;
	private float _angle;

	private IEnumerator Start()
	{
		yield return null;
		transform.position = Camera.main.WorldToScreenPoint(_tunerModel.position);
		WeakTune(0f);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
	}

	public void OnDrag(PointerEventData eventData)
	{
		float tuneValue = eventData.delta.x / 10f;
		_tunerModel.Rotate(Vector3.up, tuneValue);
		_angle += tuneValue;
        
		if (_angle < 0f)
		{
			_angle = 360f;
		}

        _value = _angle % 360f;
		_normalizedValue = _value / 360f;

		_tunerGame.SetPosition(_index, _normalizedValue);

		_secondTuner.WeakTune(tuneValue * -0.25f);
	}

	private void WeakTune(float tuneValue)
	{
		_tunerModel.Rotate(Vector3.up, tuneValue);

        _angle += tuneValue;

        if (_angle < 0f)
        {
            _angle = 360f;
        }

        _value = _angle % 360f;
        _normalizedValue = _value / 360f;
		_tunerGame.SetPosition(_index, _normalizedValue);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
	}
}
