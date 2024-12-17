using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Berserk : SlimeBoss_Base
{
    private int m_stateCount = 0;
    public int StateCount => m_stateCount;

    public SlimeBoss_Berserk(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_stateCount++;
        m_owner.Animator.SetTrigger("Is_Conversion");
        m_owner.Play_AudioSource("KingSlime_Berserk", false, 1f, GameManager.Ins.Sound.EffectSound);
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Conversion") == true)
        {
            float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1f)
            {
                m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_SUMMON);
            }
        }
    }

    public override void Exit_State()
    {
    }
}
