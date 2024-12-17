using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Phase2 : SlimeBoss_Base
{
    private SlimeBoss_Berserk m_berserk;
    private float m_stateTime = 0f;

    public SlimeBoss_Phase2(StateMachine<Monster> stateMachine, SlimeBoss_Berserk berserkState) : base(stateMachine)
    {
        m_berserk = berserkState;
    }

    public override void Enter_State()
    {
        m_stateTime = 0f;
        m_owner.Animator.SetTrigger("Is_Walk");
        m_owner.Play_AudioSource("KingSlime_Move", false, 1f, GameManager.Ins.Sound.EffectSound);

        // 이동속도가 매우 느려짐
        m_owner.SpeedMax = 1f;
        m_owner.Speed = m_owner.SpeedMax;

        // 받는 피해가 감소한다.
        m_owner.DamageVariation = 0.5f;
    }

    public override void Update_State()
    {
        // 만약 체력이 70퍼센트 이하, 40퍼센트 이하에 첫 돌입 했다면 버서커 상태로 돌입
        if (m_berserk.StateCount == 0)
        {
            if (m_owner.Hp <= m_owner.HpMax * 0.7f)
            {
                m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_BERSERK);
                return;
            }
        }
        else if (m_berserk.StateCount == 1)
        {
            if (m_owner.Hp <= m_owner.HpMax * 0.4f)
            {
                m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_BERSERK);
                return;
            }
        }

        m_stateTime += Time.deltaTime;
        if(m_stateTime >= 2f)
        {
            if (Get_PlayerDistance() > m_chaseDistance)
                m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_CHASE);
            else
                m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_SLASH);
            return;
        }

        // 플레이어를 향해 걷기
        Vector2 direction = ((Vector2)GameManager.Ins.Play.Player.transform.position - (Vector2)m_owner.transform.position).normalized;
        m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);
        m_owner.SpriteRenderer.flipX = direction.x > 0f;
    }

    public override void Exit_State()
    {
        m_owner.SpriteRenderer.color = Color.white;
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        // 플레이어에게 닿기만 해도 공격을 입힐 수 있다.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Player_K"))
        {
            m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_SLASH);
        }
    }
}
