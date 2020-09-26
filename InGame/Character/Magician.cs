using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using UnityEngine.UI;

public class Magician : MonoBehaviour
{
    private GameObject m_player;
    private NavMeshAgent m_nvAgent;
    private Animator m_animator;


    public bool attackCheck = false;
    private float m_distance;
    private Collider m_target;

    //임시 스텟
    private float m_hp;
    private float m_oriHp;
    private float m_str;
    private float m_def;
    private float m_attackRange = 4.0f;

    private List<ParticleSystem> m_particles = new List<ParticleSystem>();

    //Magician
    public GameObject m_attackMagic;
    private float m_skill1CoolTime = 6.0f;
    private Transform m_magicBallPovot;
    private float speed = 1000;
    private bool skill1Check = false;
    private float m_firstSkillDistance = 3.5f;
    private float time = 1.5f;
    private MercenaryHealth m_health;

    public bool isHitBack = false;
    private float m_hitBackTime = 0.1f;

    public GameObject GetPlayer { get { return m_player; } }
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_magicBallPovot = transform.Find("MagicBallPivot").transform;
        m_nvAgent = GetComponent<NavMeshAgent>();
        m_nvAgent.enabled = true;
        m_animator = GetComponent<Animator>();
        m_health = GetComponent<MercenaryHealth>();
        Transform t = transform.Find("Skill1Effect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }
        //임시
        m_oriHp = m_player.GetComponent<CharController>().GetTotalHp;
        m_hp = m_oriHp;
        m_distance = 100.0f;
        m_health.GetHp = m_oriHp;
        m_health.Init();
    }

    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        ////Gizmos.DrawCube(transform.position + transform.forward * 3.0f, new Vector3(2, 2, 6));
        //Gizmos.DrawSphere(transform.position + transform.forward *4.0f , 3.0f);
    }
    public void MagicianFirstSkill()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position + transform.forward * 4.0f, m_firstSkillDistance);
        foreach (Collider col in targets)
        {
            if (col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss")
            {
                //int damage = (int)(m_totalStr * FindSkillInfo("Skill3", m_SL3));
                MonsterHealth health = col.GetComponent<MonsterHealth>();
                health.DecreaseHp(m_str, false, gameObject);
                if(col.gameObject.tag == "Monster")
                    health.GetComponent<MonsterController>().PlayerAttackCheck(gameObject);
            }
        }
        
    }
    private void MagicianAttack()
    {
        if (m_target != null)
        {
            GameObject projectile = Instantiate(m_attackMagic, m_magicBallPovot.position, Quaternion.identity) as GameObject;
            projectile.transform.parent = gameObject.transform;
            projectile.transform.LookAt(m_target.transform);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
            projectile.GetComponent<MagicBall>().target = m_target.gameObject;
            projectile.GetComponent<MagicBall>().m_magician = gameObject;
            projectile.GetComponent<MagicBall>().GetStr = m_str;
        }
    }
    private void StopMagicianSkill1()
    {
        m_animator.SetBool("Recharge", false);
        foreach (ParticleSystem p in m_particles)
        {
            p.gameObject.SetActive(false);
        }
        skill1Check = false;
        if(m_nvAgent.isActiveAndEnabled)
            m_nvAgent.isStopped = false;
    }
    bool isdie = false;

    void Update()
    {
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        float currentRatio = stateInfo.normalizedTime * stateInfo.length;

        if (GetComponent<MercenaryHealth>().IsDie)
        {
            if (stateInfo.IsName("Recharge"))
            {
                foreach (ParticleSystem p in m_particles)
                {
                    p.gameObject.SetActive(false);
                }
            }

            if (!isdie)
            {
                isdie = true;
                //m_nvAgent.enabled = false;
                GetComponent<Collider>().enabled = false;
                m_animator.SetTrigger("Die");
            }
            else
            {
                if (stateInfo.IsName("Die")&&currentRatio >= stateInfo.length * 0.80f)
                    m_animator.speed = 0;
            }

            return;
        }
        float dis = Vector3.Distance(m_player.transform.position, transform.position);
        if (attackCheck)
        {
            m_skill1CoolTime -= Time.deltaTime;
            int targetCount = 0;
            if (m_target == null)
            {
                Collider[] collider = Physics.OverlapSphere(transform.position, 6.0f);
                foreach (Collider col in collider)
                {
                    if (col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss")
                    {
                        if (col.GetComponent<MonsterHealth>().IsDie)
                            continue;
                        float distance = Vector3.Distance(transform.position, col.transform.position);
                        if (distance < m_distance)
                        {
                            m_target = col;
                            m_distance = distance;
                            targetCount++;
                        }
                    }
                    if (targetCount == 0)
                    {
                        m_target = null;
                    }
                }
            }
            if (dis >= 7.0f)
            {
                m_nvAgent.SetDestination(m_player.transform.position);
                m_animator.SetBool("Run", true);
                m_animator.SetBool("MeleeAttack", false);
            }
            else if (m_target != null)
            {
                if (!m_target.GetComponent<MonsterHealth>().IsDie)
                {
                    float distance = Vector3.Distance(transform.position, m_target.transform.position);
                    
                    if (distance <= m_attackRange)
                    {
                        m_animator.SetBool("Run", false);
                        m_nvAgent.velocity = Vector3.zero;
                        if (m_skill1CoolTime <= 0 && !m_animator.IsInTransition(0) && !skill1Check)
                        {
                            skill1Check = true;
                            m_animator.SetBool("MeleeAttack", false);
                            transform.LookAt(m_target.transform.position);
                            m_animator.SetBool("Recharge", true);
                            m_nvAgent.isStopped = true;
                            foreach (ParticleSystem p in m_particles)
                            {
                                p.gameObject.SetActive(true);
                                p.Play();
                            }
                            m_skill1CoolTime = 10.0f;
                            Invoke("StopMagicianSkill1", 5.0f);
                        }
                        else if (!skill1Check)
                        {
                            if (!stateInfo.IsName("Recharge") && !m_animator.IsInTransition(0))
                            {
                                transform.LookAt(m_target.transform);
                                m_animator.SetBool("MeleeAttack", true);

                            }
                        }
                    }
                    else
                    {
                        if((!stateInfo.IsName("MeleeAttack")||!stateInfo.IsName("Recharge")) && !m_animator.IsInTransition(0))
                        {
                            if (m_nvAgent.isActiveAndEnabled)
                            {
                                m_nvAgent.SetDestination(m_target.transform.position);
                                m_animator.SetBool("Run", true);
                            }
                        }
                    }
                }
                else
                {
                    m_target = null;
                    m_distance = 100.0f;
                }
            }
            else if (m_target == null)
            {
                m_animator.SetBool("MeleeAttack", false);
                if (dis < 3.0f)
                {
                    m_animator.SetBool("Run", false);
                }
                else if(!stateInfo.IsName("Recharge"))
                {
                    m_nvAgent.SetDestination(m_player.transform.position);
                    m_animator.SetBool("Run", true);
                }
            }
            else
            {
                m_animator.SetBool("MeleeAttack", false);
            }
        }
        else
        {
            if (dis > 3.0f && !stateInfo.IsName("Recharge"))
            {
                if (!m_nvAgent.isActiveAndEnabled)
                    m_nvAgent.enabled = true;
                m_nvAgent.SetDestination(m_player.transform.position);
                m_animator.SetBool("Run", true);
            }
            else
            {
                m_animator.SetBool("Run", false);
                m_nvAgent.velocity = Vector3.zero;
                //attackCheck = true;
            }
        }

        if (skill1Check)
        {
            time += Time.deltaTime;
            if(time >= 0.8f)
            {
                MagicianFirstSkill();
                time = 0.0f;
            }
        }
        m_str = m_player.GetComponent<CharController>().GetTotalStr * 0.8f;
        m_oriHp = m_player.GetComponent<CharController>().GetTotalHp * 0.8f;

        if (isHitBack)
        {
            if (m_hitBackTime < 0.0f)
            {
                m_nvAgent.enabled = true;
                isHitBack = false;
                m_hitBackTime = 0.1f;
                return;
            }
            m_nvAgent.enabled = false;
            transform.Translate(-Vector3.forward * Time.deltaTime * 8.0f);
            m_hitBackTime -= Time.deltaTime;
        }
    }
}
