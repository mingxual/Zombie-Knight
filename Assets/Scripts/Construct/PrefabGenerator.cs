using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrefabGenerator : MonoBehaviour
{
    private Transform currObj;
    public Transform[] prefabList;
    public Transform objectParent;

    public Transform adjacentWallCheck;

    public Camera defenseCamera;
    public CameraControl defenseCameraControl;
    public Vector2 cameraMoveSpeedLimit;
    private float cameraMoveSpeed;

    public float startY;

    public Vector2 objMoveSpeedLimit;

    public float objTransparency;
    public Material redMaterial, greenMaterial;
    private Material objMaterial;

    private Rect terrainRect;
    public Transform terrain;
    private bool validPos;
    private ObjectProperty objProperty;

    public ObjectMenu objectMenu;

    public TorchManager torchManager;

    public bool isMovingObj;

    void Start()
    {
        isMovingObj = false;
        currObj = null;
        //Cursor.lockState = CursorLockMode.Confined;
        terrainRect = new Rect(terrain.position.x - terrain.localScale.x / 2.0f,
                               terrain.position.z - terrain.localScale.z / 2.0f,
                               terrain.localScale.x, terrain.localScale.z);
    }

    void Update()
    {
        cameraMoveSpeed = Mathf.Lerp(cameraMoveSpeedLimit.x, cameraMoveSpeedLimit.y,
            defenseCamera.GetComponent<CameraControl>().getRelativeHeight());

        if (currObj != null)
        {
            outBoundryMove();
            moveObject();

            checkValid();

            CreateDelete();
        }
    }

    void moveObject()
    {
        float y = startY + (objProperty.pivotCenter ? (currObj.localScale.y *
            objProperty.percentToUnitLength) / 2.0f : 0.0f);
        float xPos = Input.mousePosition.x;
        if (currObj.tag == "Spike")
            xPos += 50;
        Vector3 newPos = defenseCamera.ScreenToWorldPoint
            (new Vector3(xPos, Input.mousePosition.y, screenToWorldZ()));
        newPos.y = y;
        currObj.position = newPos;
        if (currObj.tag == "Wall")
        {
            newPos.y += currObj.localScale.y / 2.0f;
            adjacentWallCheck.position = newPos;
        }
    }

    void checkValid()
    {
        validPos = terrainRect.Contains(new Vector2(currObj.position.x, currObj.position.z)) &&
                   !objProperty.collided &&
                   torchManager.isCloseToTorch(currObj.position);

        if (!validPos)
        {
            currObj.GetComponent<MeshRenderer>().material = redMaterial;
        }
        else if (validPos)
        {
            currObj.GetComponent<MeshRenderer>().material = greenMaterial;
        }
    }

    void CreateDelete()
    {
        if (Input.GetMouseButtonDown(0) && validPos == true)
        {
            defenseCameraControl.setOrbitpivot(currObj.position); // make camera rotate with curr obj
            currObj.GetComponent<MeshRenderer>().material = objMaterial;
            finishObj();
        }
        if (Input.GetMouseButtonDown(1))
        {
            MoneyManager.instance.gainMoney(currObj.GetComponent<ObjectProperty>().cost);
            Destroy(currObj.gameObject);
            defenseCameraControl.setOrbitpivot(Vector3.zero);  // make camera rotate with (0, 0, 0)
            finishObj();
        }
    }

    void finishObj()
    {
        StartCoroutine("setMovingObj");
        currObj = null;
        Cursor.visible = true;
        objectMenu.gameObject.SetActive(true);
    }

    private IEnumerator setMovingObj()
    {
        yield return new WaitForSeconds(0.1f);
        isMovingObj = false;
    }

    void outBoundryMove()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 multiplier = Vector2.zero;
        if (mousePos.x < 0.0f) multiplier.x = -1.0f;
        else if (mousePos.x > 1915.0f) multiplier.x = 1.0f;
        if (mousePos.y < 4.0f) multiplier.y = -1.0f;
        else if (mousePos.y > 1079.0f) multiplier.y = 1.0f;
        
        Vector3 forward = new Vector3
            (defenseCamera.transform.forward.x, 0.0f, defenseCamera.transform.forward.z);
        Vector3 right = new Vector3
            (defenseCamera.transform.right.x, 0.0f, defenseCamera.transform.right.z);

        Vector3 pos = defenseCamera.transform.position;
        pos += multiplier.x * cameraMoveSpeed * defenseCamera.transform.right;
        Vector3 vertical = defenseCamera.transform.forward;
        vertical.y = 0.0f;
        pos += multiplier.y * cameraMoveSpeed * forward;
        defenseCamera.transform.position = pos;
        
        Vector3 newPos = currObj.position;
        newPos = newPos + multiplier.x * cameraMoveSpeed * right;
        newPos = newPos + multiplier.y * cameraMoveSpeed * forward;
        currObj.position = newPos;
    }

    private void objectInit()
    {
        Cursor.visible = false;
        Vector3 angle = Vector3.zero;
        if (currObj.tag == "Spike")
            angle = new Vector3(0, 180, -180);
        currObj = Instantiate(currObj, Vector3.zero, Quaternion.Euler(angle));
        currObj.SetParent(objectParent);
        objProperty = currObj.GetComponent<ObjectProperty>();
        float y = startY + (objProperty.pivotCenter ? (currObj.localScale.y * 
            objProperty.percentToUnitLength) / 2.0f : 0.0f);
        Vector3 pos = defenseCamera.ScreenToWorldPoint
            (new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenToWorldZ()));
        pos.y = y;
        currObj.position = pos;
        objectMenu.gameObject.SetActive(false);
        objMaterial = currObj.GetComponent<MeshRenderer>().material;
        isMovingObj = true;
    }

    float screenToWorldZ()
    {
        return 45.0f * defenseCamera.GetComponent<CameraControl>().getRelativeHeight() + 4.0f;
    }

    public bool objCollided()
    {
        if (objProperty) return objProperty.collided;
        return false;
    }

    public void GenerateObj(int i)
    {
        currObj = prefabList[i];
        bool res = MoneyManager.instance.deductMoney(currObj.GetComponent<ObjectProperty>().cost);
        if (!res)
        {
            currObj = null;
            return;
        }
        else
        {
            objectInit();
        }
    }
}
