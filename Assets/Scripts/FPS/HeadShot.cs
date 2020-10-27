using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadShot : MonoBehaviour
{
    private float time = 2f, currTime;
    public Image image;
    
    void Update()
    {
        currTime += Time.deltaTime;
        Color color = image.color;
        color.a = Mathf.Lerp(1f, 0f, currTime / time);
        image.color = color;
        if (currTime > time)
            Destroy(gameObject);
    }
}
