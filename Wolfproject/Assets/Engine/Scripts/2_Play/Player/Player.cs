using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public enum STATE { ST_IDLE, ST_WALK, ST_DASH, ST_ATTACKNEAR, ST_ATTACKFAR, ST_HIT, ST_DIE, ST_END }    
    public enum ATTRIBUTETYPE { AT_FIRE, AT_THUNDER, AT_END }
    public enum ATTACKTYPE { AT_NEAR, AT_FAR, AT_END }

    [SerializeField] private Slider[] m_hpSliders;
    [SerializeField] private Slider[] m_staminaSliders;
    [SerializeField] private Joystick m_joystick;
    [SerializeField] private UIBlood uiBlood;
    [SerializeField] private Inventory m_inventory;

    [SerializeField] private Image[] m_buttonImage;
    [SerializeField] private Sprite[] m_buttonSprite;

    private UISliderOwner m_hpSlider;
    private UISliderOwner m_staminaSlider;

    private float m_hp;
    private float m_hpMax;
    private float m_stamina;
    private float m_staminaMax;
    private float m_damage;

    private ATTRIBUTETYPE m_attributeType = ATTRIBUTETYPE.AT_FIRE;
    private float m_moveSpeed = 5f;

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
    public ATTRIBUTETYPE AttributeType => m_attributeType;
    public float Damage { get => m_damage; }
    public float MoveSpeed { get => m_moveSpeed; set => m_moveSpeed = value; }
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

    public void Damaged_Player(float damage)
    {
        if (m_invincibility == true) // 무적 상태
            return;

        m_hp -= damage;
        if(m_hp <= 0)
        {
            m_hp = 0;
            m_stateMachine.Change_State((int)STATE.ST_DIE);
        }
        else
        {
            if (m_hp <= m_hpMax * 0.2f)
                uiBlood.Start_Blood();
            else
                uiBlood.Stop_Blood();
            m_stateMachine.Change_State((int)STATE.ST_HIT);
        }

        // 이펙트 생성
        GameObject obj = GameManager.Ins.LoadCreate("4_Prefab/5_Effect/Hit");
        if (obj != null)
        {
            obj.transform.position = new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f), transform.position.z);
        }

        m_hpSlider.Set_Slider(m_hp);
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
        states.Add(new Player_Hit(m_stateMachine));        // 5
        states.Add(new Player_Die(m_stateMachine));        // 6
        m_stateMachine.Initialize_State(states, (int)STATE.ST_IDLE);
    }

    private void Update()
    {
        if (m_recover == true)
            Recover_Stamina();

        if(m_isSturn == true)
        {
            m_sturnTime += Time.deltaTime;
            if(m_sturnTime >= m_isSturnTime)
            {
                m_sturnTime = 0f;
                m_isSturn = false;
                Debug.Log("스턴 종료");
            }
            return;
        }

        m_stateMachine.Update_State();
    }

    public void Attack_Player(ATTACKTYPE attackType)
    {
        if (m_attackCool == true || m_isSturn == true)
            return;

        switch (attackType)
        {
            case ATTACKTYPE.AT_NEAR:
                m_stateMachine.Change_State((int)STATE.ST_ATTACKNEAR);
                break;

            case ATTACKTYPE.AT_FAR:
                if (m_stamina < 5)
                    return;
                Use_Stamina(5f);
                m_stateMachine.Change_State((int)STATE.ST_ATTACKFAR);
                break;
        }
    }

    public void Change_AttributeType()
    {
        if (m_isSturn == true)
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
        if (m_dash == true || m_dashCool == true || m_stamina < 10f || m_isSturn == true)
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
        Color color;
        if (m_stamina <= 10) 
            color = new Color(1f, 0f, 0f, 1f); // 빨간색
        else
            color = new Color(0.3603774f, 0.5231216f, 1f, 1f); // 파란색
        m_staminaSlider.FillImage.color = color;
    }


    public void Start_Sturn(float time)
    {
        m_isSturn = true;
        m_sturnTime = 0f;
        m_isSturnTime = time;

        m_rb.velocity = Vector2.zero;
        Debug.Log("스턴 시작");
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
}
