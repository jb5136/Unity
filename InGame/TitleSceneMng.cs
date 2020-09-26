using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TitleSceneMng : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Canvas/UI").SetActive(false);
        GameObject.Find("Canvas/HpCanvas").SetActive(false);
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            GameObject.Find("Canvas").transform.Find("UI").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("HpCanvas").gameObject.SetActive(true);
            player.SetActive(true);
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<NavMeshAgent>().enabled = false;
            LoadingSceneMng.LoadScene("Village");
        }
    }
}
