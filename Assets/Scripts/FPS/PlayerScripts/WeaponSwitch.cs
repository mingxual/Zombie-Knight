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
    public int num_MedicineBags = 0;
    public int num_Torches = 0;

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
        currWeapon = weaponList[weaponMagazines[index].Key];
        currWeapon.SetActive(true);
        int value = weaponMagazines[index].Value;
        numAmmoLeft.text = value.ToString();
    }

    // Get the bag list from BagManager.cs
    public void getWeaponList()
    {
        weaponMagazines.Clear();

        List<int> num_bullets = BagManager.instance.num_bullets;
        for (int i = 0; i < num_bullets.Count - 3; ++i)
        {
            if (num_bullets[i] > 0)
            {
                weaponMagazines.Add(new KeyValuePair<int, int>(i, num_bullets[i]));
            }
        }

        num_Grenades = num_bullets[num_bullets.Count - 3];
        num_MedicineBags = num_bullets[num_bullets.Count - 2];
        num_Torches = num_bullets[num_bullets.Count - 1];

        count = weaponMagazines.Count;
        currSelectedIdx = 0;

        for(int i = 0; i < count; ++i)
        {
            currSelectedIdx = i;
            switchWeapon(i);

            if (currWeapon.GetComponent<AutomaticGunScriptLPFP>())
            {
                currWeapon.GetComponent<AutomaticGunScriptLPFP>().currentAmmo = 0;
                currWeapon.GetComponent<AutomaticGunScriptLPFP>().currentAmmo = rechargeMagazine(currWeapon.GetComponent<AutomaticGunScriptLPFP>().ammo, i);
            }
            else if (currWeapon.GetComponent<BoltActionSniperScriptLPFP>())
            {
                currWeapon.GetComponent<BoltActionSniperScriptLPFP>().currentAmmo = 0;
                currWeapon.GetComponent<BoltActionSniperScriptLPFP>().currentAmmo = rechargeMagazine(currWeapon.GetComponent<BoltActionSniperScriptLPFP>().ammo, i);
            }
            else if (currWeapon.GetComponent<GrenadeLauncherScriptLPFP>())
            {
                currWeapon.GetComponent<GrenadeLauncherScriptLPFP>().currentAmmo = 0;
                currWeapon.GetComponent<GrenadeLauncherScriptLPFP>().currentAmmo = rechargeMagazine(currWeapon.GetComponent<GrenadeLauncherScriptLPFP>().ammo, i);
            }
            else if (currWeapon.GetComponent<HandgunScriptLPFP>())
            {
                currWeapon.GetComponent<HandgunScriptLPFP>().currentAmmo = 0;
                currWeapon.GetComponent<HandgunScriptLPFP>().currentAmmo = rechargeMagazine(currWeapon.GetComponent<HandgunScriptLPFP>().ammo, i);
            }
            else if (currWeapon.GetComponent<PumpShotgunScriptLPFP>())
            {
                currWeapon.GetComponent<PumpShotgunScriptLPFP>().currentAmmo = 0;
                currWeapon.GetComponent<PumpShotgunScriptLPFP>().currentAmmo = rechargeMagazine(currWeapon.GetComponent<PumpShotgunScriptLPFP>().ammo, i);
            }
            else if (currWeapon.GetComponent<RocketLauncherScriptLPFP>())
            {
                currWeapon.GetComponent<RocketLauncherScriptLPFP>().currentAmmo = 0;
                currWeapon.GetComponent<RocketLauncherScriptLPFP>().currentAmmo = rechargeMagazine(currWeapon.GetComponent<RocketLauncherScriptLPFP>().ammo, i);
            }
            else if (currWeapon.GetComponent<SniperScriptLPFP>())
            {
                currWeapon.GetComponent<SniperScriptLPFP>().currentAmmo = 0;
                currWeapon.GetComponent<SniperScriptLPFP>().currentAmmo = rechargeMagazine(currWeapon.GetComponent<SniperScriptLPFP>().ammo, i);
            }
        }

        currSelectedIdx = 0;
        switchWeapon(currSelectedIdx);
    }

    // Throw the grenade
    public bool getGrenade()
    {
        if (num_Grenades > 0)
        {
            num_Grenades -= 1;
            int index = BagManager.instance.num_bullets.Count - 3;
            BagManager.instance.num_bullets[index] = num_Grenades;
            int cellIndex = BagManager.instance.bagContent[index];
            GridControl.instance.cells[cellIndex].AdjustNumBullets(num_Grenades);

            return true;
        }

        return false;
    }

    // Check if have medicinebag
    public bool hasMedicineBag()
    {
        return num_MedicineBags > 0;
    }

    // Check if have torch
    public bool hasTorch()
    {
        return num_Torches > 0;
    }

    // Deduct one medicinebag
    public void consumeMedicineBag()
    {
        num_MedicineBags -= 1;
        int index = BagManager.instance.num_bullets.Count - 2;
        BagManager.instance.num_bullets[index] = num_MedicineBags;
        int cellIndex = BagManager.instance.bagContent[index];
        GridControl.instance.cells[cellIndex].AdjustNumBullets(num_MedicineBags);
    }

    // Deduct one medicinebag
    public void consumeTorch()
    {
        num_Torches -= 1;
        int index = BagManager.instance.num_bullets.Count - 1;
        BagManager.instance.num_bullets[index] = num_Torches;
        int cellIndex = BagManager.instance.bagContent[index];
        GridControl.instance.cells[cellIndex].AdjustNumBullets(num_Torches);
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

        return numAmmoForCharge;
    }

    public void decreaseAmmo()
    {
        int key = weaponMagazines[currSelectedIdx].Key;
        int value = BagManager.instance.num_bullets[key];
        value -= 1;

        BagManager.instance.num_bullets[key] = value;
        int cellIndex = BagManager.instance.bagContent[key];
        GridControl.instance.cells[cellIndex].AdjustNumBullets(value);
        if (WeaponDisplayArea.instance.currIndex == cellIndex)
        {
            WeaponDisplayArea.instance.numAmmos.text = value.ToString();
        }
    }

    public int rechargeMagazine(int capacity, int index)
    {
        int key = weaponMagazines[index].Key;
        int value = weaponMagazines[index].Value;

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
        weaponMagazines[index] = new KeyValuePair<int, int>(key, value);

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

