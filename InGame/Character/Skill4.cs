using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Skill4 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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

    private List<ParticleSystem> m_particles = new List<ParticleSystem>();

    private Image m_lockImg;

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
        m_animator.SetTrigger("Skill4");
        foreach (ParticleSystem p in m_particles)
        {
            p.gameObject.SetActive(true);
            p.Play();
        }
        coolDownCheck = true;
        coolDownImg.fillAmount = 1.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_animator = player.GetComponent<Animator>();
        Transform t = player.transform.Find("FourthSkillEffect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }
        GameObject b = GameObject.Find("UI/AttackButton/Skill4");
        if (b != null)
        {
            coolDownImg = b.GetComponent<Image>();
            button = b.GetComponent<Button>();
        }
        button.onClick.AddListener(Play);

        maxCoolTime = Database.instance.skills.Find(ex => ex.name == "Skill4").coolTime;
        coolTime = maxCoolTime;

        Transform r = player.transform.Find("ForthSkillRange");
        if (r != null)
            range = r.gameObject;

        m_lockImg = transform.Find("Image").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animation = m_animator.GetCurrentAnimatorStateInfo(0);
        int level = player.GetComponent<CharController>().GetLevel;
        if (level >= 4)
        {
            m_lockImg.gameObject.SetActive(false);

            if (animation.IsName("Locomotion"))
                button.enabled = true;
            else
                button.enabled = false;

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
