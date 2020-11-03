using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public GameObject[] zombieList;
    public float[] zombieTime;

    void Start()
    {
        for (int i = 0; i < zombieList.Length; i++)
        {
            StartCoroutine(GenerateZombie(i));
        }
    }

    private IEnumerator GenerateZombie(int index)
    {
        yield return new WaitForSeconds(zombieTime[index]);
        zombieList[index].SetActive(true);
    }
}
