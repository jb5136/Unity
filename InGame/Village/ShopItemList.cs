using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemList : MonoBehaviour
{
    public ItemType m_itemType;
    public EquipType m_equipType;

    public GameObject itemListBtn;
    private List<Item> items = new List<Item>();
    private List<ShopItemSlot> slots = new List<ShopItemSlot>();
    void Start()
    {
        ShopItemSlot[] slot = transform.GetComponentsInChildren<ShopItemSlot>();
        int i = 1;
        foreach (ShopItemSlot s in slot)
        {
            slots.Add(s);
            s.gameObject.name = "Slot" + i;
            i++;
            s.item = null;
        }
        m_itemType = ItemType.Equipment;
        m_equipType = EquipType.Weapon;

        ShowItemList(m_itemType, m_equipType);
    }

    public void ShowItemList(ItemType itemType , EquipType equipType = EquipType.Armor)
    {
        items.Clear();
        if (itemType == ItemType.Equipment)
        {
            if(equipType == EquipType.Weapon)
            {
                foreach (Item item in Database.instance.items)
                {
                    if(item.itemType == ItemType.Equipment && item.itemEquipType == EquipType.Weapon)
                    {
                        items.Add(item);
                    }
                }
            }
            else
            {
                foreach (Item item in Database.instance.items)
                {
                    if (item.itemType == ItemType.Equipment && item.itemEquipType != EquipType.Weapon)
                    {
                        items.Add(item);
                    }
                }
            }
        }
        else if(itemType == ItemType.Consumption)
        {
            foreach (Item item in Database.instance.items)
            {
                if (item.itemType == ItemType.Consumption)
                {
                    items.Add(item);
                }
            }
        }

        if(items.Count != 0)
        {
            int j = 0;
            foreach(ShopItemSlot slot in slots)
            {
                slot.item = null;
            }
            foreach (Item item in items)
            {
                slots[j].GetComponent<ShopItemSlot>().item = item;
                j++;
            }
        }

    }

    void Update()
    {
        
    }
}
