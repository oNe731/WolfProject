using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayManager : ScenesManager
{
    public enum LEVELSTATE { STATE_START, STATE_TREENPC1, STATE_TREENPC2, STATE_END }

    private LEVELSTATE m_levelState = LEVELSTATE.STATE_START;

    private Player m_player;
    private GameObject m_playPanel;
    private GameObject m_overPanel;
    private PlayDialog m_dialog;

    private GameObject m_door;
    private Dictionary<string, Sprite> m_profileSpr = new Dictionary<string, Sprite>();


    public LEVELSTATE LevelState { get => m_levelState; set => m_levelState = value; }
    public PlayDialog Dialog => m_dialog;

    public Dictionary<string, Sprite> ProfileSpr => m_profileSpr;
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
        m_profileSpr.Add("Portrait_NPC_Happy", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_NPC_Happy"));
        m_profileSpr.Add("Portrait_NPC_Sad",   GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_NPC_Sad"));
        m_profileSpr.Add("Portrait_Player",    GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Player"));
        m_profileSpr.Add("Portrait_Slime",     GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Slime"));
        m_profileSpr.Add("Portrait_Tree",      GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Tree"));
        m_profileSpr.Add("Portrait_Wolf",      GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Wolf"));


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
        m_dialog = canvas.transform.Find("Panel_Dialog").gameObject.GetComponent<PlayDialog>();

        m_levelState = LEVELSTATE.STATE_START;

        // 보스 출구 벽 생성
        m_door = GameManager.Ins.LoadCreate("4_Prefab/3_Map/Door");
        m_door.transform.position = new Vector3(137.5f, -28.52f, 0f);

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
        m_overPanel.SetActive(true);
    }

    public override void Exit_Game()
    {
    }

    public override void Set_Pause(bool pause)
    {
        if (m_player != null)
            m_player.Set_Pause(pause);
    }

    public void Clear_Game()
    {
        GameManager.Ins.Destroy(m_door);
    }

    public void Restart_Game()
    {
        m_overPanel.SetActive(false);
        m_player.Restart_Player(m_levelState);

        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Game());
    }
}
