using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trigger", menuName = "Space Quest/Trigger")]
public class Trigger : ScriptableObject
{
	public static event Action<Trigger> OnTriggerInvoke;

	internal void Invoke()
	{
		OnTriggerInvoke?.Invoke(this);
	}
}
