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
         * 기본 정보
약점: 번개 속성에 약함.
저항: 화염 속성에 강함.

         * Hit (피격 상태)
행동: 체력이 절반 이하로 떨어지면 속도가 빨라지고 공격 빈도가 높아짐.
전환 조건: 일정 확률로 광폭화(Berserk) 상태로 전환.
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
