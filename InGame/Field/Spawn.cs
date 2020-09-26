using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawn : MonoBehaviour
{
    private MonsterController m_mon;

    private float m_spawnTime = 0.0f;
    private GameObject m_centerPos;
    private List<Item> m_itemList = new List<Item>();
    void Start()
    {
        TextAsset txtFile = Resources.Load("Files/FieldItem") as TextAsset;
        string test = txtFile.text;
        string[] line2 = test.Split('\n');
        int i = 0;
        while (true)
        {
            if (i >= line2.Length-1)
                break;
            string[] info = line2[i].Split(',');
            Item item = Database.instance.items.Find(ex => ex.itemName == info[0]);
            if (item != null && !m_itemList.Contains(item))
            {
                m_itemList.Add(item);
            }
            ++i;
        }

        m_centerPos = GameObject.Find("Center");
    }

    // Update is called once per frame
    void Update()
    {
        m_mon = transform.GetComponentInChildren<MonsterController>(true);
        if (m_mon != null)
        {
            m_mon.isFieldMon = true;
            foreach (Item item in m_itemList)
            {
                m_mon.SetItem(item);
            }



            if (!m_mon.isActiveAndEnabled)
            {
                m_spawnTime += Time.deltaTime;
                if (m_spawnTime >= 5.0f)
                {
                    m_mon.gameObject.transform.position = gameObject.transform.position;
                    m_mon.gameObject.transform.LookAt(m_centerPos.transform);
                    m_mon.curState = MonsterController.CurrentState.idle;
                    m_mon.GetComponent<Animator>().speed = 1;
                    m_mon.gameObject.SetActive(true);
                    m_mon.Coroutine();
                    m_spawnTime = 0.0f;
                }
            }
        }
    }
}
