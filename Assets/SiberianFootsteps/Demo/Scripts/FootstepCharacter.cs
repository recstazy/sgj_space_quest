using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recstazy.SiberianFootsteps.Demo
{
    public class FootstepCharacter : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _controller;

        [SerializeField]
        private PlayFootstepsByTiming _footsteps;

        [SerializeField]
        private Transform _lookRoot;

        [SerializeField]
        private float _walkSpeed = 1f;

        [SerializeField]
        private float _runSpeed = 1.5f;

        [SerializeField]
        private float _lookSensivity = 1f;

        [SerializeField]
        private float _turnSensivity = 1f;

        [SerializeField]
        private float _runFov = 75f;

        [SerializeField]
        private float _changeFovTime = 1f;

        [SerializeField]
        private KeyCode _runKey = KeyCode.LeftShift;

        private Camera _camera;
        private float _currentLookPitch = 0f;
        private float _defaultFov;
        private float _currentFov;
        private Coroutine _changeFovRoutine;

        private bool _isRunning;
        private bool IsRunning 
        { 
            get => _isRunning; 
            set 
            { 
                _isRunning = value; 
                _footsteps.IsRunning = value;
                ChangeFov(value == true ? _runFov : _defaultFov);
            } 
        }

        private void Awake()
        {
            _camera = Camera.main;
            _defaultFov = _camera.fieldOfView;
            _currentFov = _defaultFov;
        }

        private void Update()
        {
            if (Input.GetKeyDown(_runKey))
            {
                IsRunning = true;
            }
            else if (Input.GetKeyUp(_runKey))
            {
                IsRunning = false;
            }

            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            if (!Input.GetKey(KeyCode.Mouse1)) look = Vector2.zero;

            _footsteps.IsMoving = Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.y) > 0.1f;
            _currentLookPitch += look.y * _lookSensivity;
            _currentLookPitch = Mathf.Clamp(_currentLookPitch, -90f, 90f);
            _lookRoot.localEulerAngles = new Vector3(_currentLookPitch, 0f, 0f);
            transform.localEulerAngles += Vector3.up * look.x * _turnSensivity;
            float speed = IsRunning ? _runSpeed : _walkSpeed;
            _controller.Move((transform.forward * input.y + transform.right * input.x).normalized * speed * Time.deltaTime);
        }

        private void ChangeFov(float newValue)
        {
            if (_changeFovRoutine != null) StopCoroutine(_changeFovRoutine);
            _changeFovRoutine = StartCoroutine(ChangeFovRoutine(newValue));
        }

        private IEnumerator ChangeFovRoutine(float newValue)
        {
            float startFov = _currentFov;
            float t = 0;

            while (t < 1f)
            {
                _currentFov = Mathf.Lerp(startFov, newValue, Mathf.SmoothStep(0f, 1f, t));
                _camera.fieldOfView = _currentFov;
                yield return null;
                t += Time.deltaTime / _changeFovTime;
            }

            _currentFov = newValue;
            _camera.fieldOfView = _currentFov;
            _changeFovRoutine = null;
        }
    }
}
