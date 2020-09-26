using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    public Item item;
    private GameObject m_showItem;
    //UI
    private Button m_btn;
    private Image m_itemImg;
    private Image m_bg;
    private Image m_coin;
    private Image m_line;
    private Text m_name;
    private Text m_price;
    // Start is called before the first frame update
    void Start()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(Btn);
        m_itemImg = transform.Find("Image").GetComponent<Image>();
        m_bg = transform.Find("Bg").GetComponent<Image>();
        m_coin = transform.Find("SilverCoin").GetComponent<Image>();
        m_line = transform.Find("Line").GetComponent<Image>();
        m_name = transform.Find("Name").GetComponent<Text>();
        m_price = transform.Find("Price").GetComponent<Text>();

        m_showItem = GameObject.Find("NCanvas/Consumption/Buy").transform.Find("ShowItem").gameObject;
    }
    private void Btn()
    {
        m_showItem.SetActive(true);
        m_showItem.GetComponent<ShopShowItemBuy>().item = item;
    }
    private void SetUI(bool check)
    {
        m_btn.enabled = check;
        m_itemImg.gameObject.SetActive(check);
        m_bg.gameObject.SetActive(check);
        m_coin.gameObject.SetActive(check);
        m_name.gameObject.SetActive(check);
        m_price.gameObject.SetActive(check);
        m_line.gameObject.SetActive(check);
    }
    // Update is called once per frame
    void Update()
    {
        if(item == null)
        {
            SetUI(false);
        }
        else
        {
            SetUI(true);
            m_itemImg.sprite = item.itemImage;
            m_name.text = item.itemName;
            m_price.text = item.itemPrice.ToString();
        }
    }
}
