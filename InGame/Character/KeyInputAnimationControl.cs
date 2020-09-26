using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputAnimationControl: MonoBehaviour
{
    Animator m_animator;

    int m_comboCount = 0;
    
    void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void UpdateAttack()
    {
        // 0번째 레이어에서 실행되고 있는 애니메이션 정보값을 받습니다. ( 실행되고 있는 노드의 정보값 )
        AnimatorStateInfo stateInfo =  m_animator.GetCurrentAnimatorStateInfo(0);
        //if (!stateInfo.IsName("Idle"))
        //    Debug.Log(stateInfo.normalizedTime);

        if (!stateInfo.IsName("Idle"))
        {
            // stateInfo.normalizedTime은 현재 실행되고 있는 애니메이션의 
            // 실행되고 있는 비율값을 의미합니다.

            // 한 애니메이션이 동작이 끝날때까지 다른 애니메이션을 실행한 상태가
            // 아니라면, 대기 모션으로 돌아가도록 처리한 코드입니다.
            if (stateInfo.normalizedTime > ( stateInfo.length ) )
            {

                // 현재 애니메이션이 다른 상태로 변경중인 상태라면
                // 실행되지 않도록 처리하였습니다.
                if (m_animator.IsInTransition(0))
                    return;
                m_comboCount = 0;
                m_animator.SetInteger("Combo", m_comboCount);
                m_animator.SetTrigger("Idle");
            }
        }
        //else
        //{
        //    m_comboCount = 0;
        //    m_animator.SetInteger("Combo", m_comboCount);
        //}


        if ( Input.GetKey(KeyCode.Space) )
        {
            if(stateInfo.IsName("Idle") )
            {
                m_comboCount = 1;
                m_animator.SetInteger("Combo", m_comboCount);
            }
            else
            {
                if(stateInfo .normalizedTime > stateInfo.length * 0.8f )
                {
                    if (m_animator.IsInTransition(0))
                        return;

                    ++m_comboCount;
                    m_animator.SetInteger("Combo", m_comboCount);
                }
            }
           
           
            
        }
        
    }
}
