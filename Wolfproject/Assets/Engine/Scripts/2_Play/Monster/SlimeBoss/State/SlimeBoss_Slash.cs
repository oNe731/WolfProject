using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Slash : SlimeBoss_Base
{
    public SlimeBoss_Slash(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Animator.SetTrigger("Is_Slash");
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Slash") == true)
        {
            float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1.0f)
            {
                m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_PHASE1);
            }
        }
    }

    public override void Exit_State()
    {
    }
}
