using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public static BagManager instance;
    public Dictionary<int, int> bagContent;
    public List<int> weaponCost;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bagContent = new Dictionary<int, int>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The function to be called when weapon is bought
    public void Purchase(int itemIndex)
    {
        bool result = MoneyManager.instance.deductMoney(weaponCost[itemIndex]);
        if(!result)
        {
            Debug.Log("You do not have enough money");
            return;
        }

        if(bagContent.ContainsKey(itemIndex))
        {
            ++bagContent[itemIndex];
        }
        else
        {
            bagContent.Add(itemIndex, 1);
        }
    }
}
