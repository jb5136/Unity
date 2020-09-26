using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Skill3 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject player;
    private Animator m_animator;

    //쿨타임 관련
    private bool coolDownCheck = false;
    private Image coolDownImg;
    private float coolTime;
    private float maxCoolTime = 4.0f;
    private Button button;

    //스킬범위 관련
    private bool isRangeCheck = false;
    private GameObject range;
    private Image m_lockImg;

    private List<ParticleSystem> m_particles = new List<ParticleSystem>();

    public void OnPointerDown(PointerEventData eventData)
    {
        isRangeCheck = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isRangeCheck = false;
        range.transform.Find("Circle").gameObject.SetActive(false);
    }

    private void Play()
    {
        m_animator.ResetTrigger("Skill3Finsh");
        player.GetComponent<CharController>().skillState = true;
        m_animator.SetTrigger("Skill3");
        count = 0;
        Invoke("EndSkill", 3.0f);
        coolDownCheck = true;
        coolDownImg.fillAmount = 1.0f;
    }
    private void EndSkill()
    {
        player.GetComponent<CharController>().skillState = false;
        m_animator.SetTrigger("Skill3Finsh");
    }
    // Start is called before the first frame update
    void Start()
    {
        m_animator = player.GetComponent<Animator>();
        Transform t = player.transform.Find("ThirdSkillEffect");
        if (t != null)
        {
            m_particles.Clear();
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }
        GameObject b = GameObject.Find("UI/AttackButton/Skill3");
        if (b != null)
        {
            coolDownImg = b.GetComponent<Image>();
            button = b.GetComponent<Button>();
        }
        button.onClick.AddListener(Play);

        maxCoolTime = Database.instance.skills.Find(ex => ex.name == "Skill3").coolTime;
        coolTime = maxCoolTime;
        Transform r = player.transform.Find("ThirdSkillRange");
        if (r != null)
            range = r.gameObject;

        m_lockImg = transform.Find("Image").GetComponent<Image>();
    }

    private int count = 0;
    void Update()
    {
        AnimatorStateInfo animation = m_animator.GetCurrentAnimatorStateInfo(0);
        int level = player.GetComponent<CharController>().GetLevel;
        if (level >= 3)
        {
            m_lockImg.gameObject.SetActive(false);
            if (animation.IsName("Locomotion"))
                button.enabled = true;
            else
                button.enabled = false;

            if (animation.IsName("Skill3Two"))
            {
                if (count == 0)
                {
                    foreach (ParticleSystem p in m_particles)
                    {
                        p.gameObject.SetActive(true);
                        p.Play();
                    }
                    count++;
                }
            }
            else
            {
                foreach (ParticleSystem p in m_particles)
                {
                    p.gameObject.SetActive(false);
                }
            }
            if (coolDownCheck)
            {
                button.enabled = false;
                coolTime -= Time.deltaTime;
                Text text = coolDownImg.transform.GetComponentInChildren<Text>();
                int timeText = (int)coolTime + 1;
                text.text = timeText.ToString();
                coolDownImg.fillAmount = coolTime / maxCoolTime;
                if (coolTime <= 0.0f)
                {
                    coolDownImg.fillAmount = 0.0f;
                    coolDownCheck = false;
                    button.enabled = true;
                    coolTime = maxCoolTime;
                    text.text = "";
                }
            }
            else
            {
                coolDownImg.fillAmount = 0.0f;
            }

            if (button.enabled && isRangeCheck)
            {
                range.transform.Find("Circle").gameObject.SetActive(true);

            }
        }
        else
        {
            coolDownImg.fillAmount = 1;
            m_lockImg.gameObject.SetActive(true);
            button.enabled = false;
        }
    }
}
