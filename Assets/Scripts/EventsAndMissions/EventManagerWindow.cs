using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventManagerWindow : EditorWindow
{
    private string newEventName = "";
    private Vector2 scrollPosition;

    [MenuItem("Tools/Event Manager")]
    public static void ShowWindow()
        => GetWindow<EventManagerWindow>("Event Manager");

    private void OnGUI()
    {
        GUILayout.Label("Global Event Manager", EditorStyles.boldLabel);

        // Display all registered events
        GUILayout.Label("Registered Events:", EditorStyles.label);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        for (int i = 0; i < EventManager.RegisteredEvents.Count; i++)
        {
            string eventName = EventManager.RegisteredEvents[i];
            GUILayout.BeginHorizontal();
            
            GUILayout.Label(eventName, GUILayout.Width(200));
            
            if (GUILayout.Button("Trigger", GUILayout.Width(100)))
            {
                EventManager.TriggerEvent(eventName);
            }
            
            if (GUILayout.Button("Delete", GUILayout.Width(100)))
            {
                EventManager.RemoveEvent(eventName);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();

        GUILayout.Space(10);

        // Add a new event
        GUILayout.Label("Add New Event:", EditorStyles.label);
        newEventName = GUILayout.TextField(newEventName);
        if (GUILayout.Button("Add Event") && !string.IsNullOrEmpty(newEventName))
        {
            EventManager.AddEvent(newEventName);
            newEventName = "";
        }
    }
}
