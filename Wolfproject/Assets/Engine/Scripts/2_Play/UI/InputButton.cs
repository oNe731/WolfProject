using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    public void Button_Near() // ���� ���� + ��
    {
        GameManager.Ins.Play.Player.Attack_Player(Player.ATTACKTYPE.AT_NEAR);
    }

    public void Button_Far() // ���Ÿ� ���� + ��
    {
        GameManager.Ins.Play.Player.Attack_Player(Player.ATTACKTYPE.AT_FAR);
    }

    public void Button_Change() // �Ӽ� ��ȯ
    {
        GameManager.Ins.Play.Player.Change_AttributeType();
    }

    public void Button_Dash() // �뽬 + ��
    {
        GameManager.Ins.Play.Player.Dash_Player();
    }
}
