using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemInfo : MonoBehaviour
{
    private Item m_item;

    private Slot m_slot;
    private CharEquipmentSlot m_equipSlot;

    private GameObject g;
    private Image m_itemImg;
    private Text m_itemName;
    private Text m_strength;
    private Text m_defense;
    private Text m_desc;
    private Button m_useitem;
    private Button m_putOnitem;
    private Button m_takeOffitem;
    private Button m_dump;

    //wear equipment
    private CharEquipmentSlot m_weapon;
    private CharEquipmentSlot m_helmet;
    private CharEquipmentSlot m_armor;
    private CharEquipmentSlot m_shoulder;
    private CharEquipmentSlot m_pants;
    private CharEquipmentSlot m_shoes;
    // Start is called before the first frame update
    void Start()
    {
        m_item = null;
        m_slot = null;

        g = transform.Find("ShowItemInfoBg").gameObject;
        m_itemImg = transform.Find("ShowItemInfoBg/ItemImg").GetComponent<Image>();
        m_itemName = transform.Find("ShowItemInfoBg/ItemImg/ItemName").GetComponent<Text>();
        m_strength = transform.Find("ShowItemInfoBg/Strength").GetComponent<Text>();
        m_defense = transform.Find("ShowItemInfoBg/Defense").GetComponent<Text>();
        m_desc = transform.Find("ShowItemInfoBg/Desc").GetComponent<Text>();
        m_useitem = transform.Find("ShowItemInfoBg/Use").GetComponent<Button>();
        m_putOnitem = transform.Find("ShowItemInfoBg/Puton").GetComponent<Button>();
        m_takeOffitem = transform.Find("ShowItemInfoBg/TakeOff").GetComponent<Button>();
        m_dump = transform.Find("ShowItemInfoBg/Dump").GetComponent<Button>();

        m_dump.onClick.AddListener(Dump);
        m_putOnitem.onClick.AddListener(PutOnItem);
        m_takeOffitem.onClick.AddListener(TakeOffItem);

        //wear equipment
        GameObject w = GameObject.Find("InventoryCanvas").transform.Find("CharEquipment").gameObject;
        if (w != null)
        {
            m_weapon = w.transform.Find("WeaponSlot").GetComponent<CharEquipmentSlot>();
            m_helmet = w.transform.Find("HelmetSlot").GetComponent<CharEquipmentSlot>();
            m_armor = w.transform.Find("ArmorSlot").GetComponent<CharEquipmentSlot>();
            m_shoulder = w.transform.Find("ShoulderSlot").GetComponent<CharEquipmentSlot>();
            m_pants = w.transform.Find("PantsSlot").GetComponent<CharEquipmentSlot>();
            m_shoes = w.transform.Find("ShoesSlot").GetComponent<CharEquipmentSlot>();
        }

    }
    public void SlotOfSetItem(Item item, GameObject slot)
    {
        g.SetActive(true);
        m_itemImg.sprite = item.itemImage;
        m_itemName.text = item.itemName;
        m_strength.text = item.itemStrength.ToString();
        m_defense.text = item.itemDefense.ToString();
        m_desc.text = item.itemDesc;
        m_item = item;
        m_slot = slot.GetComponent<Slot>();

        if (item.itemType == ItemType.Equipment)
        {
            m_useitem.gameObject.SetActive(false);
            m_takeOffitem.gameObject.SetActive(false);
            m_putOnitem.gameObject.SetActive(true);
        }
        else if (item.itemType == ItemType.Consumption)
        {
            m_useitem.gameObject.SetActive(true);
            m_takeOffitem.gameObject.SetActive(false);
            m_putOnitem.gameObject.SetActive(false);
        }
    }
    public void EquipSlotOfSetItem(Item item, GameObject slot)
    {
        g.SetActive(true);
        m_itemImg.sprite = item.itemImage;
        m_itemName.text = item.itemName;
        m_strength.text = item.itemStrength.ToString();
        m_defense.text = item.itemDefense.ToString();
        m_desc.text = item.itemDesc;
        m_item = item;
        m_equipSlot = slot.GetComponent<CharEquipmentSlot>();

        m_useitem.gameObject.SetActive(false);
        m_takeOffitem.gameObject.SetActive(true);
        m_putOnitem.gameObject.SetActive(false);
    }
    private void Dump()
    {
        Static.InvenItemRemoveList(m_item);
        m_slot.item = null;
        g.SetActive(false);
    }
    
    private void SetItem(CharEquipmentSlot slot)
    {
        slot.item = m_item;
        Static.InvenItemRemoveList(m_item);
        m_slot.item = null;
        Static.AddList(slot.item);
        g.SetActive(false);
    }
    private void ChangeItem(CharEquipmentSlot slot)
    {
        Item temp = slot.item;
        slot.item = m_item;
        Static.InvenItemChangeList(temp,m_item);
        m_slot.item = temp;
        Static.AddList(slot.item);
        g.SetActive(false);
        
    }
    private void PutOnItem()
    {
        if (m_item.itemType == ItemType.Equipment)
        {
            switch (m_item.itemEquipType)
            {
                case EquipType.Weapon:
                    if (m_weapon.item == null)
                        SetItem(m_weapon);
                    else
                        ChangeItem(m_weapon);
                    break;
                case EquipType.Armor:
                    if (m_armor.item == null)
                        SetItem(m_armor);
                    else                        
                        ChangeItem(m_armor);
                    break;
                case EquipType.Helmet:
                    if (m_helmet.item == null)
                        SetItem(m_helmet);
                    else
                        ChangeItem(m_helmet);
                    break;
                case EquipType.Shoulder:
                    if (m_shoulder.item == null)
                        SetItem(m_shoulder);
                    else
                        ChangeItem(m_shoulder);
                    break;
                case EquipType.Pants:
                    if (m_pants.item == null)
                        SetItem(m_pants);
                    else
                        ChangeItem(m_pants);
                    break;
                case EquipType.Shoes:
                    if (m_shoes.item == null)
                        SetItem(m_shoes);
                    else
                        ChangeItem(m_shoes);
                    break;
            }
        }
        else if (m_item.itemType == ItemType.Consumption)
        {

        }
    }
    // Update is called once per frame
    private void TakeOffItem()
    {
        Transform t = GameObject.Find("InventoryCanvas/ScrollView/Viewport/Inventory").transform;
        Slot[] slots = t.transform.GetComponentsInChildren<Slot>();
        foreach (Slot s in slots)
        {
            if(s.item == null)
            {
                s.item = m_item;
                for(int i=0; i< Static.s_wearItemList.Count; i++)
                {
                    if(Static.s_wearItemList[i].itemEquipType == m_item.itemEquipType)
                    {
                        Static.s_wearItemList.Remove(Static.s_wearItemList[i]);
                    }
                }
                Static.InvenItemAddList(s.item);
                m_equipSlot.item = null;
                g.SetActive(false);
                return;
            }
        }
    }
    void Update()
    {
        
    }
}
