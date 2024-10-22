using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private float m_time = 0f;
    private Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (m_animator.IsInTransition(0) == true)
            return;

        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f)
        {
            m_time += Time.deltaTime;
            if (m_time >= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
