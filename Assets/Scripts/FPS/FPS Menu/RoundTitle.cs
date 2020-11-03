using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTitle : MonoBehaviour
{
    public float totalTime;
    private float currTime = 0;

    public Text text;

    public AudioSource audio;

    void Update()
    {
        if (currTime == 0)
        {
            audio.Play();
        }
        currTime += Time.deltaTime;
        Color color = text.color;
        color.a = Mathf.Lerp(0.0f, 255.0f, 1.0f - currTime/totalTime) / 255.0f;
        text.color = color;
        if (currTime >= totalTime)
        {
            currTime = 0;
            gameObject.SetActive(false);
        }
    }

    public void updateText(string str)
    {
        text.text = str;
    }
}
