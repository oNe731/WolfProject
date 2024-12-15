using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : class
{
    private GameObject m_owner;
    private int m_nexState = -1;
    private int m_curState = -1;
    private int m_preState = -1;
    private int m_lasState = -1;
    private List<State<T>> m_states;

    private bool m_lock = false;

    public GameObject Owner => m_owner;
    public int NexState => m_nexState;
    public int CurState => m_curState;
    public int PreState => m_preState;
    public int LasState => m_lasState;

    public bool Lock { get => m_lock; set => m_lock = value; }

    public StateMachine(GameObject owner)
    {
        m_owner = owner;
    }

    public void Initialize_State(List<State<T>> states, int startState)
    {
        m_states = states;
        Change_State(startState);
    }

    public void Update_State()
    {
        if (m_curState == -1 || m_lock == true)
            return;

        m_states[(int)m_curState].Update_State();
    }

    public void Change_State(int stateIndex)
    {
        m_nexState = stateIndex;

        if (m_curState != -1)
            m_states[(int)m_curState].Exit_State();

        m_lasState = m_preState;
        m_preState = m_curState;
        m_curState = stateIndex;

        m_states[(int)m_curState].Enter_State();
    }

    public State<T> Get_CurrState()
    {
        if (m_curState == -1)
            return null;

        return m_states[(int)m_curState];
    }

    public State<T> Get_PreState()
    {
        if (m_preState == -1)
            return null;

        return m_states[(int)m_preState];
    }

    public void OnDrawGizmos()
    {
        for(int i = 0; i < m_states.Count; ++i)
            m_states[i].OnDrawGizmos();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_curState == -1 || m_lock == true)
            return;

        m_states[(int)m_curState].OnCollisionEnter2D(collision);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (m_curState == -1 || m_lock == true)
            return;

        m_states[(int)m_curState].OnCollisionStay2D(collision);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (m_curState == -1 || m_lock == true)
            return;

        m_states[(int)m_curState].OnCollisionExit2D(collision);
    }
}
