using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Die : Player_Base
{
    private bool m_dieEvent = false;
    private float m_time = 0f;

    public Player_Die(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // 애니메이션 변경
        m_owner.AM.SetTrigger("IsDeath");
        GameManager.Ins.Set_Pause(true);
    }

    public override void Update_State()
    {
        if (m_dieEvent == true)
            return;

        if (m_owner.AM.GetCurrentAnimatorStateInfo(0).IsName("IsDeath") == true)
        {
            float animTime = m_owner.AM.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1.0f)
            {
                m_time += Time.deltaTime;
                if(m_time >= 1f)
                {
                    m_dieEvent = true;
                    GameManager.Ins.Play.Over_Game();
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
