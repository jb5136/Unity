using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class AttackBtn : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public GameObject player;
    private Joystick m_joyStick;
    private bool isAttackBtn = false;
    AnimatorStateInfo stateInfo;
    private Animator m_animator;
    private int m_comboCount = 0;
    private float m_attackRange;
    private NavMeshAgent nvAgent;

    //주변 몬스터 찾기

    private Collider m_collider;
    
    //Slash Effect
    private List<ParticleSystem> m_particles = new List<ParticleSystem>();
    public void OnPointerDown(PointerEventData eventData)
    {
            if (!m_joyStick.GetmoveCheck)
            isAttackBtn = true;
        if (m_collider == null)
        {
            m_collider = player.GetComponent<CharController>().FIndTarget();
            player.GetComponent<CharController>().GetCollider = m_collider;
        }
        else
        {
            Collider temp = player.GetComponent<CharController>().FIndTarget();

            if (m_collider != temp)
            {
                m_collider.transform.Find("MarkerPivot/Marker.Crosshair").gameObject.SetActive(false);
                //m_collider = temp;
            }
            player.GetComponent<CharController>().GetCollider = m_collider;
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
       isAttackBtn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = player.GetComponent<Animator>();
        m_joyStick = GameObject.FindObjectOfType<Joystick>();
        m_collider = null;
        m_attackRange = 4.0f;
        nvAgent = player.GetComponent<NavMeshAgent>();
        Transform t = player.transform.Find("SlashEffect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }

    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if(m_collider != null)
        {
            float dis = Vector3.Distance(player.transform.position, m_collider.transform.position);
            if (dis > 10.0f)
            {
                m_collider = null;
                player.GetComponent<CharController>().GetCollider = null;
            }
        }

        if (stateInfo.IsName("Locomotion"))
        {
            nvAgent.enabled = true;
        }
        if (isAttackBtn)
        {
            if (m_collider == null)
                m_collider = player.GetComponent<CharController>().FIndTarget();
            else
            {
                Collider temp = player.GetComponent<CharController>().FIndTarget();

                if (m_collider != temp && m_collider.tag != "Boss")
                {
                    m_collider.transform.Find("MarkerPivot/Marker.Crosshair").gameObject.SetActive(false);
                    //m_collider = temp;
                }
            }

            if (m_collider == null || m_collider.GetComponent<MonsterHealth>().IsDie)
            {
                m_collider = null;
                nvAgent.enabled = true;
            }
            else if (m_collider != null)
            {
                player.GetComponent<CharController>().GetCollider =m_collider;
                float attackDis = Vector3.Distance(player.transform.position, m_collider.transform.position);
                if(m_collider.tag == "Monster")
                    m_collider.transform.Find("MarkerPivot/Marker.Crosshair").gameObject.SetActive(true);

                if (attackDis > m_attackRange)
                {
                    if (stateInfo.IsName("Locomotion"))
                        player.GetComponent<CharController>().AttackMove(m_collider.gameObject);
                }
                else
                {
                    player.transform.LookAt(m_collider.gameObject.transform);
                    if (stateInfo.IsName("Locomotion"))
                    {
                        nvAgent.enabled = true;
                        m_comboCount = 1;
                        m_animator.SetInteger("Combo", m_comboCount);
                        m_particles[m_comboCount - 1].gameObject.SetActive(true);
                        m_particles[m_comboCount - 1].Play();
                    }
                    else
                    {
                        float currentRatio = stateInfo.normalizedTime * stateInfo.length;

                        if (stateInfo.IsName("Combo1") || stateInfo.IsName("Combo2") || stateInfo.IsName("Combo3") || stateInfo.IsName("Combo4"))
                            nvAgent.enabled = false;

                        if ((stateInfo.IsName("Combo3") || (stateInfo.IsName("Combo4") && currentRatio <= stateInfo.length * 0.6f)) && m_collider.tag == "Monster")
                        {
                            player.GetComponent<CharacterController>().Move(player.transform.forward * Time.deltaTime * 2);
                        }

                        //Debug.Log("현재 노멀라이즈타임 : " + stateInfo.normalizedTime);
                        //Debug.Log("현재 비율 : " + currentRatio);
                        //Debug.Log("애니메이션 길이 : " + stateInfo.length);

                        // 현재 실행되고 있는 비율값이 애니메이션 길이 값의 95% 정도가 수행되었을 때
                        // 키 입력을 받았다면 다른 애니메이션이 출력되도록 처리합니다.
                        if (currentRatio >= stateInfo.length * 0.90f)
                        {
                            if (m_animator.IsInTransition(0))
                                return;

                            ++m_comboCount;
                            m_animator.SetInteger("Combo", m_comboCount);
                            m_particles[m_comboCount - 1].gameObject.SetActive(true);
                            m_particles[m_comboCount - 1].Play();
                        }
                    }
                }
            }
            else
                m_collider = null;
        }
        else
            nvAgent.enabled = true;
    }
}

public class Tex
{
    public string texturename;
    public Vector2 uv;
}

