using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image weaponIcon;
    public Button button;
    public int damagePoint;
    public int numBullets;
    public string weaponName;
    public WeaponDisplayArea panel;
    public Text numAmmos;
    public Image bg;
    public Color origin;

    // Start is called before the first frame update
    void Start()
    {
        button.enabled = false;
        numBullets = 0;
        numAmmos.text = "";
        origin = bg.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set the cell
    public void SetCell(Sprite weapon_icon, string weapon_name, int damage_point, int num_bullets)
    {
        weaponIcon.sprite = weapon_icon;
        weaponName = weapon_name;
        damagePoint = damage_point;
        AdjustNumBullets(num_bullets);
        button.enabled = true;
    }

    // Update the ammo
    public void AdjustNumBullets(int num_bullets)
    {
        numBullets = num_bullets;
        numAmmos.text = num_bullets.ToString();
    }


    // Onclick callback function
    public void OnClick(int index)
    {
        int currIndex = GridControl.instance.currSelected;
        if(currIndex != -1)
        {
            GridControl.instance.cells[currIndex].bg.color = GridControl.instance.cells[currIndex].origin;
        }
        GridControl.instance.currSelected = index;
        bg.color = Color.white;
        panel.SetContent(weaponIcon.sprite, weaponName, damagePoint, numBullets);
        panel.currIndex = index;
    }
}
