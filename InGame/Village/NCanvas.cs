using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NCanvas : MonoBehaviour
{
    private GameObject m_player;
    private GameObject m_UIcanvas;
    private GameObject m_shopUI;
    private Button m_closeShop;
    private Button m_inShopBtn;
    private Button m_closeBtn;
    private GameObject m_TextPivot;
    private GameObject m_ShowItem;
    //npc
    private GameObject m_consumptionNPC;
    public Text m_consumptionText;

    private Text m_silver;
    private Text m_gold;
    private Text m_diamond;
    //shop
    private GameObject m_sell;
    private GameObject m_buy;
    private Button m_sellBtn;
    private Button m_buyBtn;

    private Transform m_whetherPivot;
    private Button testBtn;

    private GameObject m_miniMap;
    // Start is called before the first frame update
    void Start()
    {
        testBtn = transform.Find("Button").GetComponent<Button>();
        testBtn.onClick.AddListener(TestBtn);
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_whetherPivot = transform.Find("TextPivot").transform;
        m_UIcanvas = GameObject.Find("Canvas").transform.Find("UI").gameObject;
        m_shopUI = transform.Find("Consumption").gameObject;
        m_sell = m_shopUI.transform.Find("Sell").gameObject;
        m_buy = m_shopUI.transform.Find("Buy").gameObject;
        m_sell.SetActive(true);
        m_buy.SetActive(false);
        m_sellBtn = m_shopUI.transform.Find("SellBtn").GetComponent<Button>();
        m_buyBtn = m_shopUI.transform.Find("BuyBtn").GetComponent<Button>();
        m_sellBtn.onClick.AddListener(Sell);
        m_buyBtn.onClick.AddListener(Buy);
        m_closeShop = transform.Find("Consumption/CloseBtn").GetComponent<Button>();
        m_consumptionNPC = GameObject.Find("ConsumptionNPC");
        m_ShowItem = transform.Find("Consumption/Sell/ShowItem").gameObject;
        if (m_closeShop != null)
            m_closeShop.onClick.AddListener(CloseShop);
        m_TextPivot = m_consumptionNPC.transform.Find("TextPivot").gameObject;
        m_inShopBtn = transform.Find("Shop/ShopBtn").GetComponent<Button>();
        m_closeBtn = transform.Find("Shop/CloseBtn").GetComponent<Button>();
        m_inShopBtn.onClick.AddListener(Shop);
        m_closeBtn.onClick.AddListener(Close);

        m_silver = transform.Find("Consumption/Money/Silver/Text").GetComponent<Text>();
        m_gold = transform.Find("Consumption/Money/Gold/Text").GetComponent<Text>();
        m_diamond = transform.Find("Consumption/Money/Diamond/Text").GetComponent<Text>();

        GameObject.Find("Main Camera").transform.position = new Vector3(58.0f, 4.0f, 30.0f);
        m_miniMap = transform.Find("MiniMap").gameObject;
    }

    private void TestBtn()
    {
        if(m_UIcanvas.activeSelf)
            m_UIcanvas.SetActive(false);
        else
        {
            m_UIcanvas.SetActive(true);

        }
    }
    private void Sell()
    {
        m_sell.SetActive(true);
        m_buy.SetActive(false);
    }
    private void Buy()
    {
        m_sell.SetActive(false);
        m_buy.SetActive(true);
    }
    private void CloseShop()
    {
        m_ShowItem.SetActive(false);
        m_shopUI.SetActive(false);
        m_UIcanvas.SetActive(true);
        m_miniMap.SetActive(true);
    }
    private void Shop()
    {
        transform.Find("Shop").gameObject.SetActive(false);
        m_UIcanvas.gameObject.SetActive(false);
        m_shopUI.SetActive(true);
        m_miniMap.SetActive(false);
    }
    private void Close()
    {
        transform.Find("Shop").gameObject.SetActive(false);
        m_miniMap.SetActive(true);
        m_UIcanvas.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitobj;
            if (Physics.Raycast(ray, out hitobj, 100))
            {
                if (hitobj.transform.tag == "NPC" && !m_shopUI.activeSelf)
                {
                    if(Vector3.Distance(hitobj.transform.position , m_player.transform.position) < 5.0f)
                    {
                        m_UIcanvas.gameObject.SetActive(false);
                        transform.Find("Shop").gameObject.SetActive(true);
                        m_miniMap.SetActive(false);
                    }
                    else
                    {
                        GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
                        var temp = Instantiate(whether, m_whetherPivot);
                        temp.transform.Find("Text").GetComponent<Text>().text = "너무 멀리 있습니다";
                        temp.transform.position = m_whetherPivot.transform.position;
                    }
                        
                }
            }
        }
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Collider collider = m_consumptionNPC.GetComponent<Collider>();
        if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
        {
            m_consumptionText.gameObject.SetActive(true);
            m_consumptionText.transform.position = Camera.main.WorldToScreenPoint(m_TextPivot.transform.position);
        }
        else
        {
            m_consumptionText.gameObject.SetActive(false);
        }

        //money
        m_silver.text = Static.s_silver.ToString();
        m_gold.text = Static.s_gold.ToString();
        m_diamond.text = Static.s_diamond.ToString();

        if (!m_sell.activeSelf)
        {
            Color color = m_sellBtn.GetComponent<Image>().color;
            color.a = 0.5f;
            m_sellBtn.GetComponent<Image>().color = color;
        }
        else
        {
            Color color = m_sellBtn.GetComponent<Image>().color;
            color.a = 1.0f;
            m_sellBtn.GetComponent<Image>().color = color;
        }

        if (!m_buy.activeSelf)
        {
            Color color = m_buyBtn.GetComponent<Image>().color;
            color.a = 0.5f;
            m_buyBtn.GetComponent<Image>().color = color;
        }
        else
        {
            Color color = m_buyBtn.GetComponent<Image>().color;
            color.a = 1.0f;
            m_buyBtn.GetComponent<Image>().color = color;
        }
    }
}
