using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public bool aroundAngle = true;

    private Vector3 lastPos, currPos;
    private bool leftMousePressed;

    private Vector3 orbitPoint;

    private float rotateSpeed = -0.2f;

    void Start()
    {
        lastPos = Input.mousePosition;
        leftMousePressed = false;
        orbitPoint = transform.position;
        orbitPoint.z = 0.362f;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currPos = Input.mousePosition;
            if (!leftMousePressed)
            {
                lastPos = currPos;
                leftMousePressed = true;
            }

            Vector3 offset = Input.mousePosition - lastPos;
            lastPos = Input.mousePosition;

            float angle = transform.rotation.eulerAngles.y;
            angle += offset.x * rotateSpeed;
            if (aroundAngle)
            {
                transform.RotateAround(orbitPoint,
                    new Vector3(0.0f, 1.0f, 0.0f), offset.x * rotateSpeed);
            }
            else
            {
                transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), offset.x * rotateSpeed);
            }
            lastPos = currPos;
        }
        else if (!Input.GetMouseButton(0) && leftMousePressed)
        {
            leftMousePressed = false;
        }
    }
}
