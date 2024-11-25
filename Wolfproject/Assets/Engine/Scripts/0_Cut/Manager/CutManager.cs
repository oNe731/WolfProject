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
        // 초기화
        GameObject canvas = GameObject.Find("Canvas");
        m_dialog = canvas.transform.GetChild(0).GetComponent<CutDialog>();

        Start_Game();
    }

    protected override void Start_Game()
    {
        base.Start_Game();

        // 다이얼로그 출력
        string path = "7_Data/";
        switch (GameManager.Ins.PreScene)
        {
            case (int)GameManager.SCENE.SCENE_MAIN: // 인트로 컷씬 출력
                path += "Dialog_Intro";
                break;

            case (int)GameManager.SCENE.SCENE_PLAY: // 선, 혼돈 점수에 따른 엔딩 컷씬 출력
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
