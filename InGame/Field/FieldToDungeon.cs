using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class FieldToDungeon : MonoBehaviour
{
    GameObject SceneMoveCheckObj;
    Button yes;
    Button no;
    GameObject m_camera;
    private GameObject m_player;

    private List<Item> m_dungeonItemList = new List<Item>();
    private int m_dungeonLimitLevel = 5;
    private Text m_dungeonlevelText;
    private GameObject m_dungeonAvailableItem;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = GameObject.Find("Main Camera");

        m_player = GameObject.FindGameObjectWithTag("Player");
        GameObject g = GameObject.Find("FCanvas");
        SceneMoveCheckObj = g.transform.Find("SceneMoveCheck").gameObject;
        m_dungeonlevelText = SceneMoveCheckObj.transform.Find("LevelLimit/Text").GetComponent<Text>();
        m_dungeonAvailableItem = SceneMoveCheckObj.transform.Find("AvailableItem").gameObject;
        yes = SceneMoveCheckObj.transform.Find("Yes").GetComponent<Button>();
        no = SceneMoveCheckObj.transform.Find("No").GetComponent<Button>();

        m_dungeonItemList.Add(Database.instance.items.Find(ex => ex.itemName == "Sword3"));
        m_dungeonItemList.Add(Database.instance.items.Find(ex => ex.itemName == "Helmet3"));
        m_dungeonItemList.Add(Database.instance.items.Find(ex => ex.itemName == "Armor3"));

        yes.onClick.AddListener(Yes);
        no.onClick.AddListener(No);

        foreach (Item item in m_dungeonItemList)
        {
            GameObject g2 = Instantiate(Resources.Load<GameObject>("UIPrefabs/AvailableItemSlot")) as GameObject;
            if (g2 != null)
            {
                g2.transform.Find("Image").GetComponent<Image>().sprite = item.itemImage;
                //g.transform.parent = m_dungeonAvailableItem.transform;
                g2.transform.SetParent(m_dungeonAvailableItem.transform);
                g2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }
    private void Yes()
    {
        if (m_player.GetComponent<CharController>().GetLevel >= m_dungeonLimitLevel)
        {
            LoadingSceneMng.LoadScene("Dungeon");
            //m_camera.SetActive(false);
            SceneMoveCheckObj.SetActive(false);
            GameObject g = GameObject.Find("Canvas/UI");
            m_camera.GetComponent<MainCamera>().isSceneMove = true;
            m_camera.transform.position = new Vector3(-14, 4, 5);
            g.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {

        }
    }
    private void No()
    {
        SceneMoveCheckObj.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneMoveCheckObj.SetActive(true);
            m_dungeonlevelText.text = m_dungeonLimitLevel.ToString();            
        }

        //LoadingSceneMng.LoadScene("play");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
