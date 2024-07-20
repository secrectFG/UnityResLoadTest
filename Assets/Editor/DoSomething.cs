using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DoSomthing
{
    [UnityEditor.MenuItem("Tools/Do Something")]
    public static void Do()
    {
        var go = GameObject.Find("Camera_01_4k");
        var TextureReplacer = go.GetComponent<TextureReplacer>();
        var texFolder = "Camera_01_4k/textures";
        var texLowFolder = "Assets/Resources/Camera_01_4k/textures low";
        //读取文件夹里面的所有贴图
        var texturePaths = AssetDatabase.FindAssets("t:Texture", new string[] { texLowFolder
            
        });
        var textures = texturePaths.Select(x => 
        AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(x), typeof(Texture))).ToArray();
        // Debug.Log(texturePaths.Length);
        // Debug.Log(textures.Length);
        TextureReplacer.textureMapDatas = new TextureReplacer.TextureMapData[textures.Length];
        for (int i = 0; i < textures.Length; i++)
            TextureReplacer.textureMapDatas[i] = new TextureReplacer.TextureMapData()
            {
                texture_low = textures[i] as Texture2D,
                hdTextureAssetPath = texFolder + "/" + textures[i].name
            };
        EditorUtility.SetDirty(TextureReplacer);

        foreach (var item in TextureReplacer.textureMapDatas){
            var tex = AssetDatabase.LoadAssetAtPath<Texture>(item.hdTextureAssetPath);
            if (tex == null) Debug.Log(item.hdTextureAssetPath+" not found"); 
        }
    }

    //打包AssetBundle
    [UnityEditor.MenuItem("Tools/Build AssetBundles")]
    public static void BuildAssetBundles()
    {
        var assetBundleDir = Application.streamingAssetsPath;
        if (!System.IO.Directory.Exists(assetBundleDir))
        {
            System.IO.Directory.CreateDirectory(assetBundleDir);
        }
        UnityEditor.BuildPipeline.BuildAssetBundles(assetBundleDir, UnityEditor.BuildAssetBundleOptions.UncompressedAssetBundle, UnityEditor.BuildTarget.StandaloneWindows64);
    }


}
