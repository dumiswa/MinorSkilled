using UnityEngine;

[System.Serializable]

public class MissionEvent
{
    public enum EventType
    {
        ReachWaypoint,
        KillEnemies,
        DestroyObject
    }

    public EventType eventType;
    public string eventName;
    //public string description;

    public Transform waypoint;
    public int enemyKillCount;
    public GameObject targetObject;
}
