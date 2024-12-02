using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayManager : ScenesManager
{
    private Player m_player;
    private GameObject m_playPanel;
    private GameObject m_overPanel;

    private Dictionary<string, AudioClip> m_effect = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> Effect => m_effect;

    public Player Player => m_player;

    public PlayManager() : base()
    {
        m_sceneLevel = GameManager.SCENE.SCENE_PLAY;
        m_sceneName = "MainScene";
    }

    protected override void Load_Resource()
    {
        m_effect.Add("Player_Melee", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/Player/Attack/Melee"));
        m_effect.Add("Player_fire", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/Player/Attack/fire"));
        m_effect.Add("Player_lightning", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/Player/Attack/lightning"));
        m_effect.Add("Player_dash", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/Player/dash"));
        
        m_effect.Add("Player_run", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/Player/run"));
        m_effect.Add("Enemy_damaged", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/Enemy/damaged"));
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

        GameObject canvas = GameObject.Find("Canvas");
        m_playPanel = canvas.transform.Find("Panel_Play").gameObject;
        m_overPanel = canvas.transform.Find("Panel_Over").gameObject;

        // 이름 설정
        //GameObject name = m_playPanel.transform.Find("Text_Name").gameObject;
        //name.GetComponent<TMP_Text>().text = GameManager.Ins.PlayerName;

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

    public override void Set_Pause(bool pause)
    {
    }
}
