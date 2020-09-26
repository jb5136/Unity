using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public enum CurrentState {  idle, trace, attack,hitback, dead};
    public CurrentState curState = CurrentState.idle;

    private Monster m_monsterInfo;
    private Transform m_transform;
    private Transform m_playerTransform;
    private NavMeshAgent nvAgent;
    private Animator m_animator;
    private MonsterHealth health;
    public bool isFieldMon = false;

    //죽었을 때 처리
    Shader m_deadSha;
    Material m_Material;
    Material m_deadMat;

    //몬스터 스텟
    [SerializeField]
    private float m_monsterHp;
    private float m_monsterStr;
    private float m_monsterDef;
    private float m_monsterCri;
    private int m_monsterLevel;
    private string m_monsterName;
    private float m_monsterAtDis;

    //처음 생성된 자리
    private Vector3 m_originPosition;

    //히트 될때 이펙트체크
    private bool hitEffectcheck = false;
    //hit back
    private bool hitback = false;
    private float hitbackTime = 0.1f;

    //추적
    public bool traceCheck = false;
    //적발 사정거리
    private float FIndTarget = 9.0f; 
    //추적 사정거리
    public float traceDist = 5.0f;
    //공격 사정거리
    public float attackDist = 1f;
    //사망여부
    private bool isDead = false;

    private bool isTrace = false;
    GameObject marker;

    public int GetLevel { get { return m_monsterLevel; } }
    public string GetName { get { return m_monsterName; } }
    public float GetDef { get { return m_monsterDef; } }
    public bool GetHitCheck { set { hitEffectcheck = value; } }
    public bool GetHitBack { set { hitback = value; } }
    public Monster SetMonsterInfo { set { m_monsterInfo = value; } }

    private List<Item> m_getItem = new List<Item>();

    private Transform m_whetherPivot;

    void Start()
    {
        GetComponent<NavMeshAgent>().enabled = true;
        m_transform = this.gameObject.GetComponent<Transform>();
        //m_playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        m_animator = this.gameObject.GetComponent<Animator>();
        m_originPosition = this.transform.position;
        

        marker = transform.Find("MarkerPivot/Marker.Crosshair").gameObject;
        marker.SetActive(false);

        m_Material = transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material;

        m_deadMat = Instantiate( Resources.Load<Material>("Materials/DissolveTexRampNor") );

        m_playerTransform = null;
        //m_playerTransform = FindPlayer();

        Coroutine();

        //monster info
        m_monsterHp = m_monsterInfo.monHp;
        m_monsterStr = m_monsterInfo.monStr;
        m_monsterDef = m_monsterInfo.monDef;
        m_monsterCri = m_monsterInfo.monCri;
        m_monsterAtDis = m_monsterInfo.monAtDis;
        m_monsterLevel = m_monsterInfo.monLevel;
        m_monsterName = m_monsterInfo.monName;

        health = GetComponent<MonsterHealth>();
        health.GetHp = m_monsterHp;
        health.Init();

    }
    public void Coroutine()
    {
        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }
    public void PlayerAttackCheck(GameObject player)
    {
        m_playerTransform = player.transform;
        traceCheck = true;
    }
    public Transform FindPlayer()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, FIndTarget);
        foreach(Collider col in collider)
        {
            if(col.gameObject.tag == "Player")
            {
                return col.transform;
            }
        }

        return null;
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            float dist = 100;
            yield return new WaitForSeconds(0.2f);
            if(m_playerTransform != null)
            {
                dist = Vector3.Distance(m_playerTransform.position, m_transform.position);
            }
            float oriDist = Vector3.Distance(m_originPosition, m_transform.position);
            if (!traceCheck)
                curState = CurrentState.idle;
            else if (hitback)
            {
                curState = CurrentState.hitback;
            }
            else if (dist <= attackDist)
            {
                curState = CurrentState.attack;

            }
            else if (oriDist >= 20.0f)
            {
                curState = CurrentState.idle;
                isTrace = true;
                marker.SetActive(false);
                
            }
            else if (dist <= traceDist && !isTrace)
            {
                curState = CurrentState.trace;
            }
            else 
            {
                curState = CurrentState.idle;
                marker.SetActive(false);
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            float oriDist = Vector3.Distance(m_originPosition, m_transform.position);
            float dist = 0;
            AnimatorStateInfo animation = m_animator.GetCurrentAnimatorStateInfo(0);
            if (m_playerTransform != null)
            {
                dist = Vector3.Distance(m_playerTransform.position, m_transform.position);
            }
            switch (curState)
            {
                
                case CurrentState.idle:
                    nvAgent.isStopped = false;
                        nvAgent.SetDestination(m_originPosition);
                    if(oriDist < 1.0f)
                    {
                        GetComponent<MonsterHealth>().GetTarget = null;
                        nvAgent.SetDestination(m_transform.position);
                        m_animator.SetBool("Run Forward", false);
                        isTrace = false;
                    }
                    else
                    {
                        traceCheck = false;
                        m_animator.SetBool("Run Forward", true);
                    }
                    break;
                case CurrentState.trace:
                    if (dist > attackDist && !animation.IsName("Attack"))
                    {
                        if(!m_animator.IsInTransition(0))
                        {
                            nvAgent.isStopped = false;
                            nvAgent.SetDestination(m_playerTransform.position);
                        }

                    }
                    m_animator.SetBool("Run Forward",true);
                    break;
                case CurrentState.attack:
                    m_animator.SetBool("Run Forward", false);
                    if (animation.IsName("Idle") || m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
                    {
                        if (! m_animator.IsInTransition(0))
                        {
                            nvAgent.isStopped = true ;
                            m_animator.SetTrigger("Attack");
                            m_transform.LookAt(m_playerTransform);
                        }
                    }
                    break;
                case CurrentState.hitback:
                    transform.LookAt(m_playerTransform);
                    nvAgent.isStopped = true;
                    if(!m_animator.IsInTransition(0))
                        m_animator.SetTrigger("Hit");
                    if (hitbackTime <0)
                    {
                        hitback = false;
                        hitbackTime = 0.1f;
                    }
                    transform.Translate(-Vector3.forward * Time.deltaTime*5.0f);
                    hitbackTime -= Time.deltaTime;
                    break;
            }
            yield return null;
        }
        m_animator.ResetTrigger("Attack");
    }
    private void Stop()
    {
        hitback = false;
    }
    public void Attack()
    {
        if(m_playerTransform != null)
        {
            if(m_playerTransform.gameObject.tag == "Player")
            {
                m_playerTransform.GetComponent<PlayerHealth>().DecreaseHp(m_monsterStr);
                //m_playerTransform.GetComponent<PlayerHealth>().hitcheck = true;
            }
            else if(m_playerTransform.gameObject.tag == "Mercenary")
            {
                m_playerTransform.GetComponent<MercenaryHealth>().DecreaseHp(m_monsterStr);
            }
        }
    }

    public void SetItem(Item item)
    {
        if (!m_getItem.Contains(item))
        {
            m_getItem.Add(item);
        }
    }
    private void DieToPlayer()
    {
        GameObject player;
        if (m_playerTransform.gameObject.tag == "Player")
        {
            int playerLevel = m_playerTransform.GetComponent<CharController>().GetLevel;
            float dis = m_monsterLevel - playerLevel;
            dis = dis * 8;
            if (dis <= 0)
                dis = 20;
            m_playerTransform.GetComponent<CharController>().SetExp(dis);
        }
        else if(m_playerTransform.gameObject.tag == "Mercenary")
        {
            string name = m_playerTransform.GetComponent<MercenaryHealth>().name;
            if (name == "Dwarf")
            {
                player = m_playerTransform.GetComponent<Dwarf>().GetPlayer;
            }
            else if (name == "Magician")
            {
                player = m_playerTransform.GetComponent<Magician>().GetPlayer;
            }
            else
                return;
            int playerLevel = player.GetComponent<CharController>().GetLevel;
            float dis = m_monsterLevel - playerLevel;
            dis = dis * 8;
            if (dis <= 0)
                dis = 20;
            player.GetComponent<CharController>().SetExp(dis);
        }
        if (isFieldMon)
        {
            int random = Random.Range(0, 100);
            if(random <= 10 && m_getItem.Count > 0)
            {
                int index = Random.Range(0, m_getItem.Count);
                Item item = m_getItem[index];
                Static.s_invenItemList.Add(item);
                Static.InvenItemSaveList();
                m_whetherPivot = GameObject.Find("FCanvas").transform.Find("TextPivot").transform;
                GameObject whether = Resources.Load<GameObject>("UIPrefabs/WhetherCheck");
                var temp = Instantiate(whether, m_whetherPivot);
                temp.transform.Find("Text").GetComponent<Text>().text = item.itemName + "을 획득하셧습니다";
                temp.transform.position = m_whetherPivot.transform.position;
            }
        }

    }

    float f = 0.0f;
    void Update()
    {
        if(m_playerTransform != null)
        {
            if (m_playerTransform.gameObject.tag == "Player")
            {
                if (m_playerTransform.GetComponent<PlayerHealth>().IsDie)
                {
                    m_playerTransform = null;
                    curState = CurrentState.idle;
                }
            }
            else if (m_playerTransform.gameObject.tag == "Mercenary")
            {
                if (m_playerTransform.GetComponent<MercenaryHealth>().IsDie)
                {
                    m_playerTransform = null;
                    curState = CurrentState.idle;

                }
            }
        }
        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
       
        if(m_playerTransform == null && traceCheck)
            m_playerTransform = FindPlayer();
        float hp = health.GetHp;

        if (hp <= 0 && !isDead)
        {
            //GetComponent<CapsuleCollider>().isTrigger = true;
            marker.SetActive(false);
            DieToPlayer();
            transform.Find("Body").GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            m_animator.SetTrigger("Die");
            isDead = true;
        }
        if (isDead)
        {
            nvAgent.isStopped = true;
            float currentRatio = stateinfo.normalizedTime * stateinfo.length;
            transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material = m_deadMat;
            if (stateinfo.IsName("Die") && currentRatio >= stateinfo.length * 0.5f)
            {
                if(f >= 1.0f)
                {
                    transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material = m_Material;
                    health.GetHp = m_monsterHp;
                    health.Init();
                    isDead = false;
                    traceCheck = false;
                    m_playerTransform = null;
                    f = 0.0f;
                    gameObject.SetActive(false);
                    return;
                }
                else
                {
                    f += Time.deltaTime;
                    transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material.SetFloat("_DissolveAmount", f);
                }
                if(currentRatio >= stateinfo.length * 0.95f)
                    m_animator.speed = 0;
            }
        }

        if (hitEffectcheck)
        {
           Transform t = transform.Find("RigPelvis/FrontHitPivot/MetalExplosion");
            if (t != null)
            {
                ParticleSystem particle = t.GetComponent<ParticleSystem>();
                particle.gameObject.SetActive(true);
                particle.Play();

            }
            hitEffectcheck = false;
        }
    }
}
