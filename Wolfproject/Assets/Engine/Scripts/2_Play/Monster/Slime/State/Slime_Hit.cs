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
        /*
         * �⺻ ����
����: ���� �Ӽ��� ����.
����: ȭ�� �Ӽ��� ����.

         * Hit (�ǰ� ����)
�ൿ: ü���� ���� ���Ϸ� �������� �ӵ��� �������� ���� �󵵰� ������.
��ȯ ����: ���� Ȯ���� ����ȭ(Berserk) ���·� ��ȯ.
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
