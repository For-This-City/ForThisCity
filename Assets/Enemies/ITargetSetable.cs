using UnityEngine;

public interface ITargetSetable
{
    void SetTarget(Vector3 target);
    GameObject[] GetPatrolWayPoints();
}