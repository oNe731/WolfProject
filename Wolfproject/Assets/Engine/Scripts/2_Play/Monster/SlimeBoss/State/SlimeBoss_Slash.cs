using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Slash : SlimeBoss_Base
{
    private float m_jumpInterval = 1f;  // 점프 간격
    private float m_jumpHeight = 2f;      // 점프 높이

    private Vector2 m_jumpStartPos;       // 점프 시작 위치
    private Vector2 m_jumpTargetPos;      // 점프 목표 위치

    private float m_jumpProgress = 0f;    // 점프 진행도 (0~1)

    private bool m_attack = false;
    private float m_distance = 1.5f;

    private BoxCollider2D m_ownerCollider;
    private BoxCollider2D m_ownerChildCollider;
    private BoxCollider2D m_attackCollider;
    private Transform m_effectPoint;

    private float m_stateTime = 0f;

    public SlimeBoss_Slash(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_ownerCollider = m_owner.GetComponent<BoxCollider2D>();
        m_ownerChildCollider = m_owner.transform.GetChild(0).GetComponent<BoxCollider2D>();

        m_attackCollider = m_owner.transform.GetChild(1).GetComponent<BoxCollider2D>();
        m_effectPoint = m_owner.transform.GetChild(1).GetChild(0);
    }

    public override void Enter_State()
    {
        m_owner.Animator.SetTrigger("Is_Slash");

        m_jumpStartPos = m_owner.transform.position;
        m_jumpTargetPos = GameManager.Ins.Play.Player.transform.position + new Vector3(0f, 0.3f, 0f); // 플레이어를 목표로 점프
        m_jumpProgress = 0f; // 점프 진행도 초기화

        m_attack = false;
        m_ownerCollider.isTrigger = true;
        m_ownerChildCollider.isTrigger = true;
        m_stateTime = 0f;

        //Debug.Log("보스 공격");
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        m_jumpProgress += Time.deltaTime / m_jumpInterval;
        if (m_jumpProgress >= 1f)
        {
            m_owner.transform.position = m_jumpTargetPos; // 목표 위치에 도달
            if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Slash") == true)
            {
                float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime > 0.7)
                {
                    if(m_attack == false)
                    {
                        m_attack = true;
                        Check_AttackCollision(m_attackCollider);
                    }
                    else
                    {
                        m_stateTime += Time.deltaTime;
                        if(m_stateTime >= 0.5f)
                            m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_PHASE1);
                    }
                }
            }
            return;
        }

        // 포물선 경로 계산
        float x = Mathf.Lerp(m_jumpStartPos.x, m_jumpTargetPos.x, m_jumpProgress);
        float y = Mathf.Lerp(m_jumpStartPos.y, m_jumpTargetPos.y, m_jumpProgress) + m_jumpHeight * Mathf.Sin(m_jumpProgress * Mathf.PI);
        m_owner.transform.position = new Vector2(x, y);
    }

    public override void Exit_State()
    {
        m_ownerCollider.isTrigger = false;
        m_ownerChildCollider.isTrigger = false;
    }

    private void Check_AttackCollision(BoxCollider2D collider)
    {
        // 특정 범위 안에 있는 모든 콜라이더를 가져옴 // OverlapCircle : 원 형태의 범위, 2D 물리 시스템
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(collider.transform.position, m_distance, LayerMask.GetMask("Player"));
        foreach (Collider2D hitCollider in hitColliders)
        {
            Player player = hitCollider.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Damaged_Player(m_owner.Damage);
                player.Start_Sturn(1f);
            }
        }

        // 이펙트 생성
        GameObject obj = GameManager.Ins.LoadCreate("4_Prefab/5_Effect/Slash");
        if (obj != null)
            obj.transform.position = m_effectPoint.position;
    }
}
