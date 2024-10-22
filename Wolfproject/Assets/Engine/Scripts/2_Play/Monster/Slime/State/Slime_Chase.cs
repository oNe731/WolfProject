using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Chase : Slime_Base
{


    public Slime_Chase(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.SpeedMax = 4f;
        m_owner.Speed = m_owner.SpeedMax;
        m_owner.Animator.SetBool("Is_Chase", true);
        //Debug.Log("추격");
    }

    public override void Update_State()
    {
        if(Change_Attack() == false) // 거리에 따른 공격 상태 전환
        {
            // 플레이어 추격
            Vector2 direction = ((Vector2)GameManager.Ins.Play.Player.transform.position - (Vector2)m_owner.transform.position).normalized;
            m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);
        }

        if (m_owner.Animator.IsInTransition(0) == true)
            return;
        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Chase") == true)
            m_owner.Animator.SetBool("Is_Chase", false);
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {

    }
}
