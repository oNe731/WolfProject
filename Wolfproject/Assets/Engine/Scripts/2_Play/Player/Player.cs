using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum STATE { ST_IDLE, ST_WALK, ST_DASH, ST_ATTACKNEAR, ST_ATTACKFAR, ST_HIT, ST_DIE, ST_END }
    public enum ATTRIBUTETYPE { AT_FIRE, AT_THUNDER, AT_END }
    public enum ATTACKTYPE { AT_NEAR, AT_FAR, AT_END }

    [SerializeField] private Slider[] m_hpSliders;
    [SerializeField] private Slider[] m_staminaSliders;
    [SerializeField] private Joystick m_joystick;
    [SerializeField] private UIBlood uiBlood;

    private UISliderOwner m_hpSlider;
    private UISliderOwner m_staminaSlider;

    private float m_hp;
    private float m_hpMax;
    private float m_stamina;
    private float m_staminaMax;

    private ATTRIBUTETYPE m_attributeType = ATTRIBUTETYPE.AT_FIRE;
    private float m_moveSpeed = 5f;

    private bool m_recover = false;
    private float m_recoverTime = 0f;

    private bool m_invincibility = false;
    private bool m_dash = false;
    private bool m_dashCool = false;
    private Coroutine m_dashCoroutine = null;

    private bool m_attackCool = false;
    private Coroutine m_attackCoroutine = null;

    private StateMachine<Player> m_stateMachine;

    private SpriteRenderer m_sr;
    private Rigidbody2D m_rb;

    public Joystick Joystick => m_joystick;

    public void Damaged_Player(float damage)
    {
        if (m_invincibility == true) // 무적 상태
            return;

        m_hp -= damage;
        if(m_hp <= 0)
        {
            m_hp = 0;
            GameManager.Ins.Play.Over_Game();
        }
        else
        {
            if (m_hp <= m_hpMax * 0.2f)
                uiBlood.Start_Blood();
            else
                uiBlood.Stop_Blood();
        }

        m_hpSlider.Set_Slider(m_hp);
    }

    private void Start()
    {
        m_hpMax = 100f;
        m_hp = m_hpMax;
        for(int i = 0; i < m_hpSliders.Length; ++i)
        {
            m_hpSliders[i].maxValue = m_hpMax;
            m_hpSliders[i].value = m_hp;
        }
        m_hpSlider = m_hpSliders[0].GetComponent<UISliderOwner>();
        m_hpSlider.Set_Slider(m_hp);

        m_staminaMax = 50f;
        m_stamina = m_staminaMax;
        for (int i = 0; i < m_staminaSliders.Length; ++i)
        {
            m_staminaSliders[i].maxValue = m_staminaMax;
            m_staminaSliders[i].value = m_stamina;
        }
        m_staminaSlider = m_staminaSliders[0].GetComponent<UISliderOwner>();
        Set_StaminaSlider();

        m_sr = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();

        // State
        m_stateMachine = new StateMachine<Player>(gameObject);
        List<State<Player>> states = new List<State<Player>>();
        states.Add(new Player_Idle(m_stateMachine));       // 0
        states.Add(new Player_Walk(m_stateMachine));       // 1
        states.Add(new Player_Dash(m_stateMachine));       // 2
        states.Add(new Player_AttackNear(m_stateMachine)); // 3
        states.Add(new Player_AttackFar(m_stateMachine));  // 4
        states.Add(new Player_Hit(m_stateMachine));        // 5
        states.Add(new Player_Die(m_stateMachine));        // 6
        m_stateMachine.Initialize_State(states, (int)STATE.ST_IDLE);
    }

    private void Update()
    {
        if (m_recover == true)
            Recover_Stamina();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (m_hp == 100)
        //        Damaged_Player(90);
        //    else
        //        Damaged_Player(-90);
        //}
    }

    private void FixedUpdate()
    {
        if (m_dash == true)
            return;

        if (m_joystick.IsInput == false)
        {
            m_rb.velocity = Vector2.zero;
        }
        else
        {
            if (m_stamina > 0)
                m_moveSpeed = 5f; // 기본 이동 속도
            else
                m_moveSpeed = 5f * 0.8f; // 20% 감소
            Move_Player(m_joystick.InputVector);
        }
    }

    private void Move_Player(Vector2 direct)
    {
        m_rb.velocity = direct * m_moveSpeed;
        if (m_joystick.InputVector.x < 0)
            m_sr.flipX = false;
        else if (m_joystick.InputVector.x > 0)
            m_sr.flipX = true;
    }

    public void Attack_Player(ATTACKTYPE attackType)
    {
        if (m_attackCool == true)
            return;

        m_attackCool = true;
        switch (attackType)
        {
            case ATTACKTYPE.AT_NEAR: // 근접 공격
                // 근접 상태에서만 유효한 공격.
                //*

                break;

            case ATTACKTYPE.AT_FAR: // 원거리 공격
                // 속성 공격 사용 중 스태미나가 없으면 공격 속도가 느려짐.
                if (m_stamina < 5)
                    return;
                Use_Stamina(5f);

                // 투사체 오브젝트 생성
                GameObject gameObject = GameManager.Ins.LoadCreate("4_Prefab/1_Player/Projectile");
                if(gameObject != null)
                {
                    Projectile projectile = gameObject.GetComponent<Projectile>();
                    if (projectile != null)
                        projectile.Start_Projectile(m_attributeType, m_joystick.InputVector);
                }
                break;
        }

        // 쿨타임
        if (m_attackCoroutine != null)
            StopCoroutine(m_attackCoroutine);
        m_attackCoroutine = StartCoroutine(CoolTime_Attack(1f));
    }

    private IEnumerator CoolTime_Attack(float waitTime)
    {
        float time = 0f;
        while(time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_attackCool = false;
        yield break;
    }

    public void Change_AttributeType()
    {
        switch(m_attributeType)
        {
            case ATTRIBUTETYPE.AT_FIRE:
                m_attributeType = ATTRIBUTETYPE.AT_THUNDER;
                break;

            case ATTRIBUTETYPE.AT_THUNDER:
                m_attributeType = ATTRIBUTETYPE.AT_FIRE;
                break;
        }
    }

    public void Dash_Player()
    {
        if (m_dash == true || m_dashCool == true || m_stamina < 10f)
            return;
        Use_Stamina(10f);

        if (m_dashCoroutine != null)
            StopCoroutine(m_dashCoroutine);
        m_dashCoroutine = StartCoroutine(Move_Dash());
    }

    private IEnumerator Move_Dash()
    {
        m_dash = true;
        m_invincibility = true; // 무적 상태 (적의 공격, 충돌 무시)

        // 대쉬 이동
        Vector2 direct = m_joystick.InputVector; // 조이스틱 입력 방향으로 대쉬
        float time = 0;
        m_moveSpeed = 10f; // 대쉬 이동 속도
        while(time < 0.2f) // 대쉬 거리 3미터
        {
            time += Time.deltaTime;
            Move_Player(direct);

            yield return null;
        }
        m_rb.velocity = Vector2.zero;
        m_dash = false;
        m_invincibility = false;

        // 대쉬 쿨타임
        m_dashCool = true;
        time = 0;
        while (time < 1f) // 1초
        {
            time += Time.deltaTime;
            yield return null;
        }
        m_dashCool = false;
        yield break;
    }

    private void Use_Stamina(float stamina)
    {
        m_stamina -= stamina;
        if (m_stamina <= 0)
            m_stamina = 0;

        Set_StaminaSlider();
        m_recover = true;
        m_recoverTime = 0f;
    }

    private void Recover_Stamina()
    {
        m_recoverTime += Time.deltaTime;
        if (m_recoverTime < 1.5f)
            return;

        m_stamina += Time.deltaTime * 5f;
        if (m_stamina >= m_staminaMax)
        {
            m_stamina = m_staminaMax;
            m_recover = false;
        }

        Set_StaminaSlider();
    }

    private void Set_StaminaSlider()
    {
        m_staminaSlider.Set_Slider(m_stamina);

        // 10이하일 때 경고 색상으로 변경
        Color color;
        if (m_stamina <= 10) 
            color = new Color(1f, 0f, 0f, 1f); // 빨간색
        else
            color = new Color(0.3603774f, 0.5231216f, 1f, 1f); // 파란색
        m_staminaSlider.FillImage.color = color;
    }
}
