using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;


public class GameManager : MonoBehaviour
{
    public string url = "http://localhost:5000/stop_timer"; // Python服务器的URL
    private static readonly HttpClient client = new HttpClient();

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //告诉python启动完成了
        client.PostAsync(url, null).Wait();
        //加载main场景
        
        // SceneManager.LoadScene("Main");
        // StartCoroutine(LoadSceneCo("Main"));
    }

    public void StartLoad(){
        StartCoroutine(LoadSceneCo("Main"));
    }

    //协程加载场景
    public IEnumerator LoadSceneCo(string sceneName)
    {
        var stopwatch = Stopwatch.StartNew();
        var op = SceneManager.LoadSceneAsync(sceneName);
        yield return op;
        GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "加载场景耗时：" + stopwatch.ElapsedMilliseconds + "ms";
    }
}
