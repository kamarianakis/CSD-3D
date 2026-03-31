using UnityEngine;

public enum Floor
{
    Ground = 0,
    First = 1,
    Second = 2,
    Invalid = 3,
}

public interface IFloorEnumerable
{
    Floor GetFloor();
}
