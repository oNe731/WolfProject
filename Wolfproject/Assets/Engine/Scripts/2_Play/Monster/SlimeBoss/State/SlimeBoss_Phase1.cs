using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Phase1 : SlimeBoss_Base
{
    private int m_nextPhase;
    private float m_time;

    public SlimeBoss_Phase1(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_time = 0f;
        m_nextPhase = Random.Range(0, 2);

        m_owner.Animator.SetTrigger("Is_Conversion");
        m_owner.Play_AudioSource("KingSlime_PhaseShift", false, 1f, GameManager.Ins.Sound.EffectSound);
        //Debug.Log("보스 페이즈1");
    }

    public override void Update_State() // 페이즈 변환 전 무방비 상태
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Conversion") == true)
        {
            float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if(animTime >= 0.8f)
            {
                if (m_nextPhase == 0) // 슬라임 색상 녹색으로 변경
                    m_owner.SpriteRenderer.color = new Color(0.33f, 1f, 0.35f, 1f);
                else if (m_nextPhase == 1) // 슬라임 색상 파란색으로 변경
                    m_owner.SpriteRenderer.color = new Color(0.37f, 0.62f, 1f);

                if (animTime >= 1f)
                {
                    m_time += Time.deltaTime;
                    if (m_time > 2f)
                    {
                        if (m_nextPhase == 0)
                            m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_PHASE2);
                        else if (m_nextPhase == 1)
                            m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_PHASE3);
                    }
                }
            }
        }
    }

    public override void Exit_State()
    {
    }
}
