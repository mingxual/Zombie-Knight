using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class origin : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector3 angle = transform.localEulerAngles;
        angle.z = -player.eulerAngles.y;
        transform.localEulerAngles = angle;
    }
}
