using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;

public class DungeonSceneMng : MonoBehaviour
{
    private Transform[] m_spawnPoints_1;
    private Transform[] m_spawnPoints_2;
    private Transform[] m_spawnPoints_3;
    private Transform[] m_spawnPoints_4;
    private Transform[] m_spawnPoints_5;

    private GameObject m_player;
    //Mercenary
    private GameObject m_mercenaryObj;
    private List<GameObject> m_mercenary = new List<GameObject>();
    private List<Slider> m_mercenaryHpSlider = new List<Slider>();

    //Chapter
    private GameObject m_firstRange;
    private GameObject m_secondRange;
    private bool m_firstChapter = true;
    private bool m_secondChapter = false;
    private bool m_thirdChapter = false;
    private int m_chapterCount = 1;

    //Boss
    private Transform m_bossStartPos;

    //clear UI
    public bool isClear = false;
    private GameObject m_clearUI;
    private GameObject m_avaliableItem;
    private int m_money = 1001;
    private int m_exp = 500;
    private Text m_getMoney;
    private Text m_getExp;
    private Button m_clearOutBtn;
    private List<Item> m_getItem = new List<Item>();
    private List<Item> m_ItemList = new List<Item>();

    //failUI
    private GameObject m_failUI;
    private Button m_failOutBtn;

    //OutUI
    private Button m_outBtn;
    private GameObject m_outCheck;
    private Button m_outCheckYesBtn;
    private Button m_outCheckNoBtn;

    Image mini;
    GameObject m_camera;
    private void Awake()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (m_camera != null)
        {
            m_camera.GetComponent<MainCamera>().offset = new Vector3(-4, 4, 1);
        }
        m_player = GameObject.FindGameObjectWithTag("Player");
        GameObject g = GameObject.Find("Canvas").transform.Find("UI").gameObject;
        g.SetActive(true);

        m_player.transform.position = GameObject.Find("StartPos").transform.position;
        m_player.transform.rotation = Quaternion.LookRotation(GameObject.Find("StartPos").transform.forward);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = true;
        Slider[] mon = GameObject.Find("Canvas/HpCanvas").GetComponentsInChildren<Slider>(true);
        foreach (Slider s in mon)
        {
            Destroy(s.gameObject);
        }

    }

    void Start()
    {
        GameObject.Find("Main Camera").GetComponent<MainCamera>().isSceneMove = false;

        m_spawnPoints_1 = GameObject.Find("SpawnPoint/SpawnPoints1").GetComponentsInChildren<Transform>();
        m_spawnPoints_2 = GameObject.Find("SpawnPoint/SpawnPoints2").GetComponentsInChildren<Transform>();
        m_spawnPoints_3 = GameObject.Find("SpawnPoint/SpawnPoints3").GetComponentsInChildren<Transform>();
        m_spawnPoints_4 = GameObject.Find("SpawnPoint/SpawnPoints4").GetComponentsInChildren<Transform>();
        m_spawnPoints_5 = GameObject.Find("SpawnPoint/SpawnPoints5").GetComponentsInChildren<Transform>();

        //나중에 파일로 바꾸기
        m_ItemList.Add(Database.instance.items.Find(ex => ex.itemName == "Sword3"));
        m_ItemList.Add(Database.instance.items.Find(ex => ex.itemName == "Helmet3"));
        m_ItemList.Add(Database.instance.items.Find(ex => ex.itemName == "Armor3"));

        m_mercenaryObj = GameObject.Find("Mercenary");
        Slider[] mercenaryHpSlider = GameObject.Find("DCanvas").transform.Find("MercenaryHp").GetComponentsInChildren<Slider>(true);
        foreach (Slider si in mercenaryHpSlider)
        {
            m_mercenaryHpSlider.Add(si);
        }
        //SetMercenary();
        SetMercenary("Dwarf", 1);
        SetMercenary("Magician", 2);
        m_firstRange = GameObject.Find("Range/FirstRange");
        m_secondRange = GameObject.Find("Range/SecondRange");

        m_bossStartPos = GameObject.Find("BossStartPos").transform;

        //clearUI
        m_clearUI = GameObject.Find("DCanvas").transform.Find("Clear").gameObject;
        if(m_clearUI != null)
        {
            m_avaliableItem = m_clearUI.transform.Find("AvaliableItem").gameObject;
            m_getMoney = m_clearUI.transform.Find("GetMoney/Money/Text").GetComponent<Text>();
            m_getExp = m_clearUI.transform.Find("GetExp/Exp/Text").GetComponent<Text>();
            m_clearOutBtn = m_clearUI.transform.Find("OutBtn").GetComponent<Button>();
            m_clearOutBtn.onClick.AddListener(Out);
        }
        m_clearUI.SetActive(false);

        //failUI
        m_failUI = GameObject.Find("DCanvas").transform.Find("Fail").gameObject;
        if (m_failUI != null)
            m_failOutBtn = m_failUI.transform.Find("OutBtn").GetComponent<Button>();
        m_failOutBtn.onClick.AddListener(Out);
        m_failUI.SetActive(false);

        //OutUI
        m_outBtn = GameObject.Find("DCanvas/OutDungeon").GetComponent<Button>();
        m_outBtn.onClick.AddListener(OutCheck);
        m_outCheck = GameObject.Find("DCanvas").transform.Find("OutDungeonCheck").gameObject;
        if(m_outCheck != null)
        {
            m_outCheckYesBtn = m_outCheck.transform.Find("Yes").GetComponent<Button>();
            m_outCheckNoBtn = m_outCheck.transform.Find("No").GetComponent<Button>();
            m_outCheckYesBtn.onClick.AddListener(Out);
            m_outCheckNoBtn.onClick.AddListener(OutCheckNoBtn);
        }

        mini = GameObject.Find("DCanvas/MiniMap").GetComponent<Image>();
        mini.material.mainTexture = Resources.Load<RenderTexture>("MiniMap/MiniMap");
    }

    private void OutCheck()
    {
        m_outCheck.SetActive(true);
    }
    private void OutCheckNoBtn()
    {
        m_outCheck.SetActive(false);
    }

    private void Out()
    {
        LoadingSceneMng.LoadScene("Field");
        GameObject g = GameObject.Find("Canvas/UI");
        m_camera.GetComponent<MainCamera>().isSceneMove = true;
        m_camera.transform.position = new Vector3(60, 5, 60);
        g.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().enabled = false;
    }

    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        ////Gizmos.DrawCube(transform.position + transform.forward * 3.0f, new Vector3(2, 2, 6));
        //Gizmos.DrawSphere(m_secondRange.transform.position, 22.0f);
    }
    private void SetMercenary(string name, int i)
    {
        GameObject g = Resources.Load<GameObject>("Mercenary/" + name);
        //GameObject mercenary = null;
        if (g != null)
        {
            GameObject mercenary = Instantiate(g);

            //mercenary.transform.position = GameObject.Find("Mercenary").transform.position;
            mercenary.transform.parent = GameObject.Find("Mercenary").transform;
            mercenary.GetComponent<NavMeshAgent>().enabled = false;
            //나중에 꼭 바꿔야 효율적이다
            if (name == "Dwarf")
            {
                mercenary.GetComponent<Dwarf>().attackCheck = true;
            }
            else if (name == "Magician")
            {
                mercenary.GetComponent<Magician>().attackCheck = true;
            }
            mercenary.GetComponent<MercenaryHealth>().m_slider = m_mercenaryHpSlider[i];
            mercenary.GetComponent<MercenaryHealth>().name = name;
            mercenary.transform.position = m_player.transform.position + m_player.transform.forward * -i;
            mercenary.GetComponent<NavMeshAgent>().enabled = true;
            m_mercenary.Add(mercenary);
        }
    }
    private void SetMercenary()
    {
        TextAsset txtFile = Resources.Load("Files/Mercenary") as TextAsset;
        string text = txtFile.text;
        string[] line2 = text.Split('\n');
        //string path = Application.dataPath + "/Resources/Files/Mercenary.txt";
        //StreamReader sr = new StreamReader(path);
        //sr.ReadLine();
        int i = 2;
        int j = 0;
        while (true)
        {
            if (j >= line2.Length-1)
                break;
            string line = line2[j+1];
            string[] info = line.Split(',');
            if (int.Parse(info[0]) == 1)
            {
                GameObject mercenary = Instantiate(Resources.Load<GameObject>("Mercenary/" + info[1]));
                //mercenary.transform.position = GameObject.Find("Mercenary").transform.position;
                mercenary.transform.parent = GameObject.Find("Mercenary").transform;
                mercenary.GetComponent<NavMeshAgent>().enabled = false;
                //나중에 꼭 바꿔야 효율적이다
                if(info[1] == "Dwarf")
                {
                    mercenary.GetComponent<Dwarf>().attackCheck = true;
                }
                else if(info[1] == "Magician")
                {
                    mercenary.GetComponent<Magician>().attackCheck = true;
                }

                mercenary.GetComponent<MercenaryHealth>().m_slider = m_mercenaryHpSlider[j];
                mercenary.GetComponent<MercenaryHealth>().name = info[1];
                mercenary.transform.position = m_player.transform.position + m_player.transform.forward * -i;
                m_mercenary.Add(mercenary);
                i--;
                ++j;
            }
        }
        //sr.Close();
    }
    void SpawnMonster(Transform[] spawnPoint, string name)
    {
        foreach (Transform t in spawnPoint)
        {
            int monCount = t.transform.childCount;
            if (t.gameObject.tag != "SpawnPoint" && monCount == 0)
            {
                GameObject monster = Resources.Load<GameObject>("Monsters/" + name);
                monster = Instantiate(monster);
                monster.transform.parent = t.transform;
                monster.transform.position = t.position;
                Monster monInfo = Database.instance.monsters.Find(ex => ex.monName == name);
                monster.GetComponent<MonsterController>().SetMonsterInfo = monInfo;
                monster.GetComponent<MonsterController>().traceCheck = true;
            }
        }
    }
    private void ChapterRange()
    {
        if(m_chapterCount == 1)
        {
            int count = 0;
            Collider[] targets = Physics.OverlapSphere(m_firstRange.transform.position, 22.0f);
            if(targets.Length != 0)
            {
                foreach(Collider col in targets)
                {
                    if (col.gameObject.tag == "Monster" && !col.GetComponent<MonsterHealth>().IsDie)
                    {
                        count++;
                    }
                }
            }
            if(count == 0)
            {
                GameObject.Find("Environment/TransparentWall").SetActive(false);
                m_secondChapter = true;
                m_chapterCount++;
                isRangeCheck = false;
            }
        }
        else if(m_chapterCount == 2)
        {
            int count = 0;
            Collider[] targets = Physics.OverlapSphere(m_secondRange.transform.position, 22.0f);
            if (targets.Length != 0)
            {
                foreach (Collider col in targets)
                {
                    if (col.gameObject.tag == "Monster" && !col.GetComponent<MonsterHealth>().IsDie)
                    {
                        count++;
                    }
                }
            }
            if (count == 0)
            {
                GameObject.Find("Environment/TransparentWall2").SetActive(false);
                m_thirdChapter = true;
                m_chapterCount++;
            }
        }
    }
    private void AppearBoss()
    {
        GameObject boss = Resources.Load<GameObject>("Monsters/Boss");
        boss = Instantiate(boss);
        boss.transform.position = m_bossStartPos.position;
    }
    private bool isRangeCheck = true;
    private void SecondChapterCheck()
    {
        isRangeCheck = true;
    }
    private void GetItem()
    {
        m_getItem.Clear();
        foreach(Item item in m_ItemList)
        {
            int temp = Random.Range(1, 100);
            if(temp <= 10)
            {
                m_getItem.Add(item);
            }
        }
    }
    float showclearUI = 0.0f;
    void Update()
    {
        if (m_firstChapter)
        {
            SpawnMonster(m_spawnPoints_1, "Bee");
            SpawnMonster(m_spawnPoints_2, "Cobra");
            m_firstChapter = false;
        }
        else if(m_secondChapter)
        {
            SpawnMonster(m_spawnPoints_3, "Magma");
            SpawnMonster(m_spawnPoints_4, "Wolf");
            m_secondChapter = false;
            Invoke("SecondChapterCheck", 1.0f);
        }
        else if (m_thirdChapter)
        {
            SpawnMonster(m_spawnPoints_5, "Golem");
            AppearBoss();
            m_thirdChapter = false;
        }
        if(isRangeCheck)
            ChapterRange();

        if (m_player.GetComponent<PlayerHealth>().IsDie)
        {
            m_failUI.SetActive(true);

        }
        else if (isClear)
        {
            showclearUI += Time.deltaTime;
            if (showclearUI >= 2.0f)
            {
                m_clearUI.SetActive(true);
                m_getMoney.text = m_money.ToString();
                Static.s_silver += m_money;
                m_getExp.text = m_exp.ToString();
                m_player.GetComponent<CharController>().SetExp(m_exp);
                GetItem();
                foreach (Item item in m_getItem)
                {
                    GameObject g = Instantiate(Resources.Load<GameObject>("UIPrefabs/AvailableItemSlot")) as GameObject;
                    if (g != null)
                    {
                        g.transform.Find("Image").GetComponent<Image>().sprite = item.itemImage;
                        g.transform.parent = m_avaliableItem.transform;
                        g.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                        Static.s_invenItemList.Add(item);
                        Static.InvenItemSaveList();
                    }
                }

                isClear = false;
            }
        }
    }
}
