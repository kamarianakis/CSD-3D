using UnityEngine;

public class EmergencyExitPointScript : MonoBehaviour, IFloorEnumerable
{
    public Floor floor;
    public GameObject waypointTarget;

    Floor IFloorEnumerable.GetFloor()
    {
        return floor;
    }

    public GameObject GetWaypointTarget()
    {
        return waypointTarget;
    }
}
