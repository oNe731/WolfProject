using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Attack : Player_Base
{
    protected bool m_attack = false;

    protected float m_damage = 1f; 
    protected string m_animationName;
    private Coroutine m_attackCoroutine = null;

    private Image m_buttonSprite;
    private TMP_Text m_text;

    protected BoxCollider2D m_attackCollider;
    protected Transform m_effectPoint;

    protected Vector2 m_direct;

    public Player_Attack(StateMachine<Player> stateMachine, int buttonIndex) : base(stateMachine)
    {
        m_buttonSprite = m_owner.ButtonImage[buttonIndex];
        m_text = m_owner.ButtonImage[buttonIndex].gameObject.transform.parent.GetChild(3).GetComponent<TMP_Text>();

        m_attackCollider = m_owner.transform.GetChild(2).GetComponent<BoxCollider2D>();
        m_effectPoint = m_owner.transform.GetChild(2).GetChild(0);
    }

    public override void Enter_State()
    {
        m_owner.AttackCool = true;

        // UI 변경
        m_buttonSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);

        m_attack = false;
        m_owner.AM.SetTrigger(m_animationName);

        m_direct = m_owner.Get_Direction(m_owner.Joystick.InputVector);
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

        // UI 변경
        m_buttonSprite.color = new Color(1f, 1f, 1f, 1f);
        m_text.gameObject.SetActive(false);

        yield break;
    }

    protected void Set_ColliderDirection()
    {
        Vector2 direct = m_owner.Get_Direction(m_owner.Joystick.InputVector);
        if (direct.y == 1f) // 상
            m_attackCollider.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        else if (direct.y == -1f) // 하
            m_attackCollider.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (direct.x == -1f) // 좌
            m_attackCollider.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        else if (direct.x == 1f) // 우
            m_attackCollider.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }
}
