using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : MonoBehaviour
{
    public AudioSource audio;

    [Header("Audio Clips")]
    public AudioClip[] zombieMoans;

    [Header("Delay")]
    public float startAfter;
    public float minDelay;
    public float maxDelay;
    private float delay;
    private bool isWaiting;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        isWaiting = true;
        StartCoroutine(afterTenSeconds());
    }

    void Update()
    {
        delay = (Random.Range(minDelay, maxDelay));
        if (!isWaiting)
        {
            StartCoroutine(zombieMoan());
            isWaiting = true;
        }
    }

    private IEnumerator afterTenSeconds()
    {
        yield return new WaitForSeconds(startAfter);
        isWaiting = false;
    }

    private IEnumerator zombieMoan()
    {
        audio.PlayOneShot(zombieMoans[Random.Range(0, zombieMoans.Length)]);
        yield return new WaitForSeconds(delay);
        isWaiting = false;
    }
}
