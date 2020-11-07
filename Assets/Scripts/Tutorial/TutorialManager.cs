using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TYPE { Construction, FPS };
    public TYPE type; 

    public List<Sprite> pics;
    public List<string> descs;

    [SerializeField] private Button toNext;
    [SerializeField] private Button toPrev;
    [SerializeField] private Image image;
    [SerializeField] private Text desc;

    [Header("Construction Tutorial")]
    public GameObject objectMenu;
    public GameObject moneyText;

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(index == pics.Count - 1)
        {
            toNext.gameObject.SetActive(false);
        }

        image.sprite = pics[index];
        desc.text = descs[index];
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (type == TYPE.Construction)
        {
            objectMenu.SetActive(true);
            moneyText.SetActive(true);
        }
    }

    public void OnNext()
    {
        index += 1;
        changeDisplay();

        toPrev.gameObject.SetActive(true);
        bool boundary = index == pics.Count - 1 ? false : true;
        toNext.gameObject.SetActive(boundary);
    }

    public void OnPrev()
    {
        index -= 1;
        changeDisplay();

        toNext.gameObject.SetActive(true);
        bool boundary = index == 0 ? false : true;
        toPrev.gameObject.SetActive(boundary);
    }

    private void changeDisplay()
    {
        image.sprite = pics[index];
        desc.text = descs[index];
    }
}
