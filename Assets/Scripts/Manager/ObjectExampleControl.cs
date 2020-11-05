using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;

public class ObjectExampleControl : MonoBehaviour
{
    public CameraControl defenseCamera;
    public GameObject inspectMenu, objMenu;
    public GameObject[] objLists;
    public string[] objNameList;
    public Button[] purchaseButton;
    public Button[] purchaseBulletsButton;
    public Text objName;
    private int objIndex;

    public PostProcessVolume volume;
    private DepthOfField depthOfField;

    public void showObj(int index)
    {
        defenseCamera.pause = true;
        inspectMenu.SetActive(true);
        objMenu.SetActive(false);
        if (volume.profile.TryGetSettings(out depthOfField))
            depthOfField.aperture.value = 0.1f;
        objIndex = index;
        if (index < purchaseButton.Length - 1 && BagManager.instance.bagContent.ContainsKey(index))
        {
            purchaseBulletsButton[index].gameObject.SetActive(true);
        }
        else
        {
            purchaseButton[index].gameObject.SetActive(true);
        }
        objLists[objIndex].SetActive(true);
        objName.text = objNameList[index].ToString();
    }

    public void close()
    {
        defenseCamera.pause = false;
        inspectMenu.SetActive(false);
        objMenu.SetActive(true);
        if (volume.profile.TryGetSettings(out depthOfField))
            depthOfField.aperture.value = 5.6f;
        objLists[objIndex].SetActive(false);
        if (purchaseButton[objIndex].gameObject.activeSelf)
        {
            purchaseButton[objIndex].gameObject.SetActive(false);
        }
        else
        {
            purchaseBulletsButton[objIndex].gameObject.SetActive(false);
        }
    }
}
