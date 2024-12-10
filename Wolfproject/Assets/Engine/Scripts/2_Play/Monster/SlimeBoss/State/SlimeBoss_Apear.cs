using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Apear : SlimeBoss_Base
{
    public SlimeBoss_Apear(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Animator.SetTrigger("Is_Idle");
    }

    public override void Update_State()
    {
        if (Get_PlayerDistance() <= m_talkDistance)
        {
            m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_IDLE);
            GameManager.Ins.Play.Dialog.Start_Dialog("7_Data/Boss/Dialog_BossMeet");
        }
    }

    public override void Exit_State()
    {
    }
}
