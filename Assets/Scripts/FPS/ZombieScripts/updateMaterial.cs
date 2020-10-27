using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateMaterial : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    public Material outlineMaterial;
    private Material normalMaterial;
    public SpeedUp playerSpeedUp;

    private bool lastSpeedUp;

    void Start()
    {
        lastSpeedUp = false;
        normalMaterial = mesh.material;
    }

    void Update()
    {
        if (lastSpeedUp && !playerSpeedUp.isSpeedingUp)
            mesh.material = normalMaterial;
        else if (!lastSpeedUp && playerSpeedUp.isSpeedingUp)
            mesh.material = outlineMaterial;
        lastSpeedUp = playerSpeedUp.isSpeedingUp;
    }
}
