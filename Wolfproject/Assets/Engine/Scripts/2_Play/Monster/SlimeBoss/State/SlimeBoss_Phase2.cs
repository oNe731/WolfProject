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

        // �̵��ӵ��� �ſ� ������
        m_owner.SpeedMax = 1f;
        m_owner.Speed = m_owner.SpeedMax;

        // �޴� ���ذ� �����Ѵ�.
        m_owner.DamageVariation = 0.5f;
    }

    public override void Update_State()
    {
        // ���� ü���� 70�ۼ�Ʈ ����, 40�ۼ�Ʈ ���Ͽ� ù ���� �ߴٸ� ����Ŀ ���·� ����
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

        // �÷��̾ ���� �ȱ�
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
        // �÷��̾�� ��⸸ �ص� ������ ���� �� �ִ�.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Player_K"))
        {
            m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_SLASH);
        }
    }
}
