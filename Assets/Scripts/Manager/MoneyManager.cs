using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int totalMoney;
    public int currMoney;
    public Text moneyTextConstruct, moneyTextFPS;

    public AlertMessage alertMenu;

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

    // Start is called before the first frame update
    void Start()
    {
        currMoney = totalMoney;
        moneyTextConstruct.text = currMoney.ToString();
        moneyTextFPS.text = "Money: " + currMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool deductMoney(int amount)
    {
        if(currMoney - amount < 0)
        {
            alertMenu.alertMessage("You don't have enough money");
            return false;
        }
        else
        {
            currMoney -= amount;
            moneyTextConstruct.text = currMoney.ToString();
            moneyTextFPS.text = "Money: " + currMoney.ToString();
            return true;
        }
    }

    public void buyMedicineBag(int amount)
    {
        deductMoney(amount);
    }

    public void gainMoney(int amount)
    {
        currMoney += amount;
        moneyTextConstruct.text = currMoney.ToString();
        moneyTextFPS.text = "Money: " + currMoney.ToString();
    }
}
