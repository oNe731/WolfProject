using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Wait : Mushroom_Base
{
    private float m_time = 0f;
    private float m_waitTime = 0f;

    public Mushroom_Wait(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_waitTime = Random.Range(1f, 1.5f);
        m_owner.Animator.SetTrigger("Is_Idle");
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        m_time += Time.deltaTime;
        if (m_time > m_waitTime)
        {
            m_stateMachine.Change_State((int)Mushroom.STATE.ST_CHASE);
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
