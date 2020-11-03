using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScratchEffect : MonoBehaviour
{
    public GameObject obj;
    public Image image;
    public float totalTime;
    private float currTime;

    void Start()
    {
        Vector3 pos = new Vector3(Random.Range(-300f, 300f) + 960f, 
                                  Random.Range(-100f, 100f) + 540f, 0f);
        obj.transform.position = pos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            print(image.rectTransform.position);

        currTime += Time.deltaTime;
        Color color = image.color;
        color.a = Mathf.Lerp(0f, 1f, 1-(currTime/totalTime));
        image.color = color;
        if (currTime >= totalTime)
        {
            Destroy(gameObject);
        }
    }
}
