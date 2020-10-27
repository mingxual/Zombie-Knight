using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameZombie : MonoBehaviour
{
    public Vector3 startPos, endPos;
    public float totalTime;
    private float currTime = 0.0f;

    public enum OPTION { startgame, guidemenu, exit };
    public OPTION option;

    public GameObject player, guidemenu;

    public StartSniper sniper;

    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > totalTime)
            currTime = 0.0f;
        transform.position = Vector3.Lerp(startPos, endPos, currTime/totalTime);
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
            else if (option == OPTION.guidemenu)
            {
                StartCoroutine("WaitForGuide");
            }
            else if (option == OPTION.exit)
            {
                StartCoroutine("WaitForExitGame");
            }
        }
    }

    private IEnumerator WaitForStartGame()
    {
        float counter = 0;
        sniper.enabled = false;

        while (counter < 2.0f)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
    }

    private IEnumerator WaitForGuide()
    {
        float counter = 0;
  
        while (counter < 1.0f)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        player.SetActive(false);
        guidemenu.SetActive(true);
    }

    private IEnumerator WaitForExitGame()
    {
        float counter = 0;
        sniper.enabled = false;
        while (counter < 2.0f)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        print("quit");
        Application.Quit();
    }
}
