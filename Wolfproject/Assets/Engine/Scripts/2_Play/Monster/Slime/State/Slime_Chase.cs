using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Chase : Slime_Base
{
    public Slime_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        /*
        Chase (�߰� ����)
�ൿ: �������� �������� ������ �÷��̾ �߰��ϸ�, ��ֹ��� �ѱ� ���� �ֱ������� ������.
��ȯ ����: ���� �ð� �� �÷��̾���� �Ÿ� ���� �� **Jump Attack(���� ���� ����)**���� ��ȯ.
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
