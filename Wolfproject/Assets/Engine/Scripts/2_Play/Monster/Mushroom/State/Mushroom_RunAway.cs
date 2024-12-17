using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_RunAway : Mushroom_Base
{
    private Vector2 m_randomPosition;

    public Mushroom_RunAway(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        m_randomPosition = m_owner.spawner.Get_RandomPosition();

        m_owner.SpeedMax = 3f;
        m_owner.Speed = m_owner.SpeedMax;

        m_owner.Animator.SetTrigger("Is_Walk");
        m_owner.Play_AudioSource("Mushroom_Walk", false, 1f, GameManager.Ins.Sound.EffectSound);
        //Debug.Log("사운드 재생5");
    }

    public override void Update_State()
    {
        // 잠복으로 갈지 아니면 공격쪽으로 갈지.
        Vector2 direction = (m_randomPosition - (Vector2)m_owner.transform.position).normalized;
        if (direction.x > 0f)
            m_owner.SpriteRenderer.flipX = true;
        else
            m_owner.SpriteRenderer.flipX = false;

        RaycastHit2D hit = Physics2D.Raycast(m_owner.transform.position, direction, Vector2.Distance(m_owner.transform.position, m_randomPosition), LayerMask.GetMask("Installation", "Player", "Monster"));
        if (hit.collider != null && hit.collider != m_owner.Collider2D)
        {
            //m_time = 0f;
            //m_randomPosition = m_owner.spawner.Get_RandomPosition();
            Change_State();
            return;
        }

        //m_owner.transform.position = Vector2.MoveTowards(m_owner.transform.position, m_randomPosition, m_owner.Speed * Time.deltaTime);
        m_owner.Rigidbody2D.MovePosition(m_owner.Rigidbody2D.position + direction * m_owner.Speed * Time.deltaTime);

        float distanceToTarget = Vector2.Distance(m_owner.transform.position, m_randomPosition);
        if (distanceToTarget <= 0.1f)
            Change_State();
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }

    private void Change_State()
    {
        if (Change_Attack() == false)
            m_stateMachine.Change_State((int)Mushroom.STATE.ST_STEALTH);
    }
}
