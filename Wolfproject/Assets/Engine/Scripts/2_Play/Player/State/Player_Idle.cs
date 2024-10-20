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
    }

    public override void Update_State()
    {
        if(m_owner.Joystick.IsInput == true)
        {
            m_stateMachine.Change_State((int)Player.STATE.ST_WALK);
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
