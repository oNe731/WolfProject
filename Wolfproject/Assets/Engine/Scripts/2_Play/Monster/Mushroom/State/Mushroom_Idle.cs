using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Idle : Mushroom_Base
{
    private float m_time = 0f;
    private float m_waitTime;

    public Mushroom_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_waitTime = Random.Range(1f, 2f);

        m_owner.Animator.SetTrigger("Is_Idle");
    }

    public override void Update_State()
    {
        if (Change_Stealth() == false) // 거리에따른 잠복 상태 전환
        {
            m_time += Time.deltaTime;
            if (m_time >= m_waitTime)
            {
                m_stateMachine.Change_State((int)Mushroom.STATE.ST_WALK);
            }
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
