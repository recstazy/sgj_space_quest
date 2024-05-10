using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuScene : MonoBehaviour
{
    [Inject] private SceneController sceneController;

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _settings;

    [SerializeField] private Image _progressImage;
    [SerializeField] private GameObject _progressImageHolder;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _backSettingsButton;
    
    void Start()
    {
        sceneController.StartNewScene();
        SetGameObjects();
        AddListeners();
        _startButton.interactable = true;
    }

    private void SetGameObjects()
    {
        _progressImageHolder.gameObject.SetActive(false);
        _progressText.gameObject.SetActive(false);
        _menu.gameObject.SetActive(true);
        _settings.gameObject.SetActive(false);
    }

    private void AddListeners()
    {
        _startButton.onClick.AddListener(OnButtonClick);
        _settingsButton.onClick.AddListener(OpenSettings);
        _backSettingsButton.onClick.AddListener(BackFromSettings);
        sceneController.OnProgressChange += OnLoadProgressChange;
    }

    private void RemoveListeners()
    {
        _startButton.onClick.RemoveListener(OnButtonClick);
        _settingsButton.onClick.RemoveListener(OpenSettings);
        _backSettingsButton.onClick.RemoveListener(BackFromSettings);
        sceneController.OnProgressChange -= OnLoadProgressChange;
    }

    private void OnButtonClick()
    {
        sceneController.LoadNewScene();
        _progressImageHolder.gameObject.SetActive(true);
        _progressText.gameObject.SetActive(true);
        _startButton.interactable = false;
    }

    private void OpenSettings()
    {
        _menu.SetActive(false);
        _settings.SetActive(true);
    }

    private void BackFromSettings()
    {
        _menu.SetActive(true);
        _settings.SetActive(false);
    }

    private void OnLoadProgressChange(float loadProgress)
    {
        _progressText.text = $"{(loadProgress * 100).ToString("0")}";
        _progressImage.fillAmount = loadProgress;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
