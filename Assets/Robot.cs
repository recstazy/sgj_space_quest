using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _points;

    [SerializeField]
    private NavMeshAgent _agent;

    [SerializeField]
    private float _delayBetweenPoints;

    private Vector3 _point => _points[_pointIndex].position;
    private int _pointIndex;
    private bool _isPaused;

    private void Start()
    {
        _pointIndex = 0;
        _agent.SetDestination(_point);
    }

    private void Update()
    {
        if (_isPaused) return;

        if (Time.frameCount % 10 == 0)
        {
            if (_agent.remainingDistance < 0.5f)
            {
                _pointIndex++;
                if (_pointIndex >= _points.Count)
                    _pointIndex = 0;
                StartCoroutine(Pause());
            }
        }
    }

    private IEnumerator Pause()
    {
        Debug.Log("Robot paused", gameObject);
        _isPaused = true;
        yield return new WaitForSeconds(_delayBetweenPoints);
        _isPaused = false;
        _agent.SetDestination(_point);
        if (!_agent.hasPath)
        {
            StartCoroutine(Pause());
        }        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (Transform t in _points)
        {
            Gizmos.DrawSphere(t.position, 0.3f);
        }
    }
}
