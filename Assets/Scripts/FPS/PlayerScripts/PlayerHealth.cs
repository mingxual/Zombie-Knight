using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Singleton Class
    public static PlayerHealth instance;
    public int totalHealth;
    public Text healthPanel;
    public Image health;

    private int currHealth;

    public GameObject[] scratchEffects;

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
    }

    void Start()
    {
        currHealth = totalHealth;        
    }

    public void Recover()
    {
        currHealth += 50;
        if (currHealth >= totalHealth)
            currHealth = totalHealth;
        healthPanel.text = "HP: " + currHealth.ToString();
        health.fillAmount = (float)currHealth / (float)totalHealth;
    }

    public void getHarm(int points, bool scratch)
    {
        currHealth -= points;
        currHealth = currHealth > 0 ? currHealth : 0;

        health.fillAmount = (float)currHealth / (float)totalHealth;
        healthPanel.text = "HP: " + currHealth.ToString();

        if (scratch)
        {
            Instantiate(scratchEffects[Random.Range(0, scratchEffects.Length)],
            Vector3.zero, Quaternion.identity);
        }

        if (currHealth <= 0)
        {
            GameControl.instance.failGame();
        }
    }
}
