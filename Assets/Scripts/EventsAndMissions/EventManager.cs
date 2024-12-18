using System;
using System.Collections.Generic;

public static class EventManager
{
    public static event Action<string> OnTaskEvent;

    public static void TriggerEvent(string eventName)
    {
        if (OnTaskEvent != null)
        {
            OnTaskEvent.Invoke(eventName);
            UnityEngine.Debug.Log($"Event Triggered: {eventName}");
        }
        else
        {
            UnityEngine.Debug.LogWarning($"No listeners for event: {eventName}");
        }
    }
}
