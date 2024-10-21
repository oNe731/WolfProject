using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Idle : Slime_Base
{
    public Slime_Idle(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        /*
         * Idle (��� ����)
�ൿ: �������� õõ�� ���� ��ȸ�ϸ� �÷��̾ Ž��.
��ȯ ����: �÷��̾ ���� �Ÿ� ���� ������ **Chase(�߰� ����)**�� ��ȯ.
         */
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
