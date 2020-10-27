using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectProperty : MonoBehaviour
{
    public bool collided;
    public bool pivotCenter;
    public float percentToUnitLength = 1.0f;
    public int cost;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag != "Terrain" && 
           collision.collider.gameObject.tag != "AdjacentWall" &&
           collision.collider.gameObject.tag != "SideWall")
            collided = true;
    }

    void OnCollisionExit()
    {
        collided = false;
    }
}
