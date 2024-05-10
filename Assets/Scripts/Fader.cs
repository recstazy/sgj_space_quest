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

    public IEnumerator DoFade(FadeType fadeType)
    {
        switch (fadeType)
        {
            case FadeType.StartScene:
                fader.alpha = 1;
                yield return fader.DOFade(0, fadeTime).WithCancellation(cancellationToken);
                break;
            case FadeType.EndScene:
                fader.alpha = 0;
                yield return fader.DOFade(1, fadeTime).WithCancellation(cancellationToken);
                break;
        }
    }
}

public enum FadeType
{
    StartScene,
    EndScene
}
