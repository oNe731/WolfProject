using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Monster
{
    public enum STATE { ST_IDLE, ST_WALK, ST_STEALTH, ST_WAKEUP, ST_CHASE, ST_ATTACK, ST_WAIT, ST_HIT, ST_RUNAWAY, ST_DIE, ST_END }

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
        states.Add(new Mushroom_Idle(m_stateMachine));    // 0
        states.Add(new Mushroom_Walk(m_stateMachine));    // 1
        states.Add(new Mushroom_Stealth(m_stateMachine)); // 2

        states.Add(new Mushroom_WakeUp(m_stateMachine));  // 3
        states.Add(new Mushroom_Chase(m_stateMachine));   // 4

        states.Add(new Mushroom_Attack(m_stateMachine));  // 5
        states.Add(new Mushroom_Wait(m_stateMachine));    // 6
        states.Add(new Mushroom_Hit(m_stateMachine));     // 7
        states.Add(new Mushroom_RunAway(m_stateMachine)); // 8
        states.Add(new Mushroom_Die(m_stateMachine));     // 9
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
