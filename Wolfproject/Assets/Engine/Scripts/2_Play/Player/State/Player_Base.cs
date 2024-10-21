using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : State<Player>
{
    protected Player m_owner;
    protected float m_coolTime;

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

    protected void Move_Player(Vector2 direct)
    {
        m_owner.Rb.velocity = direct * m_owner.MoveSpeed;
        if (m_owner.Joystick.InputVector.x < 0)
            m_owner.Sr.flipX = false;
        else if (m_owner.Joystick.InputVector.x > 0)
            m_owner.Sr.flipX = true;
    }
}
