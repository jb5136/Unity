using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCanvas : MonoBehaviour
{
    private GameController m_GameController;
    private CharController m_player;
    private int m_playerSkillLevel;

    private Button m_skill1;
    private Button m_skill2;
    private Button m_skill3;
    private Button m_skill4;
    private Button m_skill5;
    private Text m_S1Name;
    private Text m_S2Name;
    private Text m_S3Name;
    private Text m_S4Name;
    private Text m_S5Name;
    private Text m_S1Level;
    private Text m_S2Level;
    private Text m_S3Level;
    private Text m_S4Level;
    private Text m_S5Level;

    //skill Info
    private GameObject m_skillInfo;
    private SkillInfo m_SISkill;
    private Image m_SIImg;
    private Text m_SIName;
    private Text m_SICoolTime;
    private Text m_SIDesc;
    private Text m_SINextDamage;
    private Text m_SIPoint;
    private Button m_SIUpgrade;

    private Text m_SP;
    // Start is called before the first frame update
    void Start()
    {
        //여러명이 플레이 할거라면 여기 수정해야됨
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
        m_GameController = Transform.FindObjectOfType<GameController>();
        m_SP = transform.Find("SkillPoint/Text").GetComponent<Text>();

        Transform t = transform.Find("Skill");
        if (t != null)
        {
            m_skill1 = t.transform.Find("Skill1").GetComponent<Button>();
            m_S1Name = m_skill1.transform.Find("Name").GetComponent<Text>();
            m_S1Level = m_skill1.transform.Find("Level/Text").GetComponent<Text>();
            m_skill1.onClick.AddListener(Skill1);
            m_skill2 = t.transform.Find("Skill2").GetComponent<Button>();
            m_S2Name = m_skill2.transform.Find("Name").GetComponent<Text>();
            m_S2Level = m_skill2.transform.Find("Level/Text").GetComponent<Text>();
            m_skill2.onClick.AddListener(Skill2);
            m_skill3 = t.transform.Find("Skill3").GetComponent<Button>();
            m_S3Name = m_skill3.transform.Find("Name").GetComponent<Text>();
            m_S3Level = m_skill3.transform.Find("Level/Text").GetComponent<Text>();
            m_skill3.onClick.AddListener(Skill3);
            m_skill4 = t.transform.Find("Skill4").GetComponent<Button>();
            m_S4Name = m_skill4.transform.Find("Name").GetComponent<Text>();
            m_S4Level = m_skill4.transform.Find("Level/Text").GetComponent<Text>();
            m_skill4.onClick.AddListener(Skill4);
            m_skill5 = t.transform.Find("Skill5").GetComponent<Button>();
            m_S5Name = m_skill5.transform.Find("Name").GetComponent<Text>();
            m_S5Level = m_skill5.transform.Find("Level/Text").GetComponent<Text>();
            m_skill5.onClick.AddListener(Skill5);

        }
        m_S1Name.text = Database.instance.skills.Find(ex => ex.name == "Skill1").name;
        m_S2Name.text = Database.instance.skills.Find(ex => ex.name == "Skill2").name;
        m_S3Name.text = Database.instance.skills.Find(ex => ex.name == "Skill3").name;
        m_S4Name.text = Database.instance.skills.Find(ex => ex.name == "Skill4").name;
        m_S5Name.text = Database.instance.skills.Find(ex => ex.name == "Skill5").name;

        m_skillInfo = transform.Find("SkillInfo").gameObject;
        if (m_skillInfo != null)
        {
            m_SIImg = m_skillInfo.transform.Find("SkillImg/Image").GetComponent<Image>();
            m_SIName = m_skillInfo.transform.Find("SkillName").GetComponent<Text>();
            m_SICoolTime = m_skillInfo.transform.Find("CoolTime").GetComponent<Text>();
            m_SIDesc = m_skillInfo.transform.Find("SkillDesc").GetComponent<Text>();
            m_SINextDamage = m_skillInfo.transform.Find("NextLevel/Damage").GetComponent<Text>();
            m_SIPoint = m_skillInfo.transform.Find("NeedPoint/Text").GetComponent<Text>();
            m_SIUpgrade = m_skillInfo.transform.Find("Upgrade").GetComponent<Button>();
            m_SIUpgrade.onClick.AddListener(Upgrade);
        }
    }
    private void Upgrade()
    {
        if(m_SISkill.name == "Skill1")
        {
            int temp = m_player.GetSL1;
            if (m_player.GetSP >= ChangeSP(temp) && temp < 5)
            {
                m_player.GetSP = ChangeSP(temp);
                ++temp;
                m_player.GetSL1 = temp;
                m_playerSkillLevel = m_player.GetSL1;
                m_GameController.SaveCharState();
            }

        }   
        else if(m_SISkill.name == "Skill2")
        {
            int temp = m_player.GetSL2;
            if (m_player.GetSP >= ChangeSP(temp) && temp < 5)
            {
                m_player.GetSP = ChangeSP(temp);
                ++temp;
                m_player.GetSL2 = temp;
                m_playerSkillLevel = m_player.GetSL2;
                m_GameController.SaveCharState();
            }
        }
        else if (m_SISkill.name == "Skill3")
        {
            int temp = m_player.GetSL3;
            if (m_player.GetSP >= ChangeSP(temp) && temp < 5)
            {
                m_player.GetSP = ChangeSP(temp);
                ++temp;
                m_player.GetSL3 = temp;
                m_playerSkillLevel = m_player.GetSL3;
                m_GameController.SaveCharState();
            }
        }
        else if (m_SISkill.name == "Skill4")
        {
            int temp = m_player.GetSL4;
            if (m_player.GetSP >= ChangeSP(temp) && temp < 5)
            {
                m_player.GetSP = ChangeSP(temp);
                ++temp;
                m_player.GetSL4 = temp;
                m_playerSkillLevel = m_player.GetSL4;
                m_GameController.SaveCharState();
            }
        }
        else if (m_SISkill.name == "Skill5")
        {
            int temp = m_player.GetSL5;
            if (m_player.GetSP >= ChangeSP(temp) && temp < 5)
            {
                m_player.GetSP = ChangeSP(temp);
                ++temp;
                m_player.GetSL5 = temp;
                m_playerSkillLevel = m_player.GetSL5;
                m_GameController.SaveCharState();
            }
        }

    }
    private float ChangeLevel(SkillInfo info,int level)
    {
        switch (level)
        {
            case 1:
                return info.level1;
            case 2:
                return info.level2;
            case 3:
                return info.level3;
            case 4:
                return info.level4;
            case 5:
                return info.level5;
        }
        return 0.0f;
    }
    private int ChangeSP(int level)
    {
        switch (level)
        {
            case 1:
                return 1;
            case 2:
                return 3;
            case 3:
                return 5;
            case 4:
                return 6;
            case 5:
                return 7;
        }
        return 0;
    }
    private void SetSkillInfo()
    {
        
        m_SIImg.sprite = m_SISkill.image;
        m_SIName.text = m_SISkill.name + " Lv." + m_playerSkillLevel.ToString();
        m_SICoolTime.text = "쿨타임 : " + m_SISkill.coolTime;
        m_SIDesc.text = m_SISkill.desc1 + " " + (ChangeLevel(m_SISkill, m_playerSkillLevel) * 100).ToString() + "%" + m_SISkill.desc2;
        if (m_playerSkillLevel >= 5)
            m_SINextDamage.text = (ChangeLevel(m_SISkill, m_playerSkillLevel) * 100).ToString() + "%";
        else
            m_SINextDamage.text = (ChangeLevel(m_SISkill, m_playerSkillLevel) * 100).ToString() + "% -> " + (ChangeLevel(m_SISkill, m_playerSkillLevel + 1) * 100).ToString() + "%";
        m_SIPoint.text = ChangeSP(m_playerSkillLevel).ToString();
    }
    private void Skill1()
    {
        m_SISkill = Database.instance.skills.Find(ex => ex.name == "Skill1");
        
        m_playerSkillLevel = m_player.GetSL1;
        m_skillInfo.SetActive(true);
    }
    private void Skill2()
    {
        m_SISkill = Database.instance.skills.Find(ex => ex.name == "Skill2");
        m_playerSkillLevel = m_player.GetSL2;
        m_skillInfo.SetActive(true);
    }
    private void Skill3()
    {
        m_SISkill = Database.instance.skills.Find(ex => ex.name == "Skill3");
        m_playerSkillLevel = m_player.GetSL3;
        m_skillInfo.SetActive(true);
    }
    private void Skill4()
    {
        m_SISkill = Database.instance.skills.Find(ex => ex.name == "Skill4");
        m_playerSkillLevel = m_player.GetSL4;
        m_skillInfo.SetActive(true);
    }
    private void Skill5()
    {
        m_SISkill = Database.instance.skills.Find(ex => ex.name == "Skill5");
        m_playerSkillLevel = m_player.GetSL5;
        m_skillInfo.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        m_SP.text = m_player.GetSP.ToString();

        m_S1Level.text = "Lv." + m_player.GetSL1.ToString();
        m_S2Level.text = "Lv." + m_player.GetSL2.ToString();
        m_S3Level.text = "Lv." + m_player.GetSL3.ToString();
        m_S4Level.text = "Lv." + m_player.GetSL4.ToString();
        m_S5Level.text = "Lv." + m_player.GetSL5.ToString();

        if(m_skillInfo.activeSelf == true)
        {
            SetSkillInfo();
        }
    }
}
