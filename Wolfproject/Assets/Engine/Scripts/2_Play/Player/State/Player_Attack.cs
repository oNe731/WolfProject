using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : Player_Base
{
    protected float m_damage = 1f; 
    protected string m_animationName;
    private Coroutine m_attackCoroutine = null;

    public Player_Attack(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_owner.AttackCool = true;
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
        Start_DashCoolTime();
    }

    public override void OnDrawGizmos()
    {
    }

    private void Start_DashCoolTime()
    {
        if (m_attackCoroutine != null)
            m_owner.StopCoroutine(m_attackCoroutine);
        m_attackCoroutine = m_owner.StartCoroutine(CoolTime_Attack());
    }

    private IEnumerator CoolTime_Attack()
    {
        m_owner.AttackCool = true;

        float time = 0;
        while (time < 1f) // 1ÃÊ
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_owner.AttackCool = false;
        yield break;
    }
}
