using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Die : Player_Base
{
    public Player_Die(StateMachine<Player> stateMachine) : base(stateMachine)
    {
    }

    public override void Enter_State()
    {
        GameManager.Ins.Play.Over_Game();
    }

    public override void Update_State()
    {
    }

    public override void Exit_State()
    {
    }

    public override void OnDrawGizmos()
    {
    }
}
