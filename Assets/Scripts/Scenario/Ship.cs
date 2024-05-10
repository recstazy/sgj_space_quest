using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Transform _playerPoint;

    public Transform PlayerPoint => _playerPoint;
}
