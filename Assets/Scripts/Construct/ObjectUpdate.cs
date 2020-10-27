using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectUpdate : MonoBehaviour
{
    public NavMeshSurface surface;
    public int blood;
    private float life;

    void Start()
    {
        life = blood;
        surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
    }

    public bool Damage(int damage)
    {
        blood -= damage;
        Mathf.Clamp(blood, 0, life);
        Color color = transform.GetComponent<MeshRenderer>().material.color;
        color.a = Mathf.Lerp(0.0f, 1.0f, (float)blood/life);
        transform.GetComponent<MeshRenderer>().material.color = color;
        if (blood <= 0)
        {
            transform.gameObject.SetActive(false);
            surface.BuildNavMesh();
            return false;
        }
        return true;
    }
}
