using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsControl : MonoBehaviour
{
    public GameObject realRadar, fpsRadar;
    public GameObject environmentEffect;
    public bool usingFpsRadar, muted;
    public LightningScript lightningManager;

    void Start()
    {
        usingFpsRadar = true;
        muted = false;
    }

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

    public void selectRadar(int val)
    {
        if (val == 0)
        {
            usingFpsRadar = true;
        }
        else
        {
            usingFpsRadar = false;
        }
    }

    public void setSoundEffect(bool val)
    {
        muted = !val;
    }

    public void setEnvironmentEffect(bool val)
    {
        environmentEffect.SetActive(val);
        if (val)
            lightningManager.Reset();
    }
}
