using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class WarpEffect : MonoBehaviour
{
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

    public async UniTask Play(CancellationToken cancellation)
    {
        var trailModule = _stars.trails;
        var velocityModule = _stars.velocityOverLifetime;
        var main = _stars.main;
        var color = _stars.colorOverLifetime;
        var material = _stars.GetComponent<Renderer>().material;
        
        trailModule.enabled = true;
        velocityModule.z = _prewarpVelocity;
        velocityModule.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_prewarpTime), cancellationToken: cancellation);
        
        velocityModule.z = _warpVelocity;
        var warpAcceleration = DOTween.To(
            () => velocityModule.z.constant, 
            x => velocityModule.z = 
                x, _warpEndVelocity, 
            _warpTime);
        
        await warpAcceleration.ToUniTask(cancellationToken: cancellation);
        
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
        await trailFadeOut.ToUniTask(cancellationToken: cancellation);
        
    }

    private void Update()
    {
        if (!Application.isEditor) return;
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Play(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }
}
