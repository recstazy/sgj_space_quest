using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clipOverride;
    
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void Play()
    {
        Play(-1f);
    }

    public async void Play(float delay)
    {
        if (delay >= 0f)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay),
                cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        if (_clipOverride != null)
        {
            _source.clip = _clipOverride;
        }
        
        _source.Play();
    }
}
