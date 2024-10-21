using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackFar : Player_Attack
{
    private bool m_attack = false;
    public Player_AttackFar(StateMachine<Player> stateMachine) : base(stateMachine)
    {
        m_animationName = "IsAttackFar";
    }

    public override void Enter_State()
    {
        base.Enter_State();

        m_attack = false;
        m_owner.AM.SetTrigger(m_animationName);
    }

    public override void Update_State()
    {
        if (m_owner.AM.IsInTransition(0) == true)
            return;

        if (m_owner.AM.GetCurrentAnimatorStateInfo(0).IsName(m_animationName) == true)
        {
            float animTime = m_owner.AM.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (m_attack == false && animTime > 0.5)
            {
                m_attack = true;
                Create_Projectile();
            }
            else if (animTime >= 1.0f) // 애니메이션 종료
                m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
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
        GameObject gameObject = GameManager.Ins.LoadCreate("4_Prefab/1_Player/Projectile");
        if (gameObject != null)
        {
            Projectile projectile = gameObject.GetComponent<Projectile>();
            if (projectile != null)
                projectile.Start_Projectile(m_owner.transform.position, m_owner.AttributeType, m_owner.Joystick.InputVector);
        }
    }
}
