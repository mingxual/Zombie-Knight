using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTorch : MonoBehaviour
{
    public TorchManager torchManager;

    public float offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && hasTorch())
        {
            Vector3 pos = transform.position;
            pos.y -= offset;
            torchManager.GenerateTorch(pos);
            consumeOneTorch();
        }
    }

    // Check if there is torches in bag
    bool hasTorch()
    {
        return true;
    }

    // remove one torch from bag
    void consumeOneTorch()
    { 
        
    }
}
