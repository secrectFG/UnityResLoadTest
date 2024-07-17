using UnityEngine;
using UnityEditor;

public class RemoveMissingScriptsTool : EditorWindow
{
    [MenuItem("Tools/Remove Missing Scripts")]
    public static void RemoveMissingScripts()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected!");
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
            EditorUtility.SetDirty(obj);
        }

        Debug.Log("Removed missing scripts from selected objects.");
    }
}
