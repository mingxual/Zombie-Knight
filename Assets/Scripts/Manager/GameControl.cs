using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public GameObject Construct, FPS;

    public NavMeshSurface surface;

    public bool isFPS;

    public GameObject lightForScene;

    public GameObject bag;

    public GameObject forgetBuyWeapon;

    private int currZombieNum;
    private int currLevel = 0;

    public float FinishFPSTime;

    public RoundTitle roundTitle;

    public GameObject player;

    public GameObject constructionPauseMenu, fpsPauseMenu;
    public fpsControl fps_control;

    public Text zombieNumberText;

    public GameObject[] mLevels;

    private void Awake()
    {
        if (instance == null)
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isFPS)
            {
                constructionPauseMenu.SetActive(true);
            }
            else
            {
                /*Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                AudioListener.volume = 0;
                fpsPauseMenu.SetActive(true);*/
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            print(fps_control.muted);
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
        isFPS = false;
        Construct.SetActive(true);
        FPS.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        player.GetComponent<SpeedUp>().turnOffSpeedUp();
    }

    public void switchToFPS()
    {
        if (BagManager.instance.bagContent.Count == 0)
        {
            forgetBuyWeapon.SetActive(true);
            return;
        }

        isFPS = true;
        WeaponSwitch.instance.getWeaponList();
        Construct.SetActive(false);
        FPS.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (currLevel < mLevels.Length - 1)
        {
            roundTitle.updateText("Round " + (currLevel + 1).ToString());
        }
        else
        {
            roundTitle.updateText("Boss");
        }
        roundTitle.gameObject.SetActive(true);

        surface.BuildNavMesh();

        GameObject generator = mLevels[currLevel];
        generator.SetActive(true);
        currZombieNum = generator.GetComponent<ZombieGenerator>().zombieList.Length;
        zombieNumberText.text = "Zombie Left: " + currZombieNum.ToString();
    }

    public void killOneZombie()
    {
        currZombieNum--;
        zombieNumberText.text = "Zombie Left: " + currZombieNum.ToString();
        if (currZombieNum <= 0)
        {
            StartCoroutine("FinishCurrentLevel");
        }
    }

    private IEnumerator FinishCurrentLevel()
    {
        yield return new WaitForSeconds(FinishFPSTime);
        mLevels[currLevel].SetActive(false);
        currLevel++;
        if (currLevel == mLevels.Length)
        {
            winGame();
        }
        else
        {
            switchToConstruct();
        }
    }

    public void failGame()
    {

    }

    public void winGame()
    {

    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartGame");
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0.0f;
        }
        else 
        {
            Time.timeScale = 1.0f;
        }
    }

    public void CloseFPSPauseMenu()
    {
        if (!fps_control.muted)
        {
            AudioListener.volume = 1;
        }
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        fpsPauseMenu.SetActive(false);
    }
}