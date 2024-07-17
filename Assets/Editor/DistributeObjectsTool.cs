using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DistributeObjectsTool : EditorWindow
{
    private float radius = 5.0f;

    [MenuItem("Tools/Distribute Objects")]
    public static void ShowWindow()
    {
        GetWindow<DistributeObjectsTool>("Distribute Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Distribute Selected Objects Spherically", EditorStyles.boldLabel);
        radius = EditorGUILayout.FloatField("Radius", radius);

        if (GUILayout.Button("Distribute"))
        {
            DistributeObjectsSpherically();
        }
    }

    private void DistributeObjectsSpherically()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected!");
            return;
        }

        // Ensure the first object (center) remains stationary
        Vector3 centerPosition = selectedObjects[0].transform.position;

        // Distribute objects spherically around the center
        List<Vector3> distributedPositions = DistributeSpherically(selectedObjects.Length - 1, centerPosition, radius);

        // Apply distributed positions to selected objects
        for (int i = 1; i < selectedObjects.Length; i++)
        {
            selectedObjects[i].transform.position = distributedPositions[i - 1];
        }
    }

    private List<Vector3> DistributeSpherically(int count, Vector3 center, float radius)
    {
        List<Vector3> positions = new List<Vector3>();
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2; // Golden ratio to create evenly distributed points

        for (int i = 0; i < count; i++)
        {
            float theta = 2 * Mathf.PI * i / goldenRatio;
            float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / count);
            float x = center.x + radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = center.y + radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = center.z + radius * Mathf.Cos(phi);
            positions.Add(new Vector3(x, y, z));
        }

        return positions;
    }
}
