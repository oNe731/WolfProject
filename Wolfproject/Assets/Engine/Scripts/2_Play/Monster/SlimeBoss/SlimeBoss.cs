using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : Monster
{
    public enum STATE { 
        ST_IDLE, ST_WALK, ST_CHASE, ST_SLASH,
        ST_PHASE1, ST_PHASE2, ST_PHASE3,
        ST_HIT, ST_DIE,
        ST_END }

    private void Start()
    {
        Initialize_Monster();

        m_hpMax = 5f;
        m_hp = m_hpMax;

        m_speedMax = 2f;
        m_speed = m_speedMax;

        m_damage = 2f;

        m_hitIndex = (int)STATE.ST_HIT;
        m_dieIndex = (int)STATE.ST_DIE;

        m_stateMachine = new StateMachine<Monster>(gameObject);
        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new SlimeBoss_Idle(m_stateMachine));    // 0
        states.Add(new SlimeBoss_Walk(m_stateMachine));    // 0
        states.Add(new SlimeBoss_Chase(m_stateMachine));   // 1
        states.Add(new SlimeBoss_Slash(m_stateMachine));   // 2

        states.Add(new SlimeBoss_Phase1(m_stateMachine));  // 3
        states.Add(new SlimeBoss_Phase2(m_stateMachine));  // 4
        states.Add(new SlimeBoss_Phase3(m_stateMachine));  // 5
        states.Add(new SlimeBoss_Berserk(m_stateMachine)); // 6
        m_stateMachine.Initialize_State(states, (int)STATE.ST_IDLE);
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
