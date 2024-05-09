using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisinfectionScenario : BaseScenario
{
	[SerializeField]
	private AudioSource _disinfectionSound;
	[SerializeField]
	private Animator _doorAnimator;
	[SerializeField]
	private ParticleSystem _particleSystem;

	public override void Run()
	{
		base.Run();
		StartCoroutine(Disinfection());
	}

	private IEnumerator Disinfection()
	{
		_doorAnimator.Play("Open");
		yield return new WaitForSeconds(1);
	}

	
}
