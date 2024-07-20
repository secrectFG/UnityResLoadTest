using UnityEditor;
using UnityEngine;

public class ReplaceTextures : EditorWindow
{

    [MenuItem("Tools/Replace Textures")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceTextures>("Replace Textures");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Replace Textures for Selected Models"))
        {
            ReplaceSelectedModelTextures();
        }
    }

    private void ReplaceSelectedModelTextures()
    {
        var selectedObjects = Selection.gameObjects;

        foreach (var obj in selectedObjects)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                foreach (var mat in renderer.sharedMaterials)
                {
                    if (mat == null)
                    {
                        continue;
                    }

                    // Find and replace textures in the material
                    ReplaceMaterialTextures(mat);
                }
            }
        }

        Debug.Log("Textures replaced successfully!");
    }

    private void ReplaceMaterialTextures(Material mat)
    {
        // Iterate through all texture properties in the material
        Shader shader = mat.shader;
        int propertyCount = ShaderUtil.GetPropertyCount(shader);

        for (int i = 0; i < propertyCount; i++)
        {
            if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                string propertyName = ShaderUtil.GetPropertyName(shader, i);
                Texture originalTexture = mat.GetTexture(propertyName);

                if (originalTexture != null)
                {
                    string originalTexturePath = AssetDatabase.GetAssetPath(originalTexture);
                    string originalTextureName = originalTexture.name;
                    string newTextureAssetPath = originalTexturePath.Replace("textures low","textures"); // Replace the texture name with "NewTexture"

                    

                    Texture newTexture = AssetDatabase.LoadAssetAtPath<Texture>(newTextureAssetPath);
                    if (newTexture != null)
                    {
                        mat.SetTexture(propertyName, newTexture);
                        Debug.Log($"Replaced texture '{originalTextureName}' with '{newTextureAssetPath}'");
                    }
                    else
                    {
                        Debug.LogWarning($"New texture '{newTextureAssetPath}' not found for property '{propertyName}'");
                    }
                }
            }
        }
    }
}
