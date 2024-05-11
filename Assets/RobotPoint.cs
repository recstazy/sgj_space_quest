using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPoint : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
