using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_WakeUp : Mushroom_Base
{
    public Mushroom_WakeUp(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Animator.SetTrigger("Is_WakeUp");
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_WakeUp") == true)
        {
            float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1f)
            {
                m_stateMachine.Change_State((int)Mushroom.STATE.ST_CHASE);
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
