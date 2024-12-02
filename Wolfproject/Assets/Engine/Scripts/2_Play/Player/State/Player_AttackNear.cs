using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackNear : Player_Attack
{
    private bool m_attack = false;
    private float m_distance = 1f;
    private BoxCollider2D m_attackCollider;

    public Player_AttackNear(StateMachine<Player> stateMachine) : base(stateMachine, 0)
    {
        m_coolTime = 1f;

        m_animationName = "IsAttackNear";
        m_attackCollider = m_owner.transform.GetChild(2).GetComponent<BoxCollider2D>();
    }

    public override void Enter_State()
    {
        base.Enter_State();
        m_attack = false;
        m_owner.AM.SetTrigger(m_animationName);
    }

    public override void Update_State()
    {
        if (m_owner.AM.IsInTransition(0) == true) 
            return;

        if (m_owner.AM.GetCurrentAnimatorStateInfo(0).IsName(m_animationName) == true)
        {
            float animTime = m_owner.AM.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if(m_attack == false && animTime > 0.5)
            {
                m_attack = true;
                Check_AttackCollision(m_attackCollider);
            }
            else if (animTime >= 1.0f) // �ִϸ��̼� ����
                m_stateMachine.Change_State((int)Player.STATE.ST_IDLE);
        }
    }

    public override void Exit_State()
    {
        base.Exit_State();
    }

    public override void OnDrawGizmos()
    {
        if (m_attackCollider == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_attackCollider.transform.position, m_distance);
    }

    private void Check_AttackCollision(BoxCollider2D collider)
    {
        m_owner.Play_AudioSource("Player_Melee", false, 1f, 1f);

        // ����Ʈ ����
        GameObject obj = GameManager.Ins.LoadCreate("4_Prefab/5_Effect/Attack");
        if(obj != null)
        {
            obj.transform.position = new Vector3(m_owner.transform.position.x + Random.Range(-0.2f, 0.2f), m_owner.transform.position.y + Random.Range(-0.2f, 0.2f), m_owner.transform.position.z);
        }

        // Ư�� ���� �ȿ� �ִ� ��� �ݶ��̴��� ������ // OverlapCircle : �� ������ ����, 2D ���� �ý���
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(collider.transform.position, m_distance, LayerMask.GetMask("Monster"));
        foreach (Collider2D hitCollider in hitColliders)
        {
            Monster monster = hitCollider.gameObject.GetComponent<Monster>();
            if (monster != null)
                monster.Damaged_Monster(m_damage);
        }
    }
}
