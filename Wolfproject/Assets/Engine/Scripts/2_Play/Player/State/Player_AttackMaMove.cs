using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackMaMove : Player_Attack
{
    private float m_distance = 1f;

    public Player_AttackMaMove(StateMachine<Player> stateMachine) : base(stateMachine, 0)
    {
        m_coolTime = 0f;
        m_animationName = "IsAttackMaMove";

        m_damage = 2f;
    }

    public override void Enter_State()
    {
        base.Enter_State();
    }

    public override void Update_State()
    {
        if (m_owner.AM.IsInTransition(0) == true)
            return;

        if (m_owner.AM.GetCurrentAnimatorStateInfo(0).IsName(m_animationName) == true)
        {
            float animTime = m_owner.AM.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime > 0.5)
            {
                if (m_attack == false)
                {
                    m_attack = true;
                    Check_AttackCollision(m_attackCollider);
                }
                else if (animTime >= 1.0f) // 애니메이션 종료
                {
                    m_owner.Rb.velocity = Vector2.zero;
                    m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
                    return;
                }

                m_owner.Rb.velocity = m_direct * 3f;
            }
        }
    }

    public override void Exit_State()
    {
        base.Exit_State();
    }

    public override void OnDrawGizmos()
    {
    }

    private void Check_AttackCollision(BoxCollider2D collider)
    {
        m_owner.Play_AudioSource("Player_Melee", false, 1f, 1f);

        // 특정 범위 안에 있는 모든 콜라이더를 가져옴 // OverlapCircle : 원 형태의 범위, 2D 물리 시스템
        Set_ColliderDirection();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(collider.transform.position, m_distance, LayerMask.GetMask("Monster"));
        foreach (Collider2D hitCollider in hitColliders)
        {
            Monster monster = hitCollider.gameObject.GetComponent<Monster>();
            if (monster != null)
                monster.Damaged_Monster(m_damage, true, 5f);
        }

        // 이펙트 생성
        GameObject obj = GameManager.Ins.LoadCreate("4_Prefab/5_Effect/Attack2", m_owner.transform);
        if (obj != null)
        {
            obj.transform.position = new Vector3(m_effectPoint.position.x + Random.Range(-0.05f, 0.05f), m_effectPoint.position.y + Random.Range(-0.05f, 0.05f), m_effectPoint.position.z);
            obj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

            // 방향 설정
            DIRECTION dirName = m_owner.Get_Direction();
            if (dirName == DIRECTION.DT_RIGHT || dirName == DIRECTION.DT_UP)
                obj.GetComponent<SpriteRenderer>().flipX = true;
            if (dirName == DIRECTION.DT_DOWN || dirName == DIRECTION.DT_UP)
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + 0.1f, obj.transform.position.z);
        }
    }
}
