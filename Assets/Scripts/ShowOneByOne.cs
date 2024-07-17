using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOneByOne : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return null;
        }
        //这里这样做只是为了偷懒，实际一般使用图片进行遮挡
        GameObject MaskCam = GameObject.Find("MaskCam");
        if (MaskCam == null) yield break;
        ResLoader resLoader = GameObject.FindFirstObjectByType<ResLoader>();
        resLoader.fakeProgressSpeed *= 10f;
        while (resLoader.fakeProgress < 1f)
        {
            yield return null;
        }
        Destroy(MaskCam);
    }


}
