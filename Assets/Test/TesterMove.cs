using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterMove : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6.0f;

    void Update()
    {
        Vector3 direction = new Vector3
            (Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
}
