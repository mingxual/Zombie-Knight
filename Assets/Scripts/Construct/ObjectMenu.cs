using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMenu : MonoBehaviour
{
    public Button[] constructMenuButtonList, weaponMenuButtonList;
    private bool buttonOn = true;
    private bool onConstructMenu = true;

    public GameObject constructMenu, weaponMenu;
    public CameraControl defenseCamera;

    public Button constructButton, weaponButton;

    private Vector3 clickColor = new Vector3(0.0f, 255.0f, 226.0f);

    void Start()
    {
        changeClickColor(0, true);
    }

    public void clickObjectMenu()
    {
        defenseCamera.switchScrollView(0);
        constructMenu.SetActive(true);
        weaponMenu.SetActive(false);
        onConstructMenu = true;
        changeClickColor(1, false);
        changeClickColor(0, true);
    }

    public void clickWeaponMenu()
    {
        defenseCamera.switchScrollView(1);
        constructMenu.SetActive(false);
        weaponMenu.SetActive(true);
        onConstructMenu = false;
        changeClickColor(0, false);
        changeClickColor(1, true);
    }

    void changeClickColor(int i, bool click)
    {
        ColorBlock color = ColorBlock.defaultColorBlock;
        if (i == 0)
        {
            color = constructButton.colors;
        }
        else if (i == 1)
        {
            color = weaponButton.colors;
        }
        Color c = Color.white;
        if (click)
        {
            c = Color.grey;
        }
        color.normalColor = c;
        if (i == 0)
            constructButton.colors = color;
        else if (i == 1)
        {
            weaponButton.colors = color;
        }
    }
}
