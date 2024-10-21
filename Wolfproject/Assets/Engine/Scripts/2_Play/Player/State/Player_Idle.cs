using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle : Player_Base
{
    public Player_Idle(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // 애니메이션 변경
        m_owner.AM.SetTrigger("IsIdle");
    }

    public override void Update_State()
    {
        if(m_owner.Joystick.IsInput == true)
        {
            m_stateMachine.Change_State((int)Player.STATE.ST_WALK);
            return;
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
