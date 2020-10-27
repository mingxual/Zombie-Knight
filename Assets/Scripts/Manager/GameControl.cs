using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public GameObject Construct, FPS;

    public NavMeshSurface surface;

    public bool isFPS;

    public GameObject lightForScene;

    public GameObject bag;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Physics.IgnoreLayerCollision(9, 11);
        Physics.IgnoreLayerCollision(12, 11);
        Physics.IgnoreLayerCollision(16, 11);
        Physics.IgnoreLayerCollision(16, 18);
        Physics.IgnoreLayerCollision(11, 18);
    }


    void Start()
    {
        isFPS = false;
        lightForScene.SetActive(false);
        bag.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFPS = !isFPS;
            if (isFPS)
            {
                if (BagManager.instance.bagContent.Count == 0)
                {
                    Debug.Log("You have to buy at least one weapon to enter the game");
                }
                else
                {
                    WeaponSwitch.instance.getWeaponList();
                    switchToFPS();
                }
            }
            else switchToConstruct();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            bag.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            bag.SetActive(false);
        }
    }

    public void switchToConstruct()
    {
        Construct.SetActive(true);
        FPS.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void switchToFPS()
    {
        Construct.SetActive(false);
        FPS.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        surface.BuildNavMesh();
    }

    public void endGame()
    {

    }
}
