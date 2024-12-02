using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : ScenesManager
{
    public StartManager() : base()
    {
        m_sceneLevel = GameManager.SCENE.SCENE_MAIN;
        m_sceneName = "StartScene";
    }

    protected override void Load_Resource()
    {
    }

    public override void Enter_Game()
    {
        base.Enter_Game();
    }

    protected override void Load_Scene()
    {
        // �ʱ�ȭ
        //*

        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Game());
    }

    protected override void Start_Game()
    {
        base.Start_Game();
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
