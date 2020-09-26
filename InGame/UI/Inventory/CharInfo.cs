using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CharInfo : MonoBehaviour
{
    //ui
    private Text m_strValue;
    private Text m_defValue;
    private Text m_hpValue;
    private Slider m_hpSlider;
    private Text m_expValue;
    private Slider m_expSlider;
    private Text m_stateStr;
    private Text m_stateDef;
    private Text m_stateCri;

    //Char state
    private int m_charId;
    private string m_charName;
    private int m_charLevel;
    private int m_str;
    private int m_def;
    private float m_cri;
    private float m_hp;

    private CharEquipment m_CharEquip;
    private GameObject m_player;
    // Start is called before the first frame update
    void Start()
    {
        m_strValue = transform.Find("Strength/StrengthValue").GetComponent<Text>();
        m_defValue = transform.Find("Defence/DefenceValue").GetComponent<Text>();
        m_hpValue = transform.Find("Health/HealthValue").GetComponent<Text>();
        m_hpSlider = transform.Find("Health/HealthSlider").GetComponent<Slider>();
        m_expValue = transform.Find("Exp/ExpValue").GetComponent<Text>();
        m_expSlider = transform.Find("Exp/ExpSlider").GetComponent<Slider>();
        m_stateStr = transform.Find("State/Str/StrValue").GetComponent<Text>();
        m_stateDef = transform.Find("State/Def/DefValue").GetComponent<Text>();
        m_stateCri = transform.Find("State/Cri/CriValue").GetComponent<Text>();

        m_player = GameObject.FindGameObjectWithTag("Player");

        m_CharEquip = GameObject.Find("Canvas/InvenCanvas").transform.Find("InventoryCanvas/CharEquipment").GetComponent<CharEquipment>();
    }
    private void SetCharStat()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharState.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        StreamReader sr = new StreamReader(fs);
        while (true)
        {
            string line = sr.ReadLine();
            if (line == null)
                break;
            string[] info = line.Split(',');
            m_charId =int.Parse(info[0]);
            m_charName = info[1];
            m_charLevel = int.Parse(info[2]);
            m_str = int.Parse(info[3]);
            m_def = int.Parse(info[4]);
            m_cri = float.Parse(info[5]);
            m_hp = int.Parse(info[6]);
        }
        sr.Close();
        
    }
    // Update is called once per frame
    void Update()
    {
        SetCharStat();
        m_strValue.text = (m_str + m_CharEquip.TotalStr()).ToString();
        m_defValue.text = (m_def + m_CharEquip.TotalDef()).ToString();
        float hp = m_player.GetComponent<PlayerHealth>().GetHp;
        m_hpValue.text = hp.ToString() + " / " + m_hp.ToString();
        m_hpSlider.value = hp/m_hp;
        float exp = m_player.GetComponent<CharController>().GetExp;
        float maxExp = Static.s_maxExp[m_player.GetComponent<CharController>().GetLevel - 1];
        m_expValue.text = exp + "/" + maxExp;
        m_expSlider.value = exp/maxExp;
        m_stateStr.text = (m_str + m_CharEquip.TotalStr()).ToString();
        m_stateDef.text = (m_def + m_CharEquip.TotalDef()).ToString();
        m_stateCri.text = ((int)m_cri + (int)m_CharEquip.TotalCri()).ToString();
    }
}
