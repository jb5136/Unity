using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public int number;
    public Item item ;
    //슬롯 버튼
    public Button button;
    //아이템 이미지
    public Image image;
    private Text text;

    //ShowInfo
    private GameObject m_Showinfoitem;
    private GameObject m_ShowInfoObj;
    // Start is called before the first frame update
    void Start()
    {
        if(item != null)
            Debug.Log(item.itemName);
        image = transform.Find("Image").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
        button = GetComponent<Button>();
        if(button != null)
            button.onClick.AddListener(ShowItemInfo);

        //Showinfo
        GameObject g = GameObject.Find("InventoryCanvas").transform.Find("ShowItem/ShowItemInfoBg").gameObject;
        m_ShowInfoObj = GameObject.Find("InventoryCanvas/ShowItem");
        m_Showinfoitem = g.gameObject;
    }
    private void ShowItemInfo()
    {
        if (m_Showinfoitem.activeSelf)
        {
            m_Showinfoitem.SetActive(false);
            return;
        }

        if(item != null )
        {
            m_ShowInfoObj.GetComponent<ShowItemInfo>().SlotOfSetItem(item,gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(item == null)
        {
            image.gameObject.SetActive(false);
            text.text = "";
            button.enabled = false;
            return;

        }
        else if(item != null)
        {
            button.enabled = true;
            image.sprite = item.itemImage;
            image.gameObject.SetActive(true);
            if(item.itemValue > 1)
            {
                text.text = item.itemValue.ToString();
            }
            if(item.itemType == ItemType.Equipment)
            {
                if (item.upgrade > 1)
                    text.text = "+" + item.upgrade.ToString();
                else
                    text.text = string.Empty;
            }
        }
    }
}
