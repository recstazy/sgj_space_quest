using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Recstazy.SiberianFootsteps
{
    public class PlayRandomSound : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AudioSource _source;

        [SerializeField]
        private RandomSound _sound;

        [SerializeField]
        private float _minVolume = 1f;

        [SerializeField]
        private float _maxVolume = 1f;

        [SerializeField]
        private float _minPitch = 1f;

        [SerializeField]
        private float _maxPitch = 1f;

        #endregion

        #region Properties

        public AudioSource Source { get => _source; set => _source = value; }
        public RandomSound Sound { get => _sound; set => _sound = value; }
        public float MinVolume { get => _minVolume; set => _minVolume = value; }
        public float MaxVolume { get => _maxVolume; set => _maxVolume = value; }
        public float MinPitch { get => _minPitch; set => _minPitch = value; }
        public float MaxPitch { get => _maxPitch; set => _maxPitch = value; }

        #endregion

        public void Play(AudioSource source)
        {
            _source = source;
            PlaySound();
        }

        public void Play()
        {
            PlaySound();
        }

        private void PlaySound()
        {
            var clip = _sound.GetSound();

            float volume = Random.Range(_minVolume, _maxVolume);
            float pitch = Random.Range(_minPitch, _maxPitch);
            _source.pitch = pitch;
            _source.PlayOneShot(clip, volume);
        }

        [ContextMenu("Play in runtime")]
        private void PlayInRuntime()
        {
            if (Application.isPlaying)
            {
                Play();
            }
        }
    }
}
