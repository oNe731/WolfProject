using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRed_Base : State<Monster>
{
    protected SlimeRed m_owner;
    protected float m_bombDistance = 1f;

    public SlimeRed_Base(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
        m_owner = m_stateMachine.Owner.GetComponent<SlimeRed>();
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_owner.transform.position, m_bombDistance);
    }

    protected float Get_PlayerDistance()
    {
        if (GameManager.Ins.Play.Player == null)
            return float.MaxValue;

        return Vector3.Distance(m_stateMachine.Owner.transform.position, GameManager.Ins.Play.Player.transform.position);
    }

    protected bool Change_Attack()
    {
        if (Get_PlayerDistance() <= m_bombDistance)
        {
            m_stateMachine.Change_State((int)SlimeRed.STATE.ST_BOMB);
            return true;
        }

        return false;
    }
}
