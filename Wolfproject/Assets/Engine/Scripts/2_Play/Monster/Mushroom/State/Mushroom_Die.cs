using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Die : Mushroom_Base
{
    private float m_time = 0f;

    public Mushroom_Die(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.Rigidbody2D.velocity = Vector2.zero;
        m_owner.Animator.SetTrigger("Is_Dead");

        m_owner.Play_AudioSource("Mushroom_Death", false, 1f, GameManager.Ins.Sound.EffectSound);
        //Debug.Log("사운드 재생3");
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
                    m_owner.Create_Item();
                    GameManager.Ins.Destroy(m_owner.gameObject);
                }
            }
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
