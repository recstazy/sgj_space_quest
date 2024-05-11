using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overload
{
    public class SustainedSound : MonoBehaviour
    {
        #region Fields

        [Tooltip("Loop will play on this source")]
        [SerializeField]
        private AudioSource _loopSource;

        [Tooltip("Start and End will play on this source")]
        [SerializeField]
        private AudioSource _oneShotSource;

        [SerializeField]
        private RandomSound _start;

        [SerializeField]
        private AudioClip _loop;

        [SerializeField]
        private RandomSound _end;

        [Header("Blending Settings")]
        [Tooltip("How much time to wait after starting the Start clip before fading in the loop")]
        [Range(0f, 1f)]
        [SerializeField]
        private float _loopStartDelay;

        [Tooltip("Loop fade in time after the Loop Start Delay")]
        [Range(0f, 1f)]
        [SerializeField]
        private float _loopAttackTime;

        [Tooltip("How much time to wait after starting the End clip before fading out the loop")]
        [Range(0f, 1f)]
        [SerializeField]
        private float _loopStopDelay;

        [Tooltip("Loop volume fade out time after Loop Stop Delay")]
        [Range(0f, 1f)]
        [SerializeField]
        private float _loopReleaseTime;

        private float _defaultVolume;
        private float _currentLoopVolume;
        private Coroutine _blendRoutine;

        #endregion

        #region Properties

        public bool IsPlaying { get; private set; }

        #endregion

        private void Awake()
        {
            _defaultVolume = _loopSource.volume;
            _loopSource.playOnAwake = false;
            _loopSource.Stop();
            _loopSource.clip = _loop;
            _loopSource.loop = true;
        }

        public void SetIsPlaying(bool isPlaying)
        {
            if (isPlaying)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }

        public void Play()
        {
            if (!IsPlaying)
            {
                BeginBlending(StartRoutine);
                IsPlaying = true;
            }
        }

        public void Stop()
        {
            if (IsPlaying)
            {
                BeginBlending(EndRoutine);
                IsPlaying = false;
            }
        }

        private void BeginBlending(System.Func<IEnumerator> blendRoutine)
        {
            if (_blendRoutine != null)
            {
                StopCoroutine(_blendRoutine);
            }

            _blendRoutine = StartCoroutine(blendRoutine());
        }

        private IEnumerator StartRoutine()
        {
            _oneShotSource.PlayOneShot(_start.GetClip());
            yield return new WaitForSeconds(_loopStartDelay);

            float _startVolume = _currentLoopVolume;
            _loopSource.volume = _startVolume;
            _loopSource.Play();
            float t = 0f;

            while (t < 1f)
            {
                _currentLoopVolume = Mathf.Lerp(_startVolume, _defaultVolume, t);
                _loopSource.volume = _currentLoopVolume;
                yield return null;
                t += Time.deltaTime / _loopAttackTime;
            }

            _currentLoopVolume = _defaultVolume;
            _loopSource.volume = _currentLoopVolume;
            _blendRoutine = null;
        }

        private IEnumerator EndRoutine()
        {
            _oneShotSource.PlayOneShot(_end.GetClip());
            yield return new WaitForSeconds(_loopStopDelay);

            float _startVolume = _currentLoopVolume;
            _loopSource.volume = _startVolume;
            float t = 0f;

            while (t < 1f)
            {
                _currentLoopVolume = Mathf.Lerp(_startVolume, 0f, t);
                _loopSource.volume = _currentLoopVolume;
                yield return null;
                t += Time.deltaTime / _loopReleaseTime;
            }

            _currentLoopVolume = 0f;
            _loopSource.volume = _currentLoopVolume;
            _loopSource.Stop();
            _blendRoutine = null;
        }

    }
}
