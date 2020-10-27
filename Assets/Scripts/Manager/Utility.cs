using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public string[] tags;

    public bool belongToObj(string tag)
    {
        foreach (string s in tags)
        {
            if (tag == s)
                return true;
        }
        return false;
    }

    public bool canCollideWhilwCreating(string tag)
    {
       return tag == "okCollide" || tag == "AdjacentWall" || tag == "SideWall";
    }
}
