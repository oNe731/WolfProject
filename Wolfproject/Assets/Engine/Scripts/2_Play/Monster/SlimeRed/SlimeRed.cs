using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRed : Monster
{
    public enum STATE { ST_IDLE, ST_CHASE, ST_BOMB, ST_DIE, ST_END }

    private void Start()
    {
        Initialize_Monster();

        m_hpMax = 1f;
        m_hp = m_hpMax;

        m_speedMax = 2f;
        m_speed = m_speedMax;

        m_damage = 2f;

        m_hitIndex = (int)STATE.ST_END;
        m_dieIndex = (int)STATE.ST_DIE;

        m_stateMachine = new StateMachine<Monster>(gameObject);
        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new SlimeRed_Idle(m_stateMachine));  // 0
        states.Add(new SlimeRed_Chase(m_stateMachine)); // 1
        states.Add(new SlimeRed_Bomb(m_stateMachine));  // 2
        states.Add(new SlimeRed_Die(m_stateMachine));   // 3
        m_stateMachine.Initialize_State(states, (int)STATE.ST_CHASE);
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
        {
            if (m_stateMachine.CurState != (int)STATE.ST_IDLE)
                m_stateMachine.Change_State((int)STATE.ST_IDLE);
            return;
        }

        m_stateMachine.Update_State();
    }

    private void OnDrawGizmos()
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnDrawGizmos();
    }
}
