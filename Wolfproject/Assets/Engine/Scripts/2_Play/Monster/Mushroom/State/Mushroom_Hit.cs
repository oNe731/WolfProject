using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Hit : Mushroom_Base
{
    private float m_WaitTime = 0f;

    public Mushroom_Hit(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_WaitTime = 0f;
        m_owner.Animator.SetTrigger("Is_Idle");

        //Debug.Log("�ǰ� ����");

        m_owner.Play_AudioSource("Mushroom_Hit", false, 1f, GameManager.Ins.Sound.EffectSound);
        //Debug.Log("���� ���4");
    }

    public override void Update_State()
    {
        m_WaitTime += Time.deltaTime;
        if (m_WaitTime >= 1f)
        {
            m_stateMachine.Change_State((int)Mushroom.STATE.ST_RUNAWAY);
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
