using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cyan;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WarpEffect : MonoBehaviour
{
    [SerializeField]
    private UniversalRendererData _rendererData;
    [SerializeField]
    private ParticleSystem _stars;

    [SerializeField]
    private float _prewarpVelocity = -10f;
    [SerializeField]
    private float _prewarpTime = 1f;

    [SerializeField]
    private float _warpVelocity = 100f;
    [SerializeField]
    private float _warpEndVelocity = 200f;
    [SerializeField]
    private float _warpTime = 1f;

    [SerializeField]
    private float _trailFadeOutTime = 1f;

    [SerializeField]
    private PlayVoice _systemWarpVoice;

    public float PreWarpTime => _prewarpTime;
    public float WarpTime => _warpTime;

    public bool IsPrewarpStateComplete = default(bool);
    public bool IsSystemVoiceStateComplete = default(bool);
    public bool IsPizdecStateComplete = default(bool);
    public bool IsWarpOver = default(bool);

    public async UniTask Play(CancellationToken cancellation)
    {
        await _systemWarpVoice.Play();
        IsSystemVoiceStateComplete = true;
        var trailModule = _stars.trails;
        var velocityModule = _stars.velocityOverLifetime;
        var main = _stars.main;
        var color = _stars.colorOverLifetime;
        var material = _stars.GetComponent<Renderer>().material;
        trailModule.enabled = true;
        velocityModule.z = _prewarpVelocity;
        velocityModule.enabled = true;
        await UniTask.WaitForSeconds((_prewarpTime), cancellationToken: cancellation);
        IsPrewarpStateComplete = true;
        EnableChromaticPost();
        velocityModule.z = _warpVelocity;
        var warpAcceleration = DOTween.To(
            () => velocityModule.z.constant, 
            x => velocityModule.z = 
                x, _warpEndVelocity, 
            _warpTime);
        
        await warpAcceleration.ToUniTask(cancellationToken: cancellation);
        IsPizdecStateComplete = true;
        var trailFadeOut = DOTween.To(
            () => 1f, 
            x =>
            {
                trailModule.widthOverTrailMultiplier = x;
                material.color = Color.white * x;
            }, 
            0f, 
            _trailFadeOutTime);

        color.enabled = true;
        DisableChromaticPost();
        await trailFadeOut.ToUniTask(cancellationToken: cancellation);
        IsWarpOver = true;
    }

    private void EnableChromaticPost()
    {
        var feature = _rendererData.rendererFeatures.FirstOrDefault(x => x is Blit) as Blit;
        if (feature == null) return;
        
        feature.SetActive(true);
    }
    
    private void DisableChromaticPost()
    {
        var feature = _rendererData.rendererFeatures.FirstOrDefault(x => x is Blit) as Blit;
        if (feature == null) return;
        
        feature.SetActive(false);
    }

    public void PlayAnimation()
    {
        Play(this.GetCancellationTokenOnDestroy()).Forget();

    }
    /*
    private void Update()
    {
        if (!Application.isEditor) return;
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Play(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }*/
}
