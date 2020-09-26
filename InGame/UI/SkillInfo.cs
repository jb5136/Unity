using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo
{
    public string name;
    public float level1;
    public float level2;
    public float level3;
    public float level4;
    public float level5;
    public float coolTime;
    public string desc1;
    public string desc2;
    public Sprite image;

    public SkillInfo(string _name,float L1, float L2, float L3, float L4, float L5,float time,string d1,string d2, Sprite img)
    {
        name = _name;
        level1 = L1;
        level2 = L2;
        level3 = L3;
        level4 = L4;
        level5 = L5;
        coolTime = time;
        desc1 = d1;
        desc2 = d2;
        image = img;
    }
}


