using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMng : MonoBehaviour
{
    public static CanvasMng instance;
    private Text m_silver;
    private Text m_gold;
    private Text m_diamond;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_silver = transform.Find("InvenCanvas/InventoryCanvas/Money/Silver/Text").GetComponent<Text>();
        m_gold = transform.Find("InvenCanvas/InventoryCanvas/Money/Gold/Text").GetComponent<Text>();
        m_diamond = transform.Find("InvenCanvas/InventoryCanvas/Money/Diamond/Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        m_silver.text = Static.s_silver.ToString();
        m_gold.text = Static.s_gold.ToString();
        m_diamond.text = Static.s_diamond.ToString();
    }
}
