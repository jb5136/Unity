using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInven : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    private List<ShopSlot> slots = new List<ShopSlot>();
    ShopSlot[] slot;

    // 파일을 읽어와서 가지고 있는 아이탬들을 슬롯에 세팅해준다 
    string _itemName;
    int _itemValue;
    int _itemEquipType;
    int m_itemCount;
    int m_upgrade;
    void Start()
    {
        slot = transform.GetComponentsInChildren<ShopSlot>();
        int i = 1;
        foreach (ShopSlot s in slot)
        {
            slots.Add(s);
            s.gameObject.name = "Slot" + i;
            i++;
            s.item = null;
        }
        int j = 0;
        foreach (Item item in Static.s_invenItemList)
        {
            slots[j].item = item;
            j++;
        }
        foreach (ShopSlot s in slot)
        {
            if (s.item != null)
                m_itemCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int j = 0;
        foreach (Item i in Static.s_invenItemList)
        {
            slot[j].item = i;
            slot[j + 1].item = null;
            j++;
        }
        for (int i = 0; i < m_itemCount; i++)
        {
            if (slot[i].item == null)
            {
                slot[i].item = slot[i + 1].item;
                slot[i + 1].item = null;
            }
        }
    }
}
