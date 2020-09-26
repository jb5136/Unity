using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    public static Database instance;
    public List<Item> items = new List<Item>();
    public List<SkillInfo> skills = new List<SkillInfo>();
    public List<Monster> monsters = new List<Monster>();

    //item
    string itemName;
    int itemValue;
    int itemPrice;
    string itemDesc;
    string itemPath;
    int itemStrength;
    int itemDefense;
    float itemCritical;
    int itemEquipType;
    int itemUpgrade;

    //monster
    string monName;
    int monLevel;
    float monHp;
    int monStr;
    int monDef;
    float monCri;
    float monAtDis;

    //skillDamage
    string skillName;
    float level1;
    float level2;
    float level3;
    float level4;
    float level5;
    float cooltime;
    string desc1;
    string desc2;
    string skillPath;

    void Awake()
    {

        TextAsset txtFile = Resources.Load("Files/Equipment") as TextAsset;
        string test = txtFile.text;
        string[] line2 = test.Split('\n');
        int i = 0;
        while (true)
        {
            if (i >= line2.Length-1)
                break;
            string line = line2[i+1];
            string[] info = line.Split(',');

            int itemType = int.Parse(info[0]);
            if(itemType == 1)
            {
                itemName = info[1];
                itemValue = int.Parse(info[2]);
                itemPrice = int.Parse(info[3]);
                itemDesc = info[4];
                itemPath = info[5];
                itemEquipType = int.Parse(info[6]);
                itemStrength = int.Parse(info[7]);
                itemDefense = int.Parse(info[8]);
                itemCritical = (float)int.Parse(info[9]);
                itemUpgrade = int.Parse(info[10]);
                AddItem(itemType, itemName, itemValue, itemPrice, itemDesc, itemPath, itemEquipType,itemStrength, itemDefense,itemCritical,itemUpgrade);
            }
            else if(itemType == 2)
            {
                itemName = info[1];
                itemValue = int.Parse(info[2]);
                itemPrice = int.Parse(info[3]);
                itemDesc = info[4];
                itemPath = info[5];
                AddItem(itemType, itemName, itemValue, itemPrice, itemDesc, itemPath);
            }
            ++i;
        }

        TextAsset txtFile2 = Resources.Load("Files/Monster") as TextAsset;
        string test2 = txtFile2.text;
        string[] line3 = test2.Split('\n');
        int j = 0;
        while (true)
        {
            if (j >= line3.Length - 1)
                break;
            string line = line3[j + 1];
            string[] info = line.Split(',');
            monName = info[0];
            monLevel = int.Parse(info[1]);
            monHp = float.Parse(info[2]);
            monStr = int.Parse(info[3]);
            monDef = int.Parse(info[4]);
            monCri = float.Parse(info[5]);
            monAtDis = float.Parse(info[6]);
            AddMonster(monName, monLevel,monHp, monStr, monDef, monCri, monAtDis);
            ++j;
        }
        TextAsset txtFile3 = Resources.Load("Files/WarriorSkillInfo") as TextAsset;
        string test3 = txtFile3.text;
        string[] line4 = test3.Split('\n');
        int k = 0;
        while (true)
        {
            if (k >= line4.Length - 1)
                break;
            string line = line4[k + 1];
            string[] info = line.Split(',');
            skillName = info[0];
            level1 = float.Parse(info[1]);
            level2 = float.Parse(info[2]);
            level3 = float.Parse(info[3]);
            level4 = float.Parse(info[4]);
            level5 = float.Parse(info[5]);
            cooltime = float.Parse(info[6]);
            desc1 = info[7];
            desc2 = info[8];
            skillPath = info[9];
            skills.Add(new SkillInfo(skillName,level1, level2, level3, level4, level5,cooltime,desc1,desc2,Resources.Load<Sprite>("SkillImg/"+ skillName)));
            ++k;
        }
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
    }

    void AddItem(int itemType, string itemName, int itemValue, int itemPrice, string itemDesc,  string itemPath)
    {
        items.Add(new Item(itemType, itemName, itemValue, itemPrice, itemDesc, Resources.Load<Sprite>("ItemImg/" + itemPath)));
    }
    void AddItem(int itemType, string itemName, int itemValue, int itemPrice, string itemDesc, string itemPath, int itemEquipType, int itemStrength,int itemDefense,float itemCritical,int upgrade)
    {
        items.Add(new Item(itemType, itemName, itemValue, itemPrice, itemDesc, Resources.Load<Sprite>("ItemImg/" + itemPath), itemEquipType,itemStrength, itemDefense,itemCritical,upgrade));
    }

    void AddMonster(string _name,int _level,float _hp , int _str , int _def , float _cri, float _atDis)
    {
        monsters.Add(new Monster(_name, _level, _hp, _str, _def, _cri, _atDis));
    }
}