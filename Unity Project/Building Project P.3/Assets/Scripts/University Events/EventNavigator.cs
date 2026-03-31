using UnityEngine;

public class EventNavigator : MonoBehaviour
{
    public GameObject Room { get; set; }
    public GameObject player; // Player GameObject (WaypointSystem script should be attached here)
    public GameObject panelToHide;

    public void Naviagte()
    {
        if (Room == null)
        {
            Debug.Log("Couldn't find room");
        }
        else
        {
            WaypointSystem waypointSystem = player.GetComponent<WaypointSystem>();
            if (waypointSystem != null)
            {
                waypointSystem.SetLatestMenu(panelToHide);
                Transform s = Room.transform.GetChild(0).GetChild(2);
                waypointSystem.SetTarget(s);
            }
        }
    }
}