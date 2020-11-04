using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    public Image loadFill;
    public Text loadNum;

    public void startLoad()
    {
        StartCoroutine("Load");
    }

    IEnumerator Load()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Gameplay");
        
        if (operation.isDone)
        {
            gameObject.SetActive(false);
            loadFill.fillAmount = 0;
            loadNum.text = "Loading 0%";
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadFill.fillAmount = progress;
            loadNum.text = "Loading " + ((int)(progress * 100)).ToString() + "%";

            yield return null;
        }
    }
}
