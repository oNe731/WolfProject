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
        // 이동  속도 및 공격 속도, 빈도 증가
        m_owner.Speed = m_owner.SpeedMax * 1.5f;
        // 플레이어에게 접촉 시 큰 피해를 입힘

        // 일정 시간 후
        m_stateMachine.Change_State((int)Slime.STATE.ST_IDLE);
        //Debug.Log("광폭화");
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
