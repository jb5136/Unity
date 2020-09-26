using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    private bool m_appearCheck = false;

    private Animator m_animator;
    private NavMeshAgent m_nvAgent;
    private MonsterHealth m_bossHealth;
    private GameObject m_flyBreathRangeShader;

    private float m_distance = 0;

    private GameObject target;
    private List<GameObject> m_targetObj = new List<GameObject>();


    private MainCamera m_camera;
    private GameObject m_appearBossImg;
    private GameObject m_ui;
    private GameObject m_outDgUi;
    private GameObject m_miniMapUi;

    private float m_attackRange = 4.0f;
    private float m_attackCoolDown = 2.0f;
    private float m_breathRange = 7.0f;
    private float m_breathCoolDown = 10.0f;
    private int m_tailAttackCount = 0;

    //임시 스텟
    private float m_bossStr;
    private float m_bossDef;
    private float m_bossHp;

    private List<ParticleSystem> m_firstParticles = new List<ParticleSystem>();
    private List<ParticleSystem> m_FlyParticles = new List<ParticleSystem>();
    private List<ParticleSystem> m_AreaDamageFire = new List<ParticleSystem>();
    private List<ParticleSystem> m_tailAttackEffect = new List<ParticleSystem>();

    //path
    private List<Transform> m_path = new List<Transform>();
    private int m_currIndex = 0;
    private float m_elapsedTime = 0.0f;

    private GameObject m_MercenartHpUI;

    public float GetDef { get { return m_bossDef; } }
    public int SortByName<T>(T a, T b) where T : UnityEngine.Object
    {
        return string.Compare(a.name, b.name);
    }
    void Start()
    {
        m_MercenartHpUI = GameObject.Find("DCanvas/MercenaryHp");

        m_bossStr = 300.0f;
        m_bossDef = 200.0f;
        m_bossHp = 10000.0f;
        GameObject[] g = GameObject.FindGameObjectsWithTag("Path");
        for (int i = 0; i < g.Length; ++i)
            m_path.Add(g[i].transform);

        m_path.Sort(SortByName<Transform>);

        m_nvAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_bossHealth = GetComponent<MonsterHealth>();
        m_flyBreathRangeShader = transform.Find("FlyBreathRange").gameObject;
        target = null;

        Transform t = transform.Find("FirstEffect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_firstParticles.AddRange(ps);
        }
        t = transform.Find("SecondEffect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_FlyParticles.AddRange(ps);
        }
        t = transform.Find("AreaDamageFire");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_AreaDamageFire.AddRange(ps);
        }
        t = transform.Find("TailAttackEffect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_tailAttackEffect.AddRange(ps);
        }
        m_bossHealth.GetHp = m_bossHp;
        m_bossHealth.Init();

        m_ui = GameObject.Find("Canvas").transform.Find("UI").gameObject;
        m_outDgUi = GameObject.Find("DCanvas").transform.Find("OutDungeon").gameObject;
        m_miniMapUi = GameObject.Find("DCanvas").transform.Find("MiniMap").gameObject;

        m_appearBossImg = GameObject.Find("DCanvas").transform.Find("Boss").gameObject;
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
        m_camera.isAppearBoss = true;
        m_appearBossImg.SetActive(true);
        m_ui.SetActive(false);
        m_outDgUi.SetActive(false);
        m_miniMapUi.SetActive(false);
        m_MercenartHpUI.SetActive(false);
    }
    private void FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, 15.0f);
        if(targets.Length != 0)
        {
            m_distance = 0;
            foreach (Collider col in targets)
            {
                if(col.tag == "Player" || col.tag == "Mercenary")
                {
                    bool isAlive = TargetIsDie(col.gameObject);

                    if (!isAlive)
                    {
                        float dis = Vector3.Distance(transform.position, col.transform.position);
                        if (m_distance == 0)
                        {
                            m_distance = dis;
                            target = col.gameObject;
                        }
                        else if (dis < m_distance)
                        {
                            m_distance = dis;
                            target = col.gameObject;
                        }
                    }
                    
                }
            }
        }
    }
    private bool TargetIsDie(GameObject target)
    {
        bool isDie = false;
        if(target.tag == "Player")
            isDie = target.GetComponent<PlayerHealth>().IsDie;
        else if (target.tag == "Mercenary")
            isDie = target.GetComponent<MercenaryHealth>().IsDie;


        return isDie;
    }
    private void BiteAttack()
    {
        if(target != null)
        {
            if (target.tag == "Player")
            {
                target.GetComponent<PlayerHealth>().DecreaseHp(m_bossStr);
            }
            else if (target.tag == "Mercenary")
            {
                target.GetComponent<MercenaryHealth>().DecreaseHp(m_bossStr);
            }
        }
    }
    private void TailAttack()
    {
        m_targetObj.Clear();
        Collider[] targets = Physics.OverlapSphere(transform.position, 6.0f);
        foreach (Collider col in targets)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.tag == "Mercenary" )
            {
                TailAttackRange(col.transform);
            }
        }
        
        if (m_targetObj.Count != 0)
        {
            foreach (GameObject g in m_targetObj)
            {
                int damage = (int)m_bossStr;

                if(g.tag == "Player")
                {
                    PlayerHealth health = g.GetComponent<PlayerHealth>();
                    health.DecreaseHp(damage);
                    g.GetComponent<CharController>().isHitBack = true;
                }
                else if(g.tag == "Mercenary")
                {
                    string name = g.GetComponent<MercenaryHealth>().name;
                    MercenaryHealth health = g.GetComponent<MercenaryHealth>();
                    health.DecreaseHp(damage);
                    if (name == "Dwarf")
                        g.GetComponent<Dwarf>().isHitBack = true;
                    else if (name == "Magician")
                        g.GetComponent<Magician>().isHitBack = true;
                    else
                        Debug.Log("tail");
                }
            }
        }
    }
    public void TailAttackRange(Transform target)
    {
        float m_angleRange = 180.0f;
        float dotValue = Mathf.Cos(Mathf.Deg2Rad * (m_angleRange / 2));
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude < 6.0f)
        {
            if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
            {
                m_targetObj.Add(target.gameObject);
            }
        }

    }
    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(transform.position + transform.forward * 8.0f, new Vector3(2, 4, 9));
        //Gizmos.DrawSphere(m_secondRange.transform.position, 22.0f);
    }
    private void FireBreathRange()
    {

        float forwardRange = 8.0f;
        List<Collider> tg = new List<Collider>();
        Collider[] targets = Physics.OverlapBox(transform.position + transform.forward * forwardRange, new Vector3(2, 4, 9), transform.rotation);
        foreach(Collider col in targets)
        {
            if(col.tag == "Player" || col.tag == "Mercenary")
            {
                tg.Add(col);
            }
        }

        if(tg.Count != 0)
        {
            foreach(Collider col in tg)
            {
                if (col.tag == "Player")
                {
                    col.GetComponent<PlayerHealth>().DecreaseHp(m_bossStr * 1.2f);
                }
                else if (col.tag == "Mercenary")
                {
                    col.GetComponent<MercenaryHealth>().DecreaseHp(m_bossStr* 1.2f);
                }
            }
        }
    }
    float fireBreathCool = 1.5f;

    private void AppearBoss()
    {
        m_camera.isAppearBoss = false;
        m_appearBossImg.SetActive(false);
        m_MercenartHpUI.SetActive(true);
        m_ui.SetActive(true);
        m_outDgUi.SetActive(true);
        m_miniMapUi.SetActive(true);

    }
    int tt = 0;
    void Update()
    {
        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
        float currentRatio = stateinfo.normalizedTime * stateinfo.length;

        if (GetComponent<MonsterHealth>().IsDie)
        {
            if (stateinfo.IsName("El_Attack_FireBreath"))
            {
                foreach (ParticleSystem p in m_firstParticles)
                {
                    p.gameObject.SetActive(false);
                }
            }
            if(tt == 0)
            {
                m_animator.SetTrigger("Die");
                GameObject.Find("SceneManager").GetComponent<DungeonSceneMng>().isClear = true;
                Destroy(gameObject, 2.5f);
                tt++;
            }
            return;
        }

        if (!m_appearCheck)
        {
            m_nvAgent.enabled = false;
            if(m_currIndex <= 0)
                m_elapsedTime += Time.deltaTime * 0.2f;
            else
            {
                m_elapsedTime += Time.deltaTime * 0.4f;
            }
            m_elapsedTime = Mathf.Clamp01(m_elapsedTime);

            Vector3 position = MathHelper.CatmullRom(m_elapsedTime,
            m_path[0 + m_currIndex].position,
            m_path[1 + m_currIndex].position,
            m_path[2 + m_currIndex].position,
            m_path[3 + m_currIndex].position);
            gameObject.transform.position = position;


            if (m_elapsedTime >= 1)
            {
                ++m_currIndex;
                if (m_currIndex >= 3)
                {
                    m_animator.SetBool("Fly Forward", false);
                    m_animator.SetBool("Fly Stand", false);
                    m_animator.SetTrigger("Fall Recover");
                    m_appearCheck = true;
                    Invoke("AppearBoss", 1.5f);
                    
                }


                m_elapsedTime = 0;
            }
            float dis = Vector3.Distance(transform.position, m_path[m_currIndex+1].transform.position);
            if (m_currIndex <= 1)
            {
                //transform.Rotate(Vector3.forward, Time.deltaTime);
                transform.LookAt(m_path[m_currIndex + 2]);
            }
             if(m_currIndex < 2)
            {
                m_animator.SetBool("Fly Forward", true);
            }
             else if (m_currIndex == 2)
            {
                m_animator.SetBool("Fly Forward", false);
                m_animator.SetBool("Fly Stand", true);
            }
        }
        else if(!GetComponent<MonsterHealth>().IsDie)
        {
            m_nvAgent.enabled = true;

            if (Input.GetKeyDown(KeyCode.P))
            {
                //m_animator.SetTrigger("Tail Attack");
                m_animator.SetBool("Die", true);
                Destroy(gameObject, 2.5f);

            }
            FindTarget();
            
            
            if (target != null)
            {
                bool isDie = TargetIsDie(target);
                if (isDie)
                {
                    target = null;
                    return;
                }
                float dis = Vector3.Distance(transform.position, target.transform.position);
                transform.LookAt(transform.position);
                m_breathCoolDown -= Time.deltaTime;
                m_attackCoolDown -= Time.deltaTime;
                if (stateinfo.IsName("El_Attack_FireBreath"))
                {
                    fireBreathCool -= Time.deltaTime;
                    if (fireBreathCool <= 0.0f)
                    {
                        FireBreathRange();
                        fireBreathCool = 0.5f;
                    }
                }
                else if (dis > m_attackRange && stateinfo.IsName("Idle"))
                {
                    m_nvAgent.isStopped = false;
                    m_nvAgent.SetDestination(target.transform.position);
                    m_animator.SetBool("Walk Forward", true);
                }
                else if (m_breathCoolDown <= 0.0f && !m_animator.IsInTransition(0) && stateinfo.IsName("Idle"))
                {
                    m_nvAgent.isStopped = true;
                    m_animator.SetTrigger("FireBreath");
                    foreach (ParticleSystem p in m_firstParticles)
                    {
                        p.gameObject.SetActive(true);
                        p.Play();
                    }
                    m_breathCoolDown = 10.0f;
                }
                else if(m_tailAttackCount >= 3)
                {
                    if(stateinfo.IsName("Idle") && !m_animator.IsInTransition(0))
                    {
                        m_animator.SetTrigger("Tail Attack");
                        foreach (ParticleSystem p in m_tailAttackEffect)
                        {
                            p.gameObject.SetActive(true);
                            p.Play();
                        }
                        m_tailAttackCount = 0;
                    }
                        
                }
                else if (dis <= m_attackRange)
                {
                    m_nvAgent.isStopped = true;
                    m_nvAgent.velocity = Vector3.zero;
                    m_animator.SetBool("Walk Forward", false);
                    transform.LookAt(target.transform);
                    if (m_attackCoolDown <= 0.0f && stateinfo.IsName("Idle") && !m_animator.IsInTransition(0))
                    {
                        m_animator.SetTrigger("Bite Attack");
                        m_attackCoolDown = 2.0f;
                        m_tailAttackCount++;
                    }
                }
            }
            else
            {

            }

        }
    }
}
