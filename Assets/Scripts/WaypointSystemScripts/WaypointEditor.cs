using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointEditor : EditorWindow
{
    private bool toggleMeshRenderers = true;
    private bool toggleColliders = true;
    private string targetTag = "Waypoint";

    private GameObject waypointParent; // Parent GameObject containing all waypoints
    private List<Waypoint> waypoints = new List<Waypoint>();

    private GameObject selectedWaypoint; // Selected waypoint for configuration
    //private string waypointName = "";
    //private Waypoint.TaskType selectedTaskType = Waypoint.TaskType.ReachDestination;
    private GameObject enemyParent; // For KillEnemies task
    private GameObject targetObject; // For DestroyObject task

    private Vector2 scrollPosition; // Scroll position for the scroll view

    [MenuItem("Tools/Waypoint Editor")]
    public static void ShowWindow()
    {
        GetWindow<WaypointEditor>("Waypoint Editor");
    }

    private GUIStyle GetHeaderStyle()
    {
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter
        };
        return headerStyle;
    }


    private void OnGUI()
    {
        GUILayout.Label("Waypoint Editor", EditorStyles.boldLabel);

        // Section 1: Toggle Components for All Waypoints
        EditorGUILayout.Space();
        GUILayout.Label("Toggle Components", EditorStyles.boldLabel);

        targetTag = EditorGUILayout.TextField("Tag to Filter:", targetTag);
        toggleMeshRenderers = EditorGUILayout.Toggle("Toggle MeshRenderers", toggleMeshRenderers);
        toggleColliders = EditorGUILayout.Toggle("Toggle Colliders", toggleColliders);

        if (GUILayout.Button("Enable Components"))
        {
            ToggleComponents(true);
        }

        if (GUILayout.Button("Disable Components"))
        {
            ToggleComponents(false);
        }

        EditorGUILayout.HelpBox("Use this tool to toggle MeshRenderers or Colliders for all objects with the specified tag in the scene.", MessageType.Info);

        // Separator
        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Section 2: Configure Individual Waypoints
        GUILayout.Label("Configure Waypoint Tasks", GetHeaderStyle());
        EditorGUILayout.Space(15);

        waypointParent = (GameObject)EditorGUILayout.ObjectField("Waypoint Parent", waypointParent, typeof(GameObject), true);

        if (waypointParent != null)
        {
            if (GUILayout.Button("Load Waypoints"))
            {
                LoadWaypoints();
            }

            if (waypoints.Count > 0)
            {
                EditorGUILayout.Space(15);
                GUILayout.Label($"Configuring {waypoints.Count} Waypoints", EditorStyles.boldLabel);

                if (GUILayout.Button("Reset All Waypoints"))
                {
                    ResetAllWaypoints();
                }

                EditorGUILayout.Space(15);

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));

                // Iterate over all waypoints and display configuration options
                for (int i = 0; i < waypoints.Count; i++)
                {
                    EditorGUILayout.BeginVertical("box");

                    EditorGUILayout.LabelField($"Waypoint: {waypoints[i].gameObject.name}", EditorStyles.boldLabel);

                    // Edit waypoint properties
                    waypoints[i].waypointName = EditorGUILayout.TextField("Name", waypoints[i].waypointName);
                    waypoints[i].taskType = (Waypoint.TaskType)EditorGUILayout.EnumPopup("Task Type", waypoints[i].taskType);

                    // Show fields based on task type
                    switch (waypoints[i].taskType)
                    {
                        case Waypoint.TaskType.KillEnemies:
                            waypoints[i].EnemyParent = (GameObject)EditorGUILayout.ObjectField("Enemy Parent", waypoints[i].EnemyParent, typeof(GameObject), true);
                            break;

                        case Waypoint.TaskType.DestroyObject:
                            waypoints[i].TargetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", waypoints[i].TargetObject, typeof(GameObject), true);
                            break;

                        case Waypoint.TaskType.CollectItem:
                            waypoints[i].CollectableItemsParent = (GameObject)EditorGUILayout.ObjectField("Collectable Object", waypoints[i].CollectableItemsParent, typeof(GameObject), true);
                            break;
                    }

                    // Arrow buttons for reordering waypoints
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("↑", GUILayout.Width(30)))
                    {
                        MoveWaypointUp(i);
                    }

                    if (GUILayout.Button("↓", GUILayout.Width(30)))
                    {
                        MoveWaypointDown(i);
                    }
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button("Zoom to Waypoint"))
                    {
                        ZoomToWaypoint(waypoints[i]);
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(10);
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.Space(30);

                // Save changes
                if (GUILayout.Button("Save All Configurations"))
                {
                    SaveAllWaypoints();
                }
            }
            else EditorGUILayout.HelpBox("No waypoints found under the selected parent. Click 'Load Waypoints' to find waypoints.", MessageType.Warning);
        }
        else EditorGUILayout.HelpBox("Select a parent GameObject that contains all waypoints.", MessageType.Info);
    }

    private void MoveWaypointUp(int index)
    {
        if (index > 0)
        {
            // Swap in the list
            var temp = waypoints[index];
            waypoints[index] = waypoints[index - 1];
            waypoints[index - 1] = temp;

            // Swap in the scene hierarchy
            waypoints[index].transform.SetSiblingIndex(index - 1);
            waypoints[index - 1].transform.SetSiblingIndex(index);

            Debug.Log($"Moved Waypoint {waypoints[index].waypointName} up to position {index - 1}");
        }
    }

    private void MoveWaypointDown(int index)
    {
        if (index < waypoints.Count - 1)
        {
            // Swap in the list
            var temp = waypoints[index];
            waypoints[index] = waypoints[index + 1];
            waypoints[index + 1] = temp;

            // Swap in the scene hierarchy
            waypoints[index].transform.SetSiblingIndex(index + 1);
            waypoints[index + 1].transform.SetSiblingIndex(index);

            Debug.Log($"Moved Waypoint {waypoints[index].waypointName} down to position {index + 1}");
        }
    }


    private void LoadWaypoints()
    {
        waypoints.Clear();

        // Find all Waypoint components under the parent GameObject
        Waypoint[] foundWaypoints = waypointParent.GetComponentsInChildren<Waypoint>();

        if (foundWaypoints.Length > 0)
        {
            waypoints.AddRange(foundWaypoints);
            Debug.Log($"Loaded {waypoints.Count} waypoints.");
        }
        else
        {
            Debug.LogWarning("No Waypoint components found under the selected parent.");
        }
    }


    private void SaveAllWaypoints()
    {
        foreach (var waypoint in waypoints)
        {
            EditorUtility.SetDirty(waypoint); // Mark as dirty so Unity saves changes
        }

        Debug.Log("All waypoint configurations saved.");
    }


    private void ToggleComponents(bool enable)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objects)
        {
            if (toggleMeshRenderers)
            {
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = enable;
                }
            }

            if (toggleColliders)
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = enable;
                }
            }
        }

        SceneView.RepaintAll(); // Refresh the scene view
    }


    private void ResetAllWaypoints()
    {
        if (EditorUtility.DisplayDialog("Reset All Waypoints",
                "Are you sure you want to reset all waypoints to their default settings?", "Yes", "No"))
        {
            foreach (var waypoint in waypoints)
            {
                waypoint.waypointName = "New Waypoint";
                waypoint.taskType = Waypoint.TaskType.ReachDestination;
                waypoint.EnemyParent = null;
                waypoint.TargetObject = null;

                EditorUtility.SetDirty(waypoint); // Mark as dirty to save changes
            }

            Debug.Log("All waypoints have been reset to their default values.");
        }
    }


    private void ZoomToWaypoint(Waypoint waypoint)
    {
        if (waypoint == null) return;

        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            Vector3 waypointPosition = waypoint.transform.position;

            // Move and zoom the SceneView camera to the waypoint's position
            sceneView.pivot = waypointPosition; // Move to position
            sceneView.size = 5f; // Adjust zoom level (lower = closer)
            sceneView.Repaint();

            Selection.activeGameObject = waypoint.gameObject;

            Debug.Log($"Zoomed to Waypoint: {waypoint.waypointName} at {waypointPosition}");
        }
        else Debug.LogWarning("No active SceneView found.");    
    }
}
