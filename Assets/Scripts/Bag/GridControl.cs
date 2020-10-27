using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    public static GridControl instance;
    public List<Cell> cells;

    public int currSelected = -1;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.instance.isFPS)
        {
            if(currSelected == -1)
            {
                currSelected = 0;
                cells[currSelected].OnClick(currSelected);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currSelected - 5 >= 0)
                {
                    cells[currSelected].bg.color = cells[currSelected].origin;
                    currSelected -= 5;
                    cells[currSelected].OnClick(currSelected);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currSelected + 5 < cells.Count && cells[currSelected + 5].button.isActiveAndEnabled)
                {
                    cells[currSelected].bg.color = cells[currSelected].origin;
                    currSelected += 5;
                    cells[currSelected].OnClick(currSelected);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currSelected % 5 > 0)
                {
                    cells[currSelected].bg.color = cells[currSelected].origin;
                    currSelected -= 1;
                    cells[currSelected].OnClick(currSelected);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && cells[currSelected + 1].button.isActiveAndEnabled)
            {
                if (currSelected % 5 < 4)
                {
                    cells[currSelected].bg.color = cells[currSelected].origin;
                    currSelected += 1;
                    cells[currSelected].OnClick(currSelected);
                }
            }
        }
    }
}
