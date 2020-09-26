using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public Item item;

    //슬롯 버튼
    private Button button;
    //아이템 이미지
    private Image image;
    private Text text;
    private GameObject m_ShowItem;
    void Start()
    {
        image = transform.Find("Image").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
        button = GetComponent<Button>();
        m_ShowItem = GameObject.Find("NCanvas/Consumption/Sell").transform.Find("ShowItem").gameObject;
        if (button != null)
            button.onClick.AddListener(ShowItem);
    }
    private void ShowItem()
    {
        if(item != null)
        {
            m_ShowItem.SetActive(true);
            m_ShowItem.GetComponent<ShopShowItem>().item = item;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (item == null)
        {
            image.gameObject.SetActive(false);
            text.text = "";
            button.enabled = false;
            return;

        }
        else if (item != null)
        {
            button.enabled = true;
            image.sprite = item.itemImage;
            image.gameObject.SetActive(true);
            if (item.itemValue > 1)
            {
                text.text = item.itemValue.ToString();
            }
            if (item.itemType == ItemType.Equipment)
            {
                if (item.upgrade > 1)
                    text.text = "+" + item.upgrade.ToString();
                else
                    text.text = string.Empty;
            }
        }
    }
}
