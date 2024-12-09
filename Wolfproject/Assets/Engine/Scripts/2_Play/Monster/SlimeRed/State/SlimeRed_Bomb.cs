using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRed_Bomb : SlimeRed_Base
{
    public SlimeRed_Bomb(StateMachine<Monster> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        if (Get_PlayerDistance() <= 1f)
            GameManager.Ins.Play.Player.Damaged_Player(2f);

        GameObject obj = GameManager.Ins.LoadCreate("4_Prefab/5_Effect/Bomb");
        if (obj != null)
        {
            obj.transform.position = m_owner.transform.position;

            float size = Random.Range(0.4f, 1f);
            obj.transform.localScale = new Vector3(size, size, 1f);
        }

        m_stateMachine.Change_State((int)SlimeRed.STATE.ST_DIE);
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }
}
