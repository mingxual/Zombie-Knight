using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guidemenu : MonoBehaviour
{
    public GameObject player, guidemenu;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SetActive(true);
            guidemenu.SetActive(false);
        }
    }
}
