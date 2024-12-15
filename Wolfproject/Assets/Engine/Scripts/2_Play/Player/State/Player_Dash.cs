using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Dash : Player_Base
{
    private float m_time = 0f;
    private Vector2 m_direct;
    private Coroutine m_dashCoroutine = null;

    private Image m_buttonSprite;
    private TMP_Text m_text;

    private GameObject m_trail;
    private int m_playerLayer;
    private int m_playerLayerK;
    private int m_monsterLayer;
    private int m_monsterLayerK;

    public Player_Dash(StateMachine<Player> stateMachine) : base(stateMachine)
    {
        m_coolTime = 1f;

        m_buttonSprite = m_owner.ButtonImage[3];
        m_text = m_owner.ButtonImage[3].gameObject.transform.parent.GetChild(3).GetComponent<TMP_Text>();

        m_trail = m_owner.transform.GetChild(3).gameObject;
        m_playerLayer = LayerMask.NameToLayer("Player");
        m_playerLayerK = LayerMask.NameToLayer("Player_K");
        m_monsterLayer = LayerMask.NameToLayer("Monster");
        m_monsterLayerK = LayerMask.NameToLayer("Monster_K");
    }

    public override void Enter_State()
    {
        m_owner.Dash = true;
        m_owner.Invincibility = true; // 무적 상태 (적의 공격, 충돌 무시)

        m_time = 0f;

        m_direct = m_owner.Get_Direction(m_owner.Joystick.InputVector);
        m_owner.MoveSpeed = 10f; // 대쉬 이동 속도

        // UI 변경
        m_buttonSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);

        m_owner.AM.SetTrigger("IsDash");
        m_owner.Play_AudioSource("Player_dash", false, 1f, 1f);

        Physics2D.IgnoreLayerCollision(m_playerLayer, m_monsterLayer, true);
        Physics2D.IgnoreLayerCollision(m_playerLayer, m_monsterLayerK, true);
        Physics2D.IgnoreLayerCollision(m_playerLayerK, m_monsterLayer, true);
        Physics2D.IgnoreLayerCollision(m_playerLayerK, m_monsterLayerK, true);
        m_trail.SetActive(true);
    }

    public override void Update_State()
    {
        // 대쉬 이동
        m_time += Time.deltaTime;
        if(m_time > 0.2f)
        {
            m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
            return;
        }

        Move_Player(m_direct);
    }

    public override void Exit_State()
    {
        m_owner.Rb.velocity = Vector2.zero;
        m_owner.Dash = false;
        m_owner.Invincibility = false;

        Start_DashCoolTime();

        Physics2D.IgnoreLayerCollision(m_playerLayer, m_monsterLayer, false);
        Physics2D.IgnoreLayerCollision(m_playerLayer, m_monsterLayerK, false);
        Physics2D.IgnoreLayerCollision(m_playerLayerK, m_monsterLayer, false);
        Physics2D.IgnoreLayerCollision(m_playerLayerK, m_monsterLayerK, false);
        m_trail.SetActive(false);
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
}
