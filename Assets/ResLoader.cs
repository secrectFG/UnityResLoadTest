using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResLoader : MonoBehaviour
{
    public GameObject showButton;

    public UnityEvent<float> onProgress = new UnityEvent<float>();


    Text logText;
    Transform canvasTransform;

    private void Awake() {
        canvasTransform = GameObject.Find("Canvas").transform;
        showButton.SetActive(false);
        logText = GameObject.Find("Log").GetComponent<Text>();
    }

    void Log(string msg){
        logText.text += msg+"\n";
    }

    GameObject loadedGameObject;

    public void OnLoad(){
        var GridLayoutPrefabPath = "GridLayout";
        StartCoroutine(LoadAsync(GridLayoutPrefabPath, (obj)=>{
            loadedGameObject = obj;
            obj.transform.SetParent(canvasTransform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            //去掉clone
            obj.name = obj.name.Replace("(Clone)", "");
        }));
    }

    public void OnLoadAssetBundle(){
        var GridLayoutPrefabPath = "GridLayout";
        StartCoroutine(LoadAssetBundleAsync(GridLayoutPrefabPath, (obj)=>{
            loadedGameObject = obj;
            obj.transform.SetParent(canvasTransform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            //去掉clone
            obj.name = obj.name.Replace("(Clone)", "");
        }));
    }

    IEnumerator LoadAsync(string path, System.Action<GameObject> callback){
        showButton.SetActive(false);
        Resources.UnloadUnusedAssets();//防止编辑器模式下，资源被缓存
        var stopWatch = Stopwatch.StartNew();
        var request = Resources.LoadAsync<GameObject>(path);
        while(!request.isDone){
            onProgress.Invoke(request.progress);
            yield return null;
        }
        var prefab = request.asset as GameObject;
        var obj = Instantiate(prefab);
        callback(obj);

        onProgress.Invoke(1);
        showButton.SetActive(true);
        Log("load cost "+(stopWatch.ElapsedMilliseconds / 1000f).ToString()+"s");
    }

    IEnumerator LoadAssetBundleAsync(string path, System.Action<GameObject> callback){

        showButton.SetActive(false);
        

        var stopWatch = Stopwatch.StartNew();
        var assetBundleDir = Application.streamingAssetsPath;
        var request = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleDir+"/test.ab");
        request.SendWebRequest();
        print("start download");
        while(!request.isDone){
            onProgress.Invoke(request.downloadProgress);
            yield return null;
        }
        print("download done");
        var assetBundle = DownloadHandlerAssetBundle.GetContent(request);
        var assetBundleRequest = assetBundle.LoadAssetAsync<GameObject>(path);
        while(!assetBundleRequest.isDone){
            onProgress.Invoke(assetBundleRequest.progress);
            print(assetBundleRequest.progress);
            yield return null;
        }
        // print("load done "+(stopWatch.ElapsedMilliseconds / 1000f).ToString()+"s");
        var prefab = assetBundleRequest.asset as GameObject;
        var obj = Instantiate(prefab);
        callback(obj);

        onProgress.Invoke(1);
        showButton.SetActive(true);
        Log("load assetbundle cost "+(stopWatch.ElapsedMilliseconds / 1000f).ToString()+"s");
    }

    public void ShowLoadedGameObject(){
        var stopWatch = Stopwatch.StartNew();
        loadedGameObject.SetActive(true);
        var cost = stopWatch.ElapsedMilliseconds / 1000f;
        Log("show loaded gameobject cost "+cost.ToString()+"s");
    }

    public void AddTestScene(){
        StartCoroutine(_LoadSceneAsync("test", ()=>{
            print("load test done");
        }));
    }

    IEnumerator _LoadSceneAsync(string path, System.Action callback){
        var stopWatch = Stopwatch.StartNew();
        var request = SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
        while(!request.isDone){
            onProgress.Invoke(request.progress);
            yield return null;
        }
        onProgress.Invoke(1);
        callback();
        var cost = stopWatch.ElapsedMilliseconds / 1000f;
        Log("load scene Additive cost "+cost.ToString()+"s");
    }

    public void LoadSplit(){
        StartCoroutine(_LoadSplit());
    }
    IEnumerator _LoadSplit(){
        var GridLayout = GameObject.Find("GridLayout");
        var prefabList = new List<GameObject>();
        var stopWatch = Stopwatch.StartNew();
        for(int i=1;i<=26;i++){
            var request = Resources.LoadAsync<GameObject>($"imageprefabs/{i}");
            while(!request.isDone){
                yield return null;
            }
            onProgress.Invoke(i/26f);
            var prefab = request.asset as GameObject;
            // var obj = Instantiate(prefab);
            prefabList.Add(prefab);
            // obj.transform.SetParent(GridLayout.transform);
        }

        foreach(var prefab in prefabList){
            var obj = Instantiate(prefab);
            obj.transform.SetParent(GridLayout.transform);
        }


        onProgress.Invoke(1);
        var cost = stopWatch.ElapsedMilliseconds / 1000f;
        Log("load scene split cost "+cost.ToString()+"s");
    }

    public void LoadABSplit(){
        StartCoroutine(_LoadABSplit());
    }
    IEnumerator _LoadABSplit(){
        var GridLayout = GameObject.Find("GridLayout");
        var prefabList = new List<GameObject>();
        var stopWatch = Stopwatch.StartNew();
        var assetBundleDir = Application.streamingAssetsPath;
        var request = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleDir+"/splits.ab");
        request.SendWebRequest();
        print("start download");
        while(!request.isDone){
            onProgress.Invoke(request.downloadProgress*0.5f);
            yield return null;
        }
        onProgress.Invoke(0.5f);
        var assetBundle = DownloadHandlerAssetBundle.GetContent(request);
        for(int i=1;i<=26;i++){
            var assetBundleRequest = assetBundle.LoadAssetAsync<GameObject>($"{i}");
            while(!assetBundleRequest.isDone){
                yield return null;
            }
            onProgress.Invoke(i/26f*0.5f+0.5f);
            var prefab = assetBundleRequest.asset as GameObject;
            // var obj = Instantiate(prefab);
            prefabList.Add(prefab);
            // obj.transform.SetParent(GridLayout.transform);
        }

        foreach(var prefab in prefabList){
            var obj = Instantiate(prefab);
            obj.transform.SetParent(GridLayout.transform);
        }
        onProgress.Invoke(1);
        var cost = stopWatch.ElapsedMilliseconds / 1000f;
        Log("load scene split cost "+cost.ToString()+"s");
    }

    public void LoadTextureAsync(){
        StartCoroutine(_LoadTextureAsync());
    }

    IEnumerator _LoadTextureAsync(){
        var stopWatch = Stopwatch.StartNew();

        //遍历文件夹
        var streamingAssetsPath = Application.streamingAssetsPath + "/images";
        var files = System.IO.Directory.GetFiles(streamingAssetsPath, "*.jpg");
        var GridLayout = GameObject.Find("GridLayout");
        foreach(var file in files){
            var stopWatch2 = Stopwatch.StartNew();
            var request = UnityWebRequestTexture.GetTexture($"file://{file}");
            request.SendWebRequest();
            while(!request.isDone){
                yield return null;
            }
            print($"cost 1 {stopWatch2.ElapsedMilliseconds / 1000f}s");
            stopWatch2.Restart();
            var texture = DownloadHandlerTexture.GetContent(request);
            
            print($"cost 2 {stopWatch2.ElapsedMilliseconds / 1000f}s");
            stopWatch2.Restart();
            var sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), Vector2.zero);
            
            print($"cost 3 {stopWatch2.ElapsedMilliseconds / 1000f}s");
            stopWatch2.Restart();
            var obj = new GameObject();
            obj.AddComponent<Image>().sprite = sprite;
            obj.transform.SetParent(GridLayout.transform);
            onProgress.Invoke(1f/files.Length);
            print($"cost 4 {stopWatch2.ElapsedMilliseconds / 1000f}s");
        }
        onProgress.Invoke(1);
        var cost = stopWatch.ElapsedMilliseconds / 1000f;
        Log("load texture cost "+cost.ToString()+"s");
    }
}
