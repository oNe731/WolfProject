using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Chase : SlimeBoss_Base
{
    private bool m_isJumping = false;     // ���� ���� �÷���
    private bool m_isWalking = true;      // �ȱ� ���� �÷���

    private float m_jumpInterval = 0.5f;  // ���� ����
    private float m_jumpHeight = 1f;      // ���� ����
    private float m_jumpTimer = 0f;       // ���� Ÿ�̸�

    private Vector2 m_jumpStartPos;       // ���� ���� ��ġ
    private Vector2 m_jumpTargetPos;      // ���� ��ǥ ��ġ
    private float m_jumpProgress = 0f;    // ���� ���൵ (0~1)

    private float m_walkDuration = 0.5f;  // �ȴ� �ð�

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

        m_jumpTimer = m_walkDuration; // ó���� �ȱ���� ����

        //Debug.Log("���� �߰�");
    }

    public override void Update_State()
    {
        if (Change_Attack() == false) // �Ÿ��� ���� ���� ���� ��ȯ
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

        // �÷��̾ ���� �ȱ�
        Vector2 direction = ((Vector2)GameManager.Ins.Play.Player.transform.position - (Vector2)m_owner.transform.position).normalized;
        m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);

        // ��������Ʈ ���� ����
        m_owner.SpriteRenderer.flipX = direction.x > 0f;

        // �ȱ� �ð��� ������ ���� �غ�
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
        m_jumpTargetPos = m_owner.Rigidbody2D.position + direction * 2f; // �÷��̾ ��ǥ�� ����
        m_jumpProgress = 0f; // ���� ���൵ �ʱ�ȭ
        m_jumpTimer = m_jumpInterval; // ���� �� �ȱ� �ð� ����
    }

    private void PerformJump()
    {
        m_jumpProgress += Time.deltaTime / m_jumpInterval;

        if (m_jumpProgress >= 1f)
        {
            m_isJumping = false;
            m_isWalking = true; // ���� �� �ȱ�� ��ȯ
            m_owner.transform.position = m_jumpTargetPos; // ��ǥ ��ġ�� ����
            m_jumpTimer = m_walkDuration; // �ȱ� �ð� ����
            return;
        }

        // ������ ��� ���
        float x = Mathf.Lerp(m_jumpStartPos.x, m_jumpTargetPos.x, m_jumpProgress);
        float y = Mathf.Lerp(m_jumpStartPos.y, m_jumpTargetPos.y, m_jumpProgress) + m_jumpHeight * Mathf.Sin(m_jumpProgress * Mathf.PI);

        m_owner.transform.position = new Vector2(x, y);
    }
}
