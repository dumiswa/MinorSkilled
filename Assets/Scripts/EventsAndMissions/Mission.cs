using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission System/Mission")]
public class Mission : ScriptableObject
{
    public List<MissionEvent> missionEvents = new List<MissionEvent>();
}
