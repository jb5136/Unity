using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Buy : MonoBehaviour
{
    private Button m_weaponBtn;
    private Button m_armorBtn;
    private Button m_consumptionBtn;

    private ShopItemList m_list;
    private GameObject m_showItem;
    private Scrollbar m_scrollbar;
    void Start()
    {
        //Touch touch =  Input.GetTouch(0);

        m_list = transform.Find("ScrollView/Viewport/ShopItemList").GetComponent<ShopItemList>();

        m_weaponBtn = transform.Find("BuyList/Weapon").GetComponent<Button>();
        m_armorBtn = transform.Find("BuyList/Armor").GetComponent<Button>();
        m_consumptionBtn = transform.Find("BuyList/Consumption").GetComponent<Button>();
        m_showItem = transform.Find("ShowItem").gameObject;

        m_weaponBtn.onClick.AddListener(Weapon);
        m_armorBtn.onClick.AddListener(Armor);
        m_consumptionBtn.onClick.AddListener(Consumption);

        m_scrollbar = transform.Find("ScrollView/Scrollbar Vertical").GetComponent<Scrollbar>();
    }
    private void Weapon()
    {
        m_scrollbar.value = 1.0f;
        m_showItem.SetActive(false);
        m_list.ShowItemList(ItemType.Equipment, EquipType.Weapon);
    }
    private void Armor()
    {
        m_scrollbar.value = 1.0f;
        m_showItem.SetActive(false);
        m_list.ShowItemList(ItemType.Equipment);
    }
    private void Consumption()
    {
        m_scrollbar.value = 1.0f;
        m_showItem.SetActive(false);
        m_list.ShowItemList(ItemType.Consumption);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
