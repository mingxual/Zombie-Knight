using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handbook : MonoBehaviour
{
    public StartSniper sniper;
    public FPSControllerLPFP.FpsControllerLPFP controller;

    public GameObject prevButton, nextButton;

    public GameObject[] zombies;
    public GameObject[] descriptions;

    private int currIndex = 0;

    public void Close()
    {
        gameObject.SetActive(false);
        controller.enabled = true;
        sniper.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void prevZombie()
    {
        zombies[currIndex].SetActive(false);
        descriptions[currIndex].SetActive(false);
        if (currIndex == zombies.Length - 1)
        {
            nextButton.SetActive(true);
        }
        if (currIndex == 1)
        {
            prevButton.SetActive(false);
        }
        currIndex--;
        zombies[currIndex].SetActive(true);
        descriptions[currIndex].SetActive(true);
    }

    public void nextZombie()
    {
        zombies[currIndex].SetActive(false);
        descriptions[currIndex].SetActive(false);
        if (currIndex == zombies.Length - 2)
        {
            nextButton.SetActive(false);
        }
        if (currIndex == 0)
        {
            prevButton.SetActive(true);
        }
        currIndex++;
        zombies[currIndex].SetActive(true);
        descriptions[currIndex].SetActive(true);
    }
}
