using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hit : Player_Base
{
    float m_time = 0f;

    public Player_Hit(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_owner.AM.SetTrigger("IsHit");
    }

    public override void Update_State()
    {
        m_time += Time.deltaTime;
        if(m_time > 0.1f)
        {
            m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
