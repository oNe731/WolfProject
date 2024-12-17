using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Hit : SlimeBoss_Base
{
    private float m_WaitTime = 0f;

    public SlimeBoss_Hit(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_WaitTime = 0f;
        m_owner.Animator.SetTrigger("Is_Idle");
        m_owner.Play_AudioSource("KingSlime_Hit", false, 1f, GameManager.Ins.Sound.EffectSound);
    }

    public override void Update_State()
    {
        m_WaitTime += Time.deltaTime;
        if (m_WaitTime >= 1f)
        {
            m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_IDLE);
        }
    }

    public override void Exit_State()
    {
    }
}
