using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    public void Button_Near() // ���� ���� + ��
    {
        if(GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Attack_Player(Player.ATTACKTYPE.AT_NEAR);
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Attack_Player(Player.ATTACKTYPE.AT_NEAR);
    }

    public void Button_Far() // ���Ÿ� ���� + ��
    {
        if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Attack_Player(Player.ATTACKTYPE.AT_FAR);
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Attack_Player(Player.ATTACKTYPE.AT_FAR);
    }

    public void Button_Change() // �Ӽ� ��ȯ
    {
        if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Change_AttributeType();
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Change_AttributeType();
    }

    public void Button_Dash() // �뽬 + ��
    {
        if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Dash_Player();
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Dash_Player();
    }
}
