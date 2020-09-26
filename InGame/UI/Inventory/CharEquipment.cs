using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CharEquipment : MonoBehaviour
{
    private List<CharEquipmentSlot> slots = new List<CharEquipmentSlot>();

    int m_equipmentType;
    string m_itemName;
    int m_upgrade;
    float m_str;
    float m_def;
    float m_cri;
    float m_price;


    // 장비 합산 능력치
    private int totalStr;
    private int totalDef;
    private float totalCri;
    // Start is called before the first frame update
    void Start()
    {
        CharEquipmentSlot[] slot = transform.GetComponentsInChildren<CharEquipmentSlot>();
        foreach (CharEquipmentSlot s in slot)
        {
            slots.Add(s);
            s.item = null;
        }
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharWearEquip.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        StreamReader sr = new StreamReader(fs);
        sr.ReadLine();
        while(true)
        { 
            string line = sr.ReadLine();
            if (line == null)
                break;
            string[] charEquip = line.Split(',');
            m_equipmentType = int.Parse(charEquip[0]);
            m_itemName = charEquip[1];
            m_upgrade = int.Parse(charEquip[2]);
            m_str = float.Parse(charEquip[3]);
            m_def = float.Parse(charEquip[4]);
            m_cri = float.Parse(charEquip[5]);
            m_price = float.Parse(charEquip[6]);
            if(m_itemName != "null")
            {
                Item item = Database.instance.items.Find(ex => ex.itemName == m_itemName);
                if(m_upgrade == 1)
                SearchSlot(m_equipmentType, item);
                else
                {
                    Item newItem = new Item(ChangeType(item.itemType), item.itemName, item.itemValue, (int)m_price, item.itemDesc, item.itemImage, ChangeEquipType(item.itemEquipType), (int)m_str, (int)m_def, m_cri,m_upgrade);
                    SearchSlot(m_equipmentType, newItem);
                }
                //Static.AddList(item);
            }
        }
        sr.Close();
    }
    private int ChangeType(ItemType item)
    {
        if (item == ItemType.Equipment)
            return 1;
        else if (item == ItemType.Consumption)
            return 2;
        else if (item == ItemType.Misc)
            return 3;

        return -1;
    }
    private int ChangeEquipType(EquipType item)
    {
        if (item == EquipType.Weapon)
            return 1;
        else if (item == EquipType.Armor)
            return 2;
        else if (item == EquipType.Helmet)
            return 3;
        else if (item == EquipType.Shoulder)
            return 4;
        else if (item == EquipType.Pants)
            return 5;
        else if (item == EquipType.Shoes)
            return 6;
        else
            return 0;
    }

    public int TotalStr()
    {
        totalStr = 0;
        foreach(Item slot in Static.s_wearItemList)
        {
            totalStr += slot.itemStrength;
        }
        return totalStr;
    }
    public int TotalDef()
    {
        totalDef = 0;
        foreach (Item slot in Static.s_wearItemList)
        {
            totalDef += slot.itemDefense;
        }
        return totalDef;
    }
    public float TotalCri()
    {
        totalCri = 0;
        foreach (Item slot in Static.s_wearItemList)
        {
            totalCri += slot.itemCritical;
        }
        return totalCri;
    }

    private void SearchSlot(int type,Item item)
    {
        switch (type)
        {
            case 1:
                slots.Find(ex => ex.name == "WeaponSlot").item = item;
                break;
            case 2:
                slots.Find(ex => ex.name == "ArmorSlot").item = item;
                break;
            case 3:
                slots.Find(ex => ex.name == "HelmetSlot").item = item;
                break;
            case 4:
                slots.Find(ex => ex.name == "ShoulderSlot").item = item;
                break;
            case 5:
                slots.Find(ex => ex.name == "PantsSlot").item = item;
                break;
            case 6:
                slots.Find(ex => ex.name == "ShoesSlot").item = item;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
