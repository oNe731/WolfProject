using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dash : Player_Base
{
    float m_time = 0f;
    Vector2 m_direct;
    private Coroutine m_dashCoroutine = null;

    public Player_Dash(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Dash = true;
        m_owner.Invincibility = true; // 무적 상태 (적의 공격, 충돌 무시)

        m_time = 0f;
        m_direct = m_owner.Joystick.InputVector; // 조이스틱 입력 방향으로 대쉬
        m_owner.MoveSpeed = 10f;                 // 대쉬 이동 속도

        m_owner.AM.SetTrigger("IsDash");
    }

    public override void Update_State()
    {
        // 대쉬 이동
        m_time += Time.deltaTime;
        if(m_time > 0.2f)
        {
            m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
            return;
        }

        Move_Player(m_direct);
    }

    public override void Exit_State()
    {
        m_owner.Rb.velocity = Vector2.zero;
        m_owner.Dash = false;
        m_owner.Invincibility = false;

        Start_DashCoolTime();
    }

    public override void OnDrawGizmos()
    {
    }

    private void Start_DashCoolTime()
    {
        if (m_dashCoroutine != null)
            m_owner.StopCoroutine(m_dashCoroutine);
        m_dashCoroutine = m_owner.StartCoroutine(CoolTime_Dash());
    }

    private IEnumerator CoolTime_Dash()
    {
        m_owner.DashCool = true;

        float time = 0;
        while (time < 1f) // 1초
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_owner.DashCool = false;
        yield break;
    }
}
