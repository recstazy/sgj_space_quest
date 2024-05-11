using DG.Tweening;
using UnityEngine;

public class RotateAfterWarp : MonoBehaviour
{
    [SerializeField]
    private float _rotateCoefficient;
    [SerializeField]
    private Transform _transform;

    public bool IsRotate { get; set; }

    void Update()
    {
        if (IsRotate)
        {
            var rotation = new Vector3(
           transform.rotation.eulerAngles.x,
           transform.rotation.eulerAngles.y + Time.deltaTime * _rotateCoefficient,
           transform.rotation.eulerAngles.z);

            transform.DOLocalRotate(rotation, Time.deltaTime);

            /*
            var rotationSlave = new Vector3(
           _transform.rotation.eulerAngles.x,
           _transform.rotation.eulerAngles.y + Time.deltaTime * _rotateCoefficient,
           _transform.rotation.eulerAngles.z);

            _transform.DOLocalRotate(rotationSlave, Time.deltaTime);*/
        }
    }
}
