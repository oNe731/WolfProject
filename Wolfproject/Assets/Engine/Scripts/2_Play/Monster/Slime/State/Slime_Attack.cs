using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Attack : Slime_Base
{
    public Slime_Attack(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        /*
         * Jump Attack (���� ���� ����)
�ൿ: �������� ������ �÷��̾ ���� ���� ����.
���� ���� �� 0.5�� ���� �ο�.
��ȯ ����: ���� �� �ٽ� Chase ���·� ���ư�.
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
