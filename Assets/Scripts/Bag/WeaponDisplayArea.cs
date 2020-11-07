using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplayArea : MonoBehaviour
{
    public static WeaponDisplayArea instance;
    public GameObject panel;

    public GameObject generalPanel;
    public Image weaponIcon;
    public Text weaponName;
    public Text damagePoints;
    public Text numAmmos;

    public GameObject specialitemPanel;
    public Image itemIcon;
    public Text itemName;
    public Text nums;

    public int currIndex = -1;
    public string name = "";

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
        panel.SetActive(false);
        generalPanel.SetActive(false);
        specialitemPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set the panel content
    public void SetContent(Sprite weapon_icon, string weapon_name, int damage_point, int num_bullets)
    {
        name = weapon_name;

        if (damage_point == 0)
        {
            specialitemPanel.SetActive(true);
            generalPanel.SetActive(false);

            itemIcon.sprite = weapon_icon;
            itemName.text = weapon_name;
            nums.text = num_bullets.ToString();
        }
        else
        {
            generalPanel.SetActive(true);
            specialitemPanel.SetActive(false);

            weaponIcon.sprite = weapon_icon;
            weaponName.text = weapon_name;
            damagePoints.text = damage_point.ToString();
            numAmmos.text = num_bullets.ToString();
        }

        if(!panel.activeSelf)
        {
            panel.SetActive(true);
        }
    }

    public void SetNum(int num)
    {
        if(generalPanel.activeSelf)
        {
            numAmmos.text = num.ToString();
        }
        else
        {
            nums.text = num.ToString();
        }
    }
}
