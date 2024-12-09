using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Dead : SlimeBoss_Base
{
    private float m_time = 0f;

    public SlimeBoss_Dead(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Rigidbody2D.velocity = Vector2.zero;
        m_owner.Animator.SetTrigger("Is_Dead");
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Dead") == true)
        {
            float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1.0f)
            {
                m_time += Time.deltaTime;
                if (m_time > 0.5f)
                {
                    GameManager.Ins.Destroy(m_owner.gameObject);
                    GameManager.Ins.Play.Clear_Game();
                }
            }
        }
    }

    public override void Exit_State()
    {
    }
}
