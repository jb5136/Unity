using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharEquipmentSlot : MonoBehaviour
{
    public int number;
    public Item item = null;

    private Button button;

    private Image backgroundimage;
    private Image wearItemImge;
    private Text m_level;

    //show info
    private GameObject m_ShowInfoObj;
    private GameObject m_Showinfoitem;
    private Image m_itemImg;
    private Text m_itemName;
    private Text m_strength;
    private Text m_defense;
    private Text m_desc;
    private Button m_useitem;
    private Button m_dump;

    // Start is called before the first frame update
    void Start()
    {
        backgroundimage = transform.Find("ExImg").GetComponent<Image>();
        wearItemImge = transform.Find("Image").GetComponent<Image>();
        button = transform.GetComponent<Button>();
        m_ShowInfoObj =  GameObject.Find("InventoryCanvas/ShowItem");
        m_Showinfoitem = m_ShowInfoObj.transform.Find("ShowItemInfoBg").gameObject;
        m_level = transform.Find("Text").GetComponent<Text>();
        if (button != null)
        {
            button.onClick.AddListener(ShowItemInfo);
        }
    }

    private void ShowItemInfo()
    {
        if (m_Showinfoitem.activeSelf)
        {
            m_Showinfoitem.SetActive(false);
            return;
        }

        if (item != null)
        {
            m_ShowInfoObj.GetComponent<ShowItemInfo>().EquipSlotOfSetItem(item, gameObject);

        }
    }

    // Update is called once per frame
    void Update()
    {

        //foreach(Item item2 in Static.s_wearItemList)
        //{
        //    if(item2.itemEquipType == item.itemEquipType)
        //    {
        //        if(item2.upgrade != item.upgrade)
        //        {
        //            item = item2;
        //        }
        //    }
        //}
        
        if (item == null)
        {
            backgroundimage.gameObject.SetActive(true);
            wearItemImge.gameObject.SetActive(false);
            button.enabled = false;
            m_level.text = string.Empty;
        }
        else if(item != null)
        {
            button.enabled = true;
            backgroundimage.gameObject.SetActive(false);
            wearItemImge.sprite = item.itemImage;
            wearItemImge.gameObject.SetActive(true);
            if (item.upgrade > 1)
                m_level.text = "+" + item.upgrade.ToString();
            else
                m_level.text = string.Empty;
        }
    }
}
