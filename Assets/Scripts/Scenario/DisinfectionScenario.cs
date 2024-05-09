using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisinfectionScenario : BaseScenario
{
	[SerializeField]
	private AudioSource _scannerSound;
	[SerializeField]
	private AudioSource _disinfectionSound;
	[SerializeField]
	private Animator _doorAnimator;
	[SerializeField]
	private ParticleSystem _particleSystem;

	[SerializeField]
	private float _disinfectionTime;

	public bool PlayerInBox { get; set; }

	public override void Run()
	{
		base.Run();
		StartCoroutine(Disinfection());
	}

	private IEnumerator Disinfection()
	{
		_scannerSound.Play();
		yield return new WaitForSeconds(1f);

		_doorAnimator.Play("OpenDoor");
		yield return new WaitForSeconds(1f);

		while (!PlayerInBox)
			yield return null;

		yield return new WaitForSeconds(0.5f);
		_doorAnimator.Play("CloseDoor");
		yield return new WaitForSeconds(1f);

		_particleSystem.Play();
		_disinfectionSound.Play();

		yield return new WaitForSeconds(_disinfectionTime);
		_particleSystem.Stop();
		_doorAnimator.Play("OpenDoor");

		Finish();
	}


}
