using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Skill2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
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
    private Vector2 m_dir = Vector3.zero;
    private Quaternion m_quater;

    private Image m_lockImg;


    private List<ParticleSystem> m_particles = new List<ParticleSystem>();
    public void OnPointerDown(PointerEventData eventData)
    {
        isRangeCheck = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isRangeCheck = false;
        range.transform.Find("Arrow").gameObject.SetActive(false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(coolDownImg.rectTransform, eventData.position, eventData.pressEventCamera, out localPosition);
        m_dir = localPosition.normalized;
    }
    private void Play()
    {
        player.transform.rotation = m_quater;
        m_animator.SetTrigger("Skill2");
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
        Transform t = player.transform.Find("SecondSkillEffect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }
        GameObject b = GameObject.Find("UI/AttackButton/Skill2");
        if (b != null)
        {
            coolDownImg = b.GetComponent<Image>();
            button = b.GetComponent<Button>();
        }
        button.onClick.AddListener(Play);

        maxCoolTime = Database.instance.skills.Find(ex => ex.name == "Skill2").coolTime;
        coolTime = maxCoolTime;

        Transform r = player.transform.Find("SecondSkillRange");
        if (r != null)
            range = r.gameObject;

        m_lockImg = transform.Find("Image").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        int level = player.GetComponent<CharController>().GetLevel;
        if (level >= 2)
        {
            m_lockImg.gameObject.SetActive(false);
            if (stateInfo.IsName("Locomotion"))
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
            if (button.enabled)
            {
                if (isRangeCheck && !coolDownCheck)
                {
                    coolDownImg.fillAmount = 0.0f;
                    range.transform.Find("Arrow").gameObject.SetActive(true);
                    Vector3 dir = new Vector3(m_dir.x, 0, m_dir.y);
                    dir.Normalize();
                    Vector3 vec = Camera.main.transform.TransformDirection(dir);
                    if (vec != Vector3.zero)
                    {
                        Quaternion q = Quaternion.LookRotation(vec);
                        float y = q.eulerAngles.y;
                        m_quater = Quaternion.Euler(new Vector3(0, y, 0));
                        range.transform.rotation = m_quater;
                    }
                    else
                    {
                        m_quater = range.transform.rotation;
                    }
                }
                else
                {
                    m_dir = Vector3.zero;
                    range.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                }
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
