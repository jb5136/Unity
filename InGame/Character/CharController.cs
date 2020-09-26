using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//m_collider 나중에 수정하기 (지금은 땜빵처리로 함)
//스킬버튼 나중에 한번에 정리하기
//필드 테이블 만들어서 몬스터가 주는 아이템 골드 경험치 만들기 
//캐릭터정보 텍스트에 직업을 넣어야함

public class CharController : MonoBehaviour
{

    public static CharController instance;

    private Animator m_animator;
    private NavMeshAgent m_agent;
    private int m_comboCount = 0;
    private CharacterController m_controller;
    private float speed;
    private float gravity;
    private Vector3 move;
    private bool moveCheck;

    private Collider m_collider;

    //private Collider[] m_collider;
    private float m_distance;
    MonsterHealth m_targetHealth;
    private List<Collider> m_targetList = new List<Collider>();
    PlayerHealth m_health;

    //캐릭터 UI
    private Text m_charNameText;
    private Text m_charLevelText;
    private Slider m_charHpbar;
    private Slider m_expBar;
    //캐릭터의 스텟
    private int m_charId;
    private string m_charName;
    private int m_charLevel;
    private float m_str;
    private float m_def;
    private float m_cri;
    private float m_hp;
    private float m_totalStr;
    private float m_totalDef;
    private float m_totalCri;
    private float m_attackRange;
    private float m_charOriHp;
    private int m_SL1;
    private int m_SL2;
    private int m_SL3;
    private int m_SL4;
    private int m_SL5;
    private float m_exp;
    private int m_SP;
    private int m_silver;
    private int m_gold;
    private int m_diamond;
    private CharEquipment m_CharEquip;


    private Vector3 moveDirection = Vector3.zero;
    //스킬범위 타겟
    private List<GameObject> m_targetObj = new List<GameObject>();
    float dotValue = 0f;
    private float m_angleRange;
    Vector3 direction;

    //스킬 범위 
    private float firstDistance = 5f;
    private float thirdDistance = 3.5f;
    private float forthDistance = 7f;
    private float fifthDistance = 6.5f;
    // Effect
    private List<ParticleSystem> m_particles = new List<ParticleSystem>();
    private List<ParticleSystem> m_levelUpParticles = new List<ParticleSystem>();
    //main Camera
    private MainCamera m_camera;

    public int GetSL1 { get { return m_SL1; } set { m_SL1 = value; } }
    public int GetSL2 { get { return m_SL2; } set { m_SL2 = value; } }
    public int GetSL3 { get { return m_SL3; } set { m_SL3 = value; } }
    public int GetSL4 { get { return m_SL4; } set { m_SL4 = value; } }
    public int GetSL5 { get { return m_SL5; } set { m_SL5 = value; } }

    public int GetSP { get { return m_SP; } set { m_SP -= value; } }
    public float GetExp { get { return m_exp; } }
    public float GetTotalDef { get { return m_totalDef; } }
    public float GetTotalStr { get { return m_totalStr; } }
    public float GetTotalHp { get { return m_charOriHp; } }
    public void SetExp(float value)
    {
        m_exp += value;
    }
    public int GetLevel { get { return m_charLevel; } }
    public bool GetMoveCheck { set { moveCheck = value; } }
    public Collider GetCollider
    {
        get { return m_collider; }
        set { m_collider = value; }
    }
    public float GetStat(int count)
    {
        switch (count)
        {
            case 1:
                return m_charOriHp;
            case 2:
                return m_totalStr;
        }
        return -1;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        speed = 6.0f;
        gravity = 100.0f;
        m_distance = 20.0f;
        m_angleRange = 180f;
        move = Vector3.zero;
        m_collider = null;
        m_animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        m_controller = GetComponent<CharacterController>();
        //////////임시 스텟////////
        m_attackRange = 4.0f;
        ///////////////////////////
        CharicterStateSet();
        GameObject g = GameObject.Find("Canvas/UI/CharacterUI");
        if( g != null)
        {
            m_charNameText = g.transform.Find("PlayerName/Text").GetComponent<Text>();
            m_charLevelText = g.transform.Find("Level/Text").GetComponent<Text>();
            m_charHpbar = g.transform.Find("HealthBar").GetComponent<Slider>();
            m_expBar = g.transform.Find("Exp").GetComponent<Slider>();

            m_charNameText.text = m_charName;
        }
        m_CharEquip = GameObject.Find("Canvas").transform.Find("InvenCanvas/InventoryCanvas/CharEquipment").GetComponent<CharEquipment>();

        m_health = GetComponent<PlayerHealth>();
        m_health.GetHp = m_charOriHp;
        m_health.Init();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();

        Transform t = transform.Find("SlashEffect");
        if (t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_particles.AddRange(ps);
        }
        t = transform.Find("LevelUpEffect");
        if(t != null)
        {
            ParticleSystem[] ps = t.GetComponentsInChildren<ParticleSystem>(true);
            m_levelUpParticles.AddRange(ps);
        }
    }
    public int[] ReturnSkillLevel()
    {
        int[] state = new int[5];
        state[0] = m_SL1;
        state[1] = m_SL2;
        state[2] = m_SL3;
        state[3] = m_SL4;
        state[4] = m_SL5;

        return state;
    }
    public string[] StateReturn()
    {
        string[] state = new string[14];
        state[0] = m_charId.ToString();
        state[1] = m_charName;
        state[2] = m_charLevel.ToString();
        state[3] = m_str.ToString();
        state[4] = m_def.ToString();
        state[5] = m_cri.ToString();
        state[6] = m_charOriHp.ToString();
        state[7] = m_SL1.ToString();
        state[8] = m_SL2.ToString();
        state[9] = m_SL3.ToString();
        state[10] = m_SL4.ToString();
        state[11] = m_SL5.ToString();
        state[12] = m_exp.ToString();
        state[13] = m_SP.ToString();

        return state;
    }
    private bool CriticalCheck()
    {
        int temp = Random.Range(0, 100);
        if (temp <= m_totalCri)
        {
            m_camera.SetIsShake = true;
            return true;
        }
        else
            return false;
    }
    private void CharicterStateSet()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharState.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamReader sr = new StreamReader(fs);
        string line = sr.ReadLine();
        string[] info = line.Split(',');
        m_charId = int.Parse(info[0]);
        m_charName = info[1];
        m_charLevel = int.Parse(info[2]);
        m_str = float.Parse(info[3]);
        Debug.Log(m_str); 
        m_def = float.Parse(info[4]);
        m_cri = float.Parse(info[5]);
        m_charOriHp = float.Parse(info[6]);
        m_SL1 = int.Parse(info[7]);
        m_SL2 = int.Parse(info[8]);
        m_SL3 = int.Parse(info[9]);
        m_SL4 = int.Parse(info[10]);
        m_SL5 = int.Parse(info[11]);
        m_exp = float.Parse(info[12]);
        m_SP = int.Parse(info[13]);
        sr.Close();

        TotalState();
    }
    private void TotalState()
    {
        m_totalStr = m_str + TotalStr();
        m_totalDef = m_def + TotalDef();
        m_totalCri = m_cri + TotalCri();

    }
    private int TotalStr()
    {
        int totalStr = 0;
        foreach (Item slot in Static.s_wearItemList)
        {
            if(slot != null)
                totalStr += slot.itemStrength;
        }
        return totalStr;
    }
    private int TotalDef()
    {
        int totalDef = 0;
        foreach (Item slot in Static.s_wearItemList)
        {
            if (slot != null)
                totalDef += slot.itemDefense;
        }
        return totalDef;
    }
    private float TotalCri()
    {
        float totalCri = 0;
        foreach (Item slot in Static.s_wearItemList)
        {
            if (slot != null)
                totalCri += slot.itemCritical;
        }
        return totalCri;
    }
    public Collider FIndTarget()
    {
        if (m_collider != null && !m_collider.GetComponent<MonsterHealth>().IsDie && m_collider.gameObject.activeSelf)
        {
            return m_collider;
        }
        else
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, 15.0f);
            foreach (Collider col in collider)
            {
                if ((col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss") && !col.GetComponent<MonsterHealth>().IsDie)
                {
                    float dis = Vector3.Distance(transform.position, col.transform.position);
                    if (dis < m_distance)
                    {
                        m_collider = col;
                        m_distance = dis;
                    }
                }
            }
        }
        if (m_collider != null)
        {
            m_distance = 10.0f;
            return m_collider;
        }
        else
        {
            m_distance = 10.0f;
            return null;
        }
    }

    public bool skillState = false;
    public void UpdateAttack(bool moveCheck)
    {
        // 한 애니메이션을 실행할 때 NormalizeTime이 0이 아니라 0.7정도부터
        // 실행될 때가 있는데, 그것은 애니메이션과 애니메이션의 블랜딩을 지정해주는 상황에 
        // 따른 영향을 받는다는 것을 문서화 해 놓을 것.

        // 0번째 레이어에서 실행되고 있는 애니메이션 정보값을 받습니다. ( 실행되고 있는 노드의 정보값 )
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        //if (!stateInfo.IsName("Locomotion") && !stateInfo.IsName("Skill3Two"))
        if (!stateInfo.IsName("Locomotion") )
        {
            if (skillState)
                return;

            // stateInfo.normalizedTime은 현재 실행되고 있는 애니메이션의 
            // 실행되고 있는 비율값을 의미합니다.

            // 한 애니메이션이 동작이 끝날때까지 다른 애니메이션을 실행한 상태가
            // 아니라면, 대기 모션으로 돌아가도록 처리한 코드입니다.
            //Debug.Log(stateInfo.normalizedTime);
            //Debug.Log(stateInfo.length);


            //Debug.Log((stateInfo.length * stateInfo.normalizedTime));
            //if (stateInfo.length >= (stateInfo.length * stateInfo.normalizedTime))
            if (stateInfo.normalizedTime >=1.0f)
            {

                //Debug.Log("애니메이션 길이 : " + stateInfo.length);
                //Debug.Log("현재 비율 : " + (stateInfo.length * stateInfo.normalizedTime));

                //// 현재 애니메이션이 다른 상태로 변경중인 상태라면
                //// 실행되지 않도록 처리하였습니다.
                if (m_animator.IsInTransition(0))
                    return;
                m_comboCount = 0;
                m_animator.SetInteger("Combo", m_comboCount);
                m_animator.SetTrigger("Idle");
            }
        }

        if (Input.GetKey(KeyCode.Space) && !moveCheck)
        {
            
            if (stateInfo.IsName("Locomotion"))
            {
                m_comboCount = 1;
                m_animator.SetInteger("Combo", m_comboCount);
                m_particles[m_comboCount - 1].gameObject.SetActive(true);
                m_particles[m_comboCount - 1].Play();
            }
            else
            {
                float currentRatio = stateInfo.normalizedTime * stateInfo.length;
                if (stateInfo.IsName("Combo3") || stateInfo.IsName("Combo4"))
                {
                    if (stateInfo.IsName("Combo4") && currentRatio >= stateInfo.length * 0.5f)
                        return;
                    else
                        m_controller.Move(transform.forward * Time.deltaTime * 2);
                }
                //Debug.Log("현재 노멀라이즈타임 : " + stateInfo.normalizedTime);
                //Debug.Log("현재 비율 : " + currentRatio);
                //Debug.Log("애니메이션 길이 : " + stateInfo.length);

                // 현재 실행되고 있는 비율값이 애니메이션 길이 값의 95% 정도가 수행되었을 때
                // 키 입력을 받았다면 다른 애니메이션이 출력되도록 처리합니다.
                if (currentRatio >= stateInfo.length * 0.95f)
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

        if( Input.GetKeyDown(KeyCode.V) )
        {
            skillState = true;
            m_animator.SetTrigger("Skill3");
        }

    }
    void CheckAttack()
    {
        //Collider targetCollider =  FIndTarget();
        if (m_collider == null)
            return;
        m_targetHealth = m_collider.GetComponent<MonsterHealth>();

        if (m_targetHealth == null || m_targetHealth.IsDie)
            return;
        float distance = Vector3.Distance(m_targetHealth.transform.position, transform.position);

        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);

        if ((stateinfo.IsName("Combo3") || stateinfo.IsName("Combo4")) && m_collider.tag =="Monster")
        {
            m_collider.GetComponent<MonsterController>().GetHitBack = true;
        }
        if (distance <= m_attackRange)
        {
            m_targetHealth.DecreaseHp(m_totalStr, CriticalCheck(),gameObject);
            //m_targetHealth.DecreaseHp(10, CriticalCheck());
            if (m_collider.tag == "Monster")
            {
                Animator animator = m_targetHealth.GetComponent<Animator>();
                m_targetHealth.GetComponent<MonsterController>().GetHitCheck = true;
                m_targetHealth.GetComponent<MonsterController>().PlayerAttackCheck(gameObject);
            }
        }
    }
    private float FindSkillInfo(string skillName,int level)
    {
        switch (level)
        {
            case 1:
                return Database.instance.skills.Find(ex => ex.name == skillName).level1;
            case 2:
                return Database.instance.skills.Find(ex => ex.name == skillName).level2;
            case 3:
                return Database.instance.skills.Find(ex => ex.name == skillName).level3;
            case 4:
                return Database.instance.skills.Find(ex => ex.name == skillName).level4;
            case 5:
                return Database.instance.skills.Find(ex => ex.name == skillName).level5;
        }
        return 0;
    }

    public void SkillOne()
    {
        /*
         * mesh 사용할때 참고 
        Shader sh = Shader.Find("");
        Material mat;
        mat.shader = sh;
        */
        m_targetObj.Clear();
        Collider[] targets = Physics.OverlapSphere(transform.position, firstDistance);
        foreach (Collider col in targets)
        {
            if (col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss")
            {
                FirstSkillRange(col.transform);
            }
        }
        if (m_targetObj.Count != 0)
        {
            foreach (GameObject g in m_targetObj)
            {
                if (m_collider == null)
                    m_collider = g.GetComponent<Collider>();
                int damage = (int)(m_totalStr * FindSkillInfo("Skill1",m_SL1));

                MonsterHealth health = g.GetComponent<MonsterHealth>();
                health.DecreaseHp(damage, CriticalCheck(), gameObject);
                if (g.tag == "Monster")
                    health.GetComponent<MonsterController>().PlayerAttackCheck(gameObject);
            }
        }
    }
    public void FirstSkillRange(Transform target)
    {
        dotValue = Mathf.Cos(Mathf.Deg2Rad * (m_angleRange / 2));
        direction = target.position - transform.position;
        if (direction.magnitude < firstDistance)
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
        ////Gizmos.DrawCube(transform.position + transform.forward * 3.0f, new Vector3(2, 2, 6));
        //Gizmos.DrawSphere(transform.position + transform.forward *4.0f , 3.0f);
    }
    public void SecondSkill()
    {
        m_targetObj.Clear();
        float forwardRange = 3.0f;
        Collider[] targets = Physics.OverlapBox(transform.position+transform.forward* forwardRange, new Vector3(1,1,3.0f),transform.rotation);
        foreach(Collider col in targets)
        {
            if (col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss")
            {
                if (m_collider == null)
                    m_collider = col.GetComponent<Collider>();
                int damage = (int)(m_totalStr * FindSkillInfo("Skill2", m_SL2));
                MonsterHealth health = col.GetComponent<MonsterHealth>();
                health.DecreaseHp(damage, CriticalCheck(), gameObject);
                if (col.tag == "Monster")
                    health.GetComponent<MonsterController>().PlayerAttackCheck(gameObject);
            }
        }
    }
    public void ThirdSkill()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, thirdDistance);
        if(targets.Length != 0)
        {
            foreach (Collider col in targets)
            {
                if (col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss")
                {
                    if (m_collider == null)
                        m_collider = col.GetComponent<Collider>();
                    int damage = (int)(m_totalStr * FindSkillInfo("Skill3", m_SL3));
                    MonsterHealth health = col.GetComponent<MonsterHealth>();
                    health.DecreaseHp(damage, CriticalCheck(), gameObject);
                    if (col.tag == "Monster")
                        health.GetComponent<MonsterController>().PlayerAttackCheck(gameObject);
                }
            }
        }
    }
    public void ForthSkill()
    {
        //지금은 몬스터가 바로 앞으로 소환 되는데 이후 자연스럽게 이동하게 처리하기
        Collider[] targets = Physics.OverlapSphere(transform.position, forthDistance);
        float forwardRange = 1.0f;

        if (targets.Length != 0)
        {
            foreach (Collider col in targets)
            {
                if (col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss")
                {
                    if (m_collider == null)
                        m_collider = col.GetComponent<Collider>();
                    int damage = (int)(m_totalStr * FindSkillInfo("Skill4", m_SL4));
                    col.transform.position = transform.position + transform.forward * forwardRange;
                    MonsterHealth health = col.GetComponent<MonsterHealth>();
                    health.DecreaseHp(damage, CriticalCheck(), gameObject);
                    if (col.tag == "Monster")
                        health.GetComponent<MonsterController>().PlayerAttackCheck(gameObject);
                }
            }
        }
    }

    public void FifthSkill()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, fifthDistance);

        if (targets.Length != 0)
        {
            foreach (Collider col in targets)
            {
                if (col.gameObject.tag == "Monster" || col.gameObject.tag == "Boss")
                {
                    if (m_collider == null)
                        m_collider = col.GetComponent<Collider>();
                    int damage = (int)(m_totalStr * FindSkillInfo("Skill5", m_SL5));
                    MonsterHealth health = col.GetComponent<MonsterHealth>();
                    health.DecreaseHp(damage, CriticalCheck(), gameObject);
                    if(col.tag == "Monster")
                        health.GetComponent<MonsterController>().PlayerAttackCheck(gameObject);
                }
            }
        }
    }

    public void AttackMove(GameObject target)
    {
        transform.LookAt(target.transform);
        if (Vector3.Distance(transform.position, target.transform.position) < m_attackRange)
            return;
        m_animator.SetFloat("MoveX", 0.5f);
        m_animator.SetFloat("MoveZ", 0.5f);
           //목적지좌표 - 현재위치 = 이동할 거리량
        Vector3 moveDir = target.transform.position - transform.position;
           //캐릭터의 Y 좌표를 0으로 만든다. 3D는 경우에 따라서 Y 좌표값이 틀려지는 경우가 발생한다.
        moveDir = new Vector3(moveDir.x, 0, moveDir.z);
           // 목적지 최종 좌표 (현제 위치 + 이동할 거리량)
        moveDir = transform.position + moveDir;
         // 프레임 1당 이동할 거리량 계산.
        Vector3 framePos = Vector3.MoveTowards(transform.position, moveDir, speed * Time.deltaTime);
        // 이동할 거리를 프레임마다 갱신.
        Vector3 frameDir = framePos - transform.position;
        //CharacterController 내장함수 Move 사용. Physics의 내장함수 gravity(중력)
        m_controller.Move(frameDir + Physics.gravity);
    }
    public bool MoveCheck()
    {
        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Skill3First") || stateinfo.IsName("Skill3Two") || stateinfo.IsName("Skill3Finish"))
            return true;
        else
            return false;
    }
    public void InputFunc(Vector3 moveVec,bool Check)
    {
        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
    
        m_animator.SetFloat("MoveX", moveVec.x);
        m_animator.SetFloat("MoveZ", moveVec.y);
        Vector3 dir = new Vector3(moveVec.x, 0, moveVec.y);
        dir.Normalize();
        NavMeshPath navPath = new NavMeshPath();
        //bool moveState = m_agent.CalculatePath(transform.position + dir, navPath);
        // 네비메쉬에이전트를 활용하여 캐릭터의 1미터 앞이 이동될 수 있는 공간인지
        // 체크하고 이동처리하였습니다.
        if(Check == true)
        {
            m_agent.enabled = true;
        }
        bool moveAgent = false;
        if (m_agent.enabled)
            moveAgent = m_agent.CalculatePath(transform.position + dir, navPath);

        Vector3 move = Vector3.zero;
        moveCheck = Check;

        if (moveCheck && (stateinfo.IsName("Locomotion") || MoveCheck()) && moveAgent)
        {
            Vector3 vec = Camera.main.transform.TransformDirection(dir);
            Quaternion q = Quaternion.LookRotation(vec);
            float y = q.eulerAngles.y;
            transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
            move = transform.forward;
        }
            move.y -= gravity * Time.deltaTime;
        if(m_controller.enabled)
            m_controller.Move(move * Time.deltaTime *speed);
    }
    private void LevelUp()
    {
        m_charLevel++;
        m_str += 20;
        m_def += 20;
        m_cri += 2.0f;
        m_charOriHp += 200.0f;
        m_SP += 3;
        foreach (ParticleSystem p in m_levelUpParticles)
        {
            p.gameObject.SetActive(true);
            p.Play();
        }
        m_health.GetHp = m_charOriHp;
        m_health.GetOriHp = m_charOriHp;
        GameObject.Find("GameController").GetComponent<GameController>().SaveCharState();
    }
    private void Exp()
    {
        if(m_charLevel <= 20)
        {
            float maxExp = Static.s_maxExp[m_charLevel - 1];
            if(m_exp >= maxExp)
            {
                m_exp = m_exp - maxExp;
                LevelUp();
            }
            else
                m_expBar.value = m_exp / maxExp;
        }
    }
    public bool isHitBack = false;
    private float m_hitBackTime = 0.1f;
    public bool isDieCheck = false;
    private void Update()
    {
        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
        float currentRatio = stateinfo.normalizedTime * stateinfo.length;
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
        if (m_health.IsDie)
        {
            if (!isDieCheck)
            {
                m_animator.SetTrigger("Die");
                isDieCheck = true;
            }
            if (stateinfo.IsName("Die") && currentRatio >= stateinfo.length * 0.75f)
                m_animator.speed = 0.0f;
        }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitobj;
                if (Physics.Raycast(ray, out hitobj, 100))
                {
                    if (hitobj.transform.tag == "Monster" && !EventSystem.current.IsPointerOverGameObject())
                    {
                        if (m_collider != null)
                        {
                            m_collider.transform.Find("MarkerPivot/Marker.Crosshair").gameObject.SetActive(false);
                        }
                        m_collider = hitobj.transform.GetComponent<Collider>();
                    }
                }
            }
        }
#else

        if (m_health.IsDie)
        {
            if (!isDieCheck)
            {
                m_animator.SetTrigger("Die");
                isDieCheck = true;
            }
            if (stateinfo.IsName("Die") && currentRatio >= stateinfo.length * 0.75f)
                m_animator.speed = 0.0f;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitobj;
            if (Physics.Raycast(ray, out hitobj, 100))
            {
                if (hitobj.transform.tag == "Monster" && !EventSystem.current.IsPointerOverGameObject())
                {
                    if(m_collider != null)
                    {
                        m_collider.transform.Find("MarkerPivot/Marker.Crosshair").gameObject.SetActive(false);
                    }
                    m_collider = hitobj.transform.GetComponent<Collider>();
                }
            }
        }
#endif
        if(m_collider != null && m_collider.tag == "Monster")
        {
            m_collider.transform.Find("MarkerPivot/Marker.Crosshair").gameObject.SetActive(true);
        }
        if (m_collider != null&& m_collider.tag == "Monster" && (m_collider.GetComponent<MonsterHealth>().IsDie || !m_collider.gameObject.activeSelf))
        {
            m_collider.transform.Find("MarkerPivot/Marker.Crosshair").gameObject.SetActive(false);
            m_collider = null;

        }
        Exp();
        TotalState();
        m_charLevelText.text = m_charLevel.ToString();
        float hp = GetComponent<PlayerHealth>().GetHp;
        m_charHpbar.value = hp / m_charOriHp;
        m_charHpbar.transform.Find("Text").GetComponent<Text>().text = hp.ToString() + " / " + m_charOriHp.ToString();

        if (isHitBack)
        {
            if (m_hitBackTime < 0.0f)
            {
                m_controller.enabled = true;
                m_agent.enabled = true;
                isHitBack = false;
                m_hitBackTime = 0.1f;
            }
            else
            {
                m_agent.enabled = false;
                m_controller.enabled = false;
                transform.Translate(-Vector3.forward * Time.deltaTime * 8.0f);
                m_hitBackTime -= Time.deltaTime;
            }
        }
    }
}
    