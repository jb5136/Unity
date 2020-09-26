using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercenaryHealth : MonoBehaviour
{
    private float m_orihp = 0;
    private float m_hp = 30;
    private Slider m_CharSlider;
    private Text m_charText;
    public Slider m_slider;
    private Transform m_hpPivot;
    public string name;
    private CharController m_player;
    private List<ParticleSystem> m_particles = new List<ParticleSystem>();
    //public bool IsDie { get { return m_hp <= 0 ? true : false; } }
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
    public float GetOriHp { get { return m_orihp; } set { m_orihp = value; } }

    public void Heal(float heal)
    {
        foreach (ParticleSystem p in m_particles)
        {
            p.gameObject.SetActive(true);
            p.Play();
        }
        m_hp += heal;
        if (m_hp >= m_orihp)
            m_hp = m_orihp;
    }
    public void DecreaseHp(float hitdamege)
    {
        float state =  m_player.GetTotalDef * 0.8f;
        hitdamege = hitdamege - state/2;
        if (hitdamege >= 0)
            m_hp -= hitdamege;
        
        HealthBar();

        if (m_hp <= 0)
        {
            if (m_slider != null)
                Destroy(m_slider.gameObject);
        }
    }
    private void HealthBar()
    {
        float ratio = m_hp / m_orihp;

        if (m_slider != null)
            m_slider.value = ratio;
    }
    public void Init()
    {
        m_orihp = m_hp;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
        GameObject t = GameObject.Find("Canvas/CharacterUI/HealthBar");
        if(t != null)
        {
            Transform temp = t.transform.Find("Text");
            if(temp != null)
            {
                m_charText = temp.GetComponent<Text>();
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_slider != null)
        {
            m_slider.gameObject.SetActive(true);
            m_slider.transform.Find("Text").GetComponent<Text>().text = name;
        }
        HealthBar();
    }
}
