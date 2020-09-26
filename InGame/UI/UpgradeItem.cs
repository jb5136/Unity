using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public Item m_item;
    private Image m_image;

    private Text m_levelText;
    private Text m_nextLevelText;
    private Text m_str;
    private Text m_plusStr;
    private Text m_def;
    private Text m_plusDef;

    private Text m_needMoney;
    private Text m_haveMoney;
    private Button m_upgradeBtn;

    private Transform m_whetherPivot;
    private GameObject m_whetherPrifab;

    Transform m_equipmentInfo;

    //upgrade UI
    private GameObject m_upgradeUI;
    private Image m_upgradeItemImg;
    private GameObject m_upgrading;
    private Slider m_upgradeSlider;
    private GameObject m_result;
    private GameObject m_successUpgrade;
    private GameObject m_failUpgrade;
    private Button m_moreUpgradeBtn;
    private Button m_upgradeCloseBtn;
    private bool m_upgradeCheck = false;
    private float m_upgradeTime = 0.0f;
    private Text m_upgradeValue;
    private Text m_upgrageNext;
    private Text m_resultUpgradeText;

    //particle
    private GameObject m_upgradingEf;
    private GameObject m_successEf;
    private GameObject m_failEf;
    // Start is called before the first frame update
    void Start()
    {
        m_image =transform.Find("UpgradeItemImg").GetComponent<Image>();
        m_item = null;
        m_equipmentInfo = transform.Find("EquipmentInfo");
        if(m_equipmentInfo != null)
        {
            m_levelText = m_equipmentInfo.transform.Find("Level/Level").GetComponent<Text>();
            m_nextLevelText = m_equipmentInfo.transform.Find("Level/NextLevel").GetComponent<Text>();

            m_str = m_equipmentInfo.transform.Find("Attribute/Strength/Str").GetComponent<Text>();
            m_plusStr = m_equipmentInfo.transform.Find("Attribute/Strength/NextLevelStr").GetComponent<Text>();
            m_def = m_equipmentInfo.transform.Find("Attribute/Defense/Def").GetComponent<Text>();
            m_plusDef = m_equipmentInfo.transform.Find("Attribute/Defense/NextLevelDef").GetComponent<Text>();

            m_needMoney = m_equipmentInfo.transform.Find("NeedMoney/Money").GetComponent<Text>();
            m_haveMoney = m_equipmentInfo.transform.Find("HaveMoney/Money").GetComponent<Text>();
            m_upgradeBtn = m_equipmentInfo.transform.Find("UpgradeBtn").GetComponent<Button>();
            m_whetherPivot = transform.Find("EquipmentInfo/UpgradePivot");

            m_whetherPrifab = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");

            m_upgradeBtn.onClick.AddListener(Btn);
        }
        m_upgradeUI = transform.Find("Upgrade").gameObject;
        if(m_upgradeUI != null)
        {
            m_upgradeItemImg = m_upgradeUI.transform.Find("ItemImg").GetComponent<Image>();
            m_upgrading = m_upgradeUI.transform.Find("Upgrading").gameObject;
            m_upgradeSlider = m_upgrading.transform.Find("Slider").GetComponent<Slider>();
            m_result = m_upgradeUI.transform.Find("Result").gameObject;
            m_successUpgrade = m_upgradeUI.transform.Find("Result/Success").gameObject;
            m_failUpgrade = m_upgradeUI.transform.Find("Result/Fail").gameObject;
            m_moreUpgradeBtn = m_upgradeUI.transform.Find("Result/UpgradeBtn").GetComponent<Button>();
            m_upgradeCloseBtn = m_upgradeUI.transform.Find("Result/Close").GetComponent<Button>();
            m_upgradeValue = m_upgrading.transform.Find("UpgradeValue").GetComponent<Text>();
            m_upgrageNext = m_upgrading.transform.Find("NextLevel").GetComponent<Text>();
            m_resultUpgradeText = m_result.transform.Find("UpgradeValue").GetComponent<Text>();
            //m_upgradeUI.SetActive(false);

            m_moreUpgradeBtn.onClick.AddListener(Btn);
            m_upgradeCloseBtn.onClick.AddListener(UpgradeClose);
        }

        GameObject paticle = GameObject.Find("Canvas").transform.Find("UpgradeEffect").gameObject;
        if (paticle != null)
        {
            m_upgradingEf = paticle.transform.Find("Upgrading").gameObject;
            m_successEf = paticle.transform.Find("Success").gameObject;
            m_failEf = paticle.transform.Find("Fail").gameObject;
        }
    }
    private void UpgradeClose()
    {
        m_upgradeUI.SetActive(false);
        m_upgradeSlider.value = 0.0f;
    }
    private void Btn()
    {
        if(Static.s_silver>= NeedMoney(m_item.upgrade))
        {
            Static.s_silver -= NeedMoney(m_item.upgrade);
            
            m_upgradeUI.SetActive(true);
            m_upgradeItemImg.sprite = m_item.itemImage;
            m_upgradeCheck = true;
            //if (random <= Probability(m_item.upgrade))
            //{
            //    m_item.itemStrength += PlusStr(m_item.upgrade);
            //    m_item.itemDefense += PlusDef(m_item.upgrade);
            //    m_item.upgrade++;
            //    for(int i =0; i<Static.s_wearItemList.Count; i++)
            //    {
            //        if(Static.s_wearItemList[i].itemName == m_item.itemName)
            //            Static.s_wearItemList[i] = m_item;
            //    }
            //    GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
            //    var temp = Instantiate(whether,m_whetherPivot);
            //    temp.transform.Find("Text").GetComponent<Text>().text = "성공";
            //    temp.transform.position = m_whetherPivot.transform.position;
            //}
            //else
            //{
            //    GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
            //    var temp = Instantiate(whether, m_whetherPivot);
            //    temp.transform.Find("Text").GetComponent<Text>().text = "실패";
            //    temp.transform.position = m_whetherPivot.transform.position;
            //}
        }
    }
    // Update is called once per frame
    void Update()
    { 
        if(m_item != null)
        {
            if (m_item.upgrade >= 10)
            {
                m_upgradeBtn.enabled = false;
                m_levelText.text = "MAX";
                m_str.text = m_item.itemStrength.ToString();
                m_def.text = m_item.itemDefense.ToString();
                m_needMoney.text = string.Empty;
                m_nextLevelText.text = string.Empty;

                m_plusStr.text = string.Empty;
                m_plusDef.text = string.Empty;
            }
            else
            {
                m_upgradeBtn.enabled = true;
                m_image.gameObject.SetActive(true);
                m_equipmentInfo.gameObject.SetActive(true);
                m_image.sprite = m_item.itemImage;

                m_levelText.text = m_item.upgrade.ToString();
                m_nextLevelText.text = (m_item.upgrade + 1).ToString();
                m_str.text = m_item.itemStrength.ToString();
                m_plusStr.text = PlusStr(m_item.upgrade).ToString();
                m_def.text = m_item.itemDefense.ToString();
                m_plusDef.text = PlusDef(m_item.upgrade).ToString();
                m_needMoney.text = NeedMoney(m_item.upgrade).ToString();
                m_haveMoney.text = Static.s_silver.ToString();
            }
        }
        else
        {
            m_image.gameObject.SetActive(false);
            m_equipmentInfo.gameObject.SetActive(false);
        }

        if (m_upgradeUI.activeSelf)
        {
            if (m_upgradeCheck)
            {
                m_result.SetActive(false);
                m_upgrading.SetActive(true);
                m_upgradingEf.SetActive(true);
                m_successEf.SetActive(false);
                m_failEf.SetActive(false);
                m_upgradeValue.text = m_item.upgrade.ToString();
                m_upgrageNext.text = (m_item.upgrade + 1).ToString();
                m_upgradeTime += Time.deltaTime;
                m_upgradeSlider.value = m_upgradeTime / 1.5f;
                if(m_upgradeSlider.value >= 1.0f)
                {
                    int random = Random.Range(0, 100);
                    if(random <= Probability(m_item.upgrade))
                    {
                        m_upgrading.SetActive(false);
                        m_result.SetActive(true);
                        m_successUpgrade.SetActive(true);
                        m_upgradingEf.SetActive(false);
                        m_successEf.SetActive(true);
                        m_failEf.SetActive(false);
                        m_failUpgrade.SetActive(false);
                        m_upgradeCheck = false;
                        

                        m_item.itemStrength += PlusStr(m_item.upgrade);
                        m_item.itemDefense += PlusDef(m_item.upgrade);
                        m_item.upgrade++;
                        for (int i = 0; i < Static.s_wearItemList.Count; i++)
                        {
                            if (Static.s_wearItemList[i].itemName == m_item.itemName)
                                Static.s_wearItemList[i] = m_item;
                        }
                        m_resultUpgradeText.text = "+" + m_item.upgrade.ToString();
                    }
                    else
                    {
                        m_upgrading.SetActive(false);
                        m_result.SetActive(true);
                        m_successUpgrade.SetActive(false);
                        m_failUpgrade.SetActive(true);
                        m_upgradingEf.SetActive(false);
                        m_successEf.SetActive(false);
                        m_failEf.SetActive(true);
                        m_upgradeCheck = false;
                        m_resultUpgradeText.text = "+" + m_item.upgrade.ToString();
                    }
                    m_upgradeTime = 0.0f;
                }
            }
        }
    }
    private int Probability(int level)
    {
        switch (level)
        {
            case 1:
                return 95;
            case 2:
                return 90;
            case 3:
                return 85;
            case 4:
                return 80;
            case 5:
                return 75;
            case 6:
                return 70;
            case 7:
                return 65;
            case 8:
                return 60;
            case 9:
                return 55;
            case 10:
                return 50;
            default:
                return 40;
        }
    }
    private int NeedMoney(int level)
    {
        if (level <= 3)
            return 200;
        else if (level <= 6)
            return 400;
        else if (level <= 9)
            return 600;
        else
            return 1000;
    }
    private int PlusStr(int level)
    {
        if (level <= 3)
            return 5;
        else if (level <= 6)
            return 7;
        else if (level <= 9)
            return 10;
        else
            return 15;
    }
    private int PlusDef(int level)
    {
        if (level <= 3)
            return 3;
        else if (level <= 6)
            return 5;
        else if (level <= 9)
            return 8;
        else
            return 10;
    }
}
