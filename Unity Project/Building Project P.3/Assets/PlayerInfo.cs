using UnityEngine;

public class PlayerInfo : MonoBehaviour, IFloorEnumerable
{
    private bool _isHandicapped = false;

    /* Whether the player is currently using the emergency exit navigation */
    private bool _isExiting = false;

    public GameObject GetPlayer() { return gameObject; }

    public bool IsHandicapped() {  return _isHandicapped; }

    Floor IFloorEnumerable.GetFloor()
    {
        const float floorHeight = 3;
        const float floorOffset = 0.2f;

        int floor = Mathf.Clamp(
            (int)Mathf.Floor((transform.position.y - floorOffset) / floorHeight),
            0, 2
        );

        return (Floor)floor;
    }

    public void ToggleHandicapped()
    {
        _isHandicapped = !_isHandicapped;
    }

    public bool IsExiting()
    {
        return _isExiting;
    }

    public void SetExiting(bool state)
    {
        _isExiting = state;
    }
}
