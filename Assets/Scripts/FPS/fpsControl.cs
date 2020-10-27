using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsControl : MonoBehaviour
{
    public GameObject realRadar, fpsRadar;
    public bool usingFpsRadar;

    void Update()
    {
        if (usingFpsRadar)
        {
            fpsRadar.SetActive(true);
            realRadar.SetActive(false);
        }
        else
        {
            fpsRadar.SetActive(false);
            realRadar.SetActive(true);
        }
    }
}
