using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCanvas : MonoBehaviour
{
    private bool OnCheck = false;
    private List<UpgradeSlot> slots = new List<UpgradeSlot>();

    public bool GetOnCheck
    {
        set { OnCheck = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpgradeSlot[] slot = transform.Find("CharItem").GetComponentsInChildren<UpgradeSlot>();
        foreach(UpgradeSlot u in slot)
        {
            slots.Add(u);
            u.item = null;
        }
        //transform.Find("UpgradeItemSlot").GetComponent<UpgradeItem>().m_item = null;

    }
    private void SearchSlot(EquipType type, Item item)
    {
        switch (type)
        {
            case EquipType.Weapon:
                slots.Find(ex => ex.name == "WeaponSlot").item = item;
                break;
            case EquipType.Armor:
                slots.Find(ex => ex.name == "ArmorSlot").item = item;
                break;
            case EquipType.Helmet:
                slots.Find(ex => ex.name == "HelmetSlot").item = item;
                break;
            case EquipType.Shoulder:
                slots.Find(ex => ex.name == "ShoulderSlot").item = item;
                break;
            case EquipType.Pants:
                slots.Find(ex => ex.name == "PantsSlot").item = item;
                break;
            case EquipType.Shoes:
                slots.Find(ex => ex.name == "ShoesSlot").item = item;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (OnCheck)
        {
            foreach (Item item in Static.s_wearItemList)
            {
                SearchSlot(item.itemEquipType, item);
            }

            OnCheck = false;
        }
    }
}
