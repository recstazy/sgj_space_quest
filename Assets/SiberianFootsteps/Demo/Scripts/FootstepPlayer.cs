using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recstazy.SiberianFootsteps.Demo
{
    public class FootstepPlayer : MonoBehaviour
    {
        [SerializeField]
        private PlayRandomSound _soundPlayer;

        [SerializeField]
        private FootstepsConfig _config;

        private PhysicMaterial _currentMaterial;
        private FootstepsConfig.FootstepMapping _currentMapping;
        private bool _isRunning;

        public bool IsRunning { get => _isRunning; set { _isRunning = value; UpdateSound(); } }

        public void PlayOneStep()
        {
            _soundPlayer.Play();
        }

        private void Awake()
        {
            UpdateSound();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.sharedMaterial != _currentMaterial)
            {
                _currentMaterial = other.sharedMaterial;
                _currentMapping = _config.GetMapping(other.sharedMaterial);
                UpdateSound();
            }
        }

        private void UpdateSound()
        {
            if (_currentMapping == null) _currentMapping = _config.DefaultMapping;
            _soundPlayer.Sound = IsRunning ? _currentMapping.RunSound : _currentMapping.WalkSound;
        }
    }
}
