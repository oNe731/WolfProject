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
        Chase (추격 상태)
행동: 슬라임이 직선으로 느리게 플레이어를 추격하며, 장애물을 넘기 위해 주기적으로 점프함.
전환 조건: 일정 시간 후 플레이어와의 거리 좁힘 시 **Jump Attack(점프 공격 상태)**으로 전환.
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
