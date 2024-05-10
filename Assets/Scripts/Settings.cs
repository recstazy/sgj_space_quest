using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Settings : MonoBehaviour
{
    [Inject] private MixerSettings mixerSettings;

    [SerializeField] private Slider _mainSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    private void Start()
    {
        _mainSlider.onValueChanged.AddListener(OnMainSliderChange);
        _musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
        _soundSlider.onValueChanged.AddListener(OnSoundSliderChange);

        var mainLevel = PlayerPrefs.GetFloat(MixerSettings.MainAudioMixerPref, 1);
        var soundLevel = PlayerPrefs.GetFloat(MixerSettings.SoundAudioMixerPref, 1);
        var musicLevel = PlayerPrefs.GetFloat(MixerSettings.MusicAudioMixerPref, 1);

        _mainSlider.value = mainLevel;
        _soundSlider.value = soundLevel;
        _musicSlider.value = musicLevel;
    }
    private void OnMainSliderChange(float level)
    {
        mixerSettings.SetMainMixerLevel(level);
    }
    private void OnMusicSliderChange(float level)
    {
        mixerSettings.SetMusicMixerLevel(level);
    }
    private void OnSoundSliderChange(float level)
    {
        mixerSettings.SetSoundMixerLevel(level);
    }
    /*
    private void OnWorldSliderChange(float level)
    {
        mixerSettings.SetWorldMixerLevel(level);
    }

    private void OnUISliderChange(float level)
    {
        mixerSettings.SetUIMixerLevel(level);
    }

    private void OnVoicesSliderChange(float level)
    {
        mixerSettings.SetVoicesMixerLevel(level);
    }
    */
}
