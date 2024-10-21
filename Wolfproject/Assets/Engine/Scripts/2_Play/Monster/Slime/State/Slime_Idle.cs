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
         * Idle (대기 상태)
행동: 무작위로 천천히 맵을 배회하며 플레이어를 탐색.
전환 조건: 플레이어가 일정 거리 내에 들어오면 **Chase(추격 상태)**로 전환.
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
