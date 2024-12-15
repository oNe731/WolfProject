using System.Collections;
using UnityEngine;

public class Monster : Character
{
    public enum TYPE { TYPE_SLIME, TYPE_MUSHROOM, TYPE_SLIMEBOSS, TYPE_SLIMERED, TYPE_END }

    protected float m_hp;
    protected float m_hpMax;

    protected float m_speed;
    protected float m_speedMax;

    protected float m_damage;
    protected float m_damageVariation = 1f;

    protected StateMachine<Monster> m_stateMachine;
    protected int m_dieIndex = -1;
    protected int m_hitIndex = -1;

    private bool m_isKnockedBack = false; // 참일 때 이동 금지
    private Coroutine m_knockedBackCoroutine = null;

    private Material m_defaultMat;
    private Material m_whiteFlashMat;
    private Coroutine m_whiteCoroutine = null;

    public float Hp { get => m_hp; }
    public float HpMax { get => m_hpMax; }
    public float Speed { get => m_speed; set => m_speed = value; }
    public float SpeedMax { get => m_speedMax; set => m_speedMax = value; }
    public float Damage { get => m_damage; }
    public float DamageVariation { get => m_damageVariation; set => m_damageVariation = value; }
    public bool IsKnockedBack { get => m_isKnockedBack; }

    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;
    private Collider2D m_collider2D;
    private Animator m_animator;

    private Spawner m_spawner;
    public Spawner spawner { get => m_spawner; set => m_spawner = value; }
    public Rigidbody2D Rigidbody2D { get => m_rigidbody2D; }
    public SpriteRenderer SpriteRenderer { get => m_spriteRenderer; }
    public Collider2D Collider2D { get => m_collider2D; }
    public Animator Animator { get => m_animator; }

    public void Damaged_Monster(float damage, bool knockedBack = true)
    {
        Debug.Log(damage + "데미지 입음");

        m_hp -= (damage * m_damageVariation);
        if (m_hp <= 0)
        {
            m_hp = 0;
            if(m_dieIndex != -1 && m_stateMachine.CurState != m_dieIndex)
                m_stateMachine.Change_State(m_dieIndex);
        }
        else
        {
            if (m_hitIndex != -1)
                m_stateMachine.Change_State(m_hitIndex);

            // 넉백 적용
            if (knockedBack == true)
                Start_KnockedBack(2f);

            // 메테리얼 변경
            if (m_whiteCoroutine != null)
                StopCoroutine(m_whiteCoroutine);
            m_whiteCoroutine = StartCoroutine(Flash_Material());
        }
    }

    private IEnumerator Flash_Material()
    {
        m_spriteRenderer.material = m_whiteFlashMat;

        yield return new WaitForSeconds(0.2f);

        m_spriteRenderer.material = m_defaultMat;
    }

    private void Start_KnockedBack(float knockBackThrust)
    {
        m_isKnockedBack = true;

        Vector2 difference = (transform.position - GameManager.Ins.Play.Player.transform.position).normalized * knockBackThrust * m_rigidbody2D.mass;
        m_rigidbody2D.AddForce(difference, ForceMode2D.Impulse);

        if (m_knockedBackCoroutine != null)
            StopCoroutine(m_knockedBackCoroutine);
        m_knockedBackCoroutine = StartCoroutine(Stop_KnockedBack());
    }

    private IEnumerator Stop_KnockedBack()
    {
        yield return new WaitForSeconds(0.2f);

        m_rigidbody2D.velocity = Vector2.zero;
        m_isKnockedBack = false;
    }

    protected void Initialize_Monster()
    {
        Initialize_Character();

        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_defaultMat = m_spriteRenderer.material;
        m_collider2D = GetComponent<Collider2D>();
        m_animator = GetComponent<Animator>();

        m_whiteFlashMat = GameManager.Ins.Load<Material>("6_Material/MonsterMaterial");
    }

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnDrawGizmos();
    }
}
