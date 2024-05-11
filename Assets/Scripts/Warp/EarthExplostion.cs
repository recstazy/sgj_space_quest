using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthExplostion : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

    [SerializeField]
    private Transform _earth;

    public void Play()
    {
        _earth.gameObject.SetActive(false);
        _particles.Play();
    }
}
