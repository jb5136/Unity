using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static GameObject eventInstance;

    private Joystick m_joyStick;
    private CharController m_player;
    private Button m_bagBtn;
    private GameObject m_uiCanvas;
    private Button m_functionBtn;
    private GameObject m_FOBJ;
    private GameObject m_attackBtn;

    //inven ui
    private GameObject m_invenCanvas;
    private Button m_invenBtn;
    private Button m_charInfoBtn;
    private Button m_invencloseBtn;

    private GameObject canvas;
    private Canvas m_inventoryCanvas;
    private Canvas m_charInfoCanvas;

    //TargetUI
    private GameObject m_targetUI;
    private Slider m_targetSlider;
    private Text m_targetLevel;
    private Text m_targetName;
    private Text m_targetValue;
    //BossUI
    private GameObject m_bossUI;
    private Slider m_bossSlider;
    private Text m_bossName;
    private Text m_bossHpValue;

    //skillUI
    private Button m_skillBtn;
    private GameObject m_skillCanvas;
    private Button m_skillCloseBtn;

    //UpgradeUI
    GameObject m_upgradeCnavas;
    private Button m_upgradeBtn;
    private Button m_upgradeCloseBtn;

    //potion
    private Button m_potionBtn;
    private Image m_potionImg;
    private Text m_potionCount;
    private Item m_potion;
    private Image m_coolTimeImg;
    private bool coolDownCheck = false;
    private float coolTime = 2.0f;

    //Die
    private GameObject m_dieCanvas;
    private Button m_revival;

    private GameObject m_EventSystem;

    private Transform m_whetherPivot;

    //MiniMap
    private GameObject m_villageM;
    private GameObject m_fieldM;
    private GameObject m_dungeonM;
    private void Awake()
    {
        //m_EventSystem = GameObject.Find("EventSystem");
        //if(eventInstance != null)
        //{
        //    Destroy(m_EventSystem);
        //}
        //else
        //{
        //    eventInstance = m_EventSystem;
        //    DontDestroyOnLoad(eventInstance);
        //}
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Static.SetExp();
    }
    // Start is called before the first frame update
    void Start()
    {
        Static.SetWearEquip();

        Static.Money();
        m_joyStick = GameObject.FindObjectOfType<Joystick>( );
        m_player = GameObject.FindObjectOfType<CharController>();
        canvas = GameObject.Find("Canvas").gameObject;
        if(canvas != null)
        {
            m_uiCanvas = canvas.transform.Find("UI").gameObject;
            m_whetherPivot = m_uiCanvas.transform.Find("TextPivot");
            m_attackBtn = m_uiCanvas.transform.Find("AttackButton").gameObject;
            m_functionBtn = m_uiCanvas.transform.Find("FunctionBtn").GetComponent<Button>();
            m_FOBJ = m_functionBtn.transform.Find("Function").gameObject;
            m_functionBtn.onClick.AddListener(FunctionBtn);
            m_invenCanvas = canvas.transform.Find("InvenCanvas").gameObject;
            m_potionBtn = m_uiCanvas.transform.Find("Potion").GetComponent<Button>();
            m_potionImg = m_potionBtn.transform.Find("Image").GetComponent<Image>();
            m_potionCount = m_potionBtn.transform.Find("Text").GetComponent<Text>();
            m_coolTimeImg = m_potionBtn.transform.Find("CoolTimeImg").GetComponent<Image>();
            m_potionBtn.onClick.AddListener(PotionBtn);

            m_invenBtn = canvas.transform.Find("InvenCanvas/Btn/InventoryBtn").GetComponent<Button>();
            m_charInfoBtn = canvas.transform.Find("InvenCanvas/Btn/CharInfoBtn").GetComponent<Button>();
            m_invencloseBtn = canvas.transform.Find("InvenCanvas/CloseBtn").GetComponent<Button>();
            m_inventoryCanvas = canvas.transform.Find("InvenCanvas/InventoryCanvas").GetComponent<Canvas>();
            m_charInfoCanvas = canvas.transform.Find("InvenCanvas/CharInfoCanvas").GetComponent<Canvas>();

        }

        if (m_invenBtn != null)
            m_invenBtn.onClick.AddListener(ClickInven);
        if (m_charInfoBtn != null)
            m_charInfoBtn.onClick.AddListener(ClickChar);
        if (m_invencloseBtn != null)
            m_invencloseBtn.onClick.AddListener(InvenCloseBtn);

        canvas = GameObject.Find("UI/FunctionBtn/Function/Bag");
        if(canvas != null)
        {
            m_bagBtn = canvas.GetComponent<Button>();
            m_bagBtn.onClick.AddListener(BagBtn);
        }
        

        //target UI
        m_targetUI = GameObject.Find("Canvas/UI").gameObject.transform.Find("TargetUI").gameObject;
        if(m_targetUI != null)
        {
            m_targetSlider = m_targetUI.transform.Find("Slider").GetComponent<Slider>();
            m_targetLevel = m_targetUI.transform.Find("Level").GetComponent<Text>();
            m_targetName = m_targetUI.transform.Find("Name").GetComponent<Text>();
            m_targetValue = m_targetUI.transform.Find("Value").GetComponent<Text>();
        }
        //Boss UI
        m_bossUI = GameObject.Find("Canvas/UI").gameObject.transform.Find("BossUI").gameObject;
        if(m_bossUI != null)
        {
            m_bossSlider = m_bossUI.transform.Find("Slider").GetComponent<Slider>();
            m_bossName = m_bossUI.transform.Find("Name").GetComponent<Text>();
            m_bossHpValue = m_bossUI.transform.Find("Value").GetComponent<Text>();
        }
        //skillUI
        m_skillCanvas = GameObject.Find("Canvas").gameObject.transform.Find("SkillCanvas").gameObject;
        m_skillBtn = GameObject.Find("Canvas/UI").gameObject.transform.Find("FunctionBtn/Function/Skill").GetComponent<Button>();
        if(m_skillBtn != null)
            m_skillBtn.onClick.AddListener(SkillBtn);
        if (m_skillCanvas != null)
            m_skillCloseBtn = m_skillCanvas.transform.Find("CloseBtn").GetComponent<Button>();
        if (m_skillCloseBtn != null)
            m_skillCloseBtn.onClick.AddListener(SkillCloseBtn);
        //Upgrade
        m_upgradeCnavas = GameObject.Find("Canvas").gameObject.transform.Find("UpgradeCanvas").gameObject;
        m_upgradeBtn = m_uiCanvas.transform.Find("FunctionBtn/Function/Upgrade").GetComponent<Button>();
        m_upgradeCloseBtn = m_upgradeCnavas.transform.Find("CloseBtn").GetComponent<Button>();
        m_upgradeCloseBtn.onClick.AddListener(UpgradeCloseBtn);
        m_upgradeBtn.onClick.AddListener(UpgradeBtn);
        m_FOBJ.gameObject.SetActive(false);
        SetInven();
        //Die
        m_dieCanvas = GameObject.Find("Canvas").transform.Find("Die").gameObject;
        m_revival = m_dieCanvas.transform.Find("Btn").GetComponent<Button>();
        m_revival.onClick.AddListener(Die);

        //MiniMap
        m_villageM = m_uiCanvas.transform.Find("MiniMap/VillageM").gameObject;
    }

    private void Die()
    {
        m_dieCanvas.SetActive(false);
        ResetPlayer();
        m_player.GetComponent<CharController>().isDieCheck = false;
        LoadingSceneMng.LoadScene("Village");
    }
    
    private void ResetPlayer()
    {
        Slider[] g = GameObject.Find("Canvas/HpCanvas").GetComponentsInChildren<Slider>(true);
        foreach(Slider s in g)
        {
            Destroy(s.gameObject);
        }
        
        m_player.GetComponent<Animator>().SetTrigger("Idle");
        m_player.GetComponent<Animator>().speed = 1.0f;
        m_player.GetComponent<PlayerHealth>().GetHp = m_player.GetComponent<PlayerHealth>().GetOriHp;
    }
    private void SetInven()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharItemList.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamReader sr = new StreamReader(fs);
        sr.ReadLine();
        int j = 0;
        Item item;
        while (true)
        {
            string itemList = sr.ReadLine();
            if (itemList == null)
                break;
            string[] iteminfo = itemList.Split(',');
            int _itemType = int.Parse(iteminfo[0]);
            if (_itemType == 1)
            {
                string _itemName = iteminfo[1];
                int _itemValue = int.Parse(iteminfo[2]);
                int _itemEquipType = int.Parse(iteminfo[3]);
                int m_upgrade = int.Parse(iteminfo[4]);
                item = Database.instance.items.Find(ex => ex.itemName == _itemName);
                Item newitem = new Item(_itemType, item.itemName, _itemValue, item.itemPrice, item.itemDesc, 
                    item.itemImage, _itemEquipType, item.itemStrength, item.itemDefense, item.itemCritical, m_upgrade);
                Static.InvenItemAddList(newitem);
                j++;
            }
            else if (_itemType == 2)
            {
                string _itemName = iteminfo[1];
                int _itemValue = int.Parse(iteminfo[2]);
                item = Database.instance.items.Find(ex => ex.itemName == _itemName);
                Item newitem = new Item(_itemType, item.itemName, _itemValue, item.itemPrice, item.itemDesc, item.itemImage);
                Static.InvenItemAddList(newitem);
                j++;
            }
        }
        sr.Close();
    }

    private void PotionBtn()
    {
        if(m_potion != null)
        {
            if(m_player.GetComponent<PlayerHealth>().GetHp>= m_player.GetComponent<PlayerHealth>().GetOriHp)
            {
                GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
                var temp = Instantiate(whether, m_whetherPivot);
                temp.transform.Find("Text").GetComponent<Text>().text = "체력이 가득 차 있습니다";
                temp.transform.position = m_whetherPivot.transform.position;
                return;
            }
            Static.s_invenItemList.Find(ex => ex.itemName == "Potion").itemValue = Static.s_invenItemList.Find(ex => ex.itemName == "Potion").itemValue - 1;
            m_player.GetComponent<PlayerHealth>().Heal(300);
            if(Static.s_invenItemList.Find(ex => ex.itemName == "Potion").itemValue <= 0)
            {
                Static.s_invenItemList.Remove(Static.s_invenItemList.Find(ex => ex.itemName == "Potion"));
            }
            Static.InvenItemSaveList();
            coolDownCheck = true;
        }
    }
    private void FunctionBtn()
    {
        if (m_FOBJ.gameObject.activeSelf == false)
        {
            //m_attackBtn.transform.position =new Vector3(367.7f, -53.8f,0);
            m_attackBtn.SetActive(false);
            m_FOBJ.gameObject.SetActive(true);
        }
        else
        {
            //m_attackBtn.transform.position = new Vector3(367.7f, 99.3f, 0);
            m_attackBtn.SetActive(true);
            m_FOBJ.gameObject.SetActive(false);
        }
    }
    public void SaveCharState()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharState.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        string[] state = m_player.GetComponent<CharController>().StateReturn();
        string line = null;
        for(int i =0; i<state.Length; i++)
        {
            if(i == 0)
                line += state[i];
            else
                line += ","+state[i];
        }
        sw.WriteLine(line);
        sw.Close();
    }

    private void ClickInven()
    {
        m_inventoryCanvas.gameObject.SetActive(true);
        m_charInfoCanvas.gameObject.SetActive(false);
        Static.SaveList();
        Static.InvenItemSaveList();
      
    }
    private void ClickChar()
    {
        m_charInfoCanvas.gameObject.SetActive(true);
        m_inventoryCanvas.gameObject.SetActive(false);
        Static.SaveList();
        Static.InvenItemSaveList();
    }
    private void InvenCloseBtn()
    {
        Static.SaveList();
        Static.InvenItemSaveList();
        
        m_invenCanvas.SetActive(false);
        m_skillCanvas.SetActive(false);
        m_uiCanvas.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Render").gameObject.SetActive(false);
    }
    private void SkillCloseBtn()
    {
        m_invenCanvas.SetActive(false);
        m_skillCanvas.SetActive(false);
        m_upgradeCnavas.SetActive(false);
        m_uiCanvas.SetActive(true);
        SaveCharState();
    }
    private void UpgradeCloseBtn()
    {
        m_upgradeCnavas.transform.Find("UpgradeItemSlot").GetComponent<UpgradeItem>().m_item = null;
        m_invenCanvas.SetActive(false);
        m_skillCanvas.SetActive(false);
        m_upgradeCnavas.SetActive(false);
        m_uiCanvas.SetActive(true);
        GameObject.Find("Canvas").transform.Find("UpgradeEffect").gameObject.SetActive(false);
        Static.SaveList();
        Static.InvenItemSaveList();
    }
    private void BagBtn()
    {
        m_uiCanvas.SetActive(false);
        m_skillCanvas.SetActive(false);
        m_upgradeCnavas.SetActive(false);
        m_invenCanvas.SetActive(true);
        SaveCharState();
        GameObject.Find("Canvas").transform.Find("Render").gameObject.SetActive(true);
    }
    private void SkillBtn()
    {
        m_uiCanvas.SetActive(false);
        m_invenCanvas.SetActive(false);
        m_upgradeCnavas.SetActive(false);
        m_skillCanvas.SetActive(true);
        SaveCharState();
    }
    private void UpgradeBtn()
    {
        m_uiCanvas.SetActive(false);
        m_invenCanvas.SetActive(false);
        m_skillCanvas.SetActive(false);
        m_upgradeCnavas.SetActive(true);
        GameObject.Find("Canvas").transform.Find("UpgradeEffect").gameObject.SetActive(true);
        m_upgradeCnavas.GetComponent<UpgradeCanvas>().GetOnCheck = true;
    }
    private void FindPotion()
    {
        Item temp = Static.s_invenItemList.Find(ex => ex.itemName == "Potion");
        if (temp != null)
            m_potion = temp;
        else
        {
            m_potion = null;
        }
    }
    private void SkillOpen()
    {
        int level = m_player.GetComponent<CharController>().GetLevel;
        if(level < 2)
        {
        }
    }
    private void MiniMapSet()
    {
        if(SceneManager.GetActiveScene().name == "Village")
        {
            m_villageM.SetActive(true);
        }
    }
    private bool isDieCheck = false;
    void Update()
    {

        if (m_player.GetComponent<PlayerHealth>().IsDie)
        {
            if (!isDieCheck)
            {
                m_uiCanvas.SetActive(false);
                m_dieCanvas.SetActive(true);
                isDieCheck = true;
            }
            return;
        }

        FindPotion();
        if(m_potion != null)
        {
            m_potionImg.enabled = true;
            m_potionCount.text = m_potion.itemValue.ToString();
        }
        else
        {
            m_potionImg.enabled = false;
            m_potionCount.text = string.Empty;
        }

        if (coolDownCheck)
        {
            m_potionBtn.enabled = false;
            coolTime -= Time.deltaTime;
            Text text = m_coolTimeImg.transform.GetComponentInChildren<Text>();
            int timeText = (int)coolTime + 1;
            m_coolTimeImg.fillAmount = coolTime / 2.0f;
            if (coolTime <= 0.0f)
            {
                m_coolTimeImg.fillAmount = 0.0f;
                coolDownCheck = false;
                m_potionBtn.enabled = true;
                coolTime = 2.0f;
            }
        }
        else
            m_coolTimeImg.fillAmount = 0.0f;
        m_player.InputFunc(m_joyStick.Dir,m_joyStick.GetmoveCheck);
        m_player.UpdateAttack(m_joyStick.GetmoveCheck);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        MonsterHealth[] monsters = GameObject.FindObjectsOfType<MonsterHealth>();
        for( int i = 0; i< monsters.Length; ++ i )
        {
            Collider collider = monsters[i].GetComponent<Collider>();
            if( GeometryUtility.TestPlanesAABB(planes, collider.bounds ) )
            {
                monsters[i].SetActiveHp(true);
            }
            else
                monsters[i].SetActiveHp(false);
        }

        Collider col = m_player.GetCollider;
        if(col != null)
        {
            if (col.GetComponent<MonsterHealth>().IsDie || !col.gameObject.activeSelf)
            {
                col = null;
                m_bossUI.SetActive(false);
                m_targetUI.SetActive(false);
            }
            else
            {
                if(col.tag == "Boss")
                {
                    m_bossUI.SetActive(true);
                    m_bossSlider.value = col.GetComponent<MonsterHealth>().GetHp / col.GetComponent<MonsterHealth>().GetOriHp;
                    m_bossHpValue.text = ((int)(m_bossSlider.value * 100)).ToString() + "%";
                }
                else if(col.tag == "Monster")
                {
                    m_targetUI.SetActive(true);
                    m_targetSlider.value = col.GetComponent<MonsterHealth>().m_slider.value;
                    m_targetLevel.text = "Lv." + col.GetComponent<MonsterController>().GetLevel.ToString();
                    m_targetName.text = col.GetComponent<MonsterController>().GetName;
                    m_targetValue.text = ((int)(m_targetSlider.value * 100)).ToString() + "%";
                }
            }
        }
        else
        {
            col = null;
            m_bossUI.SetActive(false);
            m_targetUI.SetActive(false);
        }

    }
}
