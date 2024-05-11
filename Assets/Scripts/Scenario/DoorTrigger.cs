using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private List<Door> _theDoors;

    public bool IsAvailableNow { get; set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsAvailableNow) return;

        if(other.TryGetComponent(out Player player))
        {
            _theDoors.ForEach(door => door.Open());
        }
        else if(other.TryGetComponent(out Robot robot))
        {
            _theDoors.ForEach(door => door.OpenSilent());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _theDoors.ForEach(door => door.Close());
        }
        else if (other.TryGetComponent(out Robot robot))
        {
            _theDoors.ForEach(door => door.CloseSilent());
        }
    }
}
