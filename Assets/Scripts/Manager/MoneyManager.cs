using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int totalMoney;
    public int currMoney;
    public Text moneyText;

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
        moneyText.text = currMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool deductMoney(int amount)
    {
        if(currMoney - amount < 0)
        {
            return false;
        }
        else
        {
            currMoney -= amount;
            moneyText.text = currMoney.ToString();
            return true;
        }
    }

    public void gainMoney(int amount)
    {
        currMoney += amount;
        moneyText.text = currMoney.ToString();
    }
}
