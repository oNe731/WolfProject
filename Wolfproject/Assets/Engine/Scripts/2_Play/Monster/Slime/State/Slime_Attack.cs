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
         * Jump Attack (점프 공격 상태)
행동: 슬라임이 근접한 플레이어를 향해 점프 공격.
공격 성공 시 0.5초 스턴 부여.
전환 조건: 공격 후 다시 Chase 상태로 돌아감.
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
