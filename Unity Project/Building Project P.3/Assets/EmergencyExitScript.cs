using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EmergencyExitScript : MonoBehaviour
{

    public PlayerInfo playerInfo;
    public NavMeshAgent navAgent = null;
    public WaypointSystem waypointSystem;

    private GameObject[] _exits;

    public void UpdateExits()
    {
        Floor playerFloor = Floor.Invalid;

        // Attempt to retrieve the player's floor
        if(playerInfo is IFloorEnumerable floorIntf)
        {
            playerFloor = floorIntf.GetFloor();
        }

        bool isHandicapped = playerInfo.IsHandicapped();

        // Gather all emergency exits in the scene
        _exits = GameObject.FindGameObjectsWithTag("EmergencyExit").Where(exit =>
        {
            if(isHandicapped && exit.TryGetComponent<IFloorEnumerable>(out IFloorEnumerable floorIntf))
            {
                return playerFloor == floorIntf.GetFloor();
            }

            return true;
        }).ToArray();
    }

    // * Requires manual check for nav agent's existence (unchecked).
    private float CalculatePathDistanceBetween(Vector3 start, Vector3 dest)
    {
        NavMeshPath path = new NavMeshPath();

        // Find a path if one exists, distance is assumed to be infinity otherwise.
        if (
            !NavMesh.CalculatePath(start, dest, NavMesh.AllAreas, path) ||
            path.status != NavMeshPathStatus.PathComplete
        )
            return Mathf.Infinity;

        // Sum up all corners of the path (straight segments)
        float dist = 0f;
        for (int i = 1; i < path.corners.Length; i++)
            dist += Vector3.Distance(path.corners[i - 1], path.corners[i]);

        return dist;

    }

    public void NavigateToNearestExit()
    {
        if(navAgent == null)
        {
            Debug.LogError("No nav agent provided, emergency exit feature disabled.");
            return;
        }
        
        if(waypointSystem == null)
        {
            Debug.LogError("No waypoint system provided, emergency exit feature disabled.");
            return;
        }

        UpdateExits();

        GameObject player = playerInfo.GetPlayer();

        GameObject nearestObj = null;
        float nearestDist = Mathf.Infinity;

        foreach (GameObject exit in _exits)
        {
            float pathDist = CalculatePathDistanceBetween(player.transform.position, exit.transform.position);

            if(pathDist < nearestDist)
            {
                nearestDist = pathDist;
                nearestObj = exit;
            }
        }

        if (nearestObj != null)
        {
            // About to enable exit navigation, so player will in fact be exiting
            playerInfo.SetExiting(true);

            if (nearestObj.TryGetComponent<EmergencyExitPointScript>(out EmergencyExitPointScript emergencyPoint))
            {
                // Try getting the waypoint target
                waypointSystem.SetTarget(emergencyPoint.GetWaypointTarget()?.transform ?? nearestObj.transform);
            }
            else
            {
                // Fallback to default object
                waypointSystem.SetTarget(nearestObj.transform);
            }
            
        }
    }
}
