using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public List<Sprite> pics;
    public List<string> descs;

    [SerializeField] private Button toNext;
    [SerializeField] private Button toLast;
    [SerializeField] private Image image;
    [SerializeField] private Text desc;

    private int index = 0;


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
        toLast.gameObject.SetActive(false);
        if(index == pics.Count - 1)
        {
            toNext.gameObject.SetActive(false);
        }

        image.sprite = pics[index];
        desc.text = descs[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnNext()
    {
        index += 1;
        changeDisplay();

        toLast.gameObject.SetActive(true);
        bool boundary = index == pics.Count - 1 ? false : true;
        toNext.gameObject.SetActive(boundary);
    }

    public void OnLast()
    {
        index -= 1;
        changeDisplay();

        toNext.gameObject.SetActive(true);
        bool boundary = index == 0 ? false : true;
        toLast.gameObject.SetActive(boundary);
    }

    private void changeDisplay()
    {
        image.sprite = pics[index];
        desc.text = descs[index];
    }
}
