using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplayArea : MonoBehaviour
{
    public static WeaponDisplayArea instance;
    public GameObject panel;
    public Image weaponIcon;
    public Text weaponName;
    public Text damagePoints;
    public Text numAmmos;
    public int currIndex = -1;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set the panel content
    public void SetContent(Sprite weapon_icon, string weapon_name, int damage_point, int num_bullets)
    {
        weaponIcon.sprite = weapon_icon;
        weaponName.text = weapon_name;
        damagePoints.text = damage_point.ToString();
        numAmmos.text = num_bullets.ToString();

        if(!panel.activeSelf)
        {
            panel.SetActive(true);
        }
    }
}
