using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected float m_hp;
    protected float m_hpMax;

    private float m_speed;
    private float m_speedMax;

    protected StateMachine<Player> m_stateMachine;
    protected int m_dieIndex = -1;
    protected int m_hitIndex = -1;

    private bool m_isKnockedBack = false; // 참일 때 이동 금지
    private Coroutine m_knockedBackCoroutine = null;

    private Material m_defaultMat;
    private Material m_whiteFlashMat;
    private Coroutine m_whiteCoroutine = null;

    public float Speed { get => m_speed; set => m_speed = value; }
    public float SpeedMax { get => m_speedMax; }
    public bool IsKnockedBack { get => m_isKnockedBack; }

    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;

    public void Damaged_Monster(float damage, bool knockedBack = true)
    {
        m_hp -= damage;
        if (m_hp <= 0)
        {
            m_hp = 0;
            //if(m_dieIndex != -1)
            //    m_stateMachine.Change_State(m_dieIndex);

            Destroy(gameObject);
        }
        else
        {
            if (m_hitIndex != -1)
                m_stateMachine.Change_State(m_hitIndex);

            // 넉백 적용
            if(knockedBack == true)
                Start_KnockedBack(5f);

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

    protected void Start_Monster()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_defaultMat = m_spriteRenderer.material;

        m_whiteFlashMat = GameManager.Ins.Load<Material>("5_Material/MonsterMaterial");
    }

    void Update()
    {
        
    }
}
