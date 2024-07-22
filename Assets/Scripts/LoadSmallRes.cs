using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSmallRes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var res = Resources.Load<Texture2D>("images/a small picture");
        var Image = GameObject.Find("Image").GetComponent<UnityEngine.UI.Image>();
        Image.sprite = Sprite.Create(res, new Rect(0, 0, res.width, res.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
