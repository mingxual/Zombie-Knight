using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public Transform sweepTransform;
    public Transform player;

    public Transform radarPing;

    public float rotateSpeed;
    public float radarDist;

    private List<Collider> colliderList;

    private int layerMask = 1 << 9;

    void Start()
    {
        colliderList = new List<Collider>();
    }

    void Update()
    {
        Vector3 pos = player.position;
        pos.y = transform.position.y;
        transform.position = pos;

        float lastRotation = (sweepTransform.eulerAngles.y % 360f) - 180f;
        sweepTransform.eulerAngles -= new Vector3(0f, 0f, rotateSpeed * Time.deltaTime);
        float currRotation = (sweepTransform.eulerAngles.y % 360f) - 180f;

        if (lastRotation < 0f && currRotation > 0f)
        {
            colliderList.Clear();
        }

        Quaternion qua = Quaternion.AngleAxis(sweepTransform.eulerAngles.y + 90.0f, Vector3.up);
        Vector3 dir = qua * Vector3.forward;
        
        Quaternion q = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);
        Vector3 d = q * Vector3.forward;
        //Debug.DrawRay(transform.position, -Vector3.forward * radarDist, Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, radarDist, layerMask);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (!colliderList.Contains(hit.collider))
                {
                    colliderList.Add(hit.collider);
                    Quaternion angle = Quaternion.Euler(90f, 0f, 0f);
                    Transform p = Instantiate(radarPing, hit.point, angle);
                    p.SetParent(transform);
                    RadarPing ping = p.GetComponent<RadarPing>();
                    ping.SetColor(Color.red);
                    // make sure old ping disappear before new ping
                    ping.SetDisappearTimer(360f / rotateSpeed); 
                }
            }
        }
    }
}
