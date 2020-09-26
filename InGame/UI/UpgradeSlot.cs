using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public Item item;

    private Image m_EquipImg;
    private Button m_Btn;
    private UpgradeItem m_upgradeItem;
    private Text m_LevelText;
    // Start is called before the first frame update
    void Start()
    {
        m_EquipImg = transform.Find("Image").GetComponent<Image>();
        m_Btn = GetComponent<Button>();
        if (m_Btn != null)
            m_Btn.onClick.AddListener(Btn);
        m_upgradeItem = GameObject.Find("Canvas/UpgradeCanvas/UpgradeItemSlot").GetComponent<UpgradeItem>();
        m_LevelText = transform.Find("Level").GetComponent<Text>();

    }
    private void Btn()
    {
        if(item != null)
        {
            GameObject.FindObjectOfType<UpgradeCanvas>().transform.Find("UpgradeItemSlot").gameObject.SetActive(true);
            m_upgradeItem.m_item = item;
        }
    }
    void Update()
    {
        if (item != null)
        {
            m_EquipImg.gameObject.SetActive(true);
            m_LevelText.gameObject.SetActive(true);
            m_EquipImg.sprite = item.itemImage;
            if (item.upgrade > 1)
                m_LevelText.text = "+" + item.upgrade.ToString();
            else
                m_LevelText.text = string.Empty;
        }
        else
        {
            m_EquipImg.gameObject.SetActive(false);
            m_LevelText.gameObject.SetActive(false);

        }
    }
}
