using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterHealth : MonoBehaviour
{
    private float m_orihp = 0;
    private float m_hp = 30;
    public Slider m_slider;
    private Transform m_hpPivot;


    private Transform canvas;
    private Transform textPivot;
    private Material m_material;
    private GameObject m_target;
    public bool IsDie
    {
        get
        {
            if (m_hp <= 0)
                return true;
            return false;
        }
    }   

    
    public float GetHp { get { return m_hp; } set { m_hp = value; } }
    public float GetOriHp { get { return m_orihp; } }
    public GameObject GetTarget { set { m_target = value; } }
    public void SetActiveHp( bool state )
    {
        if (m_slider == null)
            return;
        m_slider.gameObject.SetActive(state);
    }
    private void Test()
    {
        if(gameObject.tag == "Monster")
            transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material = m_material;
    }
    float def = 0.0f;
    public void DecreaseHp(float damaged,bool criCheck,GameObject gameObject)
    {
        if (!IsDie)
        {
            if(gameObject.tag == "Player" && m_material != null)
            {
                Material temp = m_material;
                transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material = Resources.Load<Material>("Materials/Test");
                Invoke("Test", 0.1f);
            }
            
            if (tag == "Monster")
                def = GetComponent<MonsterController>().GetDef;
            else if (tag == "Boss")
            {
                def = GetComponent<Boss>().GetDef;
            }

            if (criCheck)
            {
                damaged = damaged - def;
                damaged = (int)(damaged * 2);
                if (damaged <= 0)
                    damaged = 20f;

                var textClone = UtilHelper.Instantiate<Text>("UIPrefabs/CriFloatingText", Vector3.zero, canvas, Quaternion.identity);
                textClone.transform.position = Camera.main.WorldToScreenPoint(textPivot.transform.position);
                Vector3 vec = new Vector3(textPivot.transform.position.x + 1, textPivot.transform.position.y, textPivot.transform.position.z + 1);
                textClone.GetComponent<FloatingText>().SetEndPos(Camera.main.WorldToScreenPoint(vec));
                textClone.GetComponent<FloatingText>().text.text = damaged.ToString();
            }
            else
            {
                damaged =(int)(damaged - def);
                if (damaged <= 0)
                    damaged = 20f;
                var textClone = UtilHelper.Instantiate<Text>("UIPrefabs/FloatingText", Vector3.zero, canvas, Quaternion.identity);
                textClone.transform.position = Camera.main.WorldToScreenPoint(textPivot.transform.position);
                Vector3 vec = new Vector3(textPivot.transform.position.x + 1, textPivot.transform.position.y, textPivot.transform.position.z + 1);
                textClone.GetComponent<FloatingText>().SetEndPos(Camera.main.WorldToScreenPoint(vec));
                textClone.GetComponent<FloatingText>().text.text = damaged.ToString();
            }
            m_hp -= damaged;
            float ratio = m_hp / m_orihp;

            if (m_slider != null)
                m_slider.value = ratio;

            if (m_hp <= 0)
            {
                if (m_slider != null)
                    Destroy(m_slider.gameObject);
            }
        }
    }

    public void Init()
    {
        Transform parent = UtilHelper.FindObjectWithTag<Transform>("HpCanvas");
        m_hpPivot = transform.Find("HpPivot");
        m_slider = null;
        if(tag == "Monster")
        {
            m_slider = UtilHelper.Instantiate<Slider>("UIPrefabs/HPSlider",Vector3.zero, parent, Quaternion.identity);
            float ratio = m_hp / m_orihp;
            m_slider.value = ratio;
        }
        
        m_orihp = m_hp;
        canvas = UtilHelper.FindObjectWithTag<Transform>("Canvas");
        textPivot = transform.Find("RigPelvis");
        if(gameObject.tag == "Monster")
            m_material = transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_slider != null && tag == "Monster" )
        {
            if (m_slider.gameObject.activeSelf&& m_target != null && m_target.GetComponent<PlayerHealth>().IsDie)
            {
                m_slider.gameObject.SetActive(false);
            }
            else if (m_hp < m_orihp)
            {
                m_slider.gameObject.SetActive(true);
                m_slider.transform.position = Camera.main.WorldToScreenPoint(m_hpPivot.position);
            }
            else
            {
                m_slider.gameObject.SetActive(false);

            }
        }
    }
}
