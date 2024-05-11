using UnityEngine;
using UnityEngine.Audio;

public class MixerSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _mainAudioMixer;
    [SerializeField] private AudioMixer _soundAudioMixer;
    [SerializeField] private AudioMixer _musicAudioMixer;
    [SerializeField] private AudioMixer _worldAudioMixer;
    [SerializeField] private AudioMixer _uiAudioMixer;
    [SerializeField] private AudioMixer _voicesAudioMixer;

    public const string MainAudioMixerPref = "mainMixer";
    public const string SoundAudioMixerPref = "soundMixer";
    public const string MusicAudioMixerPref = "musicMixer";
    public const string WorldAudioMixerPref = "worldMixer";
    public const string UIAudioMixerPref = "UIMixer";
    public const string VoicesAudioMixerPref = "voicesMixer";

    public void SetMainMixerLevel(float level)
    {
        SetMixerLevel(_mainAudioMixer, level, MainAudioMixerPref);

    }

    public void SetSoundMixerLevel(float level)
    {
        SetMixerLevel(_soundAudioMixer, level, SoundAudioMixerPref);
    }

    public void SetMusicMixerLevel(float level)
    {
        SetMixerLevel(_musicAudioMixer, level, MusicAudioMixerPref);
    }

    public void SetWorldMixerLevel(float level)
    {
        SetMixerLevel(_worldAudioMixer, level, MusicAudioMixerPref);
    }

    public void SetUIMixerLevel(float level)
    {
        SetMixerLevel(_uiAudioMixer, level, MusicAudioMixerPref);
    }

    public void SetVoicesMixerLevel(float level)
    {
        SetMixerLevel(_voicesAudioMixer, level, MusicAudioMixerPref);
    }

    private void SetMixerLevel(AudioMixer mixer, float level, string saveDataName)
    {
        float currentLevel = Mathf.Lerp(-80, 0, level);
        PlayerPrefs.SetFloat(saveDataName, level);
        mixer.SetFloat("Volume", currentLevel);
    }
}
