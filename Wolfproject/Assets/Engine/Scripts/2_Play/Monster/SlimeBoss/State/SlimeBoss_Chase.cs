using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Chase : SlimeBoss_Base
{
    private bool m_isJumping = false;     // 점프 상태 플래그
    private bool m_isWalking = true;      // 걷기 상태 플래그

    private float m_jumpInterval = 0.5f;  // 점프 간격
    private float m_jumpHeight = 1f;      // 점프 높이
    private float m_jumpTimer = 0f;       // 점프 타이머

    private Vector2 m_jumpStartPos;       // 점프 시작 위치
    private Vector2 m_jumpTargetPos;      // 점프 목표 위치
    private float m_jumpProgress = 0f;    // 점프 진행도 (0~1)

    private float m_walkDuration = 0.5f;  // 걷는 시간

    public SlimeBoss_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_isWalking = false;
        m_isJumping = false;

        m_owner.SpeedMax = 5f;
        m_owner.Speed = m_owner.SpeedMax;

        m_owner.Animator.SetTrigger("Is_Chase");
        m_owner.Play_AudioSource("KingSlime_Move", false, 1f, GameManager.Ins.Sound.EffectSound);

        m_jumpTimer = m_walkDuration; // 처음엔 걷기부터 시작

        //Debug.Log("보스 추격");
    }

    public override void Update_State()
    {
        if (Change_Attack() == false) // 거리에 따른 공격 상태 전환
        {
            if (m_isWalking)
            {
                WalkTowardsPlayer();
            }
            else if (m_isJumping)
            {
                PerformJump();
            }
            else
            {
                StartJump();
            }
        }
    }

    public override void Exit_State()
    {
    }

    private void WalkTowardsPlayer()
    {
        m_jumpTimer -= Time.deltaTime;

        // 플레이어를 향해 걷기
        Vector2 direction = ((Vector2)GameManager.Ins.Play.Player.transform.position - (Vector2)m_owner.transform.position).normalized;
        m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);

        // 스프라이트 방향 조정
        m_owner.SpriteRenderer.flipX = direction.x > 0f;

        // 걷기 시간이 끝나면 점프 준비
        if (m_jumpTimer <= 0f)
        {
            m_isWalking = false;
        }
    }

    private void StartJump()
    {
        Vector2 direction = ((Vector2)GameManager.Ins.Play.Player.transform.position - (Vector2)m_owner.transform.position).normalized;

        m_isJumping = true;
        m_jumpStartPos = m_owner.transform.position;
        m_jumpTargetPos = m_owner.Rigidbody2D.position + direction * 2f; // 플레이어를 목표로 점프
        m_jumpProgress = 0f; // 점프 진행도 초기화
        m_jumpTimer = m_jumpInterval; // 점프 후 걷기 시간 리셋
    }

    private void PerformJump()
    {
        m_jumpProgress += Time.deltaTime / m_jumpInterval;

        if (m_jumpProgress >= 1f)
        {
            m_isJumping = false;
            m_isWalking = true; // 점프 후 걷기로 전환
            m_owner.transform.position = m_jumpTargetPos; // 목표 위치에 도달
            m_jumpTimer = m_walkDuration; // 걷기 시간 리셋
            return;
        }

        // 포물선 경로 계산
        float x = Mathf.Lerp(m_jumpStartPos.x, m_jumpTargetPos.x, m_jumpProgress);
        float y = Mathf.Lerp(m_jumpStartPos.y, m_jumpTargetPos.y, m_jumpProgress) + m_jumpHeight * Mathf.Sin(m_jumpProgress * Mathf.PI);

        m_owner.transform.position = new Vector2(x, y);
    }
}
