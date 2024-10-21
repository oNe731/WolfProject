using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Idle : Slime_Base
{

    private Vector2 m_randomPosition;
    private float m_time = 0f;

    public Slime_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_randomPosition = m_owner.spawner.Get_RandomPosition();

        m_owner.SpeedMax = 2f;
        m_owner.Speed = m_owner.SpeedMax;
        m_owner.Animator.SetBool("Is_Idle", true);
        //Debug.Log("아이들");
    }

    public override void Update_State()
    {
        if(Change_Chase() == false) // 거리에따른 추격 상태 전환
        {
            // 맵을 배회
            m_time += Time.deltaTime;
            if (m_time > 1.5f || Vector2.Distance(m_owner.transform.position, m_randomPosition) < 0.1f)
            {
                m_time = 0f;
                m_randomPosition = m_owner.spawner.Get_RandomPosition();
            }

            Vector2 direction = (m_randomPosition - (Vector2)m_owner.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(m_owner.transform.position, direction, Vector2.Distance(m_owner.transform.position, m_randomPosition), LayerMask.GetMask("Installation", "Player", "Monster"));
            if (hit.collider != null && hit.collider != m_owner.Collider2D)
            {
                m_time = 0f;
                m_randomPosition = m_owner.spawner.Get_RandomPosition();
            }

            //m_owner.transform.position = Vector2.MoveTowards(m_owner.transform.position, m_randomPosition, m_owner.Speed * Time.deltaTime);
            m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);
        }

        if (m_owner.Animator.IsInTransition(0) == true) 
            return;
        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Idle") == true)
            m_owner.Animator.SetBool("Is_Idle", false);
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
