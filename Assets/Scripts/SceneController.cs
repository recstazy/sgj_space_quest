using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public event Action<float> OnProgressChange;
    [SerializeField] private Fader _fader;
    [SerializeField] private Fader _faderDieScreen;
    [SerializeField] private GameObject _dieScreen;
    [SerializeField] private Button _restartButton;

    private CancellationToken cancellationToken;

    private void Start()
    {
        _dieScreen.SetActive(false);
        cancellationToken = this.GetCancellationTokenOnDestroy();
        StartCoroutine(_fader.DoFade(FadeType.StartScene));
        _restartButton.onClick.AddListener(RestartLevel);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.AddListener(RestartLevel);
    }

    public void StartNewScene()
    {
        _dieScreen.SetActive(false);
        StartCoroutine(_fader.DoFade(FadeType.StartScene));
    }

    private void OnHeroDie()
    {
        _dieScreen.SetActive(true);
        StartCoroutine(_faderDieScreen.DoFade(FadeType.EndScene));
    }

    public void LoarStartScene()
    {
        LoadSceneAsync(0).Forget();
    }

    private void RestartLevel()
    {
        _dieScreen.SetActive(false);
        var index = SceneManager.GetActiveScene().buildIndex;
        LoadSceneAsync(index).Forget();
    }

    public void LoadNewScene()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        index += 1;
        LoadSceneAsync(index).Forget();
    }

    private async UniTaskVoid LoadSceneAsync(int buildIndex)
    {
        StartCoroutine(_fader.DoFade(FadeType.EndScene));
        await UniTask.WaitForSeconds(_fader.FadeTime, cancellationToken: cancellationToken);
        float loadingProgress;
        var _asyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        while (!_asyncOperation.isDone)
        {
            loadingProgress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            OnProgressChange?.Invoke(loadingProgress);
            await UniTask.Yield(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
        StartNewScene();
    }
}
