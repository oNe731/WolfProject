using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walk : Player_Base
{
    private float m_soundTime = 0f;


    public Player_Walk(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // 애니메이션 변경
        m_owner.AM.SetTrigger("IsWalk");
    }

    public override void Update_State()
    {
        if (m_owner.Joystick.IsInput == false)
        {
            m_owner.Rb.velocity = Vector2.zero;
            m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
            return;
        }
        else
        {
            //if (m_owner.Stamina > 0)
            //    m_owner.MoveSpeed = 5f;        // 기본 이동 속도
            //else
            //    m_owner.MoveSpeed = 5f * 0.8f; // 20% 감소
            Move_Player(m_owner.Joystick.InputVector);
            Play_WalkSound();
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }

    protected void Play_WalkSound()
    {
        m_soundTime += Time.deltaTime;
        if (m_soundTime >= 0.4f)
        {
            m_soundTime = 0f;

            int Index = Random.Range(0, 5);
            m_owner.Play_AudioSource("Player_Walk_" + Index.ToString(), false, 1f, GameManager.Ins.Sound.EffectSound);
        }
    }
}
