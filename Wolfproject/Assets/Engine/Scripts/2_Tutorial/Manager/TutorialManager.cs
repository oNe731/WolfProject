using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : ScenesManager
{
    private Player m_player;

    public Player Player => m_player;

    public TutorialManager() : base()
    {
        m_sceneLevel = GameManager.SCENE.SCENE_TUTORIAL;
        m_sceneName = "TutorialScene";
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
        // 초기화
        GameObject gameObject = GameObject.Find("Player");
        m_player = gameObject.GetComponent<Player>();

        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Game());
    }

    protected override void Start_Game()
    {
        base.Start_Game();

        // 이름 설정
        //GameObject name = m_playPanel.transform.Find("Text_Name").gameObject;
        //name.GetComponent<TMP_Text>().text = GameManager.Ins.PlayerName;
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
        if(m_player != null)
            m_player.Set_Pause(true);
    }
}
