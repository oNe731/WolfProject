using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackFar : Player_Attack
{
    public Player_AttackFar(StateMachine<Player> stateMachine) : base(stateMachine, 1)
    {
        m_coolTime = 1f;
        m_animationName = "IsAttackFar";
    }

    public override void Enter_State()
    {
        base.Enter_State();
        m_owner.AttributeType = Player.ATTRIBUTETYPE.AT_FIRE;
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
                    Create_Projectile();
                }
                else if (animTime >= 1.0f) // 애니메이션 종료
                {
                    m_owner.Rb.velocity = Vector2.zero;
                    m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
                    return;
                }

                m_owner.Rb.velocity = -m_direct * 3f;
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

    private void Create_Projectile()
    {
        // 방향 변경 (콜라이더 방향 변경)
        Set_ColliderDirection();

        GameObject gameObject = GameManager.Ins.LoadCreate("4_Prefab/1_Player/Projectile");
        if (gameObject != null)
        {
            Projectile projectile = gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Start_Projectile(m_effectPoint.position, m_owner.AttributeType, m_owner.Get_Direction(m_owner.Joystick.InputVector), m_owner.Get_Direction(), 3f);
                m_owner.Play_AudioSource("Player_Ranged", false, 1f, GameManager.Ins.Sound.EffectSound);
            }
        }
    }
}
