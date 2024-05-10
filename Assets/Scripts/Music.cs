using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource _source;
    private static Music _instnce;
    
    private void Awake()
    {
        _instnce = this;
        _source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public static void SetMute(bool isMute)
    {
        if (_instnce != null && _instnce._source != null)
        {
            _instnce._source.mute = isMute;
        }
    }
}
