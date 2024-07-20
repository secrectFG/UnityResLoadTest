using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TextureReplacer : MonoBehaviour
{
    [System.Serializable]
    public class TextureMapData
    {
        public Texture2D texture_low;
        public string hdTextureAssetPath;
    }

    public TextureMapData[] textureMapDatas;
    string[] properties = new string[]{
        "_MainTex",
        "_BumpMap",
        "_SpecGlossMap",
        "_MetallicGlossMap",
    };

    // 调用此方法开始替换贴图
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        var renderer = GetComponentInChildren<Renderer>();

        foreach (var mat in renderer.materials)
        {
            foreach (var prop in properties)
            {
                var tex = mat.GetTexture(prop);
                if (tex != null)
                {
                    var item = textureMapDatas.FirstOrDefault(x => x.texture_low == tex);
                    if( item != null){
                        var req = Resources.LoadAsync<Texture>(item.hdTextureAssetPath);
                        yield return req;
                        if(req.asset != null){
                            yield return new WaitForSeconds(0.1f);//这个例子中贴图较少，很快就加载完了，所以这里做一个模拟
                            mat.SetTexture(prop, req.asset as Texture);

                            }
                        else Debug.Log("Failed to load texture " + item.hdTextureAssetPath);    
                    }
                    else{
                        Debug.Log("No matching texture found for " + tex.name);
                    }
                }
            }
        }
    }




}
