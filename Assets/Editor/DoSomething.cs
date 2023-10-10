using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomthing 
{
    [UnityEditor.MenuItem("Tools/Do Something")]
    public static void Do()
    {
        var imagesDir = Application.dataPath + "/Resources/images";
        var files = System.IO.Directory.GetFiles(imagesDir);
        var sprites = new List<Sprite>();
        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;
            var path = "images/" + System.IO.Path.GetFileNameWithoutExtension(file);
            var sprite = Resources.Load<Sprite>(path);
            sprites.Add(sprite);
        }
        var Image = GameObject.Find("Image");
        foreach (var sprite in sprites)
        {
            var img = Object.Instantiate(Image);
            img.name = sprite.name;
            img.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
            img.transform.SetParent(Image.transform.parent);
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
