using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackNear : Player_Attack
{
    private bool m_attack = false;
    private float m_distance = 1f;
    private BoxCollider2D m_attackCollider;

    public Player_AttackNear(StateMachine<Player> stateMachine) : base(stateMachine, 0)
    {
        m_coolTime = 1f;

        m_animationName = "IsAttackNear";
        m_attackCollider = m_owner.transform.GetChild(1).GetComponent<BoxCollider2D>();
    }

    public override void Enter_State()
    {
        base.Enter_State();
        m_attack = false;
        m_owner.AM.SetTrigger(m_animationName);
    }

    public override void Update_State()
    {
        if (m_owner.AM.IsInTransition(0) == true) 
            return;

        if (m_owner.AM.GetCurrentAnimatorStateInfo(0).IsName(m_animationName) == true)
        {
            float animTime = m_owner.AM.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if(m_attack == false && animTime > 0.5)
            {
                m_attack = true;
                Check_AttackCollision(m_attackCollider);
            }
            else if (animTime >= 1.0f) // 애니메이션 종료
                m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
        }
    }

    public override void Exit_State()
    {
        base.Exit_State();
    }

    public override void OnDrawGizmos()
    {
        if (m_attackCollider == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_attackCollider.transform.position, m_distance);
    }

    private void Check_AttackCollision(BoxCollider2D collider)
    {
        // 특정 범위 안에 있는 모든 콜라이더를 가져옴 // OverlapCircle : 원 형태의 범위, 2D 물리 시스템
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(collider.transform.position, m_distance, LayerMask.GetMask("Monster"));
        foreach (Collider2D hitCollider in hitColliders)
        {
            Monster monster = hitCollider.gameObject.GetComponent<Monster>();
            if (monster != null)
                monster.Damaged_Monster(m_damage);
        }
    }
}
