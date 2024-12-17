using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Dialog : Slime_Base
{
    public Slime_Dialog(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Animator.SetTrigger("Is_Idle");
        m_owner.Play_AudioSource("Slime_Idle", false, 1f, GameManager.Ins.Sound.EffectSound);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
        m_owner.Stop_AudioSource();
    }

    public override void OnDrawGizmos()
    {
    }
}
