using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public List<Sprite> pics;
    public List<string> descs;

    [SerializeField] private Button toNext;
    [SerializeField] private Button toPrev;
    [SerializeField] private Button close;
    [SerializeField] private Image image;
    [SerializeField] private Text desc;

    public GameObject objectMenu;
    public GameObject moneyText;

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        objectMenu.SetActive(false);
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
        objectMenu.SetActive(true);
        moneyText.SetActive(true);
    }

    public void OnNext()
    {
        index += 1;
        changeDisplay();
        if (index == pics.Count - 1)
        {
            toNext.gameObject.SetActive(false);
            close.gameObject.SetActive(true);
        }
        if (index == 1)
        {
            toPrev.gameObject.SetActive(true);
        }
    }

    public void OnPrev()
    {
        index -= 1;
        changeDisplay();
        if (index == pics.Count - 2)
        {
            toNext.gameObject.SetActive(true);
        }
        if (index == 0)
        {
            toPrev.gameObject.SetActive(false);
        }
    }

    private void changeDisplay()
    {
        image.sprite = pics[index];
        desc.text = descs[index];
    }
}
