using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutManager : ScenesManager
{
    private CutDialog m_dialog;

    private Dictionary<string, Sprite> m_cutSpr = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> CutSpr => m_cutSpr;

    public CutManager() : base()
    {
        m_sceneLevel = GameManager.SCENE.SCENE_CUT;
        m_sceneName = "CutScene";
    }

    protected override void Load_Resource()
    {
        m_cutSpr.Add("CUT_I1", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Cut/Cut_I1"));
    }

    public override void Enter_Game()
    {
        base.Enter_Game();
    }

    protected override void Load_Scene()
    {
        // �ʱ�ȭ
        GameObject canvas = GameObject.Find("Canvas");
        m_dialog = canvas.transform.GetChild(0).GetComponent<CutDialog>();

        Start_Game();
    }

    protected override void Start_Game()
    {
        base.Start_Game();

        // ���̾�α� ���
        string path = "7_Data/";
        switch (GameManager.Ins.PreScene)
        {
            case (int)GameManager.SCENE.SCENE_MAIN: // ��Ʈ�� �ƾ� ���
                path += "Dialog_Intro";
                break;

            case (int)GameManager.SCENE.SCENE_PLAY: // ��, ȥ�� ������ ���� ���� �ƾ� ���
                path += "";
                break;

            default:
                path += "Dialog_Intro";
                break;
        }
        m_dialog.Start_Dialog(path);
    }

    public override void Update_Game()
    {
    }

    public override void LateUpdate_Game()
    {
    }

    public override void Exit_Game()
    {
    }
}
