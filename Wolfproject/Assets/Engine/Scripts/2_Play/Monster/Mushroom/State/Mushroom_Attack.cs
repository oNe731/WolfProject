using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Attack : Mushroom_Base
{
    private bool m_isAttack = false;

    public Mushroom_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_isAttack = false;
        m_owner.Animator.SetTrigger("Is_Attack");
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;
        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Attack") == true)
        {
            float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (m_isAttack == false && animTime >= 0.5f)
            {
                m_isAttack = true;
                if (Get_PlayerDistance() <= m_attackDistance)
                {
                    GameManager.Ins.Play.Player.Damaged_Player(m_owner.Damage);
                    GameManager.Ins.Play.Player.Start_Sturn(0.5f);
                }
            }
            if (animTime >= 1.0f)
                m_stateMachine.Change_State((int)Mushroom.STATE.ST_WAIT);
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
