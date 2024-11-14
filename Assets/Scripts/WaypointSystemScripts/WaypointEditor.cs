using UnityEditor;
using UnityEngine;

public class WaypointEditor : EditorWindow
{
    private bool toggleMeshRenderers = true; 
    private bool toggleColliders = true; 
    private string targetTag = "Waypoint"; 

    [MenuItem("Tools/Waypoint Editor")]
    public static void ShowWindow()
    {
        GetWindow<WaypointEditor>("Waypoint Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Waypoint Editor", EditorStyles.boldLabel);

        // Input field for the tag
        targetTag = EditorGUILayout.TextField("Tag to Filter:", targetTag);

        // Toggle buttons
        toggleMeshRenderers = EditorGUILayout.Toggle("Toggle MeshRenderers", toggleMeshRenderers);
        toggleColliders = EditorGUILayout.Toggle("Toggle Colliders", toggleColliders);

        // Buttons to enable or disable components
        if (GUILayout.Button("Enable Components"))
        {
            ToggleComponents(true);
        }

        if (GUILayout.Button("Disable Components"))
        {
            ToggleComponents(false);
        }

        // Separator
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("Use this tool to toggle MeshRenderers or Colliders for all objects with the specified tag in the scene.", MessageType.Info);
    }

    private void ToggleComponents(bool enable)
    {
        // Find all GameObjects with the specified tag
        GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in objects)
        {
            if (toggleMeshRenderers)
            {
                // Enable/Disable MeshRenderer
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = enable;
                }
            }

            if (toggleColliders)
            {
                // Enable/Disable Collider
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = enable;
                }
            }
        }

        // Refresh the scene view to reflect the changes immediately
        SceneView.RepaintAll();
    }
}
