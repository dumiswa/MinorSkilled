using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    [HideInInspector] public Transform[] waypoints; 

    private void Awake()
    {
        int childCount = transform.childCount;
        waypoints = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (transform.childCount > 1)
        {
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            }

            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }
    }
}
