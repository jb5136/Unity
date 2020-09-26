using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class FieldToVillage : MonoBehaviour
{
    GameObject SceneMoveCheckObj;
    Button yes;
    Button no;
    GameObject m_camera;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = GameObject.Find("Main Camera");

        GameObject g = GameObject.Find("Canvas").transform.Find("UI").gameObject;
        SceneMoveCheckObj = g.transform.Find("SceneMoveCheck").gameObject;
        yes = SceneMoveCheckObj.transform.Find("Yes").GetComponent<Button>();
        no = SceneMoveCheckObj.transform.Find("No").GetComponent<Button>();

        yes.onClick.AddListener(Yes);
        no.onClick.AddListener(No);
    }
    private void Yes()
    {
        LoadingSceneMng.LoadScene("Village");
        //m_camera.SetActive(false);
        SceneMoveCheckObj.SetActive(false);
        GameObject g = GameObject.Find("Canvas/UI");
        m_camera.GetComponent<MainCamera>().isSceneMove = true;
        m_camera.transform.position = new Vector3(58, 4, 30);
        g.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().enabled = false;
    }
    private void No()
    {
        SceneMoveCheckObj.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            SceneMoveCheckObj.SetActive(true);

        //LoadingSceneMng.LoadScene("play");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
