using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Walk : SlimeBoss_Base
{
    private Vector2 m_randomPosition;
    private float m_time = 0f;

    private float m_stateTime = 0f;
    private float m_changeTime = 0f;

    public SlimeBoss_Walk(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_stateTime = 0f;
        m_changeTime = Random.Range(2f, 3f);

        m_randomPosition = m_owner.spawner.Get_RandomPosition();

        m_owner.SpeedMax = 2f;
        m_owner.Speed = m_owner.SpeedMax;

        m_owner.Animator.SetTrigger("Is_Walk");
    }

    public override void Update_State()
    {
        if (Change_Chase() == false)
        {
            m_stateTime += Time.deltaTime;
            if (m_stateTime >= m_changeTime)
            {
                m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_IDLE);
                return;
            }

            // ¸ÊÀ» ¹èÈ¸
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

            if (direction.x > 0f)
                m_owner.SpriteRenderer.flipX = true;
            else
                m_owner.SpriteRenderer.flipX = false;
        }
    }

    public override void Exit_State()
    {
    }
}
