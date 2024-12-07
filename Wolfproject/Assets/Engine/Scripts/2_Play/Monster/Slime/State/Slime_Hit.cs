using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Hit : Slime_Base
{
    private float m_WaitTime = 0f;

    public Slime_Hit(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_WaitTime = 0f;
        //Debug.Log("피격 시작");
        if(m_stateMachine.PreState == (int)Slime.STATE.ST_HIT)
            m_stateMachine.Change_State((int)Slime.STATE.ST_IDLE);
    }

    public override void Update_State()
    {
        m_WaitTime += Time.deltaTime;
        if(m_WaitTime >= 1f)
        {
            m_stateMachine.Change_State((int)Slime.STATE.ST_IDLE);
        }
    }

    public override void Exit_State()
    {
        //Debug.Log("피격 대기 종료");
    }

    public override void OnDrawGizmos()
    {
    }
}
