using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TuneRadioGame : MonoBehaviour
{
    public event Action OnWin;

    [SerializeField]
    private Transform[] _points;
    [SerializeField]
    private float[] _targetValues;
    [SerializeField]
    private float _changeColorGap = 0.07f;
    [SerializeField]
    private float _targetAccuracy = 0.002f;

    [SerializeField]
    private AudioSource _noise;
    [SerializeField]
    private AudioSource _sound;
    [SerializeField]
    private AudioSource _reverseSound;
    [SerializeField]
    private float _maxRadioVolume = 0.5f;
    [SerializeField]
    private float _signalMinDiffToPlay = 0.5f;

    [SerializeField]
    private Canvas _canvas;

    private Material[] _pointMaterials;

    private float _minPos;
    private float _maxPos;

    public float[] _currentValues;
    public float _summDiff;

    private bool _isTuned;
    private bool _playedReverseVoice;

    private void Start()
    {
        _minPos = Mathf.Min(_points[0].transform.localPosition.x, _points[1].transform.localPosition.x);
        _maxPos = Mathf.Max(_points[0].transform.localPosition.x, _points[1].transform.localPosition.x);

        _pointMaterials = new Material[_points.Length];
        for (int i = 0; i < _points.Length; i++)
        {
            _pointMaterials[i] = _points[i].GetComponent<MeshRenderer>().material;
        }

        _currentValues = new float[_points.Length];
    }

    public void StartGame()
    {
        _canvas.gameObject.SetActive(true);
        _noise.Play();
        _sound.Play();
        PlayReversedVoice();
    }

    public void StopGame()
    {
        _canvas.gameObject.SetActive(false);
        _noise.Stop();
        _sound.Stop();
    }

    public void SetPosition(int pointIndex, float value)
    {
        if (_isTuned)
            return;

        SetPositionForPoint(pointIndex, value);
        _currentValues[pointIndex] = value;
        UpdateValues();
    }

    private void UpdateValues()
    {
        _summDiff = 0f;
        for (int i = 0; i < _targetValues.Length; i++)
        {
            _summDiff += Math.Abs(_currentValues[i] - _targetValues[i]);
        }
        _summDiff /= 2f;

        _noise.volume = _summDiff * _maxRadioVolume;
        var signalRawVolume = 1f - _summDiff;
        var signalRemappedVolume = Math.Clamp((signalRawVolume - _signalMinDiffToPlay) / (1f - _signalMinDiffToPlay), 0f, 1f);
        _sound.volume = signalRemappedVolume * _maxRadioVolume;

        if (_summDiff < _targetAccuracy)
        {
            _isTuned = true;
            Debug.LogError("Win!");
            OnWin?.Invoke();
        }
    }

    private void SetPositionForPoint(int index, float value)
    {
        var pos = _points[index].localPosition;
        pos.x = Mathf.Lerp(_minPos, _maxPos, value);

        var diff = Mathf.Abs(_targetValues[index] - value);

        if (diff < _changeColorGap)
        {
            var lerpColor = Color.Lerp(Color.green, Color.red, diff / _changeColorGap);
            _pointMaterials[index].SetColor("_EmissionColor", lerpColor);
            _pointMaterials[index].color = lerpColor;
        }
        else
        {
            _pointMaterials[index].SetColor("_EmissionColor", Color.red);
            _pointMaterials[index].color = Color.red;
        }

        //if (index == 0)
        //    Debug.Log($"pos= {pos}  value {value}");

        _points[index].localPosition = pos;
    }

    private async void PlayReversedVoice()
    {
        if (_playedReverseVoice) return;

        _playedReverseVoice = true;
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: this.GetCancellationTokenOnDestroy());
        _reverseSound.Play();
    }
}
