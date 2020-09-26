using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Static : MonoBehaviour
{
    public static List<Item> s_wearItemList = new List<Item>();
    public static List<Item> s_invenItemList = new List<Item>();
    public static List<int> s_state = new List<int>();
    public static float[] s_maxExp = new float[20];
    public static int s_silver;
    public static int s_gold;
    public static int s_diamond;

    public static void Money()
    {
        s_silver = 100000;
        s_gold = 1000;
        s_diamond = 1000;
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "Money.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine("Silver,Gold,Diamond");
        string line = s_silver.ToString() + "," + s_gold.ToString() + "," + s_diamond.ToString();
        sw.WriteLine(line);
        sw.Close();
        //StreamReader sr = new StreamReader(fs);
        //sr.ReadLine();
        //string line = sr.ReadLine();
        //string[] info = line.Split(',');
        //s_silver = int.Parse();
        //s_gold = int.Parse(info[1]);
        //s_diamond = int.Parse(info[2]);
        //sr.Close();
    }
    public static void SaveMoney()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "Money.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine("Silver,Gold,Diamond");
        string line = s_silver.ToString() + "," + s_gold.ToString() + "," + s_diamond.ToString();
        sw.WriteLine(line);
        sw.Close();
    }

    public static void SetExp()
    {
        float maxexp = 100.0f;
        for(int i = 0; i<s_maxExp.Length; i++)
        {
            s_maxExp[i] = maxexp;
            maxexp += 100.0f;
        }
    }

    public static void SetWearEquip()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharWearEquip.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        StreamReader sr = new StreamReader(fs);
        string temp = sr.ReadLine();
        while (true)
        {
            string line = sr.ReadLine();
            if (line == null)
                break;
            string[] charEquip = line.Split(',');
            int m_itemType = int.Parse(charEquip[0]);
            string m_itemName = charEquip[1];
            int m_upgrade = int.Parse(charEquip[2]);
            float m_str = float.Parse(charEquip[3]);
            float m_def = float.Parse(charEquip[4]);
            float m_cri = float.Parse(charEquip[5]);
            float m_price = float.Parse(charEquip[6]);
            if (m_itemName != "null")
            {
                Item item = Database.instance.items.Find(ex => ex.itemName == m_itemName);
                if(m_upgrade == 1)
                    AddList(item);
                else
                {
                    Item newItem = new Item(ChangeType(item.itemType), item.itemName, item.itemValue, (int)m_price, item.itemDesc, item.itemImage, ChangeEquipType(item.itemEquipType), (int)m_str, (int)m_def, m_cri,m_upgrade);
                    AddList(newItem);
                }
            }
        }
        sr.Close();
    }
    public static void InvenItemAddList(Item item)
    {
        foreach(Item i in s_invenItemList)
        {
            if(item.itemType == ItemType.Consumption && item.itemName == i.itemName)
            {
               
                i.itemValue += item.itemValue;
                return;
            }
        }
        s_invenItemList.Add(item);
    }
    public static void InvenItemRemoveList(Item item)
    {
        foreach(Item i in s_invenItemList)
        {
            if (i.itemName == item.itemName)
            {
                s_invenItemList.Remove(i);
                break;
            }
        }
    }
    public static void InvenItemChangeList(Item Oriitem,Item item)
    {
        int j = 0;
        foreach (Item item2 in Static.s_invenItemList)
        {
            if (item2.itemName == item.itemName)
            {
                Static.s_invenItemList[j] = Oriitem;
                break;
            }
            j++;
        }
        //s_invenItemList.Remove(item);
    }
    static int temp = 0;
    public static void InvenItemSaveList()
    {
        FileInfo fi = new FileInfo(Application.persistentDataPath + "/" + "CharItemList.txt");
        if (fi.Exists)
            fi.Delete();
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharItemList.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine("ItemType,ItemName,ItemValue,ItemEquipType,Upgrade " + temp);
        temp++;
        foreach(Item item in s_invenItemList)
        {
            if(item.itemType == ItemType.Equipment)
            {
                int itemtype = ChangeType(item.itemType);
                int itemEquiptype = ChangeEquipType(item.itemEquipType);
                int upgrade = item.upgrade;
                string line = itemtype +","+ item.itemName + "," + item.itemValue.ToString() + "," + itemEquiptype + "," + upgrade;
                sw.WriteLine(line);
            }
            else if(item.itemType == ItemType.Consumption)
            {
                int itemtype = ChangeType(item.itemType);
               
                string line = itemtype + "," + item.itemName + "," + item.itemValue.ToString();
                sw.WriteLine(line);
            }
        }
        sw.Close();
    }
    public static void AddList(Item item)
    {
        foreach(Item i in s_wearItemList)
        {
            if(item.itemEquipType == i.itemEquipType)
            {
                //InvenItemAddList(i);
                s_wearItemList.Remove(i);
                s_wearItemList.Add(item);
                return;
            }
        }
        s_wearItemList.Add(item);
    }
    public static void SaveList()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharWearEquip.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine("ItemEquipType,ItemName,Upgrade,ItemStrength,ItemDefense,ItemCritical,ItemPrice,7");
        foreach(Item item in s_wearItemList)
        {
            int type = ChangeEquipType(item.itemEquipType);
            int upgrade = item.upgrade;
            int str = item.itemStrength;
            int def = item.itemDefense;
            float cri = item.itemCritical;
            int price = item.itemPrice;
            string line = type.ToString() + "," + item.itemName + "," + upgrade.ToString() + "," + str.ToString() + "," + def.ToString() + "," + cri.ToString() + "," + price.ToString();
            sw.WriteLine(line);
        }
        sw.Close();
    }
    public static int ChangeEquipType(EquipType type)
    {
        switch (type)
        {
            case EquipType.Weapon:
                return 1;
            case EquipType.Armor:
                return 2;
            case EquipType.Helmet:
                return 3;
            case EquipType.Shoulder:
                return 4;
            case EquipType.Pants:
                return 5;
            case EquipType.Shoes:
                return 6;
        }
        return -1;
    }
    public static int ChangeType(ItemType type)
    {
        switch (type)
        {
            case ItemType.Equipment:
                return 1;
            case ItemType.Consumption:
                return 2;
            case ItemType.Misc:
                return 3;
        }
        return -1;
    }

}
