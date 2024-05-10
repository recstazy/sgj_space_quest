using System;
using UnityEngine;
using Zenject;

public class Wire : MonoBehaviour
{
    public event Action OnPlaced;

    [SerializeField]
    private WireColor _wireColor;

    private Camera _camera;
    private RaycastHit hit;
    private bool _isMovable;
    private Vector3 _startPosition;

    private bool _isCorectColor = default(bool);
    private Vector3 _targetPosition;

    [SerializeField]
    private bool _isPlaced = default(bool);
    public bool IsPlaced => _isPlaced;

    [Inject]
    private void Construct([Inject(Id = GameSceneInstaller.MainCameraId)] Camera mainCamera)
    {
        _camera = mainCamera;
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnMouseDown()
    {
        if (!_isPlaced)
        {
            _isMovable = true;
        }
    }

    private void OnMouseDrag()
    {
        if (_isMovable)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1f))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, hit.point.z);
            }
        }
    }

    private void OnMouseUp()
    {
        _isMovable = false;
        if (!_isPlaced)
        {
            transform.position = _startPosition;
        }
        else
        {
            OnPlaced?.Invoke();
            transform.position = _targetPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<WireEnd>(out WireEnd wireEnd))
        {
            if (wireEnd.WireColor == _wireColor)
            {
                _targetPosition = other.transform.position;
                _isPlaced = true;
                _isCorectColor = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<WireEnd>(out WireEnd wireEnd))
        {
            if (wireEnd.WireColor == _wireColor)
            {
                _isCorectColor = false;
                _targetPosition = _startPosition;
                _isPlaced = false;
                _isCorectColor = false;
            }
        }
    }
}

public enum WireColor
{
    Blue,
    Green,
    Red,
    Yellow,
}