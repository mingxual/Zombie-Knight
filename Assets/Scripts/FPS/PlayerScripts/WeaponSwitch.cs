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
        Dictionary<int, int> bagList = BagManager.instance.bagContent;
        foreach(KeyValuePair<int, int> entry in bagList)
        {
            weaponMagazines.Add(entry);
        }

        sortAlgorithm comp = new sortAlgorithm();
        weaponMagazines.Sort(comp);

        int numMagazines = -1;
        int key = -1;
        int value = -1;

        for(int i = 0; i < weaponMagazines.Count; ++i)
        {
            numMagazines = weaponMagazines[i].Value;
            key = weaponMagazines[i].Key;
            if (weaponList[weaponMagazines[i].Key].GetComponent<AutomaticGunScriptLPFP>())
            {
                value = numMagazines * weaponList[weaponMagazines[i].Key].GetComponent<AutomaticGunScriptLPFP>().ammo;
            }
            else if (weaponList[weaponMagazines[i].Key].GetComponent<HandgunScriptLPFP>())
            {
                value = numMagazines * weaponList[weaponMagazines[i].Key].GetComponent<HandgunScriptLPFP>().ammo;
            }
            else if (weaponList[weaponMagazines[i].Key].GetComponent<PumpShotgunScriptLPFP>())
            {
                value = numMagazines * weaponList[weaponMagazines[i].Key].GetComponent<PumpShotgunScriptLPFP>().ammo;
            }
            else if (weaponList[weaponMagazines[i].Key].GetComponent<BoltActionSniperScriptLPFP>())
            {
                value = numMagazines * weaponList[weaponMagazines[i].Key].GetComponent<BoltActionSniperScriptLPFP>().ammo;
            }
            else if (weaponList[weaponMagazines[i].Key].GetComponent<SniperScriptLPFP>())
            {
                value = numMagazines * weaponList[weaponMagazines[i].Key].GetComponent<SniperScriptLPFP>().ammo;
            }
            else if(weaponList[weaponMagazines[i].Key].GetComponent<GrenadeLauncherScriptLPFP>())
            {
                value = numMagazines * weaponList[weaponMagazines[i].Key].GetComponent<GrenadeLauncherScriptLPFP>().ammo;
            }
            else if(weaponList[weaponMagazines[i].Key].GetComponent<RocketLauncherScriptLPFP>())
            {
                value = numMagazines * weaponList[weaponMagazines[i].Key].GetComponent<RocketLauncherScriptLPFP>().ammo;
            }

            weaponMagazines[i] = new KeyValuePair<int, int>(key, value);
        }

        count = weaponMagazines.Count;
        currSelectedIdx = 0;
        switchWeapon(currSelectedIdx);
    }

    // Update the count (return the number of ammos available for the current magazine)
    public int rechargeMagazine(int capacity)
    {
        int key = weaponMagazines[currSelectedIdx].Key;
        int value = weaponMagazines[currSelectedIdx].Value;

        int numAmmoForCharge = -1;
        if(value >= capacity)
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
}

class sortAlgorithm : IComparer<KeyValuePair<int, int>>
{
    public int Compare(KeyValuePair<int, int> a, KeyValuePair<int, int> b)
    {
        return a.Key < b.Key ? 0 : 1;
    }
}

