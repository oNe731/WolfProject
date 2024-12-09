using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRed_Idle : SlimeRed_Base
{
    public SlimeRed_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Rigidbody2D.velocity = Vector2.zero;
        m_owner.Animator.SetTrigger("Is_Idle");
    }

    public override void Update_State()
    {
        m_stateMachine.Change_State((int)SlimeRed.STATE.ST_CHASE);
    }

    public override void Exit_State()
    {
    }
}
