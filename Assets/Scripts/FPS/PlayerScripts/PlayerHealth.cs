using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Singleton Class
    public static PlayerHealth instance;
    public int totalHealth, currHealth;
    public Text healthPanel;
    public Image health;

    public float hpShowBloodBorder;
    public GameObject bleedingBorder;

    public GameObject[] scratchEffects;

    public bool attacked;

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
        attacked = false;
    }

    public void Recover(int val)
    {
        currHealth += val;
        if (currHealth >= totalHealth)
            currHealth = totalHealth;
        if (currHealth > hpShowBloodBorder)
            bleedingBorder.SetActive(false);
        healthPanel.text = "HP: " + currHealth.ToString();
        health.fillAmount = (float)currHealth / (float)totalHealth;
    }

    public void getHarm(int points, bool scratch)
    {
        currHealth -= points;
        currHealth = currHealth > 0 ? currHealth : 0;

        StartCoroutine(UpdateAttacked());

        health.fillAmount = (float)currHealth / (float)totalHealth;
        healthPanel.text = "HP: " + currHealth.ToString();

        if (scratch)
        {
            Instantiate(scratchEffects[Random.Range(0, scratchEffects.Length)],
            Vector3.zero, Quaternion.identity);
        }

        if (currHealth <= hpShowBloodBorder)
            bleedingBorder.SetActive(true);

        if (currHealth <= 0)
        {
            GameControl.instance.failGame();
        }
    }

    private IEnumerator UpdateAttacked()
    {
        attacked = true;
        yield return new WaitForSeconds(0.1f);
        attacked = false;
    }
}
