using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{
    private Button m_invenBtn;
    private Button m_charInfoBtn;
    private Button m_closeBtn;

    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvas != null)
        {
            m_invenBtn = canvas.transform.Find("Btn/InventoryBtn").GetComponent<Button>();
            m_charInfoBtn = canvas.transform.Find("Btn/CharInfoBtn").GetComponent<Button>();
            m_closeBtn = canvas.transform.Find("CloseBtn").GetComponent<Button>();
        }
        if (m_invenBtn != null)
            m_invenBtn.onClick.AddListener(ClickInven);
        if (m_charInfoBtn != null)
            m_charInfoBtn.onClick.AddListener(ClickChar);
        if (m_closeBtn != null)
            m_closeBtn.onClick.AddListener(CloseBtn);
    }

    private void ClickInven()
    {
        canvas.transform.Find("InventoryCanvas").gameObject.SetActive(true);
        canvas.transform.Find("CharInfoCanvas").gameObject.SetActive(false);
        Static.SaveList();
    }
    private void ClickChar()
    {
        canvas.transform.Find("CharInfoCanvas").gameObject.SetActive(true);
        canvas.transform.Find("InventoryCanvas").gameObject.SetActive(false);
        Static.SaveList();
    }
    private void CloseBtn()
    {
        Static.SaveList();
        SceneManager.LoadSceneAsync("Play");
    }

    private void SaveFile()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
