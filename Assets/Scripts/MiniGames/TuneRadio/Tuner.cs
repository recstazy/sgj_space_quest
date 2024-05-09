using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuner : MonoBehaviour
{
	private float _value;
	private Vector3 _mousePosition;

	private bool _isDragging;
	private bool _isHover;

	private void OnMouseDown()
	{
		_isDragging = true;
		Debug.Log("OnMouseDown");
	}

	private void OnMouseUp()
	{
		Debug.Log("OnMouseUp");
	}

	private void OnMouseExit()
	{
		_isHover = false;
		Debug.Log("OnMouseExit");
	}

	private void OnMouseDrag()
	{
		var mousePos = Input.mousePosition;
		var delta = _mousePosition.x - mousePos.x;
		_mousePosition = mousePos;
		_value += delta;
		Debug.Log($"Drag _value {_value}");
	}
}
