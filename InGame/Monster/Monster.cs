using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster
{
    public string monName;
    public int monLevel;
    public float monHp;
    public int monStr;
    public int monDef;
    public float monCri;
    public float monAtDis;

    public Monster(string _name , int _level , float _hp , int _str, int _def, float _cri, float _atDis)
    {
        monName = _name;
        monLevel = _level;
        monHp = _hp;
        monStr = _str;
        monDef = _def;
        monCri = _cri;
        monAtDis = _atDis;
    }
}
