using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Attack : Player_Base
{
    protected float m_damage = 1f; 
    protected string m_animationName;
    private Coroutine m_attackCoroutine = null;

    private Image m_buttonSprite;
    private TMP_Text m_text;

    public Player_Attack(StateMachine<Player> stateMachine, int buttonIndex) : base(stateMachine)
    {
        m_buttonSprite = m_owner.ButtonImage[buttonIndex];
        m_text = m_owner.ButtonImage[buttonIndex].gameObject.transform.parent.GetChild(3).GetComponent<TMP_Text>();
    }

    public override void Enter_State()
    {
        m_owner.AttackCool = true;

        // UI ����
        m_buttonSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
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
        m_text.text = m_coolTime.ToString();
        m_text.gameObject.SetActive(true);

        m_owner.AttackCool = true;
        float time = m_coolTime;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            m_text.text = time.ToString("F1");

            yield return null;
        }
        m_text.text = "0";
        m_owner.AttackCool = false;

        // UI ����
        m_buttonSprite.color = new Color(1f, 1f, 1f, 1f);
        m_text.gameObject.SetActive(false);

        yield break;
    }
}
