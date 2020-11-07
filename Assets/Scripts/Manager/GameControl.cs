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

    public AlertMessage AlertMenu;

    private int currZombieNum;
    private int currLevel = 0;

    public float FinishFPSTime;

    public RoundTitle roundTitle;

    public GameObject player;

    public GameObject constructionPauseMenu, fpsPauseMenu;
    public fpsControl fps_control;

    public MoneyManager moneyManager;

    public Text zombieNumberText;

    public GameObject endGameMenu, gameFail, gameWin;
    public GameObject gameFailMusic, gameWinMusic;

    public LightningScript lightningManager;
    public ZombieSound zombieSound;

    public GameObject playerModel;

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
        Physics.IgnoreLayerCollision(8, 19);
        Physics.IgnoreLayerCollision(9, 19);
        Physics.IgnoreLayerCollision(18, 19);
        Physics.IgnoreLayerCollision(18, 18);
        Physics.IgnoreLayerCollision(18, 20);
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
                Pause(true);
                fpsPauseMenu.SetActive(true);
            }
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

        playerModel.SetActive(true);
        Vector3 pos = player.transform.position;
        pos.y = playerModel.transform.position.y;
        playerModel.transform.position = pos;
        playerModel.transform.rotation = player.transform.rotation;
    }

    public void switchToFPS()
    {
        if (BagManager.instance.bagContent.Count == 0)
        {
            AlertMenu.alertMessage("You forgot to purchase weapons");
            return;
        }

        isFPS = true;
        WeaponSwitch.instance.getWeaponList();
        Construct.SetActive(false);
        FPS.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        lightningManager.Reset();
        zombieSound.Reset();

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

        playerModel.SetActive(false);
    }

    public void killOneZombie(int val)
    {
        currZombieNum--;
        zombieNumberText.text = "Zombie Left: " + currZombieNum.ToString();
        moneyManager.gainMoney(val);
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
        StartCoroutine(fail());
    }

    private IEnumerator fail()
    {
        yield return new WaitForSeconds(FinishFPSTime);
        FPS.SetActive(false);
        endGameMenu.SetActive(true);
        StartCoroutine(WaitOneSecondAfterFinish(false));
    }

    public void winGame()
    {
        FPS.SetActive(false);
        endGameMenu.SetActive(true);
        StartCoroutine(WaitOneSecondAfterFinish(true));
    }

    private IEnumerator WaitOneSecondAfterFinish(bool win)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(1);
        StartCoroutine(GameEnd(win));
    }

    private IEnumerator GameEnd(bool win)
    {
        if (win)
            gameWin.SetActive(true);
        else
            gameFail.SetActive(true);
        yield return new WaitForSeconds(1);
        if (win)
            gameWinMusic.SetActive(true);
        else
            gameFailMusic.SetActive(true);
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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        else 
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void CloseFPSPauseMenu()
    {
        if (!fps_control.muted)
        {
            AudioListener.volume = 1;
        }
        Pause(false);
        fpsPauseMenu.SetActive(false);
    }
}