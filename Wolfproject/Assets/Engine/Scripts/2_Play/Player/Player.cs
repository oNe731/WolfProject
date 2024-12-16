using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public enum STATE 
    { 
        ST_IDLE, ST_WALK, ST_DASH, ST_ATTACKNEAR, ST_ATTACKFAR,
        ST_ATTACKNEAR_MOVE, ST_ATTACKNEAR_DASH,
        ST_ATTACKFAR_MOVE, ST_ATTACKFAR_DASH,
        ST_HIT, ST_DIE, 

        ST_END 
    }    
    public enum ATTRIBUTETYPE { AT_FIRE, AT_THUNDER, AT_END }
    public enum ATTACKTYPE { AT_NEAR, AT_FAR, AT_END }

    [SerializeField] private Slider[] m_hpSliders;
    [SerializeField] private Slider[] m_shieldSliders;
    [SerializeField] private Slider[] m_staminaSliders;
    [SerializeField] private Joystick m_joystick;
    [SerializeField] private UIBlood uiBlood;
    [SerializeField] private Inventory m_inventory;

    [SerializeField] private Image[] m_buttonImage;
    [SerializeField] private Sprite[] m_buttonSprite;

    private UISliderOwner m_hpSlider;
    private UISliderOwner m_shieldSlider;
    private UISliderOwner m_staminaSlider;

    private float m_hp;
    private float m_hpMax;
    private float m_shield;
    private float m_shieldMax;
    private float m_stamina;
    private float m_staminaMax;
    private float m_damage;

    private ATTRIBUTETYPE m_attributeType = ATTRIBUTETYPE.AT_FIRE;
    private float m_moveSpeed = 5f;
    private float m_moveSpeedMax = 10f;

    private bool m_recover = false;
    private float m_recoverTime = 0f;

    private bool m_invincibility = false;
    private bool m_dash = false;
    private bool m_dashCool = false;

    private bool m_attackCool = false;

    private bool m_isSturn = false;
    private float m_isSturnTime = 0f;
    private float m_sturnTime = 0f;

    private StateMachine<Player> m_stateMachine;

    private SpriteRenderer m_sr;
    private Rigidbody2D m_rb;
    private Animator m_am;
    private AudioSource m_as;

    public float Stamina => m_stamina;
    public ATTRIBUTETYPE AttributeType { get => m_attributeType; set => m_attributeType = value; }
    public float Damage { get => m_damage; }
    public float MoveSpeed { get => m_moveSpeed; set => m_moveSpeed = Mathf.Min(value, m_moveSpeedMax); }
    public float MoveSpeedMax { get => m_moveSpeedMax; }
    public bool Invincibility { get => m_invincibility; set => m_invincibility = value; }
    public bool Dash { get => m_dash; set => m_dash = value; }
    public bool DashCool { get => m_dashCool; set => m_dashCool = value; }
    public bool AttackCool { get => m_attackCool; set => m_attackCool = value; }
    public Inventory Inven => m_inventory;
    public Image[] ButtonImage { get => m_buttonImage; }
    public Joystick Joystick => m_joystick;
    public SpriteRenderer Sr => m_sr;
    public Rigidbody2D Rb => m_rb;
    public Animator AM => m_am;

    private bool m_activeShield = false;
    private float m_shieldTime = 0f;
    private float m_shieldTimeMax = 10f;

    private bool m_activeBuff = false;
    private float m_buffTime = 0f;
    private float m_buffTimeMax = 5f;

    public void Damaged_Player(float damage)
    {
        if (m_invincibility == true) // 무적 상태
            return;

        // 방어막
        if(m_activeShield == true)
        {
            m_shield -= damage;
            if (m_shield <= 0)
            {
                // 효과 비활성화
                m_activeShield = false;

                m_shield = 0f;
                m_shieldSlider.gameObject.SetActive(false);
                return;
            }

            m_shieldSlider.Set_Slider(m_shield);
            return;
        }

        m_hp -= damage;
        m_hpSlider.Set_Slider(m_hp);

        if (m_hp <= 0)
        {
            m_hp = 0;
            if(m_stateMachine.CurState != (int)STATE.ST_DIE)
                m_stateMachine.Change_State((int)STATE.ST_DIE);
            return;
        }

        if (m_hp <= m_hpMax * 0.2f)
            uiBlood.Start_Blood();
        else
            uiBlood.Stop_Blood();
        m_stateMachine.Change_State((int)STATE.ST_HIT);
    }

    public void Recover_Player(float recover)
    {
        m_hp += recover;
        if (m_hp > m_hpMax)
            m_hp = m_hpMax;
        m_hpSlider.Set_Slider(m_hp);

        if (m_hp <= m_hpMax * 0.2f)
            uiBlood.Start_Blood();
        else
            uiBlood.Stop_Blood();
    }

    public void Active_Shield()
    {
        m_activeShield = true;
        m_shieldTime = 0f;

        m_shield = m_shieldMax;
        m_shieldSlider.Set_Slider(m_shield);

        m_shieldSlider.gameObject.SetActive(true);
    }

    public void Active_Buff()
    {
        m_activeBuff = true;
        m_buffTime = 0f;

        MoveSpeed *= 1.2f; // + 20% 증가
        transform.GetChild(4).gameObject.SetActive(true); // 버프 이펙트 활성화
    }

    private void Start()
    {
        Initialize_Character();

        m_hpMax = 100f;
        m_hp = m_hpMax;
        for(int i = 0; i < m_hpSliders.Length; ++i)
        {
            m_hpSliders[i].maxValue = m_hpMax;
            m_hpSliders[i].value = m_hp;
        }
        m_hpSlider = m_hpSliders[0].GetComponent<UISliderOwner>();
        m_hpSlider.Initialize();
        m_hpSlider.Set_Slider(m_hp);

        // 쉴드
        m_shieldMax = 20f;
        m_shield = 0;
        for (int i = 0; i < m_shieldSliders.Length; ++i)
        {
            m_shieldSliders[i].maxValue = m_hpMax;
            m_shieldSliders[i].value = m_shield;
        }
        m_shieldSlider = m_shieldSliders[0].GetComponent<UISliderOwner>();
        m_shieldSlider.Initialize(m_shieldMax);
        m_shieldSlider.Set_Slider(m_shield);

        m_staminaMax = 50f;
        m_stamina = m_staminaMax;
        for (int i = 0; i < m_staminaSliders.Length; ++i)
        {
            m_staminaSliders[i].maxValue = m_staminaMax;
            m_staminaSliders[i].value = m_stamina;
        }
        m_staminaSlider = m_staminaSliders[0].GetComponent<UISliderOwner>();
        m_staminaSlider.Initialize();
        Set_StaminaSlider();

        m_damage = 2f;

        m_sr = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
        m_am = GetComponent<Animator>();
        m_as = GetComponent<AudioSource>();

        // State
        m_stateMachine = new StateMachine<Player>(gameObject);
        List<State<Player>> states = new List<State<Player>>();
        states.Add(new Player_Idle(m_stateMachine));       // 0
        states.Add(new Player_Walk(m_stateMachine));       // 1
        states.Add(new Player_Dash(m_stateMachine));       // 2
        states.Add(new Player_AttackNear(m_stateMachine)); // 3
        states.Add(new Player_AttackFar(m_stateMachine));  // 4

        states.Add(new Player_AttackMaMove(m_stateMachine)); // 5
        states.Add(new Player_AttackMaDash(m_stateMachine)); // 6
        states.Add(new Player_AttackRaMove(m_stateMachine)); // 7
        states.Add(new Player_AttackRaDash(m_stateMachine)); // 8

        states.Add(new Player_Hit(m_stateMachine));        // 9
        states.Add(new Player_Die(m_stateMachine));        // 10
        m_stateMachine.Initialize_State(states, (int)STATE.ST_IDLE);
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false && m_stateMachine.CurState != (int)STATE.ST_DIE)
            return;

        // 스테미나
        if (m_recover == true)
            Recover_Stamina();

        // 방어막
        if(m_activeShield == true)
        {
            m_shieldTime += Time.deltaTime;
            if(m_shieldTime >= m_shieldTimeMax)
            {
                // 효과 비활성화
                m_activeShield = false;

                m_shield = 0f;
                m_shieldSlider.gameObject.SetActive(false);
            }
        }

        // 속도 버프
        if(m_activeBuff == true)
        {
            m_buffTime += Time.deltaTime;
            if (m_buffTime >= m_buffTimeMax)
            {
                m_activeBuff = false;

                // 효과 비활성화
                MoveSpeed /= 1.2f;
                transform.GetChild(4).gameObject.SetActive(false); // 버프 이펙트 비활성화
            }
        }

        // 스턴
        if(m_isSturn == true)
        {
            m_sturnTime += Time.deltaTime;
            if(m_sturnTime >= m_isSturnTime)
            {
                m_isSturn = false;
                m_sturnTime = 0f;
            }
            return;
        }

        AM.SetFloat("X", m_joystick.InputVector.x);
        AM.SetFloat("Y", m_joystick.InputVector.y);

        if (m_stateMachine == null)
            return;
        m_stateMachine.Update_State();
    }

    public void Attack_Player(ATTACKTYPE attackType)
    {
        if (m_attackCool == true || m_isSturn == true || GameManager.Ins.IsGame == false)
            return;

        switch (attackType)
        {
            case ATTACKTYPE.AT_NEAR:
                if (m_stateMachine.CurState == (int)STATE.ST_DASH)
                    m_stateMachine.Change_State((int)STATE.ST_ATTACKNEAR_DASH);
                else if (m_joystick.IsInput == true)
                    m_stateMachine.Change_State((int)STATE.ST_ATTACKNEAR_MOVE);
                else
                    m_stateMachine.Change_State((int)STATE.ST_ATTACKNEAR);
                break;

            case ATTACKTYPE.AT_FAR:
                if (m_stamina < 5)
                    return;
                Use_Stamina(5f);

                if (m_stateMachine.CurState == (int)STATE.ST_DASH)
                    m_stateMachine.Change_State((int)STATE.ST_ATTACKFAR_DASH);
                else if (m_joystick.IsInput == true)
                    m_stateMachine.Change_State((int)STATE.ST_ATTACKFAR_MOVE);
                else
                    m_stateMachine.Change_State((int)STATE.ST_ATTACKFAR);
                break;
        }
    }

    public void Change_AttributeType()
    {
        if (m_isSturn == true || GameManager.Ins.IsGame == false)
            return;

        switch (m_attributeType)
        {
            case ATTRIBUTETYPE.AT_FIRE:
                m_attributeType = ATTRIBUTETYPE.AT_THUNDER;
                m_buttonImage[0].sprite = m_buttonSprite[1];
                m_buttonImage[1].sprite = m_buttonSprite[3];
                m_buttonImage[2].sprite = m_buttonSprite[5];
                break;

            case ATTRIBUTETYPE.AT_THUNDER:
                m_attributeType = ATTRIBUTETYPE.AT_FIRE;
                m_buttonImage[0].sprite = m_buttonSprite[0];
                m_buttonImage[1].sprite = m_buttonSprite[2];
                m_buttonImage[2].sprite = m_buttonSprite[4];
                break;
        }
    }

    public void Dash_Player()
    {
        if (m_dash == true || m_dashCool == true || m_stamina < 10f || m_isSturn == true || GameManager.Ins.IsGame == false)
            return;

        Use_Stamina(10f);
        m_stateMachine.Change_State((int)STATE.ST_DASH);
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
        //Color color;
        //if (m_stamina <= 10) 
        //    color = new Color(1f, 0f, 0f, 1f); // 빨간색
        //else
        //    color = new Color(0.3603774f, 0.5231216f, 1f, 1f); // 파란색
        //m_staminaSlider.FillImage.color = color;
    }


    public void Start_Sturn(float time)
    {
        m_isSturn = true;
        m_sturnTime = 0f;
        m_isSturnTime = time;

        m_rb.velocity = Vector2.zero;
        //Debug.Log("스턴 시작");
    }

    private void OnDrawGizmos()
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnDrawGizmos();
    }

    public void Play_AudioSource(string audioClip, bool loop, float speed, float volume)
    {
        m_as.Stop();

        m_as.clip = GameManager.Ins.Play.Effect[audioClip];
        m_as.loop = loop;
        m_as.pitch = speed; // 기본1f
        m_as.volume = volume;
        m_as.Play();
    }

    public void Set_Pause(bool pause)
    {
        if(pause == true) // 정지
        {
            Rb.velocity = Vector2.zero;
            Rb.isKinematic = true;

            if(m_stateMachine.CurState != (int)STATE.ST_DIE)
                m_stateMachine.Change_State((int)STATE.ST_IDLE);
        }
        else
        {
            Rb.isKinematic = false;
        }
    }

    public Vector2 Get_Direction(Vector2 direction)
    {
        Vector2 dir = direction;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            dir = new Vector2(dir.x > 0 ? 1 : -1, 0); // x축이 더 크면 좌우
        else
            dir = new Vector2(0, dir.y > 0 ? 1 : -1); // y축이 더 크면 상하

        return dir;
    }

    public DIRECTION Get_Direction()
    {
        Vector2 direct = Get_Direction(m_joystick.InputVector);

        DIRECTION dirName = DIRECTION.DT_END;

        if (direct.y == 1f) // 상
            dirName = DIRECTION.DT_UP;
        else if (direct.y == -1f) // 하
            dirName = DIRECTION.DT_DOWN;
        else if (direct.x == -1f) // 좌
            dirName = DIRECTION.DT_LEFT;
        else if (direct.x == 1f) // 우
            dirName = DIRECTION.DT_RIGHT;

        return dirName;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnCollisionEnter2D(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnCollisionStay2D(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnCollisionExit2D(collision);
    }

    public void Restart_Player(PlayManager.LEVELSTATE levelState)
    {
        if (levelState == PlayManager.LEVELSTATE.STATE_TREENPC1)
            transform.position = new Vector3(28.33f, 16.74f, 0f);
        else if (levelState == PlayManager.LEVELSTATE.STATE_TREENPC2)
            transform.position = new Vector3(84.6f, -41.53f, 0f);
        else
            transform.position = new Vector3(-45.9f, 15.72f, 0f);

        m_hp = m_hpMax;
        m_hpSlider.Set_Slider(m_hp);
        uiBlood.Stop_Blood();

        m_stamina = m_staminaMax;
        m_staminaSlider.Set_Slider(m_stamina);

        m_shield = 0f;
        m_activeShield = false;
        m_shieldSlider.gameObject.SetActive(false);

        m_activeBuff = false;
        transform.GetChild(4).gameObject.SetActive(false);

        m_attributeType = ATTRIBUTETYPE.AT_FIRE;

        m_joystick.InputVector = new Vector2(0f, -1f);
        AM.SetFloat("X", m_joystick.InputVector.x);
        AM.SetFloat("Y", m_joystick.InputVector.y);

        m_moveSpeed = 5f;
        m_rb.isKinematic = false;

        m_invincibility = false;
        m_dash = false;
        m_dashCool = false;
        m_attackCool = false;
        m_isSturn = false;
        m_recover = false;
        m_recoverTime = 0f;

        m_stateMachine.Change_State((int)STATE.ST_IDLE);
    }
}
