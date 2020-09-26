using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopShowItem : MonoBehaviour
{
    public Item item;

    private Image m_itemImg;
    private Text m_itemName;
    private Text m_itemDesc;
    private Text m_itemValue;
    private Button m_minus;
    private Button m_plus;
    private Text m_itemPrice;
    private Text m_Money;
    private Button m_sellBtn;

    private Transform m_whetherPivot;
    private int m_value = 1;
    void Start()
    {
        m_itemImg = transform.Find("ItemImg").GetComponent<Image>();
        m_itemName = transform.Find("Name").GetComponent<Text>();
        m_itemDesc = transform.Find("Desc").GetComponent<Text>();
        m_itemValue = transform.Find("SalesQuantity/Text").GetComponent<Text>();
        m_minus = transform.Find("SalesQuantity/Minus").GetComponent<Button>();
        m_plus = transform.Find("SalesQuantity/Plus").GetComponent<Button>();
        m_minus.onClick.AddListener(Minus);
        m_plus.onClick.AddListener(Plus);
        m_itemPrice = transform.Find("SalePrice/Text").GetComponent<Text>();
        m_Money = transform.Find("HoldingAmount/Text").GetComponent<Text>();
        m_sellBtn = transform.Find("Sell").GetComponent<Button>();
        if (m_sellBtn != null)
            m_sellBtn.onClick.AddListener(Sell);
        m_whetherPivot = GameObject.Find("NCanvas/Consumption/TextPivot").transform;
    }
    private void Sell()
    {
        Static.s_invenItemList.Find(ex => ex.itemName == item.itemName).itemValue = Static.s_invenItemList.Find(ex => ex.itemName == item.itemName).itemValue - m_value;
        if(Static.s_invenItemList.Find(ex => ex.itemName == item.itemName).itemValue <= 0)
        {
            Static.s_invenItemList.Remove(Static.s_invenItemList.Find(ex => ex.itemName == item.itemName));
        }
        GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
        var temp = Instantiate(whether, m_whetherPivot);
        temp.transform.Find("Text").GetComponent<Text>().text = "판매하였습니다";
        temp.transform.position = m_whetherPivot.transform.position;
        Static.InvenItemSaveList();
        Static.s_silver += item.itemPrice * m_value;
        Static.SaveMoney();
        m_value = 1;
        gameObject.SetActive(false);
    }
    private void Minus()
    {
        if (m_value > 1)
            --m_value;
    }
    private void Plus()
    {
        if (m_value < item.itemValue)
            m_value++;
    }
    // Update is called once per frame
    void Update()
    {
        if(item != null)
        {
            m_itemImg.sprite = item.itemImage;
            m_itemName.text = item.itemName;
            m_itemDesc.text = item.itemDesc;
            if (m_value > item.itemValue)
                m_value = 1;
            m_itemValue.text = m_value.ToString();
            m_itemPrice.text = (item.itemPrice * m_value).ToString();
            m_Money.text = Static.s_silver.ToString();
        }
    }
}
