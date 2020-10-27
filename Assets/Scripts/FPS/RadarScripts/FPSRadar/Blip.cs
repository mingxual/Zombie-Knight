using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blip : MonoBehaviour
{
    public Transform player;
    public RectTransform blip;
    public RectTransform background;
    public fpsControl control;
    public float range, radius;

    private float timer = 0f;

    void Update()
    {
        if (control.usingFpsRadar)
            blip.gameObject.SetActive(true);
        else
        {
            blip.gameObject.SetActive(false);
            return;
        }

        Vector3 forward = player.forward;
        forward.y = 0f;
        Vector3 dist = transform.position - player.position;
        dist.y = 0;

        if (dist.magnitude > range)
        {
            blip.gameObject.SetActive(false);
        }
        else 
        {
            timer += Time.deltaTime;
            Color color = blip.GetComponent<Image>().color;
            color.a = Mathf.Lerp(0.3f, 1f, timer);
            blip.GetComponent<Image>().color = color;
            if (timer >= 1.0f)
                timer = 0f;

            blip.gameObject.SetActive(true);
            float angle = Vector3.SignedAngle(forward, dist, -Vector3.up);
            
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            blip.position = background.position + (dist.magnitude / range) * radius * dir;
        }
    }
}
