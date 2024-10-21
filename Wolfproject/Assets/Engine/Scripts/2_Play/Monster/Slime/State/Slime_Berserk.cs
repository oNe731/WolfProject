using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Berserk : Slime_Base
{
    public Slime_Berserk(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // �̵�  �ӵ� �� ���� �ӵ�, �� ����
        m_owner.Speed = m_owner.SpeedMax * 1.5f;
        // �÷��̾�� ���� �� ū ���ظ� ����

        // ���� �ð� ��
        m_stateMachine.Change_State((int)Slime.STATE.ST_IDLE);
        //Debug.Log("����ȭ");
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
