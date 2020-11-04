using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    private Vector3 lastPos, currPos;

    public PrefabGenerator generator;

    public float moveSpeed, scrollSpeed, rotateSpeed;
    public float minHeight, maxHeight;
    public float minAngle, maxAngle;
    [SerializeField]private Vector3 orbitPivot;

    private bool leftMousePressed;

    private GameObject currHover;

    public RectTransform objectMenu;
    public float objectPanelMinx, objectPanelMiny;
    private Rect checkObjectMenu;

    public Scrollbar[] scrollbarList;
    private Scrollbar scrollbar;

    public Utility utility;

    public bool pause;

    void Start()
    {
        pause = false;
        orbitPivot = Vector3.zero;
        lastPos = Input.mousePosition;
        leftMousePressed = false;
        scrollbar = scrollbarList[0];
        checkObjectMenu = new Rect(objectPanelMinx, objectPanelMiny, 
                                   objectMenu.rect.width, objectMenu.rect.height);
    }

    void Update()
    {
        if (pause) return;
     
        cameraMoveandRotate();
        cameraZoom();
        
        if (hoverObject() && !hoverObjectMenu())
        {
            deleteObject();
        }
    }

    bool hoverObject()
    {
        if (!Cursor.visible)
            return false;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (utility.belongToObj(hit.collider.gameObject.tag))
            {
                currHover = hit.collider.gameObject;
                return true;
            }
        }
        return false;
    }

    void deleteObject()
    {
        if (Input.GetMouseButtonDown(0) && currHover != null && !generator.isMovingObj)
        {
            if(currHover.GetComponent<ObjectProperty>() != null)
            {
                MoneyManager.instance.gainMoney(currHover.GetComponent<ObjectProperty>().cost);
            }
            Destroy(currHover);
            currHover = null;
        }
    }

    void cameraZoom()
    {
        if (hoverObjectMenu())
        {
            scrollbar.value = Mathf.Clamp
                (scrollbar.value + Input.GetAxis("Mouse ScrollWheel")*scrollSpeed, 0.0f, 1.0f);
        }
        else 
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (transform.position.y >= minHeight)
                {
                    transform.position = transform.position + scrollSpeed * transform.forward;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (transform.position.y <= maxHeight)
                {
                    transform.position = transform.position - scrollSpeed * transform.forward;
                }
            }
        }
    }

    void cameraMoveandRotate()
    {
        if (hoverObjectMenu())
            return;
        if (Input.GetMouseButton(0))
        {
            currPos = Input.mousePosition;
            if (!leftMousePressed)
            {
                lastPos = currPos;
                leftMousePressed = true;
            }
            Vector3 offset = currPos - lastPos;
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                transform.RotateAround(orbitPivot, new Vector3(0.0f, 1.0f, 0.0f), offset.x * rotateSpeed);
                float angleX = transform.rotation.eulerAngles.x;
                if (!(Input.GetAxis("Mouse Y") > 0 && (angleX <= minAngle || angleX > 180.0f)) &&
                    !(Input.GetAxis("Mouse Y") < 0 && (angleX >= maxAngle && angleX <= 180.0f)))
                {
                    transform.RotateAround(orbitPivot, transform.right, offset.y * -rotateSpeed);
                }
            }
            else
            {
                Vector3 newPos = transform.position;
                newPos = newPos + offset.y * moveSpeed * transform.up;
                newPos = newPos + offset.x * moveSpeed * transform.right;
                transform.position = newPos;
            }

            lastPos = currPos;
        }
        else if (!Input.GetMouseButton(0) && leftMousePressed)
        {
            leftMousePressed = false;
        }
    }

    bool hoverObjectMenu() // disable camera movement when mouse is on menu
    {
        return (checkObjectMenu.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
    }

    public float getRelativeHeight()
    {
        return transform.position.y / maxHeight;
    }

    public void switchScrollView(int x)
    {
        scrollbar = scrollbarList[x];
    }

    public void setOrbitpivot(Vector3 pos)
    {
        orbitPivot = pos;
    }
}
