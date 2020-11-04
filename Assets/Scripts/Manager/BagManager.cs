using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public static BagManager instance;

    // Match the weapon to its index in the bag
    public Dictionary<int, int> bagContent;

    public List<int> weaponCost;
    public List<Sprite> weaponIcon;
    public List<string> weaponName;
    public List<int> damage_points;
    public List<int> num_bullets;
    public List<int> num_bullets_per_magazine;

    public int last_cellIndex = 0;

    public AlertMessage AlertMenu;

    private void Awake()
    {
        if (instance == null)
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
        if (!result)
        {
            AlertMenu.alertMessage("You do not have enough money");
            return;
        }

        num_bullets[itemIndex] += num_bullets_per_magazine[itemIndex];

        if (bagContent.ContainsKey(itemIndex))
        {
            int cellIndex = bagContent[itemIndex];
            GridControl.instance.cells[cellIndex].AdjustNumBullets(num_bullets[itemIndex]);
        }
        else
        {
            bagContent[itemIndex] = last_cellIndex;
            GridControl.instance.cells[last_cellIndex].SetCell(weaponIcon[itemIndex], weaponName[itemIndex],
                damage_points[itemIndex], num_bullets[itemIndex]);
            last_cellIndex += 1;
        }
    }
}
