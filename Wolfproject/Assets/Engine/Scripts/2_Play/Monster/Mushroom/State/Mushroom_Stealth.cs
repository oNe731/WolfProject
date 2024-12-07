using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Stealth : Mushroom_Base
{
    public Mushroom_Stealth(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Animator.SetTrigger("Is_Hide");
    }

    public override void Update_State()
    {
        Change_Chase();
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
