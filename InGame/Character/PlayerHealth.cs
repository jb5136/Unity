using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Animator m_animator;
    public bool hitcheck = false;

    private float m_orihp = 0;
    private float m_hp = 30;
    private Slider m_CharSlider;
    private Text m_charText;
    private Slider m_slider;
    private Transform m_hpPivot;
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
        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Locomotion") && !m_animator.IsInTransition(0))
        {
            m_animator.SetTrigger("Hit");
        }
        float state =  GetComponent<CharController>().GetTotalDef;
        hitdamege = (int)(hitdamege - state/1.5f);
        if (hitdamege > 0)
            m_hp -= hitdamege;
        else
        {
            m_hp -= 20.0f;
        }
        
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
        m_animator = GetComponent<Animator>();

        Transform parent = UtilHelper.FindObjectWithTag<Transform>("HpCanvas");
        m_hpPivot = transform.Find("HpPivot");
        m_slider = UtilHelper.Instantiate<Slider>("UIPrefabs/HPSlider",Vector3.zero, parent, Quaternion.identity);
        m_orihp = m_hp;
        GameObject t = GameObject.Find("Canvas/CharacterUI/HealthBar");
        if(t != null)
        {
            Transform temp = t.transform.Find("Text");
            if(temp != null)
            {
                m_charText = temp.GetComponent<Text>();
            }

        }
        Transform p = transform.Find("HeallingEffect");
        if (p != null)
        {
            ParticleSystem[] ps = p.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HealthBar();
        if (m_slider != null)
            m_slider.transform.position = Camera.main.WorldToScreenPoint(m_hpPivot.position);
        if (hitcheck)
        {
            AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Locomotion"))
            {
                m_animator.SetTrigger("Hit");
            }
            hitcheck = false;
        }
    }
}
