using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill1 : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IDragHandler
{
    //public Transform target;
    public GameObject player;
    private Animator m_animator;
    public float angleRange = 45f;
    public float distance = 5f;
    public bool isCollision = false;

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
    Vector2 localPosition = Vector2.zero;


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

    public void OnDrag(PointerEventData eventData)
    {
        localPosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(coolDownImg.rectTransform, eventData.position, eventData.pressEventCamera, out localPosition);
        m_dir = localPosition.normalized;
    }

    private void Play()
    {
        player.transform.rotation = m_quater;
        m_animator.SetTrigger("Skill1");
        foreach (ParticleSystem p in m_particles)
        {
            p.gameObject.SetActive(true);
            p.Play();
        }
        coolDownCheck = true;
        coolDownImg.fillAmount = 1.0f;
    }

    void Start()
    {
        m_animator = player.GetComponent<Animator>();
        Transform t = player.transform.Find("FirstSkillEffect");
        if(t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }
        GameObject b = GameObject.Find("UI/AttackButton/Skill1");
        if(b != null)
        {
            coolDownImg = b.GetComponent<Image>();
            button = b.GetComponent<Button>();
        }
        button.onClick.AddListener(Play);
        maxCoolTime = Database.instance.skills.Find(ex => ex.name == "Skill1").coolTime;
        coolTime = maxCoolTime;
        Transform r = player.transform.Find("FirstSkillRange");
        if (r != null)
            range = r.gameObject;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Locomotion"))
        {
            button.enabled = true;
            //Debug.Log("1");
        }
        else
        {
            button.enabled = false;
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
        if (button.enabled)
        {
            if (isRangeCheck&& !coolDownCheck)
            {
                range.transform.Find("Circle").gameObject.SetActive(true);
                Vector3 dir = new Vector3(m_dir.x, 0, m_dir.y);
                dir.Normalize();
                Vector3 vec = Camera.main.transform.TransformDirection(dir);
                if(vec != Vector3.zero)
                {
                
                    Quaternion q = Quaternion.LookRotation(vec);
                    float y = q.eulerAngles.y;
                    m_quater = Quaternion.Euler(new Vector3(0, y, 0));
                    range.transform.rotation = m_quater;
                }
                else
                    m_quater = range.transform.rotation;
            }
            else
            {
                m_dir = Vector3.zero;
                range.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                
            }
        }
    }
}
