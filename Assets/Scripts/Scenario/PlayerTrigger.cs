using System;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public event Action<bool> OnPlayerTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Player player))
        {
            OnPlayerTriggered?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            OnPlayerTriggered?.Invoke(false);
        }
    }
}
