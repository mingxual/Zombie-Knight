using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTorch : MonoBehaviour
{
    public TorchManager torchManager;

    public float offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 pos = transform.position;
            pos.y -= offset;
            torchManager.GenerateTorch(pos);
        }
    }
}
