using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private Mission currentMission; // Assign in Inspector
    private int currentEventIndex = 0;
    private int currentKillCount = 0;

    private void Start()
    {
        if (currentMission != null && currentMission.missionEvents.Count > 0)
        {
            StartCoroutine(HandleMissionEvents());
        }
    }

    private IEnumerator HandleMissionEvents()
    {
        while (currentEventIndex < currentMission.missionEvents.Count)
        {
            var missionEvent = currentMission.missionEvents[currentEventIndex];

            yield return StartCoroutine(HandleEvent(missionEvent));

            currentEventIndex++;
        }

        Debug.Log("Mission Completed!");
    }

    private IEnumerator HandleEvent(MissionEvent missionEvent)
    {
        switch (missionEvent.eventType)
        {
            case MissionEvent.EventType.ReachWaypoint:
                yield return HandleReachWaypoint(missionEvent);
                break;

            case MissionEvent.EventType.KillEnemies:
                yield return HandleKillEnemies(missionEvent);
                break;

            case MissionEvent.EventType.DestroyObject:
                yield return HandleDestroyObject(missionEvent);
                break;
        }
    }

    private IEnumerator HandleReachWaypoint(MissionEvent missionEvent)
    {
        while (Vector3.Distance(missionEvent.waypoint.position, transform.position) > 1f)
        {
            yield return null; 
        }

        Debug.Log($"Reached Waypoint: {missionEvent.waypoint.name}");
    }

    private IEnumerator HandleKillEnemies(MissionEvent missionEvent)
    {
        currentKillCount = 0;

        EventManager.OnTaskEvent += (eventName) =>
        {
            if (eventName == "EnemyKilled")
            {
                currentKillCount++;
            }
        };

        yield return new WaitUntil(() => currentKillCount >= missionEvent.enemyKillCount);

        Debug.Log($"Killed {missionEvent.enemyKillCount} enemies.");
    }

    private IEnumerator HandleDestroyObject(MissionEvent missionEvent)
    {
        while (missionEvent.targetObject != null) 
        {
            yield return null;
        }

        Debug.Log($"Destroyed Object: {missionEvent.targetObject.name}");
    }
}
