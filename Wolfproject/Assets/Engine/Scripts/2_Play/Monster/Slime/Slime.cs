using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    public enum STATE { ST_IDLE, ST_CHASE, ST_ATTACK, ST_HIT, ST_BERSERK, ST_DIE, ST_END }

    private void Start()
    {
        Start_Monster();

        m_hpMax = 5f;
        m_hp = m_hpMax;

        m_stateMachine = new StateMachine<Monster>(gameObject);
        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new Slime_Idle(m_stateMachine));    // 0
        states.Add(new Slime_Chase(m_stateMachine));   // 1
        states.Add(new Slime_Attack(m_stateMachine));  // 2
        states.Add(new Slime_Hit(m_stateMachine));     // 3
        states.Add(new Slime_Berserk(m_stateMachine)); // 4
        states.Add(new Slime_Die(m_stateMachine));     // 5
        m_stateMachine.Initialize_State(states, (int)STATE.ST_IDLE);
    }

    private void Update()
    {
        m_stateMachine.Update_State();
    }

    private void OnDrawGizmos()
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnDrawGizmos();
    }
    /*
���ط�: ���� �� �÷��̾�� 10�� ����.
 */
}