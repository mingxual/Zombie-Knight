using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    public static WeaponSwitch instance;
    public List<GameObject> weaponList;
    public Text numAmmoLeft;
    
    private GameObject currWeapon;
    private int count;
    private int currSelectedIdx;

    private List<KeyValuePair<int, int>> weaponMagazines;

    public int num_Grenades = 0;

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

    void Start()
    {
        // count = weaponList.Count;

        // currWeapon = weaponList[0];
        // weaponList[0].SetActive(true);
        // currSelectedIdx = 0;
        count = 0;
        currWeapon = null;
        currSelectedIdx = -1;
        weaponMagazines = new List<KeyValuePair<int, int>>();
    }

    public void Update()
    {
        if (count == 0) return;

        int prevSelectedIdx = currSelectedIdx;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ++currSelectedIdx;
            if(currSelectedIdx == count)
            {
                currSelectedIdx = 0;
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            --currSelectedIdx;
            if(currSelectedIdx < 0)
            {
                currSelectedIdx = count - 1;
            }
        }

        if(prevSelectedIdx != currSelectedIdx)
        {
            switchWeapon(currSelectedIdx);
        }
    }

    public void switchWeapon(int index)
    {
        if (currWeapon != null) currWeapon.SetActive(false);
        currWeapon = weaponList[weaponMagazines[currSelectedIdx].Key];
        currWeapon.SetActive(true);
    }

    // Get the bag list from BagManager.cs
    public void getWeaponList()
    {
        List<int> num_bullets = BagManager.instance.num_bullets;
        for (int i = 0; i < num_bullets.Count - 1; ++i)
        {
            if (num_bullets[i] > 0)
            {
                weaponMagazines.Add(new KeyValuePair<int, int>(i, num_bullets[i]));
            }
        }

        num_Grenades = num_bullets[num_bullets.Count - 1];

        count = weaponMagazines.Count;
        currSelectedIdx = 0;
        switchWeapon(currSelectedIdx);
    }

    // Throw the grenade
    public bool getGrenade()
    {
        if (num_Grenades > 0)
        {
            num_Grenades -= 1;
            int index = BagManager.instance.num_bullets.Count - 1;
            BagManager.instance.num_bullets[index] = num_Grenades;
            int cellIndex = BagManager.instance.bagContent[index];
            GridControl.instance.cells[cellIndex].AdjustNumBullets(num_Grenades);

            return true;
        }

        return false;
    }

    // Update the count (return the number of ammos available for the current magazine)
    public int rechargeMagazine(int capacity)
    {
        int key = weaponMagazines[currSelectedIdx].Key;
        int value = weaponMagazines[currSelectedIdx].Value;

        int numAmmoForCharge = -1;
        if (value >= capacity)
        {
            numAmmoForCharge = capacity;
        }
        else
        {
            numAmmoForCharge = value;
        }

        value -= numAmmoForCharge;
        numAmmoLeft.text = value.ToString();
        weaponMagazines[currSelectedIdx] = new KeyValuePair<int, int>(key, value);

        BagManager.instance.num_bullets[currSelectedIdx] = value;
        int cellIndex = BagManager.instance.bagContent[key];
        GridControl.instance.cells[cellIndex].AdjustNumBullets(value);
        if (WeaponDisplayArea.instance.currIndex == cellIndex)
        {
            WeaponDisplayArea.instance.numAmmos.text = value.ToString();
        }

        return numAmmoForCharge;
    }
}

class sortAlgorithm : IComparer<KeyValuePair<int, int>>
{
    public int Compare(KeyValuePair<int, int> a, KeyValuePair<int, int> b)
    {
        return a.Key < b.Key ? 0 : 1;
    }
}

