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
        /*
         * Berserk (����ȭ ���� - �ɼ�)
�ൿ: �̵� �ӵ��� ���� �ӵ��� �����ϸ�, �÷��̾�� ���� �� ū ���ظ� ����.
��ȯ ����: ���� �ð� �� �ٽ� Idle ���·� ���ư�.
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
