using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : State<Player>
{
    protected Player m_owner;

    public Player_Base(StateMachine<Player> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Player>();
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
