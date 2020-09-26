using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;
public class FieldScene : MonoBehaviour
{
    private Transform[] m_spawnPoints_1;
    private Transform[] m_spawnPoints_2;
    private Transform[] m_spawnPoints_3;
    private Transform[] m_spawnPoints_4;
    private Transform[] m_spawnPoints_5;
    private Transform[] m_spawnPoints_6;
    float createTime = 10.0f;
    private GameObject m_player;
    //Mercenary
    private GameObject m_mercenaryObj;
    private List<GameObject> m_mercenary = new List<GameObject>();
    private List<Slider> m_mercenaryHpSlider = new List<Slider>();

    Image mini;

    private GameObject m_centerPos;
    private void Awake()
    {
        GameObject m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (m_camera != null)
        {
            m_camera.GetComponent<MainCamera>().offset = new Vector3(5, 4, 1);
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
        //GameObject.Find("Canvas").transform.Find("UI").gameObject.SetActive(true);

        //Debug.Break();
        //Debug.Log("player : " + m_player.transform.position);
        //Debug.Log("StartPos : " + GameObject.Find("StartPos").transform.position);
        //monster 생성 및 스폰
        GameObject.Find("Main Camera").GetComponent<MainCamera>().isSceneMove = false;
        m_centerPos = GameObject.Find("Center");

        m_spawnPoints_1 = GameObject.Find("SpawnPoint/SpawnPoints1").GetComponentsInChildren<Transform>();
        m_spawnPoints_2 = GameObject.Find("SpawnPoint/SpawnPoints2").GetComponentsInChildren<Transform>();
        m_spawnPoints_3 = GameObject.Find("SpawnPoint/SpawnPoints3").GetComponentsInChildren<Transform>();
        m_spawnPoints_4 = GameObject.Find("SpawnPoint/SpawnPoints4").GetComponentsInChildren<Transform>();
        m_spawnPoints_5 = GameObject.Find("SpawnPoint/SpawnPoints5").GetComponentsInChildren<Transform>();
        m_spawnPoints_6 = GameObject.Find("SpawnPoint/SpawnPoints6").GetComponentsInChildren<Transform>();

        m_mercenaryObj = GameObject.Find("Mercenary");
        //mercenary
        Slider[] mercenaryHpSlider = GameObject.Find("Canvas/UI").transform.Find("MercenaryHp").GetComponentsInChildren<Slider>(true);
        foreach (Slider si in mercenaryHpSlider)
        {
            m_mercenaryHpSlider.Add(si);
        }
        Spawn();
        mini = GameObject.Find("FCanvas/MiniMap").GetComponent<Image>();
        mini.material.mainTexture = Resources.Load<RenderTexture>("MiniMap/MiniMap");
    }
    private void SetMercenary(string name,int i)
    {
        GameObject g = Resources.Load<GameObject>("Mercenary/" + name);
        //GameObject mercenary = null;
        if (g != null)
        {
            GameObject mercenary = Instantiate(g);

            //mercenary.transform.position = GameObject.Find("Mercenary").transform.position;
            mercenary.transform.parent = GameObject.Find("Mercenary").transform;


            mercenary.GetComponent<MercenaryHealth>().m_slider = m_mercenaryHpSlider[i];
            mercenary.GetComponent<MercenaryHealth>().name = name;
            mercenary.transform.position = m_player.transform.position + m_player.transform.forward * -i;
            mercenary.GetComponent<NavMeshAgent>().enabled = true;
            m_mercenary.Add(mercenary);
        }
    }

    //private void SetMercenary()
    //{
    //    TextAsset txtFile = Resources.Load("Files/Mercenary") as TextAsset;
    //    string text = txtFile.text;
    //    string[] line2 = text.Split('\n');
    //    int i = 2;
    //    int j = 0;
    //    while (true)
    //    {
    //        if (j >= line2.Length-1)
    //            break;
    //        string line = line2[j+1];
    //        string[] info = line.Split(',');
    //        if (int.Parse(info[0]) == 1)
    //        {
    //            GameObject g = Resources.Load<GameObject>("Mercenary/" + info[1]);
    //            //GameObject mercenary = null;
    //            if (g != null)
    //            {
    //                GameObject mercenary = Instantiate(g);

    //                //mercenary.transform.position = GameObject.Find("Mercenary").transform.position;
    //                mercenary.transform.parent = GameObject.Find("Mercenary").transform;


    //                mercenary.GetComponent<MercenaryHealth>().m_slider = m_mercenaryHpSlider[j];
    //                mercenary.GetComponent<MercenaryHealth>().name = info[1];
    //                mercenary.transform.position = m_player.transform.position + m_player.transform.forward * -i;
    //                mercenary.GetComponent<NavMeshAgent>().enabled = true;
    //                m_mercenary.Add(mercenary);
    //            }
    //            else
    //            {
    //                Debug.Log("1");
    //            }
    //        }
    //            i--;
    //            j++;
    //    }
    //    //sr.Close();
    //}
    void Spawn()
    {
        SpawnMonster(m_spawnPoints_1, "Wolf");
        SpawnMonster(m_spawnPoints_2, "Bee");
        SpawnMonster(m_spawnPoints_3, "Cobra");
        SpawnMonster(m_spawnPoints_4, "Magma");
        SpawnMonster(m_spawnPoints_5, "Treant");
        SpawnMonster(m_spawnPoints_6, "Golem");
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
                monster.gameObject.transform.LookAt(m_centerPos.transform);

                Monster monInfo = Database.instance.monsters.Find(ex => ex.monName == name);
                monster.GetComponent<MonsterController>().SetMonsterInfo = monInfo;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            LoadingSceneMng.LoadScene("Village");
            GameObject g = GameObject.Find("Canvas/UI");
            g.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
