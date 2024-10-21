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
         * Berserk (광폭화 상태 - 옵션)
행동: 이동 속도와 공격 속도가 증가하며, 플레이어에게 접촉 시 큰 피해를 입힘.
전환 조건: 일정 시간 후 다시 Idle 상태로 돌아감.
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
