using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Recstazy.SiberianFootsteps.Demo;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [Inject]
    private PlayerInputController _playerInputController;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _lookSensitivity;

    [SerializeField]
    private bool _invertLookUp = false;

    [SerializeField]
    private Transform _lookSidewaysRoot;

    [SerializeField]
    private Transform _lookUpRoot;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private CinemachineVirtualCamera _characterCamera;

    [SerializeField]
    private CapsuleCollider _playerCollider;

    [SerializeField]
    private PlayFootstepsByTiming _footstepSound;
    [SerializeField]
    private bool _useRunFootstepSounds;

    private float _lookUpAngle;
    private float _lookSidewaysAngle;

    /*[Inject]
    private void Construct([Inject(Id = GameSceneInstaller.MainCameraId)] Camera mainCamera)
    {
        _camera = mainCamera;
    }*/

    private void Awake()
    {
        _characterCamera.Priority = 10;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_playerInputController.IsPlayerActive)
        {
            HandleMovement();
            HandleCamera();
            if (!_playerCollider.enabled)
            {
                _playerCollider.enabled = true;
            }
        }
        else
        {
            _playerCollider.enabled = false;
        }
    }

    private void LateUpdate()
    {
        _characterCamera.transform.position = _lookUpRoot.position;
        _characterCamera.transform.rotation = _lookUpRoot.rotation;
    }

    private void HandleMovement()
    {
        var forward = Input.GetAxisRaw("Vertical");
        var strafe = Input.GetAxisRaw("Horizontal");

        var isMoving = forward != 0 || strafe != 0;
        _footstepSound.IsMoving = isMoving;
        if (isMoving && _useRunFootstepSounds && !_footstepSound.IsRunning)
        {
            _footstepSound.IsRunning = true;
        }
        
        if (!isMoving) return;
        var direction = transform.TransformDirection(new Vector3(strafe, 0f, forward).normalized);
        var velocity = direction * _moveSpeed;
        _characterController.SimpleMove(velocity);
    }

    private void HandleCamera()
    {
        var lookUp = Input.GetAxisRaw("Mouse Y");
        lookUp = _invertLookUp ? lookUp : -lookUp;
        var lookSideways = Input.GetAxisRaw("Mouse X");

        _lookUpAngle = Mathf.Clamp(_lookUpAngle + lookUp * _lookSensitivity, -90f, 90f);
        _lookSidewaysAngle += lookSideways * _lookSensitivity;
        _lookSidewaysAngle %= 360;

        _lookUpRoot.localEulerAngles = new Vector3(_lookUpAngle, 0f, 0f);
        _lookSidewaysRoot.localEulerAngles = new Vector3(0f, _lookSidewaysAngle, 0f);
    }
}