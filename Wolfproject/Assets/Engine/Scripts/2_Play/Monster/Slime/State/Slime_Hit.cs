using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Hit : Slime_Base
{
    public Slime_Hit(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        //Debug.Log("�ǰ�");

        // �÷��̾� ���� Ÿ��
        Player player = GameManager.Ins.Play.Player;
        switch (player.AttributeType)
        {
            case Player.ATTRIBUTETYPE.AT_FIRE: // ȭ�� �Ӽ��� ����
                m_owner.Damaged_Monster(player.Damage * 0.5f);
                break;

            case Player.ATTRIBUTETYPE.AT_THUNDER: // ���� �Ӽ��� ����
                m_owner.Damaged_Monster(player.Damage * 1.5f);
                break;
        }

        if (m_owner.Hp <= m_owner.HpMax / 2)
        {
            // ���� Ȯ���� ����ȭ ���·� ��ȯ
            int index = Random.Range(0, 2);
            if (index == 0)
                m_stateMachine.Change_State((int)Slime.STATE.ST_BERSERK);
            else
                m_stateMachine.Change_State((int)Slime.STATE.ST_IDLE);
        }
        else
        {
            m_stateMachine.Change_State((int)Slime.STATE.ST_IDLE);
        }
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
