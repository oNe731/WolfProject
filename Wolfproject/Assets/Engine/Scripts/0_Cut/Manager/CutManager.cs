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
        m_cutSpr.Add("CUT_I2", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Cut/Cut_I2"));

        m_cutSpr.Add("Cut_Es1", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Cut/Cut_Es1"));
        m_cutSpr.Add("Cut_Es2", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Cut/Cut_Es2"));

        m_cutSpr.Add("Cut_Ef1", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Cut/Cut_Ef1"));
        m_cutSpr.Add("Cut_Ef2", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Cut/Cut_Ef2"));
    }

    public override void Enter_Game()
    {
        base.Enter_Game();
    }

    protected override void Load_Scene()
    {
        // √ ±‚»≠
        GameObject canvas = GameObject.Find("Canvas");
        m_dialog = canvas.transform.GetChild(0).GetComponent<CutDialog>();

        Start_Game();
    }

    protected override void Start_Game()
    {
        base.Start_Game();

        // ¥Ÿ¿ÃæÛ∑Œ±◊ √‚∑¬
        string path = "7_Data/";
        switch (GameManager.Ins.PreScene)
        {
            case (int)GameManager.SCENE.SCENE_MAIN: // ¿Œ∆Æ∑Œ ƒ∆æ¿ √‚∑¬
                path += "Dialog_Intro";
                GameManager.Ins.Sound.Play_AudioSourceBGM("Cut_Intro", true, 1f);
                break;

            case (int)GameManager.SCENE.SCENE_PLAY: // º±, »•µ∑ ¡°ºˆø° µ˚∏• ø£µ˘ ƒ∆æ¿ √‚∑¬
                if(GameManager.Ins.ZenScore > GameManager.Ins.ChaosScore)
                {
                    path += "Dialog_EndingS";
                    GameManager.Ins.Sound.Play_AudioSourceBGM("Cut_Ending_Law", true, 1f);
                }
                else
                {
                    path += "Dialog_EndingF";
                    GameManager.Ins.Sound.Play_AudioSourceBGM("Cut_Ending_Chaos", true, 1f);
                }
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

    public override void Set_Pause(bool pause)
    {
    }
}
