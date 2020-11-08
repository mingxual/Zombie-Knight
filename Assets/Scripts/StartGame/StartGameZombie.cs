using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameZombie : MonoBehaviour
{
    public Vector3 startPos, endPos;
    public float totalTime;
    private float currTime = 0.0f;

    public enum OPTION { startgame, handbook, exit };
    public OPTION option;

    public GameObject handbook, loadMenu;

    public FPSControllerLPFP.FpsControllerLPFP controller;
    public StartSniper sniper;

    void Start()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1;
    }

    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > totalTime)
            currTime = 0.0f;
        transform.position = Vector3.Lerp(startPos, endPos, currTime/totalTime);

        if (Input.GetKeyDown(KeyCode.J))
            print(Time.timeScale);
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.gameObject.tag;
        if (tag != "Concrete" && tag != "Dirt" && tag != "Wood")
        {
            if (option == OPTION.startgame)
            {
                StartCoroutine("WaitForStartGame");
            }
            else if (option == OPTION.handbook)
            {
                StartCoroutine("WaitForHandbook");
            }
            else if (option == OPTION.exit)
            {
                StartCoroutine("WaitForExitGame");
            }
        }
    }

    private IEnumerator WaitForStartGame()
    {
        controller.enabled = false;
        sniper.enabled = false;
        yield return new WaitForSeconds(1);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loadMenu.SetActive(true);
        StartCoroutine("WaitForStartGame2");
    }

    private IEnumerator WaitForStartGame2()
    {
        yield return new WaitForSeconds(1);
        loadMenu.GetComponent<LoadMenu>().startLoad();
    }

    private IEnumerator WaitForHandbook()
    {
        controller.enabled = false;
        sniper.enabled = false;
        yield return new WaitForSeconds(1);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        handbook.SetActive(true);
    }

    private IEnumerator WaitForExitGame()
    {
        controller.enabled = false;
        sniper.enabled = false;
        yield return new WaitForSeconds(2);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Application.Quit();
    }
}
