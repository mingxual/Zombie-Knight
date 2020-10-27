using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDotControl : MonoBehaviour
{
    public static RedDotControl instance;
    public GameObject redDot;
    public Image first;
    public Image second;
    public Camera cam;
    public List<string> tags;

    private Vector3 originPoint;
    private Vector3 terminalPoint;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        raycastToScene();
    }

    public void setActive(bool value)
    {
        redDot.SetActive(value);
    }

    public void changeToGreen()
    {
        first.color = Color.green;
        second.color = Color.green;
    }

    public void changeToRed()
    {
        first.color = Color.red;
        second.color = Color.red;
    }

    public void raycastToScene()
    {
        originPoint = cam.ScreenToWorldPoint
            (new Vector3(first.transform.position.x, first.transform.position.y, cam.nearClipPlane));
        terminalPoint = cam.ScreenToWorldPoint
            (new Vector3(first.transform.position.x, first.transform.position.y, cam.farClipPlane));
        Vector3 dir = (terminalPoint - originPoint).normalized;
        RaycastHit raycastHit;
        if (Physics.Raycast(originPoint, dir, out raycastHit, Mathf.Infinity))
        {
            // Debug.DrawRay(originPoint, dir * raycastHit.distance, Color.red);
            string target = raycastHit.transform.tag;
            for(int i = 0; i < tags.Count; ++i)
            {
                if(tags[i] == target)
                {
                    changeToRed();
                    return;
                }
            }
            changeToGreen();
        }
        else
        {
            changeToGreen();
        }
    }
}
