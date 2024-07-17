using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public float avgFramerate;
    public float refresh = 0.5f;
    string display = "{0} FPS";
    private Text m_Text;
 
    private IEnumerator Start()
    {
        m_Text = GetComponent<Text>();
        var waitForSecondsRealtime = new WaitForSecondsRealtime(refresh);
        while (true)
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return waitForSecondsRealtime;
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;
 
            // Display it
            avgFramerate = frameCount / timeSpan;
            m_Text.text = string.Format(display, avgFramerate.ToString("0.00"));
        }
    }
 
 

    
}
