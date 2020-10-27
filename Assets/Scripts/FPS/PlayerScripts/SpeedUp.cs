using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SpeedUp : MonoBehaviour
{
    public PostProcessResources postProcessResources;
    public PostProcessLayer postProcessLayer;
    public GameObject volume;
    public FPSControllerLPFP.FpsControllerLPFP controller;
    public float speedingTime;

    public bool isSpeedingUp;
    public float fastSpeed;

    public Image fillin;
    public GameObject xkey;

    private ChromaticAberration chromaticAberration;

    void Start()
    {
        postProcessLayer.Init(postProcessResources);
        isSpeedingUp = false;
    }

    void Update()
    {
        if (fillin.fillAmount == 1)
        {
            xkey.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.X) && !isSpeedingUp && fillin.fillAmount == 1)
        {
            fillin.fillAmount = 0f;
            StartCoroutine(speedUp());
            xkey.SetActive(false);
        }
    }

    public void increaseFillin(bool head)
    {
        fillin.fillAmount += head ? 0.05f : 0.02f;
    }

    private IEnumerator speedUp()
    {
        isSpeedingUp = true;
        controller.walkingSpeed = fastSpeed;
        volume.SetActive(true);

        yield return new WaitForSeconds(speedingTime);

        isSpeedingUp = false;
        controller.walkingSpeed = 5.0f;
        volume.SetActive(false);
    }
}
