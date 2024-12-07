using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Chase : Mushroom_Base
{
    public Mushroom_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.SpeedMax = 4f;
        m_owner.Speed = m_owner.SpeedMax / 2.5F;
        m_owner.Animator.SetTrigger("Is_Chase");
    }

    public override void Update_State()
    {
        if (Change_Attack() == false) // �Ÿ��� ���� ���� ���� ��ȯ
        {
            // �÷��̾� �߰�
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

    public override void OnDrawGizmos()
    {
    }
}
