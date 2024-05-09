using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanFaceScenario : BaseScenario
{
	[SerializeField]
	private AudioSource _scannerSound;

	public override void Run()
	{
		base.Run();
		_scannerSound.Play();
	}
}
