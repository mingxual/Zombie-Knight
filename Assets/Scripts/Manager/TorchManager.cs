using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchManager : MonoBehaviour
{
    public Transform torchPrefab;

    public List<Transform> torchs;
 
    public float radius;

    private int currIndex = 5;

    public bool isCloseToTorch(Vector3 pos)
    {
        foreach (Transform torch in torchs)
        {
            if (torch == null)
                break;
            if (Vector3.Distance(torch.position, pos) <= radius)
                return true;
        }
        return false;
    }

    public void GenerateTorch(Vector3 pos)
    {
        Transform t = Instantiate(torchPrefab, pos, Quaternion.identity);
        torchs[currIndex++] = t;
        t.SetParent(transform);
    }
}
