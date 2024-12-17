using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : MonoBehaviour, IPointerDownHandler
{
    public enum TYPE { TYPE_END, TYPE_NEAR, TYPE_FAR, TYPE_CHANGE, TYPE_DASH }

    [SerializeField] private TYPE m_buttonType;

    public void OnPointerDown(PointerEventData eventData)
    {
         switch(m_buttonType)
        {
            case TYPE.TYPE_NEAR:
                Button_Near();
                break;

            case TYPE.TYPE_FAR:
                Button_Far();
                break;

            case TYPE.TYPE_CHANGE:
                Button_Change();
                break;

            case TYPE.TYPE_DASH:
                Button_Dash();
                break;
        }
    }

    private void Button_Near() // ���� ���� + ��
    {
        if(GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Attack_Player(Player.ATTACKTYPE.AT_NEAR);
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Attack_Player(Player.ATTACKTYPE.AT_NEAR);
    }

    private void Button_Far() // ���Ÿ� ���� + ��
    {
        if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Attack_Player(Player.ATTACKTYPE.AT_FAR);
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Attack_Player(Player.ATTACKTYPE.AT_FAR);
    }

    private void Button_Change() // �Ӽ� ��ȯ
    {
        if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Change_AttributeType();
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Change_AttributeType();
    }

    private void Button_Dash() // �뽬 + ��
    {
        if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_TUTORIAL)
            GameManager.Ins.Tutorial.Player.Dash_Player();
        else if (GameManager.Ins.CurScene == (int)GameManager.SCENE.SCENE_PLAY)
            GameManager.Ins.Play.Player.Dash_Player();
    }
}
