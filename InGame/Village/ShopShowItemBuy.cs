using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopShowItemBuy : MonoBehaviour
{
    public Item item;
    private int m_value = 1;
    //UI
    private Image m_itemImg;
    private Text m_itemName;
    private Text m_str;
    private Text m_def;
    private Text m_cri;
    private Button m_minus;
    private Button m_plus;
    private Text m_itemValue;
    private Text m_price;
    private Text m_money;
    private Button m_buy;
    private Text m_desc;

    private Transform m_whetherPivot;
    void Start()
    {

        m_itemImg = transform.Find("ItemImg").GetComponent<Image>();
        m_itemName = transform.Find("ItemName").GetComponent<Text>();
        m_desc = transform.Find("Desc").GetComponent<Text>();
        m_str = transform.Find("Str/Text").GetComponent<Text>();
        m_def = transform.Find("Def/Text").GetComponent<Text>();
        m_cri = transform.Find("Cri/Text").GetComponent<Text>();
        m_minus = transform.Find("SalesQuantity/Minus").GetComponent<Button>();
        m_plus = transform.Find("SalesQuantity/Plus").GetComponent<Button>();
        m_minus.onClick.AddListener(Minus);
        m_plus.onClick.AddListener(Plus);
        m_itemValue = transform.Find("SalesQuantity/Text").GetComponent<Text>();
        m_price = transform.Find("SalePrice/Text").GetComponent<Text>();
        m_money = transform.Find("HoldingAmount/Text").GetComponent<Text>();
        m_buy = transform.Find("Buy").GetComponent<Button>();
        m_buy.onClick.AddListener(BuyItem);
        m_whetherPivot = GameObject.Find("NCanvas/Consumption/TextPivot").transform;
    }
    private void BuyItem()
    {
        if (Static.s_silver >= item.itemPrice)
        {
            if (item.itemType == ItemType.Consumption)
            {
                if (Static.s_invenItemList.Exists(ex => ex.itemName == item.itemName))
                    Static.s_invenItemList.Find(ex => ex.itemName == item.itemName).itemValue = Static.s_invenItemList.Find(ex => ex.itemName == item.itemName).itemValue + m_value;
                else
                {
                    item.itemValue = m_value;
                    Static.s_invenItemList.Add(item);
                }
                Static.s_silver -= item.itemPrice * m_value;
                GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
                var temp = Instantiate(whether, m_whetherPivot);
                temp.transform.Find("Text").GetComponent<Text>().text = "구매하였습니다";
                temp.transform.position = m_whetherPivot.transform.position;
            }
            else if (item.itemType == ItemType.Equipment)
            {
                Static.s_invenItemList.Add(item);
                Static.s_silver -= item.itemPrice;
                GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
                var temp = Instantiate(whether, m_whetherPivot);
                temp.transform.Find("Text").GetComponent<Text>().text = "구매하였습니다";
                temp.transform.position = m_whetherPivot.transform.position;
            }
            Static.InvenItemSaveList();
            Static.SaveMoney();
            m_value = 1;
        }
    }
    private void Minus()
    {
        if (m_value > 1)
            --m_value;
    }
    private void Plus()
    {
        if(item.itemType == ItemType.Consumption)
            m_value++;
    }
    void Update()
    {
        
        if(item != null)
        {
            m_itemImg.sprite = item.itemImage;
            m_itemName.text = item.itemName;
            if(item.itemType == ItemType.Equipment)
            {
                m_str.transform.parent.gameObject.SetActive(true);
                m_def.transform.parent.gameObject.SetActive(true);
                m_cri.transform.parent.gameObject.SetActive(true);
                m_desc.gameObject.SetActive(false);
                m_str.text = item.itemStrength.ToString();
                m_def.text = item.itemDefense.ToString();
                m_cri.text = item.itemCritical.ToString();
            }
            else if(item.itemType == ItemType.Consumption)
            {
                m_str.transform.parent.gameObject.SetActive(false);
                m_def.transform.parent.gameObject.SetActive(false);
                m_cri.transform.parent.gameObject.SetActive(false);
                m_desc.gameObject.SetActive(true);
                m_desc.text = item.itemDesc;
            }
            
            m_itemValue.text = m_value.ToString();
            m_price.text = (item.itemPrice * m_value).ToString();
            m_money.text = Static.s_silver.ToString();
        }
    }
}
