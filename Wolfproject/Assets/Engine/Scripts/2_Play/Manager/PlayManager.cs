using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : ScenesManager
{
    private Player m_player;
    private GameObject m_overPanel;

    public Player Player => m_player;

    public PlayManager() : base()
    {
        m_sceneLevel = GameManager.SCENE.SCENE_PLAY;
        m_sceneName = "MainScene";
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
        // √ ±‚»≠
        GameObject gameObject = GameObject.Find("Player");
        m_player = gameObject.GetComponent<Player>();
        GameObject canvas = GameObject.Find("Canvas");
        m_overPanel = canvas.transform.Find("Panel_Over").gameObject;

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

    public override void Over_Game()
    {
        GameManager.Ins.IsGame = false;
        m_overPanel.SetActive(true);
    }

    public override void Exit_Game()
    {
    }
}
