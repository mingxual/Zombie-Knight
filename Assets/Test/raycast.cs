using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycast : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        
        Vector3 dir = new Vector3(1, 0, 0);
        Debug.DrawRay(transform.position, dir * 10, Color.blue);
        if (Input.GetKeyDown(KeyCode.S))
        {
            print(dir);
        }

       /* if (Physics.Raycast(transform.position, dir, out hit, 10))
        {
            if (hit.collider.gameObject.layer == zombieLayer &&
                 !colliderList.Contains(hit.collider))
            {
                print("collide");
                colliderList.Add(hit.collider);
            }
        }*/
    }
}
