using UnityEngine;

public class DummyMover : MonoBehaviour
{
    public WaypointSystem waypointSystem; 
    public float speed = 5f;
    public float rotationSpeed = 5f; 
    private Transform currentWaypoint; 
    private int currentWaypointIndex = 0; 

    private float fixedYPosition; 

    private void Start()
    {
        if (waypointSystem == null || waypointSystem.waypoints.Length == 0)
        {
            enabled = false;
            return;
        }

        fixedYPosition = transform.position.y;
        currentWaypoint = waypointSystem.waypoints[currentWaypointIndex];
    }

    private void Update()
    {
        if (currentWaypoint == null) return;

        MoveTowardsWaypoint(currentWaypoint);
    }

    private void MoveTowardsWaypoint(Transform waypoint)
    {
        Vector3 targetPosition = new Vector3(waypoint.position.x, fixedYPosition, waypoint.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up); 
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == currentWaypoint)
        {
            Debug.Log($"Reached waypoint {currentWaypointIndex + 1}");
            currentWaypointIndex++;

            if (currentWaypointIndex < waypointSystem.waypoints.Length)
                currentWaypoint = waypointSystem.waypoints[currentWaypointIndex];
            else currentWaypoint = null; 
            
        }
    }
}
