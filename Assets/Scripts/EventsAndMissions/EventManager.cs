using System;
using System.Collections.Generic;
public static class EventManager
{
    public static event Action<string> OnTaskEvent;
    public static List<string> RegisteredEvents { get; private set; } = new List<string>();

    private static HashSet<string> completedEvents = new HashSet<string>();


    public static void TriggerEvent(string eventName)
    {
        OnTaskEvent?.Invoke(eventName);
        completedEvents.Add(eventName);
    }

    public static void AddEvent(string eventName)
    {
        if (!RegisteredEvents.Contains(eventName))
            RegisteredEvents.Add(eventName);
    }

    public static void RemoveEvent(string eventName)
    {
        if (!RegisteredEvents.Contains(eventName))
        {
            RegisteredEvents.Remove(eventName);
            completedEvents.Remove(eventName);
        }
    }

    public static bool IsEventCompleted(string eventName)
    {
        return completedEvents.Contains(eventName);
    }

    public static void ResetCompletedEvents()
    {
        completedEvents.Clear();
    }
}
