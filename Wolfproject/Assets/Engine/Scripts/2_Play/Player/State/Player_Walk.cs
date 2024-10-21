using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walk : Player_Base
{
    public Player_Walk(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // �ִϸ��̼� ����
        m_owner.AM.SetTrigger("IsWalk");
    }

    public override void Update_State()
    {
        if (m_owner.Joystick.IsInput == false)
        {
            m_owner.Rb.velocity = Vector2.zero;
            m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
            return;
        }
        else
        {
            if (m_owner.Stamina > 0)
                m_owner.MoveSpeed = 5f;        // �⺻ �̵� �ӵ�
            else
                m_owner.MoveSpeed = 5f * 0.8f; // 20% ����
            Move_Player(m_owner.Joystick.InputVector);
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}