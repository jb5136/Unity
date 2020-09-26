using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Inventory : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    private List<Slot> slots = new List<Slot>();
    // Start is called before the first frame update

    Slot[] slot;

    // 파일을 읽어와서 가지고 있는 아이탬들을 슬롯에 세팅해준다 
    string _itemName;
    int _itemValue ;
    int _itemEquipType ;
    int m_itemCount;
    int m_upgrade;
    void Start()
    {
        slot = transform.GetComponentsInChildren<Slot>();
        int i = 1;
        foreach (Slot s in slot)
        {
            slots.Add(s);
            s.gameObject.name = "Slot" + i;
            i++;
            s.item = null;
        }
        int j = 0;
        foreach(Item item in Static.s_invenItemList)
        {
            slots[j].item = item;
            j++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        int j = 0;
        //m_itemCount = 0;
        //foreach (Slot s in slot)
        //{
        //    if (s.item != null)
        //        m_itemCount++;
        //}
        //Debug.Log(m_itemCount);
        foreach (Item i in Static.s_invenItemList)
        {
            slot[j].item = i;
            slot[j + 1].item = null;
            j++;
        }
        for(int i = Static.s_invenItemList.Count; i<slot.Length; i++)
        {
            slot[i].item = null;
        }
        //for (int i = 0; i < m_itemCount; i++)
        //{
        //    if (slot[i].item == null)
        //    {
        //        slot[i].item = slot[i + 1].item;
        //        slot[i + 1].item = null;
        //    }
        //}
        //for(int k =0; k<slot.Length; k++)
        //{
        //    if(slot[k].item == null)
        //    {
        //        slot[k].image.enabled = false;
        //        slot[k].button.enabled = false;
        //    }
        //}

        //int i = 0;
        //foreach (Item item in Static.s_invenItemList)
        //{
        //    if(slot[i].item == null)
        //    {
        //        slot[i].item = item;

        //    }
        //    i++;
        //}
    }
}



//string path = Application.dataPath + "/Resources/Files/CharItemList.txt";
//StreamReader sr = new StreamReader(path);
//sr.ReadLine();
//int j = 0;
//Item item;
//while (true)
//{
//    string itemList = sr.ReadLine();
//    if (itemList == null)
//        break;
//    string[] iteminfo = itemList.Split(',');
//    int _itemType = int.Parse(iteminfo[0]);
//    if(_itemType == 1)
//    {
//        _itemName = iteminfo[1];
//        _itemValue = int.Parse(iteminfo[2]);
//        _itemEquipType = int.Parse(iteminfo[3]);
//        m_upgrade = int.Parse(iteminfo[4]);
//        item = Database.instance.items.Find(ex => ex.itemName == _itemName);
//        slots[j].item = new Item(_itemType, item.itemName, _itemValue, item.itemPrice, item.itemDesc, item.itemImage, _itemEquipType, item.itemStrength,item.itemDefense,item.itemCritical,m_upgrade);
//        Static.InvenItemAddList(slots[j].item);
//        j++;
//    }
//    else if( _itemType == 2)
//    {
//        _itemName = iteminfo[1];
//        _itemValue = int.Parse(iteminfo[2]);
//        item = Database.instance.items.Find(ex => ex.itemName == _itemName);
//        slots[j].item = new Item(_itemType, item.itemName, _itemValue, item.itemPrice, item.itemDesc, item.itemImage);
//        Static.InvenItemAddList(slots[j].item);
//        j++;
//    }
//}
//sr.Close();
