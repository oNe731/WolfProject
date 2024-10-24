using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float m_speedMax = 5f;
    private float m_speed = 0f;

    private float m_time = 0f;
    private float m_totalTime = 0f;
    private Monster m_monster = null;
    private Player m_player = null;

    private Player.ATTRIBUTETYPE m_type;
    private Vector2 m_direction;

    private bool m_isSuccess = false;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    public void Start_Projectile(Vector3 startPosition, Player.ATTRIBUTETYPE type, Vector2 direct)
    {
        m_player = GameManager.Ins.Play.Player;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();

        m_type = type;
        m_direction = direct.normalized;

        transform.position = startPosition + (new Vector3(m_direction.x, m_direction.y, 0f) * 0.1f);
        if (m_type == Player.ATTRIBUTETYPE.AT_FIRE)
        {
            m_animator.SetTrigger("IsFire");
            GameManager.Ins.Play.Player.Play_AudioSource("Player_fire", false, 1f, 1f);
        }
        else if (m_type == Player.ATTRIBUTETYPE.AT_THUNDER)
        {
            m_animator.SetTrigger("IsThunder");
            GameManager.Ins.Play.Player.Play_AudioSource("Player_lightning", false, 1f, 1f);
        }
    }

    void Update()
    {
        if(m_isSuccess == false)
        {
            if (m_player.Stamina <= 0f)
                m_speed = m_speedMax * 0.5f; // 속도 감소
            else
                m_speed = m_speedMax;
            transform.Translate(m_direction * m_speed * Time.deltaTime);
        }
        else
        {
            if (m_type == Player.ATTRIBUTETYPE.AT_FIRE)
            {
                m_totalTime += Time.deltaTime;
                if (m_totalTime > 2f)
                {
                    m_totalTime = 0f;
                    Destroy(gameObject);
                }
                else
                {
                    m_time += Time.deltaTime;
                    if (m_time >= 0.5f) // 지속 피해
                    {
                        m_time = 0f;
                        if (m_monster != null)
                            m_monster.Damaged_Monster(0.5f, false);
                        else
                            Destroy(gameObject);
                    }
                }
            }
            else if (m_type == Player.ATTRIBUTETYPE.AT_THUNDER)
            {
                m_totalTime += Time.deltaTime;
                if(m_totalTime > 1f)
                {
                    m_totalTime = 0f;
                    m_monster.Speed = m_monster.SpeedMax;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isSuccess == true)
            return;

        if (collision.CompareTag("Enemy") == true)
        {
            m_isSuccess = true;
            //m_spriteRenderer.enabled = false;
            m_animator.SetBool("IsBoom", true);

            m_monster = collision.gameObject.GetComponent<Monster>();
            if (m_type == Player.ATTRIBUTETYPE.AT_THUNDER)
            {
                if(m_monster != null)
                    m_monster.Speed = m_monster.SpeedMax * 0.5f; // 이동 속도 감소
            }
        }
    }
}
