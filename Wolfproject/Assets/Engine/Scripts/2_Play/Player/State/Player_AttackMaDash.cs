using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_AttackMaDash : Player_Base
{
    private float m_time = 0f;
    private Vector2 m_direct;
    private Coroutine m_dashCoroutine = null;

    private Image m_buttonSprite;
    private TMP_Text m_text;

    private GameObject m_trail;

    public Player_AttackMaDash(StateMachine<Player> stateMachine) : base(stateMachine)
    {
        m_coolTime = 1f;

        m_buttonSprite = m_owner.ButtonImage[3];
        m_text = m_owner.ButtonImage[3].gameObject.transform.parent.GetChild(3).GetComponent<TMP_Text>();

        m_trail = m_owner.transform.GetChild(3).gameObject;
    }

    public override void Enter_State()
    {
        m_owner.Dash = true;
        m_owner.Invincibility = true; // 무적 상태 (적의 공격, 충돌 무시)

        m_time = 0f;

        m_direct = m_owner.Get_Direction(m_owner.Joystick.InputVector);
        m_owner.MoveSpeed *= 2f; // 대쉬 이동 속도 2배

        // UI 변경
        m_buttonSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);

        m_owner.AM.SetTrigger("IsAttackMaDash");
        m_owner.Play_AudioSource("Player_dash", false, 1f, 1f);

        m_trail.SetActive(true);
        m_owner.Invincibility = true; // 무적
    }

    public override void Update_State()
    {
        // 대쉬 이동
        if (m_owner.AM.IsInTransition(0) == true)
            return;

        if (m_owner.AM.GetCurrentAnimatorStateInfo(0).IsName("IsAttackMaDash") == true)
        {
            float animTime = m_owner.AM.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1f)
            {
                m_time += Time.deltaTime;
                if (m_time > 0.2f)
                {
                    m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
                    return;
                }
            }
            else if (animTime >= 0.6f && animTime <= 0.75f)
                Move_Player(m_direct);
            else
                m_owner.Rb.velocity = Vector2.zero;
        }
    }

    public override void Exit_State()
    {
        m_owner.Rb.velocity = Vector2.zero;
        m_owner.Dash = false;
        m_owner.Invincibility = false;

        Start_DashCoolTime();

        m_trail.SetActive(false);
        m_owner.Invincibility = false; // 무적

        m_owner.MoveSpeed /= 2f;
    }

    public override void OnDrawGizmos()
    {
    }

    private void Start_DashCoolTime()
    {
        if (m_dashCoroutine != null)
            m_owner.StopCoroutine(m_dashCoroutine);
        m_dashCoroutine = m_owner.StartCoroutine(CoolTime_Dash());
    }

    private IEnumerator CoolTime_Dash()
    {
        m_text.text = m_coolTime.ToString();
        m_text.gameObject.SetActive(true);

        m_owner.DashCool = true;
        float time = m_coolTime;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            m_text.text = time.ToString("F1");

            yield return null;
        }
        m_text.text = "0";
        m_owner.DashCool = false;

        // UI 변경
        m_buttonSprite.color = new Color(1f, 1f, 1f, 1f);
        m_text.gameObject.SetActive(false);

        yield break;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            if (monster != null)
                monster.Damaged_Monster(1f, true, 5f);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Monster_K"))
        {
            Monster monster = collision.gameObject.transform.parent.GetComponent<Monster>();
            if (monster != null)
                monster.Damaged_Monster(1f, true, 5f);
        }
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {

    }

    public override void OnCollisionExit2D(Collision2D collision)
    {

    }
}
