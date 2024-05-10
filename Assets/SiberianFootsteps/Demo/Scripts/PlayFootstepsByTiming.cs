using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recstazy.SiberianFootsteps.Demo
{
    public class PlayFootstepsByTiming : MonoBehaviour
    {
        [SerializeField]
        private FootstepPlayer _player;

        [SerializeField]
        private float _walkDelay = 1f;

        [SerializeField]
        private float _runDelay = 0.5f;

        private Coroutine _loopRoutine;

        public bool IsMoving { get; set; }
        public bool IsRunning 
        { 
            get => _player.IsRunning;
            set
            {
                _player.IsRunning = value;
                if (value == true) StartLoop();
            } 
        }
        
        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitUntil(() => IsMoving);
                StartLoop();

                yield return new WaitUntil(() => !IsMoving);
                StopCoroutine(_loopRoutine);
                _loopRoutine = null;
            }
        }

        private void StartLoop()
        {
            if (_loopRoutine != null) StopCoroutine(_loopRoutine);
            _loopRoutine = StartCoroutine(FootstepLoopRoutine());
        }

        private IEnumerator FootstepLoopRoutine()
        {
            var walkSeconds = new WaitForSeconds(_walkDelay);
            var runSeconds = new WaitForSeconds(_runDelay);

            while (true)
            {
                _player.PlayOneStep();
                yield return IsRunning ? runSeconds : walkSeconds;
            }
        }
    }
}
