using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector3 pos = player.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
