using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.IO;

public class VillageSceneMng : MonoBehaviour
{
    private GameObject m_player;
    private GameObject m_UIcanvas;
    Image mini;
    void Awake()
    {
        Slider[] mon = GameObject.Find("Canvas/HpCanvas").GetComponentsInChildren<Slider>(true);
        foreach (Slider s in mon)
        {
            Destroy(s.gameObject);
        }
        GameObject m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (m_camera != null)
        {
            m_camera.GetComponent<MainCamera>().offset = new Vector3(0, 4, -6);
        }
    }
    void Start()
    {
        GameObject.Find("Main Camera").GetComponent<MainCamera>().isSceneMove = false;


        m_player = GameObject.FindGameObjectWithTag("Player");
        m_UIcanvas = GameObject.Find("Canvas").transform.Find("UI").gameObject;
        m_UIcanvas.SetActive(true);
        m_player.transform.position = GameObject.Find("StartPos").transform.position;
        m_player.GetComponent<CharacterController>().enabled = true;

        mini = GameObject.Find("Canvas/UI/MiniMap").transform.Find("VillageM").GetComponent<Image>();
        Material rt = Resources.Load<Material>("Materials/MiniMap_m");
        mini.material = rt;
        
    }
    

    // Update is called once per frame
    void Update()
    {
       
    }
}
