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

    public void getHarm(int points)
    {
        currHealth -= points;
        currHealth = currHealth > 0 ? currHealth : 0;

        health.fillAmount = (float)currHealth / (float)totalHealth;
        healthPanel.text = "HP: " + currHealth.ToString();

        if (currHealth <= 0)
        {
            GameControl.instance.endGame();
        }
    }
}
