using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Base : State<Monster>
{
    protected Slime m_owner;
    protected float m_chaseDistance = 2f;
    protected float m_attackDistance = 1f;

    public Slime_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<Slime>();
    }

    public override void Enter_State()
    {
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_owner.transform.position, m_chaseDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_owner.transform.position, m_attackDistance);
    }

    protected float Get_PlayerDistance()
    {
        if (GameManager.Ins.Play.Player == null)
            return float.MaxValue;

        return Vector3.Distance(m_stateMachine.Owner.transform.position, GameManager.Ins.Play.Player.transform.position);
    }

    protected bool Change_Chase()
    {
        if (Get_PlayerDistance() <= m_chaseDistance)
        {
            m_stateMachine.Change_State((int)Slime.STATE.ST_CHASE);
            return true;
        }

        return false;
    }

    protected bool Change_Attack()
    {
        if (Get_PlayerDistance() <= m_attackDistance)
        {
            m_stateMachine.Change_State((int)Slime.STATE.ST_ATTACK);
            return true;
        }

        return false;
    }
}
