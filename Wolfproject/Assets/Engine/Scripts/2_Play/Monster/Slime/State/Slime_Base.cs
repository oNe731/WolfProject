using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Base : State<Monster>
{
    protected Slime m_owner;

    public Slime_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Slime>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
