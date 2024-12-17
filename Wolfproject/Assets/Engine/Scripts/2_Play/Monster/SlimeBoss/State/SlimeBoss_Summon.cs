using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss_Summon : SlimeBoss_Base
{
    private float m_stateTime = 0f;
    private float m_damageTime = 0f;
    private bool m_createMonster = false;

    public SlimeBoss_Summon(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        // 이동 속도 크게 증가. 
        m_owner.SpeedMax = 6f;
        m_owner.Speed = m_owner.SpeedMax;

        m_stateTime = 0f;
        m_damageTime = 0.5f;
        m_createMonster = false;
        m_owner.Animator.SetTrigger("Is_Summon");
        m_owner.Play_AudioSource("KingSlime_Summon", false, 1f, GameManager.Ins.Sound.EffectSound);
        Debug.Log("자폭 슬라임 소환");
    }

    public override void Update_State()
    {
        if (m_owner.Animator.IsInTransition(0) == true)
            return;

        if (m_owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Is_Summon") == true)
        {
            float animTime = m_owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime >= 1f)
            {
                // 자폭 슬라임 3개 소환
                if (m_createMonster == false)
                {
                    m_createMonster = true;

                    GameObject monsterObj = GameManager.Ins.Load<GameObject>("4_Prefab/2_Monster/SlimeRed");
                    for (int i = 0; i < 3; ++i)
                    {
                        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        dir = dir.normalized;
                        Vector3 spawnPosition = m_owner.transform.position + dir * Random.Range(2f, 3.5f);
                        GameObject obj = GameManager.Ins.Create(monsterObj, spawnPosition, Quaternion.identity, m_owner.spawner.transform);
                        if (obj != null)
                        {
                            Monster monster = obj.GetComponent<Monster>();
                            if (monster != null)
                            {
                                monster.spawner = m_owner.spawner;
                            }
                        }
                    }
                }

                // 플레이어를 일정 시간 압박한(난동 피운후) 후 다시 Chase 상태로 전환
                m_stateTime += Time.deltaTime;
                if (m_stateTime >= 2f)
                {
                    if (Get_PlayerDistance() > m_chaseDistance)
                        m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_CHASE);
                    else
                        m_stateMachine.Change_State((int)SlimeBoss.STATE.ST_SLASH);
                    return;
                }

                if (Change_Attack() == false)
                {
                    // 플레이어 쫓아다닌다.
                    Vector2 direction = ((Vector2)GameManager.Ins.Play.Player.transform.position - (Vector2)m_owner.transform.position).normalized;
                    m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);
                    m_owner.SpriteRenderer.flipX = direction.x > 0f;
                }
            }
        }
    }

    public override void Exit_State()
    {
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        // 플레이어는 대왕슬라임과 닿을시 데미지를 입는다. 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Player_K"))
        {
            m_damageTime += Time.deltaTime;
            if(m_damageTime >= 1f)
            {
                m_damageTime = 0f;
                GameManager.Ins.Play.Player.Damaged_Player(1f);
            }
        }
    }
}
