using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private float fadeTime = 1f;
    public float FadeTime => fadeTime;

    private CancellationToken cancellationToken;

    private void Start()
    {
        cancellationToken = this.GetCancellationTokenOnDestroy();
    }

    public IEnumerator FadeIn(float time = -1)
    {
        if (time == -1)
            time = fadeTime;

        UnityEngine.Debug.LogError($"FadeIn");
        fader.alpha = 1f;
        fader.DOFade(0f, time).SetEase(Ease.Linear);
        yield return new WaitForSeconds(time);
    }

    public IEnumerator FadeOut(float time = -1)
    {
        if (time == -1)
            time = fadeTime;

        UnityEngine.Debug.LogError($"FadeOut");
        fader.alpha = 0f;
        fader.DOFade(1f, time).SetEase(Ease.Linear);
        yield return new WaitForSeconds(time);
    }
}

public enum FadeType
{
    StartScene,
    EndScene
}
