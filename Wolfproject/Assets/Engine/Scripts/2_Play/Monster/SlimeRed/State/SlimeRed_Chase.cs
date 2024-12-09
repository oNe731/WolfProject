using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRed_Chase : SlimeRed_Base
{
    private float m_time = 0f;

    public SlimeRed_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_owner.Animator.SetTrigger("Is_Chase");
    }

    public override void Update_State()
    {
        if (Change_Attack() == false) // 거리에 따른 공격 상태 전환
        {
            m_time += Time.deltaTime;
            if(m_time >= 5f)
            {
                m_stateMachine.Change_State((int)SlimeRed.STATE.ST_BOMB);
                return;
            }

            // 플레이어 추격
            Vector2 direction = ((Vector2)GameManager.Ins.Play.Player.transform.position - (Vector2)m_owner.transform.position).normalized;
            m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);

            if (direction.x > 0f)
                m_owner.SpriteRenderer.flipX = true;
            else
                m_owner.SpriteRenderer.flipX = false;
        }
    }

    public override void Exit_State()
    {
    }
}
